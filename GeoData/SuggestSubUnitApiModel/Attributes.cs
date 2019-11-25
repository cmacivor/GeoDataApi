using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.SuggestSubUnitApiModel
{
    public class Attributes
    {
        public string Score { get; set; }
        public string Match_addr { get; set; }
        public string House { get; set; }
        public string PreDir { get; set; }
        public string PreType { get; set; }
        public string StreetName { get; set; }
        public string SufType { get; set; }
        public string SufDir { get; set; }
        public string UnitType { get; set; }
        public string UnitNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string Ref_ID { get; set; }
        public string User_fld { get; set; }
        public string Addr_type { get; set; }
    }
}
