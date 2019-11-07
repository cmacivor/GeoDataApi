using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeoDataService.Controllers
{
  
    public class AddressCandidatesController : ApiController
    {
        //make this static, according to this article: https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient _httpClient;

        public AddressCandidatesController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://gisdev.richmondgov.com/arcgis/rest/services/Geocode/RichmondAddress/GeocodeServer/findAddressCandidates");
        }


        public async Task<IHttpActionResult> Get(string street)
        {
            //var arguments = new Dictionary<string, string>();
            //arguments.Add("Street", street);
            //arguments.Add("Out Fields", "*");
            //arguments.Add("f", "pjson"); //tells the GIS service to return JSON instead of HTML

            //var request = new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress)
            //{
            //    Content = new FormUrlEncodedContent(arguments)
            //};
            string encodedAddress = System.Web.HttpUtility.UrlEncode(street);

            string url = $"https://gisdev.richmondgov.com/arcgis/rest/services/Geocode/RichmondAddress/GeocodeServer/findAddressCandidates?Street=&ZIP=&Single+Line+Input={encodedAddress}&category=&outFields=*&maxLocations=&outSR=&searchExtent=&location=&distance=&magicKey=&f=pjson";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var serialized = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoData.AddressCandidates.RootObject>(result);

                return Ok(serialized);
            }

            return null;
        }
    }
}
