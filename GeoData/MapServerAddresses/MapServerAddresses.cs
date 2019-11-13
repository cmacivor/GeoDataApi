using System.Collections.Generic;

namespace GeoData.MapServer
{
    public class MapServerAddresses
    {
        public string displayFieldName { get; set; }
        public FieldAliases fieldAliases { get; set; }
        public List<Field> fields { get; set; }
        public List<Feature> features { get; set; }
    }
}
