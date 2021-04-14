using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.DTO
{
    public class Settings
    {
        public string Weather_Token { get; set; }
        public string Weather_Url { get; set; }
    }

    public class WeatherAPIResponse
    {
        public Settings Settings { get; set; }
    }
}
