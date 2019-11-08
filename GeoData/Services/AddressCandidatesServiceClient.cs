using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GeoData.AddressCandidates;

namespace GeoData.Services
{
    //TODO: rename this to "AddressCandidatesServiceClient"
    public class AddressCandidatesServiceClient
    {
        private static HttpClient _httpClient;

        //public string EncodedAddress { get; set; }

        public string AddressCandidatesApiUrl { get; set; }

        public AddressCandidatesServiceClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<AddressCandidates.RootObject> GetAsync(string encodedStreet)
        {
            string url = string.Format(AddressCandidatesApiUrl, encodedStreet);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var serialized = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoData.AddressCandidates.RootObject>(result);

                return serialized;
            }

            return null;
        }
    }
}
