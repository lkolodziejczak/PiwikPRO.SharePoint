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
    class SharepointClassicListsTests : TestBase
    {
        IJavaScriptExecutor jse;
        SharepointClassicLists spList;
        [OneTimeSetUp]
        public void BeforeTest()
        {
            var loginPage = new LoginPage(_webDriver);
            jse = (IJavaScriptExecutor)_webDriver;
            loginPage.LoginToSharepoint(sharepointUserToTest, sharepointUserPasswordToTest);
            spList = new SharepointClassicLists(_webDriver);
        }
        [SetUp]
        public void ForEachTest()
        {
            _webDriver.Navigate().GoToUrl(siteContentsClassicToTest);
            Thread.Sleep(1500);
        }

        [Test]
        public void CreateClassicListFromSiteContents()
        {
            spList.CreateClassicListFromSiteContents();
            {
                var search2 = new Search(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listCreatedEvent = null;
                string listId = null;
                string listTitle = null;
                string listUrl = null;
                string createdBy = null;

                for (int i = 0; i < 30; i++)
                {
                    listCreatedEvent = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listCreated')");
                    if (listCreatedEvent != null)
                    {
                        var json = JsonConvert.SerializeObject(listCreatedEvent);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("listID", out listId);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listURL", out listUrl);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listCreatedEvent);
                Assert.NotNull(listId);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(createdBy);
            }
        }

        [Test]
        public void CreateClassicListFromSiteContentsAdvancedMode()
        {
            spList.CreateClassicListFromSiteContentsAdvancedMode();
            {
                var search2 = new Search(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listCreatedEvent = null;
                string listId = null;
                string listTitle = null;
                string listUrl = null;
                string createdBy = null;

                for (int i = 0; i < 30; i++)
                {
                    listCreatedEvent = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listCreated')");
                    if (listCreatedEvent != null)
                    {
                        var json = JsonConvert.SerializeObject(listCreatedEvent);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("listID", out listId);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listURL", out listUrl);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listCreatedEvent);
                Assert.NotNull(listId);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(createdBy);
            }
        }

        [Test]
        public void CreateClassicLibraryFromSiteContents()
        {
            spList.CreateClassicLibraryFromSiteContents();
            {
                var search2 = new Search(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listCreatedEvent = null;
                string listId = null;
                string listTitle = null;
                string listUrl = null;
                string createdBy = null;

                for (int i = 0; i < 30; i++)
                {
                    listCreatedEvent = jse.ExecuteScript("return dataLayer.find(x => x.event === 'documentLibraryCreated')");
                    if (listCreatedEvent != null)
                    {
                        var json = JsonConvert.SerializeObject(listCreatedEvent);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("listID", out listId);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listURL", out listUrl);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listCreatedEvent);
                Assert.NotNull(listId);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(createdBy);
            }
        }

        [Test]
        public void CreateClassicLibraryFromSiteContentsAdvancedMode()
        {
            spList.CreateClassicLibraryFromSiteContentsAdvancedMode();
            {
                var search2 = new Search(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listCreatedEvent = null;
                string listId = null;
                string listTitle = null;
                string listUrl = null;
                string createdBy = null;

                for (int i = 0; i < 30; i++)
                {
                    listCreatedEvent = jse.ExecuteScript("return dataLayer.find(x => x.event === 'documentLibraryCreated')");
                    if (listCreatedEvent != null)
                    {
                        var json = JsonConvert.SerializeObject(listCreatedEvent);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("listID", out listId);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listURL", out listUrl);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listCreatedEvent);
                Assert.NotNull(listId);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(createdBy);
            }
        }

        [Test]
        public void DeleteClassicListFromListSettings()
        {
            spList.DeleteClassicList();
            {
                var search2 = new Search(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listDeletedEvent = null;
                string listId = null;
                string listTitle = null;
                string listUrl = null;
                string deletedBy = null;

                for (int i = 0; i < 30; i++)
                {
                    listDeletedEvent = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listDeleted')");
                    if (listDeletedEvent != null)
                    {
                        var json = JsonConvert.SerializeObject(listDeletedEvent);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("deletedBy", out deletedBy);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listDeletedEvent);
                Assert.NotNull(listId);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(deletedBy);
            }
        }

    }
}
