using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.SuggestApiModel
{
    public class SuggestionReturnResult
    {
        public List<Suggestion> suggestions { get; set; }

        public HttpStatusCode HttpResponseStatusCode { get; set; }
    }
}
