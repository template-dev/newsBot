using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Music.API
{
    public interface IMusicResponse
    {
        void GetMusic(string track, int chatID);
    }
}
