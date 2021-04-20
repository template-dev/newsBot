using AppMain;
using ChatBot.Core.Config;
using ChatBot.Core.Downloaders;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Text.Json;
using TelegramBot.DTO;
using TelegramBot.DTO.TelegramAPI;
using TelegramBot.Providers;

namespace ChatBot.Core.Readers
{
    public class TelegramUpdatesReader : IChatUpdatesReader<TelegramAPIResponse>
    {
        //https://api.telegram.org/bot1379326678:AAEDY2WgyRJBKybR1sujyD6viVIE-Zuh3Rw/getUpdates?offset=1
        private static string API_Token;
        private static string response;
        private string command;
        private static string ChatID;
        private readonly TelegramBotSettings telegramBotSettings;
        private readonly IWebContentDownloader webContentDownloader;

        Commands setCommand = new Commands();

        FileChatProvider fileChatProvider = new FileChatProvider();
        public static int GetChatID() { return Convert.ToInt32(ChatID); }

        public static string GetAPI_Token() { return API_Token; }

        public TelegramUpdatesReader() { }

        public TelegramUpdatesReader(TelegramBotSettings telegramBotSettings, IWebContentDownloader webContentDownloader)
        {
            this.telegramBotSettings = telegramBotSettings;
            this.webContentDownloader = webContentDownloader;
        }

        public TelegramAPIResponse GetUpdate(int offset)
        {
            API_Token = this.telegramBotSettings.TelegramAPI + this.telegramBotSettings.Token;
            var url = $"{API_Token}/getUpdates?offset={offset}";

            response = this.webContentDownloader.DownloadContent(url);
            var data = JsonSerializer.Deserialize<TelegramAPIResponse>(response);

            var N = JSON.Parse(response);
            foreach (JSONNode r in N["result"].AsArray)
            {
                command = r["message"]["text"];
                ChatID = r["message"]["chat"]["id"];
                setCommand.SetChatCommands(command, ChatID);
            }
            return data;
        }
    }
}
