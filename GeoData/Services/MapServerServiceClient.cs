using GeoData.LocationSummaryApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.Services
{
    public class MapServerServiceClient
    {
        private static HttpClient _httpClient;

        private static HttpClient _addressesHttpClient;

        public string MapServerApiUrl { get; set; }

        public string CommonBoundariesApiUrlParameters { get; set; }

        private readonly string CouncilDistricts = "Council Districts";
        private readonly string FederalCongressionalDistricts = "Federal Congressional Districts";
        private readonly string StateSenateDistricts = "State Senate Districts";
        private readonly string StateHouseDistricts = "State House Districts";
        private readonly string VoterPrecincts = "Voter Precincts";


        public MapServerServiceClient(string commonBoundariesApiUrl)
        {
            _httpClient = new HttpClient();
            _addressesHttpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(commonBoundariesApiUrl);
        }

        public async Task<MapServerReturnResult> Get(double x, double y, string streetAddress)
        {
            var commonBoundariesApiResponse = await GetCommonBoundariesApiResponse(x, y);

            if (!commonBoundariesApiResponse.IsSuccessStatusCode)
            {           
                return new MapServerReturnResult
                {
                    HttpResponseStatusCode = commonBoundariesApiResponse.StatusCode,
                    ErrorMessage = "Unable to retrieve data from the Common Boundaries API."
                };
            }

            var result = await commonBoundariesApiResponse.Content.ReadAsStringAsync();

            var serializedParcelLayers = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoData.MapServerApiModel.PacelLayers>(result);
            
            //map the result to ReturnResult class for council district, etc
            var returnResult = MapVoterInformationToReturnResult(serializedParcelLayers);

            var mapServerResponse = await GetMapServerApiResponse(streetAddress);

            if (!mapServerResponse.IsSuccessStatusCode)
            {
                return new MapServerReturnResult
                {
                    HttpResponseStatusCode = mapServerResponse.StatusCode,
                    ErrorMessage = "Unable to retrieve data from the map server API."
                };
            }

            var serializedMapServerResponse = await mapServerResponse.Content.ReadAsStringAsync();

            var mapServerAddresses = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoData.MapServer.MapServerAddresses>(serializedMapServerResponse);        

            MapAddressInformationToReturnResult(returnResult, mapServerAddresses);

            returnResult.HttpResponseStatusCode = System.Net.HttpStatusCode.OK;

            return returnResult;
        }

        /// <summary>
        /// this is the second API call. It's for returning Address information like street direction, the Mailable attribute, etc.
        /// </summary>
        /// <param name="streetAddress"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetMapServerApiResponse(string streetAddress)
        {
            string formattedAddress = string.Format("\'{0}\'", streetAddress);

            string mailableUrl = string.Format(MapServerApiUrl, formattedAddress);

            var mapServerRequest = new HttpRequestMessage(HttpMethod.Get, mailableUrl);

            var mapServerResponse = await _addressesHttpClient.SendAsync(mapServerRequest);

            //var serializedMapServerResponse = await mapServerResponse.Content.ReadAsStringAsync();
            //return serializedMapServerResponse;
            return mapServerResponse;
        }

        /// <summary>
        /// this API call is for retrieving Voter information- Council District, State Senate District, et.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetCommonBoundariesApiResponse(double x, double y)
        {
            string parameters = string.Format(CommonBoundariesApiUrlParameters, x, y);

            var commonBoundariesApiRequest = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress + parameters);

            var commonBoundariesApiResponse = await _httpClient.SendAsync(commonBoundariesApiRequest);
            return commonBoundariesApiResponse;
        }

        private  void MapAddressInformationToReturnResult(MapServerReturnResult returnResult, MapServer.MapServerAddresses mapServerAddresses)
        {
            returnResult.Mailable = mapServerAddresses.features.FirstOrDefault().attributes.Mailable;

            returnResult.BuildingNumber = mapServerAddresses.features.FirstOrDefault().attributes.BuildingNumber;

            returnResult.UnitType = mapServerAddresses.features.FirstOrDefault().attributes.UnitType;

            returnResult.StreetDirection = mapServerAddresses.features.FirstOrDefault().attributes.StreetDirection;

            returnResult.StreetName = mapServerAddresses.features.FirstOrDefault().attributes.StreetName;

            returnResult.UnitValue = mapServerAddresses.features.FirstOrDefault().attributes.UnitValue;

            returnResult.ZipCode = mapServerAddresses.features.FirstOrDefault().attributes.ZipCode;
        }

        private  MapServerReturnResult MapVoterInformationToReturnResult(MapServerApiModel.PacelLayers serializedParcelLayers)
        {
            return new MapServerReturnResult
            {
                CouncilDistrict = serializedParcelLayers.results.FirstOrDefault(council => council.layerName == CouncilDistricts).attributes.Name,
                CongressionalDistrict = serializedParcelLayers.results.FirstOrDefault(council => council.layerName == FederalCongressionalDistricts).attributes.Name,
                HouseDistrict = serializedParcelLayers.results.FirstOrDefault(district => district.layerName == StateHouseDistricts).attributes.Name,
                SenateDistrict = serializedParcelLayers.results.FirstOrDefault(senate => senate.layerName == StateSenateDistricts).attributes.Name,
                VoterPrecinctNumber = serializedParcelLayers.results.FirstOrDefault(precinct => precinct.layerName == VoterPrecincts).attributes.Name,

            };
        }


    }
}
