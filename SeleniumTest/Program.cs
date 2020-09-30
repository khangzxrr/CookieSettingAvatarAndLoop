using System;
using System.Collections.Generic;
using System.Linq;
using WebDriverManager;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using OpenQA.Selenium;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;
using VNKClass;
using System.Windows.Forms;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTest
{
    class Program
    {

        static bool cookieOnly = false;
        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
        }

        static void Performing(InputInfo info)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--lang=vi");
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            var webDriver = new ChromeDriver(options);

            Logger logger = new Logger(info.email, info.password, info.recoverEmail);

            var streamWriter = File.AppendText(Properties.Resources.cookiesFile);
            streamWriter.AutoFlush = true;
            streamWriter.WriteLine($"{info.email}\t{info.password}\t{info.recoverEmail}");

            //get cookies
            if (logger.Login(webDriver))
            {
                ChannelSwitcher channelSwitcher = new ChannelSwitcher(webDriver);
                channelSwitcher.GetChannelSwitchLinks();

                foreach (var channelSwitchLink in channelSwitcher.channelSwitchLinks)
                {
                    CookieGetter cookieGetter = new CookieGetter(webDriver);
                    var cookieStr = cookieGetter.GetCookieFrom(channelSwitchLink);

                    if (cookieOnly)
                    {
                        Console.WriteLine("Got new cookie!");
                    }
                    else
                    {
                        Console.WriteLine("Got new cookie! starting to do things in a channel");
                    }

                    if (!cookieOnly)
                    {
                        //============================== PHOTO ==============================
                        AvatarSetter avatarSetter = null;
                        string photoLink = null;
                        avatarSetter = new AvatarSetter(webDriver);
                        photoLink = avatarSetter.GetPhotoLink();

                        if (photoLink == null)
                        {
                            Console.WriteLine("Skip this cookie...");
                            continue;
                        }


                        //===================================================================

                        ApplyDefaultSetting applyDefaultSetting = new ApplyDefaultSetting(webDriver);

                        //assume the preference setting of channel is openned
                        var channelLink = applyDefaultSetting.GetChannelLink();

                        //replace ?disable_polymer=true to https://youtube.com/channel/ID 
                        cookieStr = channelLink.Replace("?disable_polymer=true", "") + '\t' + cookieStr;
                        streamWriter.WriteLine(cookieStr);

                        Console.WriteLine("Saved new channel + cookie to file!");


                        applyDefaultSetting.SetDefaultSetting(channelLink); //set default setting
                                                                            //=========================================================

                        if (logger.Login(webDriver))
                        {
                            avatarSetter.SetImage(photoLink);
                        }

                    }
                    else
                    {

                        var wait = DriverUtil.GetDriverWait(webDriver);
                        var channelIconSelector = By.CssSelector("#channel-info > a");
                        wait.Until(ExpectedConditions.ElementIsVisible(channelIconSelector));

                        var url = webDriver.FindElement(channelIconSelector).GetAttribute("href");
                        cookieStr = url + '\t' + cookieStr;
                        streamWriter.WriteLine(cookieStr);

                        Console.WriteLine("Saved new cookie to file!");
                    }




                }

                Console.WriteLine($"Finish getting cookies to file {Properties.Resources.cookiesFile}");
            }


            //==========================================================
            /*
            

            */
            //====================================================================

            streamWriter.Close(); //remember to close
            webDriver.Quit();
        }
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "cookie":
                        Console.WriteLine("Get cookie only!");
                        cookieOnly = true;
                        break;
                }
            }
            new DriverManager().SetUpDriver(new ChromeConfig());

            if (File.Exists(Properties.Resources.cookiesFile))
            {
                Console.WriteLine("Removed old cookies file");
                File.Delete(Properties.Resources.cookiesFile);
            }

            var inputs = Input.GetInputInfos();
            foreach (var input in inputs)
            {
                Performing(input);
                MessageBox.Show("Finish all channel from " + input.email + " Please check the cookie file, and click OK to continue");
            }

            Console.WriteLine("***** FINISH THE APPLICATION!! ******");
        }
    }
}
