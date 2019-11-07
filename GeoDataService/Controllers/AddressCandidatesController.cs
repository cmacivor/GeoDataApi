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
        //make this static, according to this article: https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient _httpClient;

        public AddressCandidatesController()
        {
            //_httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://gisdev.richmondgov.com/arcgis/rest/services/Geocode/RichmondAddress/GeocodeServer/findAddressCandidates");
        }


        public async Task<IHttpActionResult> Get(string street)
        {          
            string encodedAddress = System.Web.HttpUtility.UrlEncode(street);
            
            var service = new AddressCandidatesService();
            service.AddressCandidatesApiUrl = ConfigurationManager.AppSettings["AddressCandidatesApiUrl"];
            var results = await service.GetAsync(encodedAddress);

            return Ok(results);
            //return null;
        }
    }
}
