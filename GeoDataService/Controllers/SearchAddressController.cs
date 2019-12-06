using GeoData.AddressCandidates;
using GeoData.ParcelAddressWithSubaddressDapper;
using GeoData.SuggestSubUnitApiModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeoDataService.Controllers
{
    public class SearchAddressController : ApiController
    {
        public async Task<IHttpActionResult> Get(string street)
        {            
            if (string.IsNullOrEmpty(street))
            {
                return BadRequest();
            }

            int minCharacterCount = Convert.ToInt32(ConfigurationManager.AppSettings["MinimumCharacterCount"]);

            if (street.Count() <= minCharacterCount)
            {
                return Ok(new AddressCandidatesReturnResult());
            }

            var query = new ParcelAddressWithSubAddressQuery();

            var result = await query.Get(street);

            var candidates = MapQueryResultToAddressCandidateReturnResult(result);

            return Ok(candidates);
        }

        private  AddressCandidatesReturnResult MapQueryResultToAddressCandidateReturnResult(List<GeoData.ParcelAddressWithSubaddressDapperModel.ParcelAddressWithSubAddressModel> result)
        {
            //map to AddressCandidates return result
            var candidates = new AddressCandidatesReturnResult
            {
                candidates = new List<GeoData.AddressCandidates.Candidate>(),
                spatialReference = new GeoData.AddressCandidates.SpatialReference(),

            };

            foreach (var row in result)
            {
                candidates.candidates.Add(new GeoData.AddressCandidates.Candidate
                {
                    address = row.AddressLabel,
                    attributes = new GeoData.AddressCandidates.Attributes
                    {
                        StAddr = row.AddressLabel,
                        SubAddType = row.UnitType,
                        SubAddUnit = row.UnitValue,
                        ZIP = row.ZipCode,
                        Ref_ID = row.AddressId,
                        CouncilDistrict = row.CouncilDistrict
                    },

                    location = new GeoData.AddressCandidates.Location
                    {
                        x = Convert.ToDouble(row.StatePlaneX),
                        y = Convert.ToDouble(row.StatePlaneY)
                    }
                });
            }

            return candidates;
        }
    }
}
