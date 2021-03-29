﻿using AppMain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TelegramBot.XML
{
    namespace IndependentN
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

    public class Independent
    {
        ChatSendMessage chatSendMessage = new ChatSendMessage();
        XmlDocument xDoc = new XmlDocument();
        public void GetNews(int chatID)
        {
            List<IndependentN.Items> items = new List<IndependentN.Items>();
            xDoc.Load("http://www.independent.co.uk/news/world/rss");
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    IndependentN.Items item = new IndependentN.Items();

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
    }
}
