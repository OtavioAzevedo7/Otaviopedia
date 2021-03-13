using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Otaviopedia.Models.NASA;

namespace Otaviopedia.HttpClients
{
    public class NasaClient : IHttpCustomClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public NasaClient(HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("NASA_OpenAPIs")["BaseURL"]);
            _apiKey = _configuration.GetSection("NASA_OpenAPIS")["Key"];
        }

        public async Task<ImageAPOD> GetAPODAsync()
        {
            DateTime dataBase = DateTime.Now.Date.AddDays(new Random().Next(0, 1000) * -1);
            string parameters = $"apod?api_key={_apiKey}&date={dataBase.ToString("yyyy-MM-dd")}";
            HttpResponseMessage response = await _httpClient.GetAsync(parameters);

            response.EnsureSuccessStatusCode();
            string content = response.Content.ReadAsStringAsync().Result;

            dynamic result = JsonConvert.DeserializeObject(content);

            ImageAPOD image = new ImageAPOD();
            image.Date = dataBase;
            image.Title = result.title;
            image.Description = result.explanation;
            image.Url = result.url;
            image.MediaType = result.media_type;
            image.Copyright = result.copyright;

            return image;
        }

        public string GetBaseAddress()
        {
            return _httpClient.BaseAddress.ToString();
        }
    }
}