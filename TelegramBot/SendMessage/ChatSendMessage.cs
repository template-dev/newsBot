using System;
using System.Collections.Specialized;
using System.Net;
using ChatBot.Core.Readers;

namespace AppMain
{
    public class ChatSendMessage : IChatSendMessage
    {
        private static string api_token = TelegramUpdatesReader.GetAPI_Token();

        public void SendMessage(string message, int chatid)
        {
            using (var webClient = new WebClient())
            {
                var pars = new NameValueCollection();

                pars.Add("text", message);
                pars.Add("chat_id", chatid.ToString());

                webClient.UploadValues(api_token + "/sendMessage", pars);
            }
        }
    }
}
