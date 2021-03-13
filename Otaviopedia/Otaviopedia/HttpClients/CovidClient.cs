using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Otaviopedia.Models.COVID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Otaviopedia.HttpClients
{
    public class CovidClient : IHttpCustomClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CovidClient(HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetSection("COVID_OpenAPIs")["BaseURL_Worldometers"]);
        }

        public string GetBaseAddress()
        {
            return _httpClient.BaseAddress.ToString();
        }

        public async Task<List<CasesByCountry>> GetCasesByCountry()
        {
            List<CasesByCountry> cases = new List<CasesByCountry>();

            HttpResponseMessage response = await _httpClient.GetAsync("countries");
            response.EnsureSuccessStatusCode();

            string content = response.Content.ReadAsStringAsync().Result;

            dynamic result = JsonConvert.DeserializeObject(content);

            foreach (var country in result)
            {

                cases.Add(new CasesByCountry
                {
                    Country = country.country,
                    Flag = country.countryInfo.flag,
                    Cases = country.cases,
                    TotalDeaths = country.deaths,
                    TodayCases = country.todayCases,
                    TodayDeaths = country.todayDeaths,
                    Population = country.population,
                    Continent = country.continent,
                    OneCasePerPeople = country.oneCasePerPeople,
                    OneDeathPerPeople = country.oneDeathPerPeople
                });
            }

            return cases;
        }
    }
}