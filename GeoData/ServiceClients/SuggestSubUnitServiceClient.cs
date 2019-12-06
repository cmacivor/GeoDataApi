using GeoData.SuggestSubUnitApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.Services
{
    public class SuggestSubUnitServiceClient
    {
        public string SuggestSubUnitApiUrl { get; set; }

        private static HttpClient _suggestSubUnitHttpClient;

        public SuggestSubUnitServiceClient(string suggestSubUnitApiUrl)
        {
            _suggestSubUnitHttpClient = new HttpClient();
            SuggestSubUnitApiUrl = suggestSubUnitApiUrl;
        }

        public async Task<SuggestSubUnitReturnResult> GetAsync(string encodedStreet)
        {
            string url = string.Format(SuggestSubUnitApiUrl, encodedStreet);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _suggestSubUnitHttpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new SuggestSubUnitReturnResult();
            }

            var result = await response.Content.ReadAsStringAsync();

            var serializedResult = Newtonsoft.Json.JsonConvert.DeserializeObject<SuggestSubUnitReturnResult>(result);

            serializedResult.HttpResponseStatusCode = response.StatusCode;

            if (serializedResult != null)
            {
                return serializedResult;
            }
            
            return null;
        }
    }
}
