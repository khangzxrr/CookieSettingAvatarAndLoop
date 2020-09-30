using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumTest
{
    class ApplyDefaultSetting
    {
        IWebDriver webDriver;
        WebDriverWait wait;

        public ApplyDefaultSetting(IWebDriver driver)
        {
            webDriver = driver;
            wait = DriverUtil.GetDriverWait(webDriver);
        }

        public void SetDefaultSetting(string channelLink)
        {
            webDriver.Url = channelLink;

            Thread.Sleep(1000);

            IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
            string xsrf_token = (string) js.ExecuteScript("return yt.config_.XSRF_TOKEN;");

            Console.WriteLine("Got youtube channel token..");

            string URI = "https://www.youtube.com/channels_settings_ajax?action_update_channel_settings=1";

            Console.WriteLine("Generate setting request.. start to post data...waiting for result");

            using (WebClientEx myWebClient = new WebClientEx(new CookieContainer()))
            {
                CookieCollection cc = new CookieCollection();
                foreach (OpenQA.Selenium.Cookie cook in webDriver.Manage().Cookies.AllCookies)
                {
                    System.Net.Cookie cookie = new System.Net.Cookie();
                    cookie.Name = cook.Name;
                    cookie.Value = cook.Value;
                    cookie.Domain = cook.Domain;
                    cc.Add(cookie);
                }
                myWebClient.CookieContainer.Add(cc);
                myWebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var dictionary = new NameValueCollection();
                dictionary.Add("keep_subscriptions_undiscoverable", "1");
                dictionary.Add("keep_saved_playlists_undiscoverable", "1");
                dictionary.Add("overview_tab_enabled", "1");
                dictionary.Add("session_token", xsrf_token);

                var response = myWebClient.UploadValues(URI, "POST", dictionary);
                var responseStr = Encoding.UTF8.GetString(response);
                Console.WriteLine(responseStr);

            }
        }

        //assume the preference setting of channel is openned
        public string GetChannelLink()
        {
            

            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("ytd-compact-link-renderer > a")));
            var channelLinkElements = webDriver.FindElements(By.CssSelector("ytd-compact-link-renderer > a"));

            if (channelLinkElements.Count <= 0)
            {
                MessageBox.Show("Are you sure the perference setting tab is opened?", "Channel link error");
                return null;
            }

            var channelDefaultLink = channelLinkElements[0].GetAttribute("href");


            return channelDefaultLink.Replace("?view_as=subscriber", "?disable_polymer=true");
        }
    }
}
