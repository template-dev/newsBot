using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.DTO
{
    public class GifSettings
    {
        public string Giphy_API_Token { get; set; }
    }

    public class GifAPIsResponse
    {
        public GifSettings Settings { get; set; }
    }
}
