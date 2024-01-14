//using OpenAINET.Chat;

//namespace OpenAINET.Testbed
//{
//    public class ChatStreamedTest : Test, IDisposable
//    {
//        public ChatConversation Conversation;
//        public ChatAPIStreamed ChatAPIStreamed;

//        public ChatStreamedTest(string apiKey, OpenAIModelType modelType) : base(apiKey, modelType)
//        {
//            Conversation = new(ModelType);
//            ChatAPIStreamed = new(APIKey, Conversation);

//            ChatAPIStreamed.OnTokenReceived += (token) =>
//            {
//                Console.Write(token);
//            };

//            ChatAPIStreamed.OnMessageComplete += (message) =>
//            {
//                Console.WriteLine();
//            };

//            ChatAPIStreamed.OnError += (error) =>
//            {
//                Console.WriteLine($"Error: {error}");
//            };

//            ChatAPIStreamed.OnException += (ex) =>
//            {
//                Console.WriteLine($"Exception: {ex}");
//            };
//        }

//        public override async Task AddUserInput(string input)
//        {
//            if (!string.IsNullOrWhiteSpace(input))
//                Conversation.AddMessage(ChatMessage.FromUser(input));

//            ChatAPIStreamed.StartStreamingResponse();
//            ChatAPIStreamed.WaitForResponse();
//        }

//        public void Dispose()
//        {
//            ChatAPIStreamed?.Dispose();
//            ChatAPIStreamed = null;
//        }
//    }
//}
