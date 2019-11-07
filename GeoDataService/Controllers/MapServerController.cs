using GeoData.LocationSummaryApiModel;
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


        public async Task<IHttpActionResult> Get(double x, double y)
        {
            //TODO: check to ensure x and y are valid values

            //this is the URL as it's currently in Election Admin
            string parameters = $"?geometryType=esriGeometryPoint&geometry={x}%2C{y}&sr=102747&layers=all%3A0%2C1%2C2%2C3%2C4%2C5%2C6%2C7%2C8%2C9%2C10%2C11%2C12%2C13%2C14%2C15%2C16%2C17%2C18%2C19%2C20%2C21%2C22%2C23%2C24%2C25%2C26%2C27%2C28%2C29%2C30%2C31%2C32%2C33%2C34%2C35%2C36%2C37%2C38%2C39&time=&layerTimeOptions=&layerdefs=&tolerance=1&mapExtent=11743500%2C3687943%2C11806063%2C3744740&imageDisplay=600%2C550%2C96&returnGeometry=false&maxAllowableOffset=&f=pjson";

            var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress + parameters);
           
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var serializedParcelLayers = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoData.MapServerApiModel.LocationSummary>(result);

                //map the result to ReturnResult class for council district, etc
                var returnResult = new ReturnResult();
 
                //var parcelLayer = serialized.results.Where(layername => layername.layerName == "Parcel");

                var councilDistrictLayer = serializedParcelLayers.results.FirstOrDefault(council => council.layerName == "Council Districts");

                returnResult.CouncilDistrict = councilDistrictLayer.attributes.Name;

                var congressionalDistrictLayer = serializedParcelLayers.results.FirstOrDefault(council => council.layerName == "Federal Congressional Districts");

                returnResult.CongressionalDistrict = congressionalDistrictLayer.attributes.Name;

                var stateHouseDistrictLayer = serializedParcelLayers.results.FirstOrDefault(district => district.layerName == "State House Districts");

                returnResult.HouseDistrict = stateHouseDistrictLayer.attributes.Name;

                var senateDistrictLayer = serializedParcelLayers.results.FirstOrDefault(senate => senate.layerName == "State Senate Districts");

                returnResult.SenateDistrict = senateDistrictLayer.attributes.Name;

                var voterPrecinctLayer = serializedParcelLayers.results.FirstOrDefault(precinct => precinct.layerName == "Voter Precincts");
          
                returnResult.VoterPrecinctNumber = voterPrecinctLayer.attributes.Name;

                
                //string test = "\'900 E Broad St'";
                string test = "\'900 E Broad St'";

                //make second call to get the Mailable field
                var mailableUrl = $"https://gisdev.richmondgov.com/arcgis/rest/services/StatePlane4502/Addresses/MapServer/0/query?where=AddressLabel={test}&outFields=*&returnGeometry=false&returnIdsOnly=false&f=json";

                var mailableRequest = new HttpRequestMessage(HttpMethod.Get, mailableUrl)
                {

                };

                var mailableResponse = await _addressesHttpClient.SendAsync(mailableRequest);

                var serializedMailable = await mailableResponse.Content.ReadAsStringAsync();

                var mailableRootObject = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoData.Mailable.RootObject>(serializedMailable);

                //TODO check for null here

                returnResult.Mailable = mailableRootObject.features.FirstOrDefault().attributes.Mailable;

                //TODO; add BuildingNumber, UnitType, StreetDirection, StreetName, UnitValue, ZipCode
                returnResult.BuildingNumber = mailableRootObject.features.FirstOrDefault().attributes.BuildingNumber;

                returnResult.UnitType = mailableRootObject.features.FirstOrDefault().attributes.UnitType;

                returnResult.StreetDirection = mailableRootObject.features.FirstOrDefault().attributes.StreetDirection;

                returnResult.StreetName = mailableRootObject.features.FirstOrDefault().attributes.StreetName;

                returnResult.UnitValue = mailableRootObject.features.FirstOrDefault().attributes.UnitValue;

                returnResult.ZipCode = mailableRootObject.features.FirstOrDefault().attributes.ZipCode;

                return Ok(returnResult);
            }

            return null;
        }
    }
}
