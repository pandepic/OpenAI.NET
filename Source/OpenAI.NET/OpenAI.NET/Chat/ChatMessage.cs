using System;
using System.Collections.Generic;
using System.Text;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat
{
    public enum ChatMessageRole
    {
        System,
        User,
        Assistant,
    }

    public class ChatMessage
    {
        public static Dictionary<ChatMessageRole, string> RoleStrings = new Dictionary<ChatMessageRole, string>()
        {
            { ChatMessageRole.System, "system" },
            { ChatMessageRole.User, "user" },
            { ChatMessageRole.Assistant, "assistant" },
        };

        public ChatMessageRole Role { get; set; }
        public string Message { get; set; }
        public int Tokens { get; set; }

        public ChatMessage() { }

        public ChatMessage(ChatMessageRole role, string message)
        {
            Role = role;
            Message = message;
        }

        public ChatMessage(ChatAPIRequestMessage message)
        {
            Role = message.role switch
            {
                "system" => ChatMessageRole.System,
                "user" => ChatMessageRole.User,
                "assistant" => ChatMessageRole.Assistant,
                _ => throw new NotImplementedException(),
            };

            Message = message.content;
        }

        public string GetRoleString() => RoleStrings[Role];

        public static ChatMessage FromSystem(string message) => new ChatMessage(ChatMessageRole.System, message);
        public static ChatMessage FromUser(string message) => new ChatMessage(ChatMessageRole.User, message);
        public static ChatMessage FromAssistant(string message) => new ChatMessage(ChatMessageRole.Assistant, message);
    }
}
