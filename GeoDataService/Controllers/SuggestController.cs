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
    public class SuggestController : ApiController
    {
        public async Task<IHttpActionResult> Get(string street)
        {
            if (string.IsNullOrEmpty(street))
            {
                return BadRequest();
            }

            //TODO: what if the address were something like "3 Fry"? Maybe make this configurable?
            //TODO: consider just returning an empty AddressCandidatesReturnResult instead of bad request. 
            if (street.Count() <= 5)
            {
                return BadRequest();
            }

            string encodedAddress = System.Web.HttpUtility.UrlEncode(street);

            var suggestUrl = ConfigurationManager.AppSettings["SuggestApiUrl"];
            var addressCandidatesMagicApiUrl = ConfigurationManager.AppSettings["AddressCandidatesWithMagicKeyApiUrl"];

            var serviceClient = new SuggestAddressCandidatesServiceClient(suggestUrl, addressCandidatesMagicApiUrl);

            var result = await serviceClient.GetAsync(encodedAddress);

            if (result == null)
            {       
                return NotFound();
            }

            return Ok(result);
        }
    }
}
