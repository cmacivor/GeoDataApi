using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeoDataService.Controllers
{
    public class LocationSummaryController : ApiController
    {
        private static HttpClient _httpClient;

        public LocationSummaryController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://gisdev.richmondgov.com/arcgis/rest/services/StatePlane4502/CommonBoundaries/MapServer/identify");
        }


        public async Task<IHttpActionResult> Get()
        {
            string parameters = "?geometryType=esriGeometryPoint&geometry=11783277.5142022%2C3704036.45466257&sr=102747&layers=all%3A0%2C1%2C2%2C3%2C4%2C5%2C6%2C7%2C8%2C9%2C10%2C11%2C12%2C13%2C14%2C15%2C16%2C17%2C18%2C19%2C20%2C21%2C22%2C23%2C24%2C25%2C26%2C27%2C28%2C29%2C30%2C31%2C32%2C33%2C34%2C35%2C36%2C37%2C38%2C39&time=&layerTimeOptions=&layerdefs=&tolerance=1&mapExtent=11743500%2C3687943%2C11806063%2C3744740&imageDisplay=600%2C550%2C96&returnGeometry=false&maxAllowableOffset=&f=pjson";

            //string url = "https://gis.richmondgov.com/ArcGIS/rest/services/StatePlane4502/CommonBoundaries/MapServer/identify?geometryType=esriGeometryPoint&geometry=11783277.5142022%2C3704036.45466257&sr=102747&layers=all%3A0%2C1%2C2%2C3%2C4%2C5%2C6%2C7%2C8%2C9%2C10%2C11%2C12%2C13%2C14%2C15%2C16%2C17%2C18%2C19%2C20%2C21%2C22%2C23%2C24%2C25%2C26%2C27%2C28%2C29%2C30%2C31%2C32%2C33%2C34%2C35%2C36%2C37%2C38%2C39&time=&layerTimeOptions=&layerdefs=&tolerance=1&mapExtent=11743500%2C3687943%2C11806063%2C3744740&imageDisplay=600%2C550%2C96&returnGeometry=false&maxAllowableOffset=&f=pjson";

            var request = new HttpRequestMessage(HttpMethod.Get , _httpClient.BaseAddress + parameters)
            {
                //Content = new FormUrlEncodedContent(arguments)
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var serialized = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoData.LocationSummaryApiModel.LocationSummary>(result);

                return Ok(serialized);
            }

            return null;
        }
    }
}
