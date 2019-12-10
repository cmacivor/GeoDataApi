using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.AddressCandidates
{
    public class Attributes
    {
        public double Score { get; set; }
        public string Match_addr { get; set; }
        public string Addr_type { get; set; }
        public string AddNum { get; set; }
        public string Side { get; set; }
        public string StPreDir { get; set; }
        public string StPreType { get; set; }
        public string StName { get; set; }
        public string StType { get; set; }
        public string StDir { get; set; }
        public string SubAddType { get; set; }
        public string SubAddUnit { get; set; }
        public string StAddr { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string StateAbbr { get; set; }
        public string ZIP { get; set; }
        public string ZIP4 { get; set; }
        public string Country { get; set; }
        public string LangCode { get; set; }
        public int Distance { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string DisplayX { get; set; }
        public string DisplayY { get; set; }
        public string Xmin { get; set; }
        public string Xmax { get; set; }
        public string Ymin { get; set; }
        public string Ymax { get; set; }

        public string Ref_ID { get; set; }

        public string CouncilDistrict { get; set; }

        private string addressLabel;

        public string AddressLabel
        {
            //get;
            get
            {

                addressLabel = StAddr;

                if (!string.IsNullOrEmpty(SubAddType))
                {
                    addressLabel = addressLabel + " " + SubAddType;
                }
                if (!string.IsNullOrEmpty(SubAddUnit))
                {
                    addressLabel = addressLabel + " " + SubAddUnit;
                }

                return addressLabel;
            }
        }

    }
}
