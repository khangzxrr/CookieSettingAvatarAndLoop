using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNKClass
{
    class CookieManager
    {
        public string cookiesFile { get; set; }

        public CookieManager(string cookiesFile)
        {
            this.cookiesFile = cookiesFile;
        }

        public string[] GetCookies()
        {
            return File.ReadAllLines(cookiesFile);
        }

        public static void SetCookie(IWebDriver driver, string cookiesRawStr)
        {
            try
            {

                driver.Url = "https://youtube.com";

                string[] cookiesSplited = cookiesRawStr.Split('|');
                foreach (var cookieStr in cookiesSplited)
                {
                    string[] cookieInfo = cookieStr.Split(';');

                    //DateTime dateTime = Convert.ToDateTime(cookieInfo[4]);
                    Dictionary<string, object> rawCookie = new Dictionary<string, object>();
                    rawCookie.Add("name", cookieInfo[0]);
                    rawCookie.Add("value", cookieInfo[1]);
                    rawCookie.Add("domain", cookieInfo[2]);
                    rawCookie.Add("path", cookieInfo[3]);
                    rawCookie.Add("expiry", cookieInfo[4]);
                    rawCookie.Add("secure", (cookieInfo[5] == "true") ? true : false);
                    rawCookie.Add("httpOnly", (cookieInfo[6] == "true") ? true : false);

                    var cookie = Cookie.FromDictionary(rawCookie);
                    driver.Manage().Cookies.AddCookie(cookie);
                }

                //foreach (var cookieRaw in cookiesRAw)
                //{
                //    var cookieDictionary = new Dictionary<string, object>();

                //    string cookieDomain = (string)cookieRaw["domain"];
                //    cookieDictionary.Add("domain", "youtube.com");

                //    cookieDictionary.Add("ExpirySeconds", cookieRaw["expirationDate"]);

                //    bool hostOnly = (bool)cookieRaw["hostOnly"];
                //    cookieDictionary.Add("hostOnly", hostOnly);

                //    bool isHttpOnly = (bool)cookieRaw["httpOnly"];
                //    cookieDictionary.Add("httpOnly", isHttpOnly);

                //    string cookieName = (string)cookieRaw["name"];
                //    cookieDictionary.Add("name", cookieName);

                //    string cookiePath = (string)cookieRaw["path"];
                //    cookieDictionary.Add("path", cookiePath);

                //    string cookiesameSite = (string)cookieRaw["sameSite"];
                //    cookieDictionary.Add("sameSite", cookiesameSite);

                //    bool cookieSecure = (bool)cookieRaw["secure"];
                //    cookieDictionary.Add("secure", cookieSecure);

                //    bool cookieSession = (bool)cookieRaw["session"];
                //    cookieDictionary.Add("session", cookieSession);

                //    string storeId = (string)cookieRaw["storeId"];
                //    cookieDictionary.Add("storeId", storeId);


                //    string cookieValue = (string)cookieRaw["value"];
                //    cookieDictionary.Add("value", cookieValue);


                //    var newCookie = Cookie.FromDictionary(cookieDictionary);

                //    driver.Manage().Cookies.AddCookie(newCookie);
                //}

                driver.Navigate().Refresh();

            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
