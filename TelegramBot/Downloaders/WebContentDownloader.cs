using System.Net;

namespace ChatBot.Core.Downloaders
{
    public class WebContentDownloader : IWebContentDownloader
    {
        private readonly WebClient webClient;

        public WebContentDownloader(WebClient webClient)
        {
            this.webClient = webClient;
        }

        public string DownloadContent(string url)
        {
            return webClient.DownloadString(url);
        }
    }
}
