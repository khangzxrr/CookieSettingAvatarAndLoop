using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTest
{
    class DriverUtil
    {
        public static TimeSpan waitTime = TimeSpan.FromSeconds(30);
        public static WebDriverWait GetDriverWait(IWebDriver webDriver)
        {
            return new WebDriverWait(webDriver, waitTime);
        }
    }
}
