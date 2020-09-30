using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumTest
{
    class ChannelSwitcher
    {
        private IWebDriver webDriver;
        private WebDriverWait wait;
        public List<string> channelSwitchLinks { get; set;  }
        public ChannelSwitcher(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
            wait = DriverUtil.GetDriverWait(webDriver);
            
        }

        public void GetChannelSwitchLinks()
        {
            webDriver.Navigate().GoToUrl(Properties.Resources.channelsPage);

            var liElements = webDriver.FindElements(By.CssSelector("#ytcc-existing-channels > li"));

            channelSwitchLinks = new List<string>();

            do
            {
                liElements = webDriver.FindElements(By.CssSelector("#ytcc-existing-channels > li"));

                if (liElements.Count <= 2)
                {
                    Console.WriteLine("Catched one channel error solve: select channel and re-collect channels");

                    if (!webDriver.Url.Contains("youtube.com"))
                    {
                        Console.WriteLine("Detect stuck at login page...start nagivating to youtube page");
                        webDriver.Navigate().GoToUrl(Properties.Resources.channelsPage);
                    }

                    var firstChannelBtnSelector = By.CssSelector("#identity-prompt-account-list > ul > label > li > span > span > input");
                    wait.Until(ExpectedConditions.ElementToBeClickable(firstChannelBtnSelector));
                    var firstChannelRadioBtn = webDriver.FindElement(firstChannelBtnSelector);
                    firstChannelRadioBtn.Click();

                    var confirmBtn = webDriver.FindElement(By.CssSelector("#identity-prompt-confirm-button"));
                    confirmBtn.Click();
                }

            } while (liElements.Count <= 2);

            //0 - create new channel
            //1 - main channel
            for (int i = 2; i < liElements.Count; i++)
            {
                var aElement = liElements[i].FindElement(By.CssSelector("a"));
                channelSwitchLinks.Add(aElement.GetAttribute("href"));
            }

            Console.WriteLine($"Got {channelSwitchLinks.Count} channels!");
        }

    }
}
