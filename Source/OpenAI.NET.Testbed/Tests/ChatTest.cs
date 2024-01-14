using OpenAINET.Chat;

namespace OpenAINET.Testbed
{
    public class ChatTest : Test
    {
        public ChatConversation Conversation;
        public ChatAPI ChatAPI;

        public ChatTest(string apiKey, OpenAIModelType modelType) : base(apiKey, modelType)
        {
            ChatAPI = new(APIKey);
            Conversation = new(ChatAPI, ModelType);
        }

        public override async Task AddUserInput(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
                Conversation.AddMessage(ChatMessage.FromUser(input));

            var message = await Conversation.GetNextAssistantMessageAsync();

            if (message != null && message is ChatMessageAssistant messageAssistant)
                Console.WriteLine($"{messageAssistant.Role}: {messageAssistant.Content}");

            Console.WriteLine($"Total tokens: {Conversation.TotalTokens}");
        }
    }
}
