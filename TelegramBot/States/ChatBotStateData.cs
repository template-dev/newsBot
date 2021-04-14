using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.States
{
    public class ChatData
    {
        public List<int> ChatIds { get; set; }
        public int UpdatedID { get; set; }
    }

    public class RootChatBotStateData
    {
        public ChatData ChatData { get; set; }
    }
}
