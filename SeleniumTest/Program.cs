using OpenQA.Selenium;
using System;
using System.Text;
using System.Threading;
using System.Linq;

namespace SeleniumTest
{
    class Program
    {
        public static string[] TrackingNumbers()
        {
            return new string[]
            {
                "UW21175013CNMODIFY",
                "UW20841796CNMODIFY",
                "UW20897700CNMODIFY",
                "UW21021980CNMODIFY",
                "UW21096128CNMODIFY"
            };
        }

        static void Main(string[] args)
        {
            StringBuilder message = new StringBuilder();
            decimal pageSize = 2m;
            int index = 0;

            using (IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver())
            {
                driver.Navigate().GoToUrl("");  //driver.Url = "http://www.baidu.com"是一样的

                var account = driver.FindElement(By.Id("log_ph"));
                account.Click();
                account.SendKeys("");

                var password = driver.FindElement(By.Id("log_ps"));
                password.Click();
                password.SendKeys("");

                var login = driver.FindElement(By.Id("log_ok"));
                login.Click();

                driver.Navigate().GoToUrl("");

                //var page1 = driver.PageSource;
                driver.SwitchTo().Frame(0);
                //var page = driver.PageSource;

                var pageNumber = Math.Ceiling(TrackingNumbers().Length / pageSize);
                var flag = true;
                for (int i = 0; i < pageNumber; i++)
                {
                    try
                    {
                        var trackingNumbers = TrackingNumbers().Skip(i * (int)pageSize).Take((int)pageSize).ToArray();
                        index = i;
                        Information(driver, trackingNumbers, i);
                        message.AppendLine($"index:{index},Success!");
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        message.AppendLine($"index:{index},Error:{ex.Message}");
                    }
                }
                if (flag)
                {
                    message.AppendLine("all success!");
                }
            }
            Console.WriteLine(message.ToString());
            Console.ReadKey();
        }

        public static void Information(IWebDriver driver, string[] trackingNumbers, int index)
        {
            var searchButton = driver.FindElement(By.CssSelector(".search-input"));
            searchButton.Click();

            if (index != 0)
            {
                Thread.Sleep(1000);
                var deleteButton = driver.FindElement(By.CssSelector(".el-button--default:nth-child(1) > span"));
                deleteButton.Click();
            }

            var searchBox = driver.FindElement(By.Id("cc"));
            //searchBox.SendKeys("UW211785013CNMODIFY\nUW208491796CNMODIFY");//abc\nabc\nabc
            searchBox.SendKeys(string.Join("\n", trackingNumbers));

            var search = driver.FindElement(By.CssSelector(".button_box> .el-button"));
            search.Click();

            Thread.Sleep(3000);

            var allSelect = driver.FindElement(By.CssSelector(".has-gutter .el-checkbox__inner"));
            allSelect.Click();

            Thread.Sleep(1000);

            var informationButton = driver.FindElement(By.CssSelector(".el-dropdown:nth-child(1) span"));
            informationButton.Click();

            Thread.Sleep(1000);

            var informationAction = driver.FindElement(By.CssSelector(".el-dropdown-menu__item--divided"));
            informationAction.Click();

            Thread.Sleep(1000);
        }

        public void Original()
        {
            //https://www.cnblogs.com/zhaotianff/p/11330810.html
            using (IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver())
            {
                driver.Navigate().GoToUrl("");  //driver.Url = "http://www.baidu.com"是一样的

                var account = driver.FindElement(By.Id("log_ph"));
                account.Click();
                account.SendKeys("");

                var password = driver.FindElement(By.Id("log_ps"));
                password.Click();
                password.SendKeys("");

                var login = driver.FindElement(By.Id("log_ok"));
                login.Click();

                driver.Navigate().GoToUrl("");

                var page1 = driver.PageSource;
                driver.SwitchTo().Frame(0);
                var page = driver.PageSource;

                var searchButton = driver.FindElement(By.CssSelector(".search-input"));
                searchButton.Click();

                var searchBox = driver.FindElement(By.Id("cc"));
                searchBox.SendKeys("UW123CNMODIFY\nUW456CNMODIFY");//abc\nabc\nabc

                var search = driver.FindElement(By.CssSelector(".button_box> .el-button"));
                search.Click();

                var allSelect = driver.FindElement(By.CssSelector(".has-gutter .el-checkbox__inner"));
                allSelect.Click();

                var informationButton = driver.FindElement(By.CssSelector(".el-dropdown:nth-child(1) span"));
                informationButton.Click();

                Thread.Sleep(1000);

                var informationAction = driver.FindElement(By.CssSelector(".el-dropdown-menu__item--divided"));
                informationAction.Click();

            }

        }
    }
}
