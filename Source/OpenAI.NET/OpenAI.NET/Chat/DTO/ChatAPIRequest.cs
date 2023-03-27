using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAINET.Chat.DTO
{
    public class ChatAPIRequestMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class ChatAPIRequest
    {
        public string model { get; set; }
        public List<ChatAPIRequestMessage> messages { get; set; }

        public ChatAPIRequest() { }

        public ChatAPIRequest(ChatConversation conversation)
        {
            model = OpenAIModel.Models[conversation.ModelType].ModelString;
            messages = new List<ChatAPIRequestMessage>();

            foreach (var message in conversation.Messages)
            {
                messages.Add(new ChatAPIRequestMessage()
                {
                    role = message.GetRoleString(),
                    content = message.Message,
                });
            }
        }
    }
}
