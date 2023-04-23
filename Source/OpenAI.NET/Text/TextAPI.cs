using System.Collections.Generic;
using OpenAINET.Chat.DTO;
using System.Threading;
using OpenAINET.Text.DTO;
using System;
using System.Threading.Tasks;

namespace OpenAINET.Text
{
    public class TextAPI : BaseOpenAIAPI
    {
        public const string API_PATH = "https://api.openai.com/v1/completions";

        public readonly OpenAIModel Model;

        public TextAPI(OpenAIModelType modelType, string apiKey) : base(apiKey, API_PATH)
        {
            Model = OpenAIModel.Models[modelType];
        }

        public async Task<(List<string> Responses, int PromptTokens, int ResponseTokens, int TotalTokens)> GetResponse(string prompt, float temperature = 1f, int? maxTokens = null, TimeSpan? timeout = null)
        {
            return await GetResponse(new string[] { prompt }, temperature, maxTokens, timeout);
        }

        public async Task<(List<string> Responses, int PromptTokens, int ResponseTokens, int TotalTokens)> GetResponse(string[] prompt, float temperature = 1f, int? maxTokens = null, TimeSpan? timeout = null)
        {
            var useMaxTokens = maxTokens ?? Model.MaxTokens;

            var request = new TextAPIRequest()
            {
                model = Model.ModelString,
                prompt = prompt,
                max_tokens = useMaxTokens,
                temperature = temperature,
            };

            var response = await PostRequest<TextAPIResponse>(API_PATH, request, new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {APIKey}" },
            },
            timeout);

            if (response.error != null)
                throw new Exception($"OpenAI API Error: [Code: {response.error.code ?? ""}] [Type: {response.error.type ?? ""}] [Param: {response.error.param ?? ""}] {response.error.message ?? ""}");

            var responses = new List<string>();
            var promptTokens = 0;
            var responseTokens = 0;
            var totalTokens = 0;

            if (response.choices != null)
            {
                foreach (var choice in response.choices)
                {
                    if (string.IsNullOrEmpty(choice.text))
                        continue;

                    responses.Add(choice.text);
                }
            }

            if (response.usage != null)
            {
                promptTokens = response.usage.prompt_tokens;
                responseTokens = response.usage.completion_tokens;
                totalTokens = response.usage.total_tokens;
            }

            return (responses, promptTokens, responseTokens, totalTokens);
        }
        
        public async Task<TextAPIResponse> GetRawResponse(TextAPIResponse request, TimeSpan? timeout = null)
        {
            var response = await PostRequest<TextAPIResponse>(API_PATH, request, new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {APIKey}" },
            },
            timeout);

            return response;
        }
    }
}
