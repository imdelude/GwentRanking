using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
namespace GwentRanking.Data
{
    public class RankingScraper : IRankingScraper
    {
        private const string BaseUrl = "https://masters.playgwent.com/en/rankings/pro-ladder/";

        public List<RankingDataRow> Scrape(int numberOfRanks = 1000)
        {
            var rankingRows = new List<RankingDataRow>();

            var webClient = new WebClient();
            var index = 1;
            var rank = 1;
            while (rank <= numberOfRanks)
            {
                var page = webClient.DownloadString(BaseUrl + index++);
                var doc = new HtmlDocument();
                doc.LoadHtml(page);
                var table = doc.DocumentNode.SelectSingleNode("//div[@class='c-ranking-mobile-table__body']");
                
                foreach (var node in table.ChildNodes)
                {
                    var row = new RankingDataRow();
                    row.Name = node.ChildNodes[1].InnerText;
                    
                    if (rankingRows.Any(x => x.Name == row.Name)) continue;
                        
                    row.Rank = rank++;
                    row.Country = GetCountry(node);
                    row.Mmr = int.Parse(node.ChildNodes[2].FirstChild.InnerText.Replace(",", ""));
                    rankingRows.Add(row);
                    
                }
            }
            return rankingRows;
        }

        private string GetCountry(HtmlNode node)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            var countryCode = node.ChildNodes[1].InnerHtml.Split("flag-icon-")[1].Substring(0, 2).ToUpper();
            var rawCountry = cultures.FirstOrDefault(x => x.Name.Substring(x.Name.LastIndexOf('-') + 1) == countryCode).EnglishName.Split('(')[1].Substring(0);
            var cleanedCountry = rawCountry.Remove(rawCountry.Length - 1);
            return cleanedCountry.Contains(",") ? cleanedCountry.Split(',')[1].Trim() : cleanedCountry;
        }
    }
}