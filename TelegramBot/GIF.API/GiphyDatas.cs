using System;
using System.Collections.Generic;
using System.Text;
using TelegramBot.DTO;
using TelegramBot.Providers;
using Newtonsoft.Json;
using AppMain;
using System.Net;

namespace TelegramBot.GIF.API
{
    public class GiphyDatas : IGiphyDatas
    {
        ChatSendMessage chatSendMessage = new ChatSendMessage();
        FileChatProvider fileChatProvider = new FileChatProvider();

        public void GetGIF(int chatID)
        {
            var APIResponse = JsonConvert.DeserializeObject<GifAPIsResponse>(fileChatProvider.ReadAllText("gifSettings.json"));
            var apiToken = APIResponse.Settings.Giphy_API_Token;
            string response;

            using (var webClient = new WebClient())
            {
                response = webClient.DownloadString(apiToken);
            }
            var gifResponse = JsonConvert.DeserializeObject<GifRoot>(response);
            chatSendMessage.SendMessage(gifResponse.data.image_url, chatID);
        }
    }
}

