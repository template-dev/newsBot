using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.DTO
{
    public class SettingsMusic
    {
        public string LastFM_Token { get; set; }
        public string LastFM_API { get; set; }
    }

    public class MusicLastFMResponse
    {
        public SettingsMusic Settings { get; set; }
    }
}
