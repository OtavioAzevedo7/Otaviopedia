using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Otaviopedia.Models;
using Otaviopedia.Models.COVID;
using Otaviopedia.Models.NASA;
using Otaviopedia.Models.OxfordDictionary;
using Otaviopedia.Models.SPACE_X;

namespace Otaviopedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            OxfordModel oxford = new OxfordModel();
            ViewData["dictEncyclopedia"] = oxford.GetDefinitionByWord("encyclopedia");
            ViewData["dictAPI"] = oxford.GetDefinitionByWord("API");
            return View();
        }

        public IActionResult NASAAPOD()
        {
            NasaModel nasa = new NasaModel();
            ViewData["APOD"] = nasa.GetAPOD();
            return View("~/Views/NASA/APOD.cshtml");
        }

        public IActionResult SpaceXGetCrew()
        {
            SpaceXModel spacex = new SpaceXModel();
            ViewData["crew"] = spacex.GetCrew();

            return View("~/Views/SpaceX/Crew.cshtml");
        }

        public IActionResult COVIDGetCasesByCountry()
        {
            COVIDModel covid = new COVIDModel();
            ViewData["cases"] = covid.GetCasesByCountry();

            return View("~/Views/COVID/CasesByCountry.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
