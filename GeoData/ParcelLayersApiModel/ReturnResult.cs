using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.LocationSummaryApiModel
{
    public class MapServerReturnResult
    {
        public string CouncilDistrict { get; set; }

        public string CongressionalDistrict { get; set; }

        public string HouseDistrict { get; set; }

        public string SenateDistrict { get; set; }

        public string VoterPrecinctNumber { get; set; }

        public string Mailable { get; set; }

        public string BuildingNumber { get; set; }

        public string StreetDirection { get; set; }

        public string StreetName { get; set; }

        public string UnitType { get; set; }

        public string UnitValue { get; set; }

        public string ZipCode { get; set; }

        public string AddressId { get; set; }

        public HttpStatusCode HttpResponseStatusCode { get; set; }

        public string ErrorMessage { get; set; }

    }
}
