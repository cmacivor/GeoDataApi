using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.SuggestSubUnitApiModel
{
    public class SuggestSubUnitReturnResult
    {
        public SpatialReference spatialReference { get; set; }
        public List<Candidate> candidates { get; set; }

        public HttpStatusCode HttpResponseStatusCode { get; set; }
    }
}
