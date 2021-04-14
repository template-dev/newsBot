using System.Collections.Generic;
using TelegramBot.DTO.TelegramAPI;

namespace TelegramBot.DTO.TelegramAPI
{
    public class TelegramAPIResponse
    {
        public bool ok { get; set; }
        public List<Result> result { get; set; }
    }
}
