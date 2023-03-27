using System.Collections.Generic;

namespace OpenAINET.Chat
{
    public class ChatConversation
    {
        public OpenAIModelType ModelType { get; protected set; }
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public int TotalTokens { get; set; }

        public ChatConversation(OpenAIModelType modelType)
        {
            ModelType = modelType;
        }

        public void AddMessage(ChatMessage message)
        {
            Messages.Add(message);
        }
    }
}
