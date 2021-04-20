using AppMain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TelegramBot.DTO;

namespace TelegramBot.Twitch
{
    public class TwitchDatas : ITwitchDatas
    {
        ChatSendMessage chatSendMessage = new ChatSendMessage();
        public async System.Threading.Tasks.Task GetTwitchDatasAsync(string mes, int chatID)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://146.148.98.174/:8080null"),
                Headers =
                {
                    { "x-rapidapi-key", "4b2c31ac18msh90e356bd13cd0ecp14347cjsn5ecf4754ad10" },
                    { "x-rapidapi-host", "146.148.98.174:8080" },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "query", "mes" },
                    { "clientId", "chatID" },
                }),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                chatSendMessage.SendMessage(body, chatID);
            }
        }

        void ITwitchDatas.GetTwitchDatasAsync(int chatID)
        {
            throw new NotImplementedException();
        }
    }
}
