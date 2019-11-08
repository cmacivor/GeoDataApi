using GeoData.LocationSummaryApiModel;
using GeoData.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace GeoDataService.Controllers
{
    public class MapServerController : ApiController
    {
        private static HttpClient _httpClient;

        private static HttpClient _addressesHttpClient;

        public MapServerController()
        {
            string commonBoundariesApiUrl = ConfigurationManager.AppSettings["CommonBoundariesApiUrl"].ToString();

            _httpClient = new HttpClient();
            _addressesHttpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(commonBoundariesApiUrl);
        }


        public async Task<IHttpActionResult> Get(double x, double y, string streetAddress)
        {
            //TODO: check to ensure x and y are valid values
            if (string.IsNullOrEmpty(streetAddress))
            {
                return BadRequest();
            }

            var mapServerClient = new MapServerServiceClient(ConfigurationManager.AppSettings["CommonBoundariesApiUrl"]);

            mapServerClient.MapServerApiUrl = ConfigurationManager.AppSettings["MapServerApiUrl"];

            mapServerClient.CommonBoundariesApiUrlParameters = ConfigurationManager.AppSettings["CommonBoundariesApiParameters"];

            var result = await mapServerClient.Get(x, y, streetAddress);

            if (result == null)
            {
                return NotFound();
            }

            if (result.HttpResponseStatusCode != HttpStatusCode.OK)
            {
                throw new HttpResponseException(result.HttpResponseStatusCode);
            }

            return Ok(result);
        }
    }
}
