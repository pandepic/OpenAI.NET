using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat
{
    public class ChatAPI : BaseOpenAIAPI
    {
        public const string API_PATH = "https://api.openai.com/v1/chat/completions";
        public static readonly HashSet<OpenAIModelType> SupportedModels = new HashSet<OpenAIModelType>()
        {
            OpenAIModelType.GPT3_5Turbo,
            OpenAIModelType.GPT4_8k,
            OpenAIModelType.GPT4_32k,
        };

        private List<ChatMessage> _sharedMessageList = new List<ChatMessage>();

        public readonly ChatConversation Conversation;

        public ChatAPI(string apiKey) : base(apiKey) { }

        public ChatAPI(string apiKey, ChatConversation conversation) : base(apiKey)
        {
            if (!SupportedModels.Contains(conversation.ModelType))
                throw new Exception($"Conversation model type not supported by chat API: {conversation.ModelType}");

            Conversation = conversation;
        }

        public async Task<List<ChatMessage>> GetResponseAsync(
            float? temperature = null,
            TimeSpan? timeout = null)
        {
            var request = new ChatAPIRequest(Conversation);

            if (temperature.HasValue)
                request.temperature = temperature.Value;

            var response = await PostRequestAsync<ChatAPIResponse>(API_PATH, request, new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {APIKey}" },
            },
            timeout);

            if (response.error != null)
                throw new Exception($"OpenAI API Error: [Code: {response.error.code ?? ""}] [Type: {response.error.type ?? ""}] [Param: {response.error.param ?? ""}] {response.error.message ?? ""}");

            var completionTokens = response.usage?.completion_tokens ?? 0;
            Conversation.TotalTokens = response.usage?.total_tokens ?? 0;

            _sharedMessageList.Clear();

            if (response.choices != null)
            {
                foreach (var choice in response.choices)
                {
                    var message = new ChatMessage(choice.message);

                    if (response.choices.Count == 1)
                        message.Tokens = completionTokens;

                    _sharedMessageList.Add(message);
                    Conversation.Messages.Add(message);
                }
            }

            return _sharedMessageList;
        }

        public async Task<ChatAPIResponse> GetRawResponseAsync(
            ChatAPIRequest request,
            TimeSpan? timeout = null)
        {
            var response = await PostRequestAsync<ChatAPIResponse>(API_PATH, request, new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {APIKey}" },
            },
            timeout);

            return response;
        }
    }
}
