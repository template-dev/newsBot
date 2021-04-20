using ChatBot.Core.Readers;
using ChatBot.Core.Downloaders;
using ChatBot.Core.Config;
using FluentAssertions;
using System;
using Xunit;
using Moq;

namespace ChatBot.Core.Tests
{
    public class BotSettingsTest
    {       
        [Fact]
        public void BotSettings()
        {
            // Arrange
            var telegramBotSettings = new TelegramBotSettings();
            telegramBotSettings.Token = "9878798as987d98";
            telegramBotSettings.TelegramAPI = "https://api.telegram.org/bot";

            var webContentDownloaderMock = new Mock<IWebContentDownloader>();
            webContentDownloaderMock.Setup(s => s.DownloadContent(It.IsAny<string>())).Throws(new Exception("Sorry, telegram is not available!"));    

            // Act
            /*var telegramUpdatesReader = new TelegramUpdatesReader(telegramBotSettings, webContentDownloaderMock.Object);
            var telUpdate = telegramUpdatesReader.GetUpdate();

            // Assert
            var error = telUpdate.Should().Contain("404");
            if (error.Equals("404")) throw (new Exception("Error! 404."));*/
        }

        [Fact]
        public void ContentDownload()
        {
            
        }
    }
}
