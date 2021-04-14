using AppMain;
using Newtonsoft.Json;
using System;
using System.Net;
using TelegramBot.DTO;
using TelegramBot.Providers;

namespace TelegramBot.Music.API
{
    public class MusicResponse : IMusicResponse
    {
        ChatSendMessage chatSendMessage = new ChatSendMessage();
        FileChatProvider fileChatProvider = new FileChatProvider();

        public void GetMusic(string track, int chatID)
        {
            var APIResponse = JsonConvert.DeserializeObject<MusicLastFMResponse>(fileChatProvider.ReadAllText("musicLastFMSettings.json"));
            var apiUrl = APIResponse.Settings.LastFM_API;
            var apiToken = APIResponse.Settings.LastFM_Token;
            string url = $"{apiUrl}method=track.search&track={track}&api_key={apiToken}&format=json";
            string response;

            using (var webClient = new WebClient())
            {
                response = webClient.DownloadString(url);
            }
            var musicResponse = JsonConvert.DeserializeObject<RootMusicDatas>(response);

            foreach (var tr in musicResponse.results.trackmatches.track)
            {
                if (!musicResponse.results.trackmatches.track.Equals(track))
                {
                    Console.WriteLine("Name of track {0}.\nArtist is {1}.\nUrl: {2}",tr.name, tr.artist, tr.url);
                    chatSendMessage.SendMessage($"Track: {tr.name}.\nArtist: {tr.artist}.\n{tr.url}", chatID);
                    break;
                }
            }
        }
    }
}
