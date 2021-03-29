using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.DTO
{
    public class TemperatureInfo
    {
        public float Temp { get; set; }
    }

    public class WindInfo
    {
        public float Speed { get; set; }
    }

    class RootWeatherDatas
    {
        public TemperatureInfo Main { get; set; }
        public WindInfo Wind { get; set; }
        public string Name { get; set; }
    }
}
