using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Twitch
{
    public interface ITwitchDatas
    {
        public void GetTwitchDatasAsync(int chatID);
    }
}
