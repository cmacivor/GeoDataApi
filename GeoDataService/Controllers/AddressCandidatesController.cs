using GeoData.AddressCandidates;
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
  
    public class AddressCandidatesController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Get(string street)
        {
            if (string.IsNullOrEmpty(street))
            {
                return BadRequest();
            }

            int minCharacterCount = Convert.ToInt32(ConfigurationManager.AppSettings["MinimumCharacterCount"]);

            if (street.Count() <= minCharacterCount)
            {
                return Ok(new AddressCandidatesReturnResult());
            }

            string encodedAddress = System.Web.HttpUtility.UrlEncode(street);

            var service = new AddressCandidatesServiceClient();

            service.AddressCandidatesApiUrl = ConfigurationManager.AppSettings["AddressCandidatesApiUrl"];

            HttpStatusCode[] httpStatusCodesWorthRetrying = {
               HttpStatusCode.RequestTimeout, // 408
               HttpStatusCode.InternalServerError, // 500
               HttpStatusCode.BadGateway, // 502
               HttpStatusCode.ServiceUnavailable, // 503
               HttpStatusCode.GatewayTimeout // 504
            };

            var retryPolicy = Policy
                .HandleResult<AddressCandidatesReturnResult>(r => httpStatusCodesWorthRetrying.Contains(r.HttpResponseStatusCode))
                .OrResult(r => r == null)
                .RetryAsync(3);

            var results = await retryPolicy.ExecuteAsync(async () => await GetResultFromFindAddressCandidates(encodedAddress, service));
           
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

        private static async Task<AddressCandidatesReturnResult> GetResultFromFindAddressCandidates(string encodedAddress, AddressCandidatesServiceClient service)
        {
            return await service.GetAsync(encodedAddress);
        }
    }
}
