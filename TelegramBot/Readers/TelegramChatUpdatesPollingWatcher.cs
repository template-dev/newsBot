using ChatBot.Core.Readers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TelegramBot.DTO;
using TelegramBot.DTO.TelegramAPI;
using TelegramBot.Providers;
using TelegramBot.States;

namespace TelegramBot.Readers
{
    public class TelegramChatUpdatesPollingWatcher : IChatUpdatesWatcher<Message>
    {
        private readonly IChatUpdatesReader<TelegramAPIResponse> telegramUpdatesReader;
        private readonly IChatBotState chatBotState;

        public event Action<Message> MessageIsArrived;
        
        public TelegramChatUpdatesPollingWatcher(IChatUpdatesReader<TelegramAPIResponse> telegramUpdatesReader, IChatBotState chatBotState)
        {
            this.telegramUpdatesReader = telegramUpdatesReader;
            this.chatBotState = chatBotState;
        }

        public void StartWatch(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1500);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var updates = this.telegramUpdatesReader.GetUpdate(this.chatBotState.Data.ChatData.UpdatedID);

                    if (updates.result == null || !updates.result.Any())
                    {
                        continue;
                    }

                    var newMessages = updates.result.Select(s => s.message);

                    this.chatBotState.Data.ChatData.UpdatedID = updates.result.Max(x => x.update_id);

                    foreach (var message in newMessages)
                    {
                        if(message != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            this.MessageIsArrived?.Invoke(message);
                            Console.ForegroundColor = ConsoleColor.White;
                            if (!chatBotState.Data.ChatData.ChatIds.Equals(message.chat.id))
                            {
                                this.chatBotState.Data.ChatData.ChatIds.Add(message.chat.id);
                            }
                        }
                    }
                    this.chatBotState.Data.ChatData.UpdatedID++;
                }
            }, cancellationToken);
        }
    }
}
