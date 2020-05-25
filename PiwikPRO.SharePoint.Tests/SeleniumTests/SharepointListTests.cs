using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using PiwikPRO.SharePoint.Tests.Lists;
using PiwikPRO.SharePoint.Tests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.Tests.SeleniumTests
{
    class SharepointListTests : TestBase
    {
        IJavaScriptExecutor jse;
        SharepointList spList;
        [OneTimeSetUp]
        public void BeforeTest()
        {
            var loginPage = new LoginPage(_webDriver);
            jse = (IJavaScriptExecutor)_webDriver;
            loginPage.LoginToSharepoint(sharepointUserToTest, sharepointUserPasswordToTest);
            spList = new SharepointList(_webDriver);
        }
        [SetUp]
        public void ForEachTest()
        {
            _webDriver.Navigate().GoToUrl(listToTest);
            Thread.Sleep(1500);
        }

        [Test]
        public void ListQuickEdit()
        {
            spList.QuickEdit();
            {
                var search2 = new Search(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listUpdatedEvent = null;
                string listId = null;
                string listTitle = null;
                string listUrl = null;
                string createdBy = null;

                for (int i = 0; i < 30; i++)
                {
                    listUpdatedEvent = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listUpdated')");
                    if (listUpdatedEvent != null)
                    {
                        var json = JsonConvert.SerializeObject(listUpdatedEvent);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listUpdatedEvent);
                Assert.NotNull(listId);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(createdBy);
            }
        }

        [Test]
        public void ListAddNewItem()
        {
            spList.AddNewItem();
            {
                var search2 = new Search(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listUpdatedEvent = null;
                string listId = null;
                string listTitle = null;
                string listUrl = null;
                string createdBy = null;

                for (int i = 0; i < 30; i++)
                {
                    listUpdatedEvent = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listUpdated')");
                    if (listUpdatedEvent != null)
                    {
                        var json = JsonConvert.SerializeObject(listUpdatedEvent);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listUpdatedEvent);
                Assert.NotNull(listId);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(createdBy);
            }
        }

        [Test]
        public void ListAddNewItemFromTopSave()
        {
            spList.AddNewItemFromTopSave();
            {
                var search2 = new Search(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listUpdatedEvent = null;
                string listId = null;
                string listTitle = null;
                string listUrl = null;
                string createdBy = null;

                for (int i = 0; i < 30; i++)
                {
                    listUpdatedEvent = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listUpdated')");
                    if (listUpdatedEvent != null)
                    {
                        var json = JsonConvert.SerializeObject(listUpdatedEvent);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listUpdatedEvent);
                Assert.NotNull(listId);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(createdBy);
            }
        }
    }
}
