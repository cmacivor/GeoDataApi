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

        public MapServerServiceClient(string commonBoundariesApiUrl)
        {
            _httpClient = new HttpClient();
            _addressesHttpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(commonBoundariesApiUrl);
        }

        public async Task<MapServerReturnResult> Get(double x, double y, string streetAddress)
        {
            //this is the URL as it's currently in Election Admin            
            string parameters = string.Format(CommonBoundariesApiUrlParameters, x, y);

            var request = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress + parameters);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var serializedParcelLayers = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoData.MapServerApiModel.LocationSummary>(result);

                if (serializedParcelLayers == null)
                {
                    //do some friendly error handling and logging here
                    return new MapServerReturnResult();
                }

                //map the result to ReturnResult class for council district, etc
                var returnResult = MapVoterInformationToReturnResult(serializedParcelLayers);

                string formattedAddress = string.Format("\'{0}\'", streetAddress);

                string mailableUrl = string.Format(MapServerApiUrl, formattedAddress);

                var mapServerRequest = new HttpRequestMessage(HttpMethod.Get, mailableUrl);

                var mapServerResponse = await _addressesHttpClient.SendAsync(mapServerRequest);

                var serializedMapServerResponse = await mapServerResponse.Content.ReadAsStringAsync();

                var mapServerAddresses = Newtonsoft.Json.JsonConvert.DeserializeObject<GeoData.MapServer.MapServerAddresses>(serializedMapServerResponse);

                //TODO check for null here

                MapAddressInformationToReturnResult(returnResult, mapServerAddresses);

                return returnResult;
            }

            return null;
        }

        private static void MapAddressInformationToReturnResult(MapServerReturnResult returnResult, MapServer.MapServerAddresses mapServerAddresses)
        {
            returnResult.Mailable = mapServerAddresses.features.FirstOrDefault().attributes.Mailable;

            returnResult.BuildingNumber = mapServerAddresses.features.FirstOrDefault().attributes.BuildingNumber;

            returnResult.UnitType = mapServerAddresses.features.FirstOrDefault().attributes.UnitType;

            returnResult.StreetDirection = mapServerAddresses.features.FirstOrDefault().attributes.StreetDirection;

            returnResult.StreetName = mapServerAddresses.features.FirstOrDefault().attributes.StreetName;

            returnResult.UnitValue = mapServerAddresses.features.FirstOrDefault().attributes.UnitValue;

            returnResult.ZipCode = mapServerAddresses.features.FirstOrDefault().attributes.ZipCode;
        }

        private static MapServerReturnResult MapVoterInformationToReturnResult(MapServerApiModel.LocationSummary serializedParcelLayers)
        {
            return new MapServerReturnResult
            {
                CouncilDistrict = serializedParcelLayers.results.FirstOrDefault(council => council.layerName == "Council Districts").attributes.Name,
                CongressionalDistrict = serializedParcelLayers.results.FirstOrDefault(council => council.layerName == "Federal Congressional Districts").attributes.Name,
                HouseDistrict = serializedParcelLayers.results.FirstOrDefault(district => district.layerName == "State House Districts").attributes.Name,
                SenateDistrict = serializedParcelLayers.results.FirstOrDefault(senate => senate.layerName == "State Senate Districts").attributes.Name,
                VoterPrecinctNumber = serializedParcelLayers.results.FirstOrDefault(precinct => precinct.layerName == "Voter Precincts").attributes.Name,

            };
        }

        //public static string SurroundWithSingleQuotes(this string text)
        //{
        //    return SurroundWith(text, "\'");
        //}

        //public static string SurroundWith(this string text, string ends)
        //{
        //    return ends + text + ends;
        //}

    }
}
