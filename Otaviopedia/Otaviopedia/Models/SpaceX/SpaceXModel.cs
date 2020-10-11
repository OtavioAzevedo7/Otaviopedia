using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Otaviopedia.Models.SPACE_X
{
    public class SpaceXModel
    {
        public List<Crew> GetCrew()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SPACEX_OpenAPIs")["BaseURL_v4"];

                HttpResponseMessage response = client.GetAsync(baseURL + "crew").Result;

                response.EnsureSuccessStatusCode();

                string content = response.Content.ReadAsStringAsync().Result;

                dynamic result = JsonConvert.DeserializeObject(content);

                List<Crew> crewList = new List<Crew>();

                foreach (var person in result)
                {
                    crewList.Add(new Crew
                    {
                        Name = person.name,
                        Agency = person.agency,
                        Image = person.image,
                        Wikipedia = person.wikipedia,
                        //Launches = person.launches
                    });
                }

                crewList[0].SourceAPI = "https://docs.spacexdata.com/";

                return crewList;
            }
        }
    }
}
