using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.SuggestSubUnitApiModel
{
    public class Candidate
    {
        public string address { get; set; }
        public Location location { get; set; }
        public string score { get; set; }
        public Attributes attributes { get; set; }
    }
}
