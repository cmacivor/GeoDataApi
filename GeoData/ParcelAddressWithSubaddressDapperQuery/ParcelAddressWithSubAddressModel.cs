using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.ParcelAddressWithSubaddressDapperModel
{
    public class ParcelAddressWithSubAddressModel
    {
        public string AddressId { get; set; }

        public string AddressLabel { get; set; }

        public string BuildingNumber { get; set; }

        public string StreetDirection { get; set; }

        public string StreetName { get; set; }

        public string StreetType { get; set; }

        public string Extension { get; set; }

        public string UnitType { get; set; }

        public string UnitValue { get; set; }

        public string ZipCode { get; set; }

        public string Mailable { get; set; }

        public string StatePlaneX { get; set; }

        public string StatePlaneY { get; set; }

        public string CouncilDistrict { get; set; }
    }
}
