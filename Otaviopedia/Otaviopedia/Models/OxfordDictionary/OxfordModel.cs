using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Otaviopedia.Models.OxfordDictionary
{
    public class OxfordModel
    {
        public Dictionary GetDefinitionByWord(string word)
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("app_id", "b2324805");
                client.DefaultRequestHeaders.Add("app_key", "77208af2af241713833472394d1636b7");

                string baseURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Oxford_OpenAPIs")["BaseURL_Dictionaries"];
                
                baseURL += $"entries/en-us/{word}";

                HttpResponseMessage response = client.GetAsync(baseURL).Result;
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                dynamic result = JsonConvert.DeserializeObject(content);

                Dictionary dict = new Dictionary();
                
                dict.Word = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.id.ToString().ToLower());
                dict.Provider = result.metadata.provider.ToString();
                dict.Etymologies = result.results[0].lexicalEntries[0].entries[0].etymologies.ToString();
                dict.Definition = result.results[0].lexicalEntries[0].entries[0].senses[0].definitions.ToString();
                dict.ShortDefinition = result.results[0].lexicalEntries[0].entries[0].senses[0].shortDefinitions.ToString();
                //dict.AudioFile = result.results[0].lexicalEntries[0].entries[0].pronunciations[1].audioFile.ToString();
                dict.PhoneticSpelling = result.results[0].lexicalEntries[0].entries[0].pronunciations[1].phoneticSpelling.ToString();

                dict.SourceAPI = "https://developer.oxforddictionaries.com/";

                return dict;
            }
        }
    }
}
