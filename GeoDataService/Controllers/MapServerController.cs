using GeoData.GeoDataApiViewDapperQuery;
using GeoData.LocationSummaryApiModel;
using GeoData.Services;
using Polly;
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
        [HttpGet]
        public async Task<IHttpActionResult> Get(string streetAddress)
        {
            if (string.IsNullOrEmpty(streetAddress))
            {
                return BadRequest();
            }
            
            var query = new GeoDataApiViewQuery();
            var result = await query.Get(streetAddress);

            if (result == null)
            {
                var responseMessage = Request.CreateResponse(HttpStatusCode.NotFound);
                responseMessage.Content = new StringContent("Unable to locate address " + streetAddress);
                //Elmah will raise an exception
                throw new HttpResponseException(responseMessage);
            }

            return Ok(result);
        }

    }
}
