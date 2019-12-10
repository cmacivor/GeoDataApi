using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GeoData.AddressCandidates;

namespace GeoData.Services
{
    public class AddressCandidatesServiceClient
    {
        //making this static per guidance from these articles:
        //https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        //https://stackoverflow.com/questions/40228146/httpclient-singleton-implementation-in-asp-net-mvc
        private static HttpClient _httpClient;

        public string AddressCandidatesApiUrl { get; set; }

        public string AddressCandidatesWithMagicKeyApiUrl { get; set; }

        public AddressCandidatesServiceClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<AddressCandidatesReturnResult> GetAsync(string encodedStreet, string magicKey = "")
        {
            string url = "";
            if (string.IsNullOrEmpty(magicKey))
            {
                url = string.Format(AddressCandidatesApiUrl, encodedStreet);
            }
            else
            {
                url = string.Format(AddressCandidatesWithMagicKeyApiUrl, encodedStreet, magicKey);   
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var serialized = Newtonsoft.Json.JsonConvert.DeserializeObject<AddressCandidatesReturnResult>(result);

                serialized.HttpResponseStatusCode = response.StatusCode;
                if (serialized != null)
                {
                    return serialized;
                }
            }
            else
            {
                return new AddressCandidatesReturnResult { HttpResponseStatusCode = response.StatusCode };
            }

            return null;
        }
    }
}
