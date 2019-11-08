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
  
    public class AddressCandidatesController : ApiController
    {
        public async Task<IHttpActionResult> Get(string street)
        {      
            if (string.IsNullOrEmpty(street))
            {
                return BadRequest();
            }

            //TODO: send bad request if less than 2-3 characters
            if (street.Count() <= 5)
            {
                return BadRequest();
            }

            string encodedAddress = System.Web.HttpUtility.UrlEncode(street);
            
            var service = new AddressCandidatesServiceClient();

            service.AddressCandidatesApiUrl = ConfigurationManager.AppSettings["AddressCandidatesApiUrl"];

            var results = await service.GetAsync(encodedAddress);

            if (results == null)
            {
                //TODO: log the address that causes this
                return NotFound();
            }

            if (results.HttpResponseStatusCode != HttpStatusCode.OK)
            {
                throw new HttpResponseException(results.HttpResponseStatusCode);
            }
        
            return Ok(results);            
        }
    }
}
