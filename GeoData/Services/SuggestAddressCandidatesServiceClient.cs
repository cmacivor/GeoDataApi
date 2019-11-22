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

        public string AddressCandidatesWithMagicKeyApiUrl { get; set; }

        public SuggestAddressCandidatesServiceClient(string suggestApiUrl, string addressCandidatesMagicKeyApiUrl)
        {
            SuggestApiUrl = suggestApiUrl;
            AddressCandidatesWithMagicKeyApiUrl = addressCandidatesMagicKeyApiUrl;
        }

        public async Task<AddressCandidatesReturnResult> GetAsync(string encodedStreet)
        {
            //1. first get the suggestions
            string url = string.Format(SuggestApiUrl, encodedStreet);
            var suggestServiceClient = new SuggestServiceClient(SuggestApiUrl);

            var suggestionResults = await suggestServiceClient.GetAsync(encodedStreet);

            var addressCandidateTasks = new List<Task<AddressCandidatesReturnResult>>();

            //2. for each suggestion, create a task request to address candidates and await on them
            foreach (var result in suggestionResults.suggestions)
            {
                //need to call the addressCandidatesClient with both text and magicKey
                var addressCandidatesClient = new AddressCandidatesServiceClient();
                addressCandidatesClient.AddressCandidatesWithMagicKeyApiUrl = AddressCandidatesWithMagicKeyApiUrl;
                Task<AddressCandidatesReturnResult> addressCandidate = addressCandidatesClient.GetAsync(result.text, result.magicKey);

                addressCandidateTasks.Add(addressCandidate);
            }

            var returnResult = new AddressCandidatesReturnResult();
            returnResult.candidates = new List<Candidate>();

            //see this for more info on what this does: https://stackoverflow.com/questions/19431494/how-to-use-await-in-a-loop
            foreach (var task in await Task.WhenAll(addressCandidateTasks))
            {
                if (task.candidates != null && task.candidates.Count() > 0)
                {
                    //3. map the address candidates results and return them
                    foreach (var candidate in task.candidates)
                    {
                        returnResult.candidates.Add(candidate);
                    }
                }
            }

            return returnResult;
        }
    }
}
