using System.Collections.Generic;

namespace GwentRanking.Data 
{
    public interface IRankingScraper 
    {
        List<RankingDataRow> Scrape(int numberOfRanks);
    }
}