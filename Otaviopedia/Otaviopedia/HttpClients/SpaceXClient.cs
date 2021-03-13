using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Otaviopedia.Models.SPACE_X;

namespace Otaviopedia.HttpClients
{
    public class SpaceXClient : IHttpCustomClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SpaceXClient(HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("SPACEX_OpenAPIs")["BaseURL_v4"]);
        }

        public string GetBaseAddress()
        {
            return _httpClient.BaseAddress.ToString();
        }

        public async Task<List<Crew>> GetCrew(){
            HttpResponseMessage response = await _httpClient.GetAsync("crew");
            return await response.Content.ReadAsAsync<List<Crew>>();
        }
    }
}