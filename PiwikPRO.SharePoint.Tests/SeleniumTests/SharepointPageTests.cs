using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using PiwikPRO.SharePoint.Tests.Pages;
using System.Collections.Generic;
using System.Threading;

namespace PiwikPRO.SharePoint.Tests.SeleniumTests
{
    class SharepointPageTests : TestBase
    {
        SharepointPage sharePointSite;
        IJavaScriptExecutor jse;
        [SetUp]
        public void BeforeTest()
        {
            var loginPage = new LoginPage(_webDriver);
            sharePointSite = new SharepointPage(_webDriver);
            jse = (IJavaScriptExecutor)_webDriver;
            loginPage.LoginToSharepoint(sharepointUserToTest, sharepointUserPasswordToTest);
            _webDriver.Navigate().GoToUrl(sitePagesLibrary);
        }

        [Test]
        public void WikiPageFromSitePagesLibraryCreation()
        {
            sharePointSite.WikiPageCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageCreated = null;
                string fileUrl = null;
                string filename = null;
                string contentTypeId = null;
                string fileId = null;
                string listId = null;
                string contentType = null;
                for (int i = 0; i < 30; i++)
                {
                    pageCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageCreated')");
                    if (pageCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(pageCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("fileUrl", out fileUrl);
                        dictionary.TryGetValue("filename", out filename);
                        dictionary.TryGetValue("contentTypeId", out contentTypeId);
                        dictionary.TryGetValue("fileId", out fileId);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("contentType", out contentType);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageCreated);
                Assert.NotNull(fileUrl);
                Assert.NotNull(filename);
                Assert.NotNull(contentTypeId);
                Assert.NotNull(fileId);
                Assert.NotNull(listId);
                Assert.NotNull(contentType);
            }
        }
        [Test]
        public void WebPartPageFromSitePagesLibraryCreation()
        {
            sharePointSite.WebPartPageCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageCreated = null;
                string fileUrl = null;
                string filename = null;
                string contentTypeId = null;
                string fileId = null;
                string listId = null;
                string contentType = null;
                for (int i = 0; i < 30; i++)
                {
                    pageCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageCreated')");
                    if (pageCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(pageCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("fileUrl", out fileUrl);
                        dictionary.TryGetValue("filename", out filename);
                        dictionary.TryGetValue("contentTypeId", out contentTypeId);
                        dictionary.TryGetValue("fileId", out fileId);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("contentType", out contentType);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageCreated);
                Assert.NotNull(fileUrl);
                Assert.NotNull(filename);
                Assert.NotNull(contentTypeId);
                Assert.NotNull(fileId);
                Assert.NotNull(listId);
                Assert.NotNull(contentType);
            }
        }

        [Test]
        public void SitePageFromSitePagesLibraryCreation()
        {
            sharePointSite.SitePageCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageCreated = null;
                string fileId = null;
                string fileUrl = null;
                string contentTypeId = null;
                string listId = null;
                string pageTitle = null;
                string createdBy = null;
                string contentType = null;
                for (int i = 0; i < 30; i++)
                {
                    pageCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageCreated')");
                    if (pageCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(pageCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("fileId", out fileId);
                        dictionary.TryGetValue("fileUrl", out fileUrl);
                        dictionary.TryGetValue("contentTypeId", out contentTypeId);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        dictionary.TryGetValue("contentType", out contentType);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageCreated);
                Assert.NotNull(fileId);
                Assert.NotNull(fileUrl);
                Assert.NotNull(contentTypeId);
                Assert.NotNull(listId);
                Assert.NotNull(pageTitle);
                Assert.NotNull(createdBy);
                Assert.NotNull(contentType);
            }
        }

        [Test]
        public void SitePageFromSiteContentsCreation()
        {
            _webDriver.Navigate().GoToUrl(siteContents);
            sharePointSite.SitePageFromSiteContentsCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageCreated = null;
                string fileId = null;
                string fileUrl = null;
                string contentTypeId = null;
                string listId = null;
                string pageTitle = null;
                string createdBy = null;
                string contentType = null;
                for (int i = 0; i < 30; i++)
                {
                    pageCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageCreated')");
                    if (pageCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(pageCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("fileId", out fileId);
                        dictionary.TryGetValue("fileUrl", out fileUrl);
                        dictionary.TryGetValue("contentTypeId", out contentTypeId);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        dictionary.TryGetValue("contentType", out contentType);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageCreated);
                Assert.NotNull(fileId);
                Assert.NotNull(fileUrl);
                Assert.NotNull(contentTypeId);
                Assert.NotNull(listId);
                Assert.NotNull(pageTitle);
                Assert.NotNull(createdBy);
                Assert.NotNull(contentType);
            }
        }
        [Test]
        public void SitePageFromHomePageCreation()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            sharePointSite.SitePageFromHomePageCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageCreated = null;
                string fileId = null;
                string fileUrl = null;
                string contentTypeId = null;
                string listId = null;
                string pageTitle = null;
                string createdBy = null;
                string contentType = null;
                for (int i = 0; i < 30; i++)
                {
                    pageCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageCreated')");
                    if (pageCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(pageCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("fileId", out fileId);
                        dictionary.TryGetValue("fileUrl", out fileUrl);
                        dictionary.TryGetValue("contentTypeId", out contentTypeId);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        dictionary.TryGetValue("contentType", out contentType);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageCreated);
                Assert.NotNull(fileId);
                Assert.NotNull(fileUrl);
                Assert.NotNull(contentTypeId);
                Assert.NotNull(listId);
                Assert.NotNull(pageTitle);
                Assert.NotNull(createdBy);
                Assert.NotNull(contentType);
            }
        }

    }
}
