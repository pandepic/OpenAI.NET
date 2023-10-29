using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAINET.Embeddings.DTO;

namespace OpenAINET.Embeddings
{
    public class EmbeddingsAPI : BaseOpenAIAPI
    {
        public const string API_PATH = "https://api.openai.com/v1/embeddings";

        public readonly OpenAIModel Model;

        public EmbeddingsAPI(string apiKey) : base(apiKey)
        {
            Model = OpenAIModel.Models[OpenAIModelType.TextEmbeddingAda002];
        }

        public async Task<(List<EmbeddingResponseData> Data, EmbeddingsAPIResponseUsage Usage)> CreateEmbeddingVectorAsync(
            string input,
            TimeSpan? timeout = null)
        {
            var request = new EmbeddingsAPIRequest()
            {
                model = Model.ModelString,
                input = input,
            };

            var response = await PostRequestAsync<EmbeddingsAPIResponse>(API_PATH, request, new Dictionary<string, string>()
            {
                {"Authorization", $"Bearer {APIKey}"},
            },
            timeout);
            
            if (response.error != null)
                throw new Exception($"OpenAI API Error: [Code: {response.error.code ?? ""}] [Type: {response.error.type ?? ""}] [Param: {response.error.param ?? ""}] {response.error.message ?? ""}");

            return (response.data, response.usage);
        }

        public async Task<EmbeddingsAPIResponse> GetRawResponseAsync(
            EmbeddingsAPIRequest request,
            TimeSpan? timeout = null)
        {
            var response = await PostRequestAsync<EmbeddingsAPIResponse>(API_PATH, request, new Dictionary<string, string>()
            {
                {"Authorization", $"Bearer {APIKey}"},
            },
            timeout);

            return response;
        }
    }
}