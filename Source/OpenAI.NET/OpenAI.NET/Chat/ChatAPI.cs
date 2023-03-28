using System;
using System.Collections.Generic;
using System.Linq;
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

        public ChatAPI(string apiKey, ChatConversation conversation) : base(apiKey, API_PATH)
        {
            Conversation = conversation;
        }

        public async Task<List<ChatMessage>> GetResponse(TimeSpan? timeout = null)
        {
            if (!SupportedModels.Contains(Conversation.ModelType))
                throw new Exception($"Conversation model type not supported by chat API: {Conversation.ModelType}");

            var request = new ChatAPIRequest(Conversation);

            var response = await PostRequest<ChatAPIResponse>(API_PATH, request, new Dictionary<string, string>()
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
    }
}
