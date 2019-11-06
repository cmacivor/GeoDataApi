using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.AddressCandidates
{
    public class RootObject
    {
        public SpatialReference spatialReference { get; set; }
        public List<Candidate> candidates { get; set; }
    }
}
