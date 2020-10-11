using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Otaviopedia.Models.NASA
{
    public class NasaModel
    {
        /*TODO, why dont work in .net core 3.1?
        private readonly IConfiguration _config;
		*/
        public ImageAPOD GetAPOD()
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("NASA_OpenAPIs")["BaseURL"]; ;
                string key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("NASA_OpenAPIs")["Key"]; ;

                DateTime dataBase = DateTime.Now.Date.AddDays(
                    new Random().Next(0, 7) * -1);

                HttpResponseMessage response = client.GetAsync(
                    baseURL + "apod?" +
                    $"api_key={key}&" +
                    $"date={dataBase.ToString("yyyy-MM-dd")}").Result;

                response.EnsureSuccessStatusCode();
                string conteudo = response.Content.ReadAsStringAsync().Result;

                dynamic resultado = JsonConvert.DeserializeObject(conteudo);

                ImageAPOD image = new ImageAPOD();
                image.Date = dataBase;
                image.SourceAPI = "https://api.nasa.gov";
                image.Title = resultado.title;
                image.Description = resultado.explanation;
                image.Url = resultado.url;
                image.MediaType = resultado.media_type;
                image.Copyright = resultado.copyright;

                return image;
            }
        }
    }
}