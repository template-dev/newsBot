using AppMain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TelegramBot.XML
{
    namespace TheGuardianN
    {
        public class Items
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string PubDate { get; set; }
            public string Link { get; set; }
        }
    }

    public class TheGuardian
    {
        ChatSendMessage chatSendMessage = new ChatSendMessage();
        XmlDocument xDoc = new XmlDocument();
        public void GetNews(int chatID)
        {
            try
            {
                List<TheGuardianN.Items> items = new List<TheGuardianN.Items>();
                xDoc.Load("https://www.theguardian.com/world/rss");
                XmlElement xRoot = xDoc.DocumentElement;

                foreach (XmlNode xnode in xRoot)
                {
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        TheGuardianN.Items item = new TheGuardianN.Items();

                        if (childnode.Name == "item")
                        {
                            foreach (XmlNode childnode2 in childnode)
                            {
                                if (childnode2.Name == "title")
                                    item.Title = childnode2.InnerText;

                                if (childnode2.Name == "description")
                                    item.Description = childnode2.InnerText;

                                if (childnode2.Name == "pubDate")
                                    item.PubDate = childnode2.InnerText;

                                if (childnode2.Name == "category")
                                    item.Category = childnode2.InnerText;

                                if (childnode2.Name == "link")
                                    item.Link = childnode2.InnerText;

                            }
                            items.Add(item);
                        }
                    }
                }
                for (int i = 0; i < 8; ++i)
                    chatSendMessage.SendMessage($"Название: {items[i].Title}\n\nКатегория: {items[i].Category}\n\nДата публикации: {items[i].PubDate}\n\nСсылка на статью: {items[i].Link}", chatID);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
