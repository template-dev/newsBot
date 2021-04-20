using System;
using System.Threading;
using System.Net;
using ChatBot.Core.Config;
using ChatBot.Core.Downloaders;
using ChatBot.Core.Readers;
using Microsoft.Extensions.Configuration;
using TelegramBot.DTO;
using TelegramBot.States;
using TelegramBot.Readers;
using TelegramBot.Providers;
using System.Data.SqlClient;
using System.Data;

namespace AppMain
{
    class Program
    {
        static public void InsertDataToDB(int id_user, string nick)
        {
            SqlConnection con;
            SqlCommand cmd;
            con = new SqlConnection();
            con.ConnectionString = @"Integrated Security = SSPI; data source = DESKTOP-LT9TUC4\SQLEXPRESS; Initial Catalog = Bot";
            con.Open();

            cmd = new SqlCommand("sp_USERS_INSERT_ALL_DATA", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_user_telegram", id_user);
            cmd.Parameters.AddWithValue("@nick ", nick);

            cmd.ExecuteNonQuery();

            cmd.Dispose();
            con.Close();
        }
        static void Main(string[] args)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Integrated Security = SSPI; data source = DESKTOP-LT9TUC4\SQLEXPRESS; Initial Catalog = Bot";
            Console.Title = "Telegram Bot";
            SqlCommand command;
            SqlDataReader reader;

            var builder = new ConfigurationBuilder()
              .AddJsonFile($"appsettings.json", true, true);

            var configuration = builder.Build();

            using (var webClient = new WebClient())
            {
                con.Open();

                var fileProvider = new FileChatProvider();
                var appConfig = configuration.GetSection("TelegramBot").Get<TelegramBotSettings>();
                var webContentDownloader = new WebContentDownloader(webClient);
                var telegramUpdatesReader = new TelegramUpdatesReader(appConfig, webContentDownloader);
                var chatBotState = new ChatBotState(fileProvider);
                chatBotState.Load();

                var pollingWatcher = new TelegramChatUpdatesPollingWatcher(telegramUpdatesReader, chatBotState);

                var cancellationTokenSource = new CancellationTokenSource();

                pollingWatcher.MessageIsArrived += (msg) =>
                {
                    try
                    {
                        command = new SqlCommand("SELECT ID_USER_TELEGRAM FROM USERS WHERE ID_USER_TELEGRAM = @val", con);
                        command.Parameters.Add("@val", SqlDbType.Int).Value = msg.from.id;
                        reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var val = reader.GetValue(0);
                                if (msg.from.id.ToString() != val.ToString())
                                {
                                    InsertDataToDB(msg.from.id, msg.from.username);
                                    break;
                                }
                            }
                            reader.Close();
                        }
                        else if (!reader.HasRows)
                        {
                            InsertDataToDB(msg.from.id, msg.from.username);
                        }
                        reader.Close();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Console.WriteLine($"{msg.text} from {msg.from.username}");
                };

                pollingWatcher.StartWatch(cancellationTokenSource.Token);

                Console.ReadLine();

                cancellationTokenSource.Cancel();

                con.Close();
                chatBotState.Save();
            }
        }
    }
}
