using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat
{
    public class ChatAPI : BaseOpenAIAPI
    {
        private List<ChatMessage> _sharedMessageList = new List<ChatMessage>();

        public const string API_PATH = "https://api.openai.com/v1/chat/completions";

        public static readonly HashSet<OpenAIModelType> SupportedModels = new HashSet<OpenAIModelType>()
        {
            OpenAIModelType.GPT3_5Turbo,
        };

        public ChatAPI(string apiKey) : base(apiKey, API_PATH) { }

        public async Task<List<ChatMessage>> UpdateConversation(ChatConversation conversation, TimeSpan? timeout = null)
        {
            if (!SupportedModels.Contains(conversation.ModelType))
                throw new Exception($"Conversation model type not supported by chat API: {conversation.ModelType}");

            var request = new ChatAPIRequest(conversation);

            var response = await PostRequest<ChatAPIResponse>(API_PATH, request, new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {APIKey}" },
            },
            timeout);

            var completionTokens = response.usage?.completion_tokens ?? 0;
            conversation.TotalTokens = response.usage?.total_tokens ?? 0;

            _sharedMessageList.Clear();

            if (response.choices != null)
            {
                foreach (var choice in response.choices)
                {
                    var message = new ChatMessage(choice.message);

                    if (response.choices.Count == 1)
                        message.Tokens = completionTokens;

                    _sharedMessageList.Add(message);
                    conversation.Messages.Add(message);
                }
            }

            return _sharedMessageList;
        }
    }
}
