using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.DTO
{
    public class Root
    {
        public List<Phrases> Greetings { get; set; }
    }

    public class Phrases
    {
        public List<string> Tags { get; set; }
        public List<string> Values { get; set; }
    }
}
