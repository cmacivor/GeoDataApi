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
        public async Task<IHttpActionResult> Get(double x, double y, string streetAddress)
        {
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
                var responseMessage = Request.CreateResponse(result.HttpResponseStatusCode);
                responseMessage.Content = new StringContent(result.ErrorMessage);
                throw new HttpResponseException(responseMessage);
            }

            return Ok(result);
        }
    }
}
