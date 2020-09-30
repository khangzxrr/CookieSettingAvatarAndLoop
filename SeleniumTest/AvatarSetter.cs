using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumTest
{
    class AvatarSetter
    {
        IWebDriver webDriver;
        private WebDriverWait wait;

        public AvatarSetter(IWebDriver driver)
        {
            webDriver = driver;
            wait = DriverUtil.GetDriverWait(webDriver);
        }

        public string GetPhotoLink()
        {
            
            if (webDriver.Url != "https://youtube.com")
            {
                webDriver.Url = "https://youtube.com";
            }
            

            var wait = DriverUtil.GetDriverWait(webDriver);

            try
            {
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#avatar-btn")));
                var avatarBtn = webDriver.FindElement(By.CssSelector("#avatar-btn"));
                avatarBtn.Click();

                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#manage-account > a")));
                var googleLinkElement = webDriver.FindElement(By.CssSelector("#manage-account > a"));

                var googleLink = googleLinkElement.GetAttribute("href");
                var photoLink = googleLink.Substring(0, googleLink.IndexOf("/?")) + "/profile/photo/edit";

                return photoLink;
            }catch(WebDriverTimeoutException e)
            {
                MessageBox.Show("failed to waiting for element... maybe cookie is dead? (cannot login)", "error");
                return null;
            }

            


            

        }


        public IWebElement GetValidIframe()
        {
            webDriver.SwitchTo().DefaultContent();

            var iframeSelector = By.CssSelector("iframe");
            wait.Until(ExpectedConditions.ElementExists(iframeSelector));

            var iframeElements = webDriver.FindElements(iframeSelector);
            IWebElement correctIframe = null;
            foreach (var iframe in iframeElements)
            {
                if (iframe.GetAttribute("name") != "")
                {
                    correctIframe = iframe;
                    break;
                }
            }

            return correctIframe;
        }
        public void SetImage(string photoLink)
        {

            var rand = new Random();
            var files = Directory.EnumerateFiles(Properties.Resources.imagesPath, "*.*", SearchOption.AllDirectories)
                        .Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".jpeg")).ToArray();
            var selectedImage = files[rand.Next(files.Length)];

            webDriver.Url = photoLink;

            webDriver.Manage().Window.Size = new System.Drawing.Size(300, 300);

            var setImageButton = By.CssSelector("button");
            wait.Until(ExpectedConditions.ElementExists(setImageButton));

            var buttonElements = webDriver.FindElements(setImageButton);
            foreach(var button in buttonElements)
            {
                if (button.Text.Contains("Thêm ảnh hồ sơ") || button.Text.Contains("Thay đổi")) {
                    button.Click();
                    break;
                }
            }

            var correctIframe = GetValidIframe();
            webDriver.SwitchTo().Frame(correctIframe); //switch to correct iframe

            var inputFile = webDriver.FindElement(By.CssSelector("input[type=file]")); //image file selector
            inputFile.SendKeys(Path.GetFullPath(selectedImage));


            var tdSelector = By.CssSelector("td");
            wait.Until(ExpectedConditions.ElementExists(tdSelector));

            do
            {
                Thread.Sleep(500);
                Console.WriteLine("Waiting for upload...");
            } while (webDriver.FindElements(tdSelector).Count <= 2);


            Console.WriteLine("Clicking confirm button image..");

            var confirmButtonSelector = By.CssSelector("div[role=button]");
            wait.Until(ExpectedConditions.ElementIsVisible(confirmButtonSelector));

            Thread.Sleep(500);

            Console.WriteLine("Switching to default content frame");

            webDriver.Manage().Window.Maximize();

            do
            {
                correctIframe = GetValidIframe(); 
                webDriver.SwitchTo().Frame(correctIframe); //switch to correct iframe

                var applyElements = webDriver.FindElements(confirmButtonSelector);
                foreach (var applyBtn in applyElements)
                {
                    if (applyBtn.Text.Contains("Đặt làm ảnh hồ sơ"))
                    {
                        try {
                            new Actions(webDriver).MoveToElement(applyBtn).Click().Perform();
                            break;
                        }
                        catch (WebDriverException) //no need to worry
                        {
                            Console.WriteLine("[Đặt làm ảnh hồ sơ] Button is no longer exist... no worry, skip it!");
                        } 
                    }
                }

                Thread.Sleep(500);
                Console.WriteLine("Waiting for upload finish...");

                webDriver.SwitchTo().DefaultContent(); //remember to switch back main html
            } while (!webDriver.FindElement(By.CssSelector("div.picker-dialog")).GetAttribute("style").Contains("none"));
            //waiting for closing dialog


            Console.WriteLine("Finish set avatar!");

            


        }
    }
}
