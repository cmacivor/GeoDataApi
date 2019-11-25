using GeoData.AddressCandidates;
using GeoData.Services;
using GeoData.SuggestSubUnitApiModel;
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

            int minCharacterCount = Convert.ToInt32(ConfigurationManager.AppSettings["MinimumCharacterCount"]);

            if (street.Count() <= minCharacterCount)
            {
                return Ok(new SuggestSubUnitReturnResult());
            }

            string encodedAddress = System.Web.HttpUtility.UrlEncode(street);

            var suggestUrl = ConfigurationManager.AppSettings["SuggestSubUnitApiUrl"];

            var client = new SuggestSubUnitServiceClient(suggestUrl);

            var result = await client.GetAsync(encodedAddress);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
            
        }
    }
}
