using System;
using System.Threading;
using System.Net;
using ChatBot.Core.Config;
using ChatBot.Core.Downloaders;
using ChatBot.Core.Readers;
using Microsoft.Extensions.Configuration;
using TelegramBot.DTO;
using TelegramBot.States;
using TelegramBot.Readers;
using TelegramBot.Providers;

namespace AppMain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Telegram Bot";

            var builder = new ConfigurationBuilder()
              .AddJsonFile($"appsettings.json", true, true);

            var configuration = builder.Build();

            using (var webClient = new WebClient())
            {
                var fileProvider = new FileChatProvider();
                var appConfig = configuration.GetSection("TelegramBot").Get<TelegramBotSettings>();
                var webContentDownloader = new WebContentDownloader(webClient);
                var telegramUpdatesReader = new TelegramUpdatesReader(appConfig, webContentDownloader);
                var chatBotState = new ChatBotState(fileProvider);
                chatBotState.Load();

                var pollingWatcher = new TelegramChatUpdatesPollingWatcher(telegramUpdatesReader, chatBotState);

                var cancellationTokenSource = new CancellationTokenSource();

                pollingWatcher.MessageIsArrived += (msg) =>
                {
                    Console.WriteLine($"{msg.text} from {msg.from.username}");
                };

                pollingWatcher.StartWatch(cancellationTokenSource.Token);

                Console.ReadLine();

                cancellationTokenSource.Cancel();

                chatBotState.Save();
            }
        }
    }
}
