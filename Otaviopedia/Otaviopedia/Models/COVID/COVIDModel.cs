using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Otaviopedia.Models.COVID
{
    public class COVIDModel
    {
        public List<CasesByCountry> GetCasesByCountry()
        {
            List<CasesByCountry> cases = new List<CasesByCountry>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("COVID_OpenAPIs")["BaseURL_Worldometers"];

                HttpResponseMessage response = client.GetAsync($"{baseURL}countries").Result;

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



                cases[0].SourceAPI = "https://disease.sh";

                return cases;
            }
        }
        public CasesByCountry GetCasesByCountry(string country)
        {
            CasesByCountry cases = new CasesByCountry();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("COVID_OpenAPIs")["BaseURL_Worldometers"];

                HttpResponseMessage response = client.GetAsync($"{baseURL}countries/{ country}").Result;

                response.EnsureSuccessStatusCode();

                string content = response.Content.ReadAsStringAsync().Result;

                dynamic result = JsonConvert.DeserializeObject(content);

                cases.Country = result.country;
                cases.Flag = result.countryInfo.flag;
                cases.Cases = result.cases;
                cases.TotalDeaths = result.deaths;
                cases.TodayCases = result.todayCases;
                cases.TodayDeaths = result.todayDeaths;
                cases.Population = result.population;
                cases.Continent = result.continent;
                cases.OneCasePerPeople = result.oneCasePerPeople;
                cases.OneDeathPerPeople = result.oneDeathPerPeople;

                cases.SourceAPI = "https://disease.sh";

                return cases;
            }
        }
    }
}
