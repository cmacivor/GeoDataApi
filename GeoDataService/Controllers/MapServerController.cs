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
        public async Task<IHttpActionResult> Get(double x, double y, string streetAddress)
        {
            if (string.IsNullOrEmpty(streetAddress))
            {
                return BadRequest();
            }

            var mapServerClient = new MapServerServiceClient(ConfigurationManager.AppSettings["CommonBoundariesApiUrl"]);

            mapServerClient.MapServerApiUrl = ConfigurationManager.AppSettings["MapServerApiUrl"];

            mapServerClient.CommonBoundariesApiUrlParameters = ConfigurationManager.AppSettings["CommonBoundariesApiParameters"];

            //fallback policy
            //var policy = Policy<Task<MapServerReturnResult>>
            //    .Handle<Exception>()                
            //    .Fallback<Task<MapServerReturnResult>>(() => GetFromDatabase(streetAddress));

            //Handle both exceptions and return values in one policy
            HttpStatusCode[] httpStatusCodesWorthRetrying = {
               HttpStatusCode.RequestTimeout, // 408
               HttpStatusCode.InternalServerError, // 500
               HttpStatusCode.BadGateway, // 502
               HttpStatusCode.ServiceUnavailable, // 503
               HttpStatusCode.GatewayTimeout // 504
            };


            //these two at least work
            //var fallbackPolicy = Policy.Handle<Exception>()
            //    .FallbackAsync(async (cancellationToken) => await GetFromDatabase(streetAddress));

            //var retryPolicy = Policy
            ////.Handle<Exception>()
            //.HandleResult<Task<MapServerReturnResult>>(r => httpStatusCodesWorthRetrying.Contains(r.Result.HttpResponseStatusCode))
            //.OrResult(r => r.Result == null)
            //.RetryAsync(1);



            //var result = await fallbackPolicy.ExecuteAsync(async () => await GetResultFromLocatorApi(x, y, streetAddress, mapServerClient))
            //var result = await fallbackPolicy.ExecuteAsync(async () => await GetResultFromLocatorApi(x, y, streetAddress, mapServerClient)).

            //var result = fallbackPolicy.WrapAsync(retryPolicy).ExecuteAsync(async () => await GetResultFromLocatorApi(x, y, streetAddress, mapServerClient);

            //var result = await fallbackPolicy.WrapAsync(retryPolicy)
            //    //.ExecuteAsync(async () => await GetResultFromLocatorApi(x, y, streetAddress, mapServerClient)).ConfigureAwait(false)
            //    .ExecuteAsync(() =>  GetResultFromLocatorApi(x, y, streetAddress, mapServerClient) );



            //var policyWrap = Policy.Wrap(retryPolicy, fallbackPolicy);



            //var result = await Policy
            //    .Handle<Exception>()
            //    .OrResult<Task<MapServerReturnResult>>(r => httpStatusCodesWorthRetrying.Contains(r.Result.HttpResponseStatusCode))
            //    .Fallback<Task<MapServerReturnResult>>(() => GetFromDatabase(streetAddress))
            //    .Execute(() => GetResultFromLocatorApi(x, y, streetAddress, mapServerClient));


            //var fallBackPolicy = await Policy<Task<MapServerReturnResult>>
            //    .Handle<Exception>()
            //    //.OrResult<Task<MapServerReturnResult>>(r => httpStatusCodesWorthRetrying.Contains(r.Result.HttpResponseStatusCode))
            //    .FallbackAsync<Task<MapServerReturnResult>>(ct =>
            //    {
            //        return GetFromDatabase(streetAddress).Result;
            //        //var result = await GetFromDatabase(streetAddress);
            //    });

            //var errorCodePolicy = Policy.Handle<Task<MapServerReturnResult>>(r => httpStatusCodesWorthRetrying.Contains(r.Result.HttpResponseStatusCode))

            //.FallbackAsync<Task<MapServerReturnResult>>(f => { GetFromDatabase(streetAddress) } )
            //.Execute(() => GetResultFromLocatorApi(x, y, streetAddress, mapServerClient));

            // var result = await GetResultFromLocatorApi(x, y, streetAddress, mapServerClient);
            //var result = await fallBackPolicy.ExecuteAsync(() => GetResultFromLocatorApi(x, y, streetAddress, mapServerClient));

            MapServerReturnResult result;

            //try
           // {
                var retryPolicy = Policy
                //.Handle<Exception>()
                  .HandleResult<MapServerReturnResult>(r => httpStatusCodesWorthRetrying.Contains(r.HttpResponseStatusCode))
                  .OrResult(r => r == null)
                  .RetryAsync(1);

            var apiResult = await retryPolicy.ExecuteAsync(async () => await GetResultFromLocatorApi(x, y, streetAddress, mapServerClient));

            if (apiResult != null && string.IsNullOrEmpty(apiResult.ErrorMessage))
            {
                result = apiResult;
            }
            else
            {
                result = await GetFromDatabase(streetAddress);
            }
        
            if (result == null || (result.HttpResponseStatusCode != 0 && result.HttpResponseStatusCode != HttpStatusCode.OK))
            {
                var responseMessage = Request.CreateResponse(HttpStatusCode.NotFound);
                responseMessage.Content = new StringContent("Unable to locate address " + streetAddress + " at coordinates x, y:" + x + " " + y);
                //Elmah will raise an exception
                throw new HttpResponseException(responseMessage);
            }

            //if (result.HttpResponseStatusCode != HttpStatusCode.OK)
            //{
            //    var responseMessage = Request.CreateResponse(result.HttpResponseStatusCode);
            //    responseMessage.Content = new StringContent(result.ErrorMessage);
            //    throw new HttpResponseException(responseMessage);
            //}

            return Ok(result);
        }

        private  async Task<MapServerReturnResult> GetResultFromLocatorApi(double x, double y, string streetAddress, MapServerServiceClient mapServerClient)
        {
            return await mapServerClient.Get(x, y, streetAddress);
        }

        public async Task<MapServerReturnResult> GetFromDatabase(string streetAddress)
        {
            var query = new GeoDataApiViewQuery();
            return await query.Get(streetAddress);
        }


    }
}
