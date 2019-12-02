using GeoData.ParcelAddressWithSubaddressDapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeoDataService.Controllers
{
    public class SearchAddressController : ApiController
    {
        public async Task<IHttpActionResult> Get(string searchString)
        {
            var query = new ParcelAddressWithSubAddressQuery();

            var result = query.Get(searchString);

            return Ok(result);
        }

    }
}
