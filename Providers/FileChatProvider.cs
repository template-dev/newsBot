using System;
using System.IO;

namespace TelegramBot.Providers
{
    public class FileChatProvider : IFileChatProvider
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
