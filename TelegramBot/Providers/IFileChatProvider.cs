using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Providers
{
    public interface IFileChatProvider
    {
        string ReadAllText(string path);
        void WriteAllText(string path, string content);
    }
}
