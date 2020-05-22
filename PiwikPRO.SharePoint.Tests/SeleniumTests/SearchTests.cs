using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using PiwikPRO.SharePoint.Tests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.Tests.SeleniumTests
{
    class SearchTests : TestBase
    {
        IJavaScriptExecutor jse;
        Search search;
        [OneTimeSetUp]
        public void BeforeTest()
        {
            var loginPage = new LoginPage(_webDriver);
            jse = (IJavaScriptExecutor)_webDriver;
            loginPage.LoginToSharepoint(sharepointUserToTest, sharepointUserPasswordToTest);
            search = new Search(_webDriver);
        }
        [SetUp]
        public void ForEachTest()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);
        }

        [Test]
        public void SearchInputType()
        {
            search.SearchFromTopSearch();
            {
                var search2 = new Search(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object searchEvent = null;
                string count = null;
                string queryString = null;
                string userLogin = null;

                for (int i = 0; i < 30; i++)
                {
                    searchEvent = jse.ExecuteScript("return dataLayer.find(x => x.event === 'search')");
                    if (searchEvent != null)
                    {
                        var json = JsonConvert.SerializeObject(searchEvent);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("count", out count);
                        dictionary.TryGetValue("queryString", out queryString);
                        dictionary.TryGetValue("userLogin", out userLogin);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(searchEvent);
                Assert.NotNull(queryString);
                Assert.NotNull(count);
                Assert.NotNull(userLogin);
            }
        }
    }
}
