using GeoData.SuggestApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.Services
{
    public class SuggestServiceClient
    {
        private static HttpClient _suggestServiceHttpClient;

        public string SuggestApiUrl { get; set; }

        public SuggestServiceClient(string suggestApiUrl)
        {
            _suggestServiceHttpClient = new HttpClient();
            SuggestApiUrl = suggestApiUrl;
        }

        public async Task<SuggestionReturnResult> GetAsync(string encodedStreet)
        {
            string url = string.Format(SuggestApiUrl, encodedStreet);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _suggestServiceHttpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new SuggestionReturnResult { HttpResponseStatusCode = response.StatusCode };
            }

            var result = await response.Content.ReadAsStringAsync();

            var serialized = Newtonsoft.Json.JsonConvert.DeserializeObject<SuggestionReturnResult>(result);

            serialized.HttpResponseStatusCode = response.StatusCode;

            if (serialized != null)
            {
                return serialized;
            }

            return null;
        }

    }
}
