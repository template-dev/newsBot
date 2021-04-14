using AppMain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TelegramBot.XML
{
    public class Items
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }

    public class Liga
    {
        ChatSendMessage chatSendMessage = new ChatSendMessage();
        XmlDocument xDoc = new XmlDocument();
        public void GetNews(int chatID)
        {
            List<Items> items = new List<Items>();
            xDoc.Load("https://www.liga.net/biz/all/rss.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    Items item = new Items();
                    if (childnode.Name == "title")
                        item.Title = childnode.InnerText;

                    if (childnode.Name == "description")
                        item.Description = childnode.InnerText;

                    if (childnode.Name == "item")
                    {
                        foreach (XmlNode childnode2 in childnode)
                        {
                            if (childnode2.Name == "title")
                                item.Title = childnode2.InnerText;

                            if (childnode2.Name == "description")
                                item.Description = childnode2.InnerText;

                            if (childnode2.Name == "link")
                                item.Link = childnode2.InnerText;
                        }
                        items.Add(item);
                    }
                }
            }
            for(int i = 0; i < 8; ++i)
                chatSendMessage.SendMessage($"\nНазвание:  {items[i].Title}\n\n Описание: {items[i].Description}\n\nСсылка на статью: {items[i].Link}", chatID);
        }
    }
}
