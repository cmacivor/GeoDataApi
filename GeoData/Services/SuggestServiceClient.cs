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

        public async Task<SuggestionReturnResult> Get(string encodedStreet)
        {
            string url = string.Format(SuggestApiUrl, encodedStreet);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _suggestServiceHttpClient.SendAsync(request);


            return null;
        }

    }
}
