using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.DTO
{
    public class Image
    {
        public string text { get; set; } 
        public string size { get; set; }
    }

    public class Track
    {
        public string name { get; set; }
        public string artist { get; set; }
        public string url { get; set; }
        public string streamable { get; set; }
        public string listeners { get; set; }
        public List<Image> image { get; set; }
        public string mbid { get; set; }
    }

    public class Trackmatches
    {
        public List<Track> track { get; set; }
    }

    public class Results
    {
        public Trackmatches trackmatches { get; set; }
    }

    public class RootMusicDatas
    {
        public Results results { get; set; }
    }
}
