using AppMain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TelegramBot.DTO;
using TelegramBot.GIF.API;
using TelegramBot.Music.API;
using TelegramBot.States;
using TelegramBot.Weather.API;
using TelegramBot.Twitch;
using TelegramBot.XML;
using System.Data.SqlClient;

namespace ChatBot.Core.Readers
{
    public class Commands : IChatCommands
    {
        ChatSendMessage chatSendMessage = new ChatSendMessage();
        Random rand = new Random();
        WeatherData weatherData = new WeatherData();
        MusicResponse musicResponse = new MusicResponse();
        GiphyDatas gif = new GiphyDatas();
        TwitchDatas twitch = new TwitchDatas();
        Liga liga = new Liga();
        MyKinoKadr movies = new MyKinoKadr();
        Esquire esquire = new Esquire();
        Lenta lenta = new Lenta();
        TheGuardian theGuardian = new TheGuardian();
        SputnikNews sputnikNews = new SputnikNews();
        Independent independent = new Independent();
        LATime lATime = new LATime();
        EuroNews euroNews = new EuroNews();

        private string name;

        public Commands() { }
        public Commands(string name) { this.name = name; }

        public void SetChatCommands(string TextCommand, string ChatID)
        {
                if (TextCommand == null) { }
                else
                {
                    var com = TextCommand.Split(' ');
                    var command = "";
                    if (com.Length >= 2)
                    {
                        this.name = com[1];
                        command = com[0] + " " + com[1];
                    }

                    var root = JsonConvert.DeserializeObject<Root>(File.ReadAllText("phrases.json"));

                    var phraseTagRegular = from s in root.Greetings
                                           where s.Tags.Any(t => t == "Regular")
                                           select s;

                    var phraseTagMorning = from s in root.Greetings
                                           where s.Tags.Any(t => t == "Morning")
                                           select s;

                    var phraseTagEvening = from s in root.Greetings
                                           where s.Tags.Any(t => t == "Evening")
                                           select s;

                    var phraseTagNight = from s in root.Greetings
                                         where s.Tags.Any(t => t == "Night")
                                         select s;

                    var phraseTagWeeekend = from s in root.Greetings
                                            where s.Tags.Any(t => t == "Weeekend")
                                            select s;
                    Console.ForegroundColor = ConsoleColor.White;
                    if (TextCommand == "/start")
                    {
                        if (DateTime.Now.Hour >= 5 && DateTime.Now.Hour <= 16)
                        {
                            Console.WriteLine(phraseTagMorning.First().Values[rand.Next(0, phraseTagEvening.First().Values.Count)]);
                            chatSendMessage.SendMessage(phraseTagMorning.First().Values[rand.Next(0, 1)], Convert.ToInt32(ChatID));
                        }
                        else if (DateTime.Now.Hour >= 17 && DateTime.Now.Hour <= 23)
                        {

                            Console.WriteLine(phraseTagEvening.First().Values[rand.Next(0, phraseTagEvening.First().Values.Count)]);
                            chatSendMessage.SendMessage(phraseTagEvening.First().Values[rand.Next(0, 1)], Convert.ToInt32(ChatID));
                        }
                        else if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 4)
                        {
                            Console.WriteLine(phraseTagNight.First().Values[rand.Next(0, phraseTagEvening.First().Values.Count)]);
                            chatSendMessage.SendMessage(phraseTagNight.First().Values[rand.Next(0, 1)], Convert.ToInt32(ChatID));
                        }
                        else if (DateTime.Now.DayOfWeek == 0)
                        {
                            Console.WriteLine(phraseTagWeeekend.First().Values[rand.Next(0, phraseTagEvening.First().Values.Count)]);
                            chatSendMessage.SendMessage(phraseTagWeeekend.First().Values[rand.Next(0, 1)], Convert.ToInt32(ChatID));
                        }
                        else
                        {
                            Console.WriteLine(phraseTagRegular.First().Values[rand.Next(0, phraseTagEvening.First().Values.Count)]);
                            chatSendMessage.SendMessage(phraseTagRegular.First().Values[rand.Next(0, 1)], Convert.ToInt32(ChatID));
                        }
                        chatSendMessage.SendMessage("Команды:" +
                            "\n--- [ ПОГОДА ] ---" +
                            "\n/getweather [город] - узнать текущую температуру в конкретном городе" +
                            "\n\n--- [ МУЗЫКА ] ---" +
                            "\n/getmusic [название песни] - найти песню по названию" +
                            "\n\n--- [ НОВОСТИ (РУ) ] ---" +
                            "\n/getnews_liga - вывести актуальные новости - Liga.net" +
                            "\n/getesquire - вывести актуальные новости журнала - Esquire" +
                            "\n/getmovies - вывести актуальные новости киноиндустрии" +
                            "\n/getnews_lenta24 - вывести главные новости за последние сутки - Lenta.ru" +
                            "\n\n--- [ НОВОСТИ (АНГ) ] ---" +
                            "\n/getnews_theguardian - вывести актуальные новости - theguardian.com" +
                            "\n/getnews_sputnik - вывести актуальные новости - sputniknews.com" +
                            "\n/getnews_independent - вывести актуальные новости - independent.co.uk" +
                            "\n/getnews_LATime - вывести актуальные новости - latimes.com" +
                            "\n/getnews_EuroNews - вывести актуальные новости - euronews.com" +
                            "\n\n--- [ ГИФКИ ] ---" +
                            "\n/getgif - вывести случайную гифку", Convert.ToInt32(ChatID));
                    }

                    if (command == ($"/getweather {name}"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже узнаю!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            weatherData.GetWeather(name, Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else if (TextCommand == ($"/getweather"))
                    {
                        chatSendMessage.SendMessage("Вы забыли указать название города!", Convert.ToInt32(ChatID));
                    }

                    if (command == ($"/getmusic {name}"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже ищу!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            musicResponse.GetMusic(name, Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else if (TextCommand == ($"/getmusic"))
                    {
                        chatSendMessage.SendMessage("Вы забыли указать артиста или название песни!", Convert.ToInt32(ChatID));
                    }

                    if (TextCommand == ($"/getnews_liga"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже узнаю!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            liga.GetNews(Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (TextCommand == ($"/getesquire"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже узнаю!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            esquire.GetNews(Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (TextCommand == ($"/getmovies"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже ищу!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            movies.GetMovies(Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (TextCommand == ($"/getnews_EuroNews"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже ищу!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            euroNews.GetNews(Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (TextCommand == ($"/getnews_LATime"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже ищу!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            lATime.GetNews(Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (TextCommand == ($"/getnews_independent"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже ищу!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            independent.GetNews(Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (TextCommand == ($"/getnews_theguardian"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже ищу!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            theGuardian.GetNews(Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (TextCommand == ($"/getnews_sputnik"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже ищу!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            sputnikNews.GetNews(Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (TextCommand == ($"/getnews_lenta24"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже ищу!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            lenta.GetNews(Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (TextCommand == ($"/getgif"))
                    {
                        try
                        {
                            chatSendMessage.SendMessage("Уже ищу!", Convert.ToInt32(ChatID));
                            Thread.Sleep(1000);
                            gif.GetGIF(Convert.ToInt32(ChatID));
                        }
                        catch (Exception ex)
                        {
                            chatSendMessage.SendMessage("Не корректные данные!", Convert.ToInt32(ChatID));
                            Console.WriteLine(ex.Message);
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}