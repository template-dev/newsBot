using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Core.Config
{
    public class RootConfig
    {
        public TelegramBotSettings telegramBotSettings { get; set; }
    }

    public class TelegramBotSettings
    {
        public string Token{ get; set; }
        public string TelegramAPI { get; set; }
    }
}
