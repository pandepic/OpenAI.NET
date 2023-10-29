using OpenAINET.Chat;

namespace OpenAINET.Testbed
{
    public class ChatTest : Test
    {
        public ChatConversation Conversation;
        public ChatAPI ChatAPI;

        public ChatTest(string apiKey, OpenAIModelType modelType) : base(apiKey, modelType)
        {
            Conversation = new(ModelType);
            ChatAPI = new(APIKey, Conversation);
        }

        public override async Task AddUserInput(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
                Conversation.AddMessage(ChatMessage.FromUser(input));

            var newMessages = await ChatAPI.GetResponseAsync();

            foreach (var newMessage in newMessages)
                Console.WriteLine($"{newMessage.Role}: {newMessage.Message}");

            Console.WriteLine($"Total tokens: {Conversation.TotalTokens}");
        }
    }
}
