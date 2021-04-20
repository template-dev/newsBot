namespace ChatBot.Core.Downloaders
{
    public interface IWebContentDownloader
    {
        string DownloadContent(string url);
    }
}
