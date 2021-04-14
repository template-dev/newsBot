using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.States
{
    public interface IChatBotState
    {
        RootChatBotStateData Data { get; }

        void Load();
        void Save();
    }
}
