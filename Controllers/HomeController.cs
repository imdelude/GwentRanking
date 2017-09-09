using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GwentRanking.Models;
using Newtonsoft.Json;
using System.IO;

namespace GwentRanking.Controllers
{
    public class HomeController : Controller
    {
        private const string FilePath = "./content/ranks.txt";
        public IActionResult Index()
        {
            var path = Path.GetFullPath(FilePath);
            var rankingList = JsonConvert.DeserializeObject<List<RankingViewModel>>(System.IO.File.ReadAllText(Path.GetFullPath(FilePath)));
            return View(rankingList);
        }
    }
}
