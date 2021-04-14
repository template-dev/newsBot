using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Weather.API
{
    public interface IWeatherDatas
    {
        void GetWeather(string name, int chatID);
    }
}
