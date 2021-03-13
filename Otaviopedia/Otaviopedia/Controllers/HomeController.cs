using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Otaviopedia.HttpClients;
using Otaviopedia.Models;
using Otaviopedia.Models.OxfordDictionary;

namespace Otaviopedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CovidClient _covidClient;
        private readonly NasaClient _nasaClient;
        private readonly SpaceXClient _spaceXClient;
        private readonly OxfordDictionaryClient _oxfordDictionaryClient;

        public HomeController(ILogger<HomeController> logger, CovidClient covidClient, NasaClient nasaClient, SpaceXClient spaceXClient, OxfordDictionaryClient oxfordDictionaryClient)
        {
            _logger = logger;
            _covidClient = covidClient;
            _nasaClient = nasaClient;
            _spaceXClient = spaceXClient;
            _oxfordDictionaryClient = oxfordDictionaryClient;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ViewData["dictEncyclopedia"] = await _oxfordDictionaryClient.GetDefinitionByWord("encyclopedia");
            ViewData["dictAPI"] = await _oxfordDictionaryClient.GetDefinitionByWord("API");
            ViewData["sourceAPI"] = _oxfordDictionaryClient.GetBaseAddress();
            return View();
        }

        public IActionResult GetOxfordDictionary(){
            ViewData["sourceAPI"] = _oxfordDictionaryClient.GetBaseAddress();
            return View("~/Views/OxfordDictionary/OxfordDictionary.cshtml");
        }

        public async Task<IActionResult> GetDefinitionByWordAsync(string word){
            ViewData["dictResult"] = await _oxfordDictionaryClient.GetDefinitionByWord(word);
            return PartialView("~/Views/OxfordDictionary/DictionaryDefinitionPartial.cshtml");
        }

        public async Task<IActionResult> NASAAPODAsync()
        {
            ViewData["APOD"] = await _nasaClient.GetAPODAsync();
            ViewData["sourceAPI"] = _nasaClient.GetBaseAddress();
            return View("~/Views/NASA/APOD.cshtml");
        }

        public async Task<IActionResult> SpaceXGetCrewAsync()
        {
            ViewData["crew"] = await _spaceXClient.GetCrew();
            ViewData["sourceAPI"] = _spaceXClient.GetBaseAddress();

            return View("~/Views/SpaceX/Crew.cshtml");
        }
        public async Task<IActionResult> COVIDGetCasesByCountry()
        {
            ViewData["cases"] = await _covidClient.GetCasesByCountry();
            ViewData["sourceAPI"] = _covidClient.GetBaseAddress();
            return View("~/Views/COVID/CasesByCountry.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
