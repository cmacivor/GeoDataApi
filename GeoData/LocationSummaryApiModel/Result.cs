using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.MapServerApiModel
{
    public class Result
    {
        public int layerId { get; set; }
        public string layerName { get; set; }
        public string displayFieldName { get; set; }
        public string value { get; set; }
        public Attributes attributes { get; set; }
    }


}
