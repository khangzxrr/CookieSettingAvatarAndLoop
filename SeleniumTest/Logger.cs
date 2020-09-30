using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumTest
{
    class Logger
    {
        public string email { get; set; }
        public string password { get; set; }
        public string emailRecover { get; set; }

        public Logger(string email, string password, string emailRecover)
        {
            this.email = email;
            this.password = password;
            this.emailRecover = emailRecover;
        }

        private bool IsElementPresent(IWebDriver driver, By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private void findGoogleLoginBtn(IWebDriver webDriver)
        {
            var googleLoginBtn = webDriver.FindElements(By.CssSelector("#openid-buttons > button"));
            if (googleLoginBtn.Count == 0)
            {
                MessageBox.Show("NOT FOUND GOOGLE LOGIN BUTTON? ARE YOU SURE YOU ARE IN STACKOVERFLOW?");
            }
            else
            {
                googleLoginBtn[0].Click();
            }
            
        }

        [Obsolete]
        public bool Login(IWebDriver webDriver)
        {
            try
            {
                webDriver.Url = "https://stackoverflow.com/users/login";

                var wait = DriverUtil.GetDriverWait(webDriver);
                findGoogleLoginBtn(webDriver);


                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

                Thread.Sleep(500);

                if (webDriver.Title.Contains("Human verification"))
                {
                    MessageBox.Show("Please finish the captcha and click OK to continue", "Human verification", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    findGoogleLoginBtn(webDriver);
                }

                if (webDriver.Title == "")
                {
                    return true;
                }

                do
                {
                    try
                    {
                        var identifierSelector = By.CssSelector("#identifierId");
                        wait.Until(ExpectedConditions.ElementToBeClickable(identifierSelector));
                        var loginElement = webDriver.FindElement(identifierSelector);
                        loginElement.SendKeys(email);

                        var nextButton = webDriver.FindElement(By.CssSelector("#identifierNext > div > button"));
                        nextButton.Click();

                        break;
                    }
                    catch(WebDriverTimeoutException)
                    {
                        Console.WriteLine("Time out waiting for username field!, automatic solve: refresh the page and continue!");
                        continue;
                    }
                    
                    

                } while (true);
                

                do
                {
                    try
                    {
                        var passwordSelector = By.XPath("//input[@type='password']");
                        wait.Until(ExpectedConditions.ElementToBeClickable(passwordSelector));
                        var passwordField = webDriver.FindElement(passwordSelector);
                        passwordField.SendKeys(password);
                        break;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        Console.WriteLine("Password field is not attached error, automatic resolve it");

                    }
                } while (true);
                
                

                var nextBtn = webDriver.FindElement(By.CssSelector("#passwordNext > div > button"));
                nextBtn.Click();

                Thread.Sleep(2000);
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

                if (IsElementPresent(webDriver, By.XPath("//div[.='Xác nhận email khôi phục của bạn']")))
                {
                    var confirmEmailBtn = webDriver.FindElement(By.XPath("//div[.='Xác nhận email khôi phục của bạn']"));
                    confirmEmailBtn.Click();

                    var preregistedEmailSelector = By.CssSelector("#knowledge-preregistered-email-response");
                    wait.Until(ExpectedConditions.ElementToBeClickable(preregistedEmailSelector));

                    var preregistedEmailField = webDriver.FindElement(preregistedEmailSelector);
                    preregistedEmailField.SendKeys(emailRecover);
                    preregistedEmailField.SendKeys(OpenQA.Selenium.Keys.Return);

                    Thread.Sleep(2000);
                }
                
                return true;
            }catch(Exception e)
            {
                MessageBox.Show("Error: " + e.Message + "\nStacktrace:" + e.StackTrace);
                return false;
            }
            
        }
    }
}
