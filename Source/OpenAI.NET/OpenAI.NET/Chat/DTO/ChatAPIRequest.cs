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
        public bool stream { get; set; }
        public float temperature { get; set; } = 1f;
        public float top_p { get; set; } = 1f;
        public int n { get; set; } = 1;
        public string[] stop { get; set; }
        public float presence_penalty { get; set; }
        public float frequency_penalty { get; set; }

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
