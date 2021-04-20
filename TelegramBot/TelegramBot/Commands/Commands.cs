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
using System.Data;

namespace ChatBot.Core.Readers
{
	class MyTranslate
	{
		public bool checkLetter(char a)
		{
			string lettersRusS1 = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
			string lettersRusS2 = "яюэьыъщшчцхфутсрпонмлкйизжёедгвба";
			string lettersRusB1 = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
			string lettersRusB2 = "ЯЮЭЬЫЪЩШЧЦХФУТСРПОНМЛКЙИЗЖЁЕДГВБА";

			for (int i = 0; i < lettersRusB1.Length; i++)
			{
				if (a == lettersRusS1[i] || a == lettersRusS2[i] || a == lettersRusB1[i] || a == lettersRusB2[i])
				{
					return true;
				}
			}
			return false;
		}

		public char convertENtoRU(char a)
		{
			Dictionary<char, string> letters = new Dictionary<char, string>
			{
				['q'] = "й",
				['w'] = "ц",
				['e'] = "у",
				['r'] = "к",
				['t'] = "е",
				['y'] = "н",
				['u'] = "г",
				['i'] = "ш",
				['o'] = "щ",
				['p'] = "з",
				['['] = "х",
				[']'] = "ъ",
				['a'] = "ф",
				['s'] = "ы",
				['d'] = "в",
				['f'] = "а",
				['g'] = "п",
				['h'] = "р",
				['j'] = "о",
				['k'] = "л",
				['l'] = "д",
				[';'] = "ж",
				[(char)39] = "э",
				['z'] = "я",
				['x'] = "ч",
				['c'] = "с",
				['v'] = "м",
				['b'] = "и",
				['n'] = "т",
				['m'] = "ь",
				[','] = "б",
				['.'] = "ю"
			};

			foreach (char c in letters.Keys)
			{
				int code = (int)c - 32;
				if (c == a || code == (int)a)
				{
					return letters[c][0];
				}
			}

			return a;
		}
	}

	class Translation
	{
		public string data { get; set; }
	}

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

		private string name = "";

		public Commands() { }
		public Commands(string name) { this.name = ""; }

		public void SetChatCommands(string TextCommand, string ChatID)
		{
			SqlConnection con;
			SqlCommand cmd;
			SqlDataReader reader;
			con = new SqlConnection();
			con.ConnectionString = @"Integrated Security = SSPI; data source = DESKTOP-LT9TUC4\SQLEXPRESS; Initial Catalog = Bot";
			try
			{
				con.Open();
				cmd = new SqlCommand("SELECT TOP(1) ID_USER_TELEGRAM, USER_POSITION FROM USERS WHERE ID_USER_TELEGRAM = @val", con);
				cmd.Parameters.Add("@val", SqlDbType.Int).Value = int.Parse(ChatID);
				reader = cmd.ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						var id_user = reader.GetValue(0);
						var pos = reader.GetValue(1);

						if (ChatID == id_user.ToString() && pos.ToString() == "1")
						{
							if (TextCommand == null) { }
							else
							{
								this.name = "";
								var com = TextCommand.Split(' ');
								var command = "";
								if (com.Length >= 2)
								{
									for (int i = 1; i < com.Length; i++)
									{
										this.name += com[i];
										if (i != com.Length - 1)
										{
											this.name += " ";
										}

									}
									command = String.Join(" ", com);
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
										"\n/convertENtoRU [текст] конвертировать текст на англ расскладке в русский текст" +
										"\n/encrypt [способ] [текст] зашифровать сообщение" +
										"\n--- [ Как использовать ] ---" +
										"\n1 перевод на кирпичный язык(есть поддержка латинских символов)" +
										"\n2 щифрование только букв" +
										"\n3 максимальное шифрование" +
										"\n/translate перевод [текст] (RU to EN)" +
										"\n\n--- [ ПОГОДА ] ---" +
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
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
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
										Console.WriteLine(ex.Message);
									}
								}

								if (command == ($"/translate {name}"))
								{
									try
									{
										chatSendMessage.SendMessage("Перевожу!", Convert.ToInt32(ChatID));
										Thread.Sleep(1000);
										chatSendMessage.SendMessage(name, Convert.ToInt32(ChatID));

										if (name.Length > 0)
										{
											WebRequest request = WebRequest.Create("https://fasttranslator.herokuapp.com/api/v1/text/to/text?"
												+ "source=" + name
												+ "&lang=ru-en");

											WebResponse response = request.GetResponse();

											using (StreamReader stream = new StreamReader(response.GetResponseStream()))
											{
												string line;

												if ((line = stream.ReadLine()) != null)
												{
													Translation translation = JsonConvert.DeserializeObject<Translation>(line);

													chatSendMessage.SendMessage($"Ваш текст: {translation.data}", Convert.ToInt32(ChatID));
												}
											}
										}
										else
											chatSendMessage.SendMessage("Не получилось(", Convert.ToInt32(ChatID));
									}
									catch (Exception ex)
									{
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
										Console.WriteLine(ex.Message);
									}
								}

								if (command == ($"/encrypt {name}"))
								{
									try
									{
										string answer = "";
										if (name.Length > 0)
										{
											if (name[0] == '1')
											{
												chatSendMessage.SendMessage("Перевожу на кирпичный!", Convert.ToInt32(ChatID));

												string vowelLetters = "аоэеиыуёюяАОЭЕИЫУЁЮЯaeiouyAEIOUY";

												for (int i = 2; i < name.Length; i++)
												{
													answer += name[i];
													for (int j = 0; j < vowelLetters.Length; j++)
													{
														if (name[i] == vowelLetters[j])
														{
															if (j > 19)
															{
																answer += 'k';
															}
															else
															{
																answer += 'к';
															}
														}
													}
												}
												chatSendMessage.SendMessage($"Ваше зашифрованое сообщение: {answer}", Convert.ToInt32(ChatID));
											}
											else if (name[0] == '2')
											{
												chatSendMessage.SendMessage("Кодирую соблюдая пунктуацию", Convert.ToInt32(ChatID));

												MyTranslate t = new MyTranslate();

												for (int i = 2; i < name.Length; i++)
												{
													int code = (int)name[i];
													if (code > 64 && 91 > code || code > 96 && 123 > code || t.checkLetter(name[i]))
													{
														answer += (char)(code + 5);
													}
													else
													{
														answer += name[i];
													}
												}
												chatSendMessage.SendMessage($"Ваше зашифрованое сообщение: {answer}", Convert.ToInt32(ChatID));
											}
											else if (name[0] == '3')
											{
												chatSendMessage.SendMessage("Кодирую чтобы никто не понял", Convert.ToInt32(ChatID));

												for (int i = 2; i < name.Length; i++)
												{
													int random = rand.Next(32, 128);
													answer += (char)random;
												}
												chatSendMessage.SendMessage($"Ваше зашифрованое сообщение: {answer}", Convert.ToInt32(ChatID));
											}
											else
											{
												chatSendMessage.SendMessage("Неверный параметр кодировки", Convert.ToInt32(ChatID));
											}
										}
										else
											chatSendMessage.SendMessage("Не получилось(", Convert.ToInt32(ChatID));
									}
									catch (Exception ex)
									{
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
										Console.WriteLine(ex.Message);
									}
								}

								if (command == ($"/convertENtoRU {name}"))
								{
									try
									{
										chatSendMessage.SendMessage("Уже меняю расскладку!", Convert.ToInt32(ChatID));

										if (name.Length > 0)
										{
											string answer = "";
											MyTranslate t = new MyTranslate();

											for (int i = 0; i < name.Length; i++)
											{
												answer += t.convertENtoRU(name[i]);
											}

											chatSendMessage.SendMessage($"Вот что получилось: {answer}", Convert.ToInt32(ChatID));
										}
										else
											chatSendMessage.SendMessage("Не получилось(", Convert.ToInt32(ChatID));
									}
									catch (Exception ex)
									{
										chatSendMessage.SendMessage("Некорректные данные!", Convert.ToInt32(ChatID));
										Console.WriteLine(ex.Message);
									}
								}
								Console.ForegroundColor = ConsoleColor.White;
							}
						}
						else if(ChatID == id_user.ToString() && pos.ToString() == "0")
                        {
							chatSendMessage.SendMessage("К сожалению Вы забанены администратором!", Convert.ToInt32(ChatID));
							TextCommand = "\0";
						}
					}
					reader.Close();
				}
				con.Close();
			}
			catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
		}
	}
}