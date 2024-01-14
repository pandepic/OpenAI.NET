using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class ChatAPI : BaseOpenAIAPI
{
    public const string API_PATH = "https://api.openai.com/v1/chat/completions";

    public static readonly HashSet<OpenAIModelType> SupportedModels = new HashSet<OpenAIModelType>()
    {
        OpenAIModelType.GPT3_5Turbo,
        OpenAIModelType.GPT4_8k,
        OpenAIModelType.GPT4_32k,
        OpenAIModelType.GPT4_Turbo,
        OpenAIModelType.GPT4_Turbo_Vision,
    };

    public ChatAPI(string apiKey) : base(apiKey) { }

    public async Task<ChatAPIResponse> GetRawResponseAsync(
        ChatAPIRequest request,
        TimeSpan? timeout = null,
        JsonSerializerSettings serializerSettings = null)
    {
        var response = await PostRequestAsync<ChatAPIResponse>(
            API_PATH,
            request,
            new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {APIKey}" },
            },
            timeout,
            serializerSettings);

        return response;
    }
}
