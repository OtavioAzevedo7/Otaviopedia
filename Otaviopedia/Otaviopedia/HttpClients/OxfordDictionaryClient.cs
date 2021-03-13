using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Otaviopedia.Models.OxfordDictionary;

namespace Otaviopedia.HttpClients
{
    public class OxfordDictionaryClient : IHttpCustomClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OxfordDictionaryClient(HttpClient httpClient,IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_configuration.GetSection("Oxford_OpenAPIs")["BaseURL_Dictionaries"]);
            _httpClient.DefaultRequestHeaders.Add("app_id", _configuration.GetSection("Oxford_OpenAPIs")["AppId"]);
            _httpClient.DefaultRequestHeaders.Add("app_key", _configuration.GetSection("Oxford_OpenAPIs")["AppKey"]);
        }

        public async Task<Dictionary> GetDefinitionByWord(string word)
        {
            try
            {
                string parameters = $"entries/en-us/{word}";
                HttpResponseMessage response = await _httpClient.GetAsync(parameters);
                response.EnsureSuccessStatusCode();

                string content = response.Content.ReadAsStringAsync().Result;
                dynamic result = JsonConvert.DeserializeObject(content);
                
                Dictionary dict = new Dictionary();
                dict.Word = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.id.ToString().ToLower());
                dict.Provider = result.metadata.provider.ToString();
                dict.Etymologies = result.results[0].lexicalEntries[0].entries[0].etymologies.ToString();
                dict.Definition = result.results[0].lexicalEntries[0].entries[0].senses[0].definitions.ToString();
                dict.ShortDefinition = result.results[0].lexicalEntries[0].entries[0].senses[0].shortDefinitions.ToString();
                dict.AudioFile = result.results[0].lexicalEntries[0].entries[0].pronunciations[1].audioFile ?? "";
                dict.PhoneticSpelling = result.results[0].lexicalEntries[0].entries[0].pronunciations[1].phoneticSpelling.ToString();

                return dict;
            }
            catch
            {
                return null;
            }
        }

        public string GetBaseAddress()
        {
            return _httpClient.BaseAddress.ToString();
        }
    }
}