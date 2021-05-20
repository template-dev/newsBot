# newsBot

--- [ NEWS BOT ] ---

Функционал бота:
1. Админ панель;
2. Команды
3. База данных

Админ панель реализована на языке программирования C#.

Команды:
1. /convertENtoRU [текст] - конвертировать текст на англ расскладке в русский текст
2. /encrypt [способ] [текст] - зашифровать сообщение
   * перевод на кирпичный язык(есть поддержка латинских символов)
   * щифрование только букв
   * максимальное шифрование
3. /translate [текст] (RU to EN) - перевести текст с русского на английский
4. /getweather [город] - узнать текущую температуру в конкретном городе
5. /getmusic [название песни] - найти песню по названию
6. /getnews_liga - вывести актуальные новости - Liga.net
7. /getesquire - вывести актуальные новости журнала - Esquire
8. /getmovies - вывести актуальные новости киноиндустрии
9. /getnews_lenta24 - вывести главные новости за последние сутки - Lenta.ru
10. /getnews_theguardian - вывести актуальные новости - theguardian.com
11. /getnews_sputnik - вывести актуальные новости - sputniknews.com
12. /getnews_independent - вывести актуальные новости - independent.co.uk
13. /getnews_LATime - вывести актуальные новости - latimes.com
14. /getnews_EuroNews - вывести актуальные новости - euronews.com
15. /getgif - вывести случайную гифку

База данных реализована на языке запросов SQL (MSSQL) в среде разработки Microsoft SQL Server Management Studio.
