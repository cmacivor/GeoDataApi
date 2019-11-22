using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.SuggestApiModel
{
    public class Suggestion
    {
        public string text { get; set; }
        public string magicKey { get; set; }
        public bool isCollection { get; set; }
    }
}
