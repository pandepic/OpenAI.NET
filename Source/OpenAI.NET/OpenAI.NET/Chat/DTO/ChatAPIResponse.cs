using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAINET.Chat.DTO
{
    public class ChatAPIResponseChoice
    {
        public ChatAPIRequestMessage message { get; set; }
        public string finish_reason { get; set; }
        public int index { get; set; }
    }

    public class ChatAPIResponseUsage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    public class ChatAPIResponse
    {
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public ChatAPIResponseUsage usage { get; set; }
        public List<ChatAPIResponseChoice> choices { get; set; }
    }
}
