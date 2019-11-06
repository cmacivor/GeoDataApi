using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.GeoDataApiModel
{
    public class Candidate
    {
        public string address { get; set; }
        public Location location { get; set; }
        public double score { get; set; }
        public Attributes attributes { get; set; }
        public Extent extent { get; set; }
    }
}
