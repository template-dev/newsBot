using AppMain;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using TelegramBot.DTO;
using TelegramBot.Providers;

namespace TelegramBot.Weather.API
{
    public class WeatherData : IWeatherDatas
    {
        ChatSendMessage chatSendMessage = new ChatSendMessage();
        FileChatProvider fileChatProvider = new FileChatProvider();

        public void GetWeather(string name, int chatID)
        {
            var APIResponse = JsonConvert.DeserializeObject<WeatherAPIResponse>(fileChatProvider.ReadAllText("weatherSettings.json"));
            var apiUrl = APIResponse.Settings.Weather_Url;
            var apiToken = APIResponse.Settings.Weather_Token;
            string url = $"{apiUrl}{name}&units=metric&appid={apiToken}";

            string response;

            using (var webClient = new WebClient())
            {
                response = webClient.DownloadString(url);
            }
            var weatherResponse = JsonConvert.DeserializeObject<RootWeatherDatas>(response);

            Console.WriteLine("Сейчас в {0}: {1} °C", weatherResponse.Name, weatherResponse.Main.Temp);
            Console.WriteLine("Скорость ветра {0} м/с.", weatherResponse.Wind.Speed);
            chatSendMessage.SendMessage($"Сейчас в {weatherResponse.Name}: {weatherResponse.Main.Temp} °C.\n" +
                $"Скорость ветра {weatherResponse.Wind.Speed} м/с.", chatID);
        }
    }
}
