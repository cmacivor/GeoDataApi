using GeoData.AddressCandidates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData.Services
{
    public class SuggestAddressCandidatesServiceClient
    {
        public string SuggestApiUrl { get; set; }

        public SuggestAddressCandidatesServiceClient(string suggestApiUrl)
        {
            SuggestApiUrl = suggestApiUrl;
        }

        public async Task<AddressCandidatesReturnResult> GetAsync(string encodedStreet)
        {
            //1. first get the suggestions
            var suggestServiceClient = new SuggestServiceClient(SuggestApiUrl);

            var suggestionResults = await suggestServiceClient.GetAsync(encodedStreet);

            foreach (var result in suggestionResults.suggestions)
            {
                //need to call the addressCandidatesClient with both text and magicKey
            }

            //2. for each suggestion, create a task request to address candidates and await on them

            //3. map the address candidates results and return them

            return null;
        }
    }
}
