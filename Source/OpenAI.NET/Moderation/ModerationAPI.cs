using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAINET.Moderation.DTO;

namespace OpenAINET.Moderation
{
    public class ModerationAPI : BaseOpenAIAPI
    {
        public const string API_PATH = "https://api.openai.com/v1/moderations";
        
        public ModerationAPI(string apiKey) : base(apiKey)
        {
        }

        public async Task<(string Model, List<ModerationAPIResults> Results)> GetResultsAsync(
            string input,
            TimeSpan? timeout = null)
        {
            var request = new ModerationAPIRequest()
            {
                input = input,
            };
            
            var response = await PostRequestAsync<ModerationAPIResponse>(API_PATH, request, new Dictionary<string, string>()
            {
                {"Authorization", $"Bearer {APIKey}"},
            },
            timeout);

            return (response.model, response.results);
        }
    }
}