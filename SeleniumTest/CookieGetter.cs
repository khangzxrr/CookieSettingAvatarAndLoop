using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumTest
{
    class CookieGetter
    {
        IWebDriver webDriver;

        public CookieGetter(IWebDriver driver)
        {
            this.webDriver = driver;
        }

        public String GetCookieFrom(string site)
        {
            try
            {
                webDriver.Url = site;

                var cookies = webDriver.Manage().Cookies.AllCookies;

                var cookieString = "";
                foreach(var cookie in cookies)
                {
                    
                    cookieString += $"{cookie.Name};{cookie.Value};{cookie.Domain};{cookie.Path};{cookie.Expiry};{cookie.Secure};{cookie.IsHttpOnly}|";
                }

                return cookieString;

                //List<JObject> cookieObjects = new List<JObject>();
                //foreach (var cookie in cookies)
                //{

                //    var cookieJsonObject = new JObject(
                //        new JProperty("domain", "youtube.com"),
                //        new JProperty("expirationDate", cookie.Expiry),
                //        new JProperty("hostOnly", false),
                //        new JProperty("httpOnly", false),
                //        new JProperty("name", cookie.Name),
                //        new JProperty("path", cookie.Path),
                //        new JProperty("sameSite", "no_restriction"),
                //        new JProperty("secure", cookie.Secure),
                //        new JProperty("session", false),
                //        new JProperty("storeId", "0"),
                //        new JProperty("value", cookie.Value)
                //    );

                //    cookieObjects.Add(cookieJsonObject);
                //}

                //var jsonObj = new JObject(
                //    new JProperty("url", "https://youtube.com"),
                //    new JProperty("cookies", cookieObjects));

                //return jsonObj;
            }
            catch(Exception e)
            {
                MessageBox.Show($"Error {e.Message} \nStackTrace: {e.StackTrace}");
            }
            

            return null;
        }
    }
}
