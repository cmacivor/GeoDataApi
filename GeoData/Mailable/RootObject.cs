using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.Mailable
{
    public class RootObject
    {
        public string displayFieldName { get; set; }
        public FieldAliases fieldAliases { get; set; }
        public List<Field> fields { get; set; }
        public List<Feature> features { get; set; }
    }
}
