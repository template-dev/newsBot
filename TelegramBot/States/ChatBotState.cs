using AppMain;
using ChatBot.Core.Readers;
using Microsoft.Extensions.FileProviders;
using System;
using System.Text.Json;
using TelegramBot.Providers;

namespace TelegramBot.States
{
    public class ChatBotState : IChatBotState
    {
        private const string fileName = "chatbotstate.json";
        private readonly IFileChatProvider fileProvider;
        private RootChatBotStateData stateData;
        ChatSendMessage chatSendMessage = new ChatSendMessage();
        private readonly IChatBotState chatBotState;

        public RootChatBotStateData Data
        {
            get { return stateData; }
        }

        public ChatBotState(IFileChatProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        public void Load()
        {
            try
            {
                this.stateData = JsonSerializer.Deserialize<RootChatBotStateData>(fileProvider.ReadAllText(fileName));
            }
            catch
            {
                this.stateData = new RootChatBotStateData();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Deserialize is failed!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void Save()
        {
            fileProvider.WriteAllText(fileName, JsonSerializer.Serialize(this.stateData));
        }
    }
}
