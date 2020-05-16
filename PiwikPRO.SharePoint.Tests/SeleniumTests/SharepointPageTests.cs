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
        [OneTimeSetUp]
        public void BeforeTest()
        {
            var loginPage = new LoginPage(_webDriver);
            sharePointSite = new SharepointPage(_webDriver);
            jse = (IJavaScriptExecutor)_webDriver;
            loginPage.LoginToSharepoint(sharepointUserToTest, sharepointUserPasswordToTest);
        }
        [SetUp]
        public void ForEachTest()
        {
            _webDriver.Navigate().GoToUrl(sitePagesLibrary);
            Thread.Sleep(1500);
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
            Thread.Sleep(1500);

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
            Thread.Sleep(1500);

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
        [Test]
        public void NewsLinkFromHomePageCreation()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.NewsLinkFromHomePageCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object newsLink = null;
                string postTitle = null;
                string contentType = null;
                string postId = null;
                string postUrl = null;
     
                for (int i = 0; i < 30; i++)
                {
                    newsLink = jse.ExecuteScript("return dataLayer.find(x => x.event === 'newsLink')");
                    if (newsLink != null)
                    {
                        var json = JsonConvert.SerializeObject(newsLink);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("postTitle", out postTitle);
                        dictionary.TryGetValue("contentType", out contentType);
                        dictionary.TryGetValue("postId", out postId);
                        dictionary.TryGetValue("postUrl", out postUrl);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(newsLink);
                Assert.NotNull(postTitle);
                Assert.NotNull(contentType);
                Assert.NotNull(postId);
                Assert.NotNull(postUrl);
            }
        }
        [Test]
        public void NewListFromHomePageCreation()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.NewListFromHomePageCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listCreated = null;
                string createdBy = null;
                string listTitle = null;
                string listUrl = null;
                string listId = null;
                string documentTemplateUrl = null;

                for (int i = 0; i < 30; i++)
                {
                    listCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listCreated')");
                    if (listCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(listCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("documentTemplateUrl", out documentTemplateUrl);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listCreated);
                Assert.NotNull(createdBy);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(listId);
                Assert.NotNull(documentTemplateUrl);
            }
        }
        [Test]
        public void NewListFromSiteContentsCreation()
        {
            _webDriver.Navigate().GoToUrl(siteContents);
            Thread.Sleep(1500);

            sharePointSite.NewListFromSiteContentsCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listCreated = null;
                string createdBy = null;
                string listTitle = null;
                string listUrl = null;
                string listId = null;
                string documentTemplateUrl = null;

                for (int i = 0; i < 30; i++)
                {
                    listCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listCreated')");
                    if (listCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(listCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("documentTemplateUrl", out documentTemplateUrl);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listCreated);
                Assert.NotNull(createdBy);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(listId);
                Assert.NotNull(documentTemplateUrl);
            }
        }
        [Test]
        public void NewSubsiteFromSiteContentsCreation()
        {
            _webDriver.Navigate().GoToUrl(siteContents);
            Thread.Sleep(1500);

            sharePointSite.NewSubsiteFromSiteContentsCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object subsiteCreated = null;
                string pageName = null;
                string createdBy = null;
                string template = null;

                for (int i = 0; i < 30; i++)
                {
                    subsiteCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'subsiteCreated')");
                    if (subsiteCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(subsiteCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageName", out pageName);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        dictionary.TryGetValue("template", out template);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(subsiteCreated);
                Assert.NotNull(pageName);
                Assert.NotNull(createdBy);
                Assert.NotNull(template);

            }
        }
        [Test]
        public void DeleteList()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);
            string listname = sharePointSite.NewListFromHomePageCreation();
            //_webDriver.Navigate().GoToUrl(siteContents);

            sharePointSite.DeleteList(listname);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listDeleted = null;
                string deletedBy = null;
                string listId = null;

                for (int i = 0; i < 30; i++)
                {
                    listDeleted = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listDeleted')");
                    if (listDeleted != null)
                    {
                        var json = JsonConvert.SerializeObject(listDeleted);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("deletedBy", out deletedBy);
                        dictionary.TryGetValue("listId", out listId);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listDeleted);
                Assert.NotNull(deletedBy);
                Assert.NotNull(listId);

            }
        }
        [Test]
        public void ListItemDirectView()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);
            sharePointSite.NewListFromHomePageCreation();
            Thread.Sleep(1000);
            sharePointSite.NewListItemCreation();

            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listItemDirectView = null;
                string listUrl = null;
                string listTitle = null;
                string listID = null;
                string itemTitle = null;
                string itemUrl = null;

                for (int i = 0; i < 30; i++)
                {
                    listItemDirectView = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listItemDirectView')");
                    if (listItemDirectView != null)
                    {
                        var json = JsonConvert.SerializeObject(listItemDirectView);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listID", out listID);
                        dictionary.TryGetValue("itemTitle", out itemTitle);
                        dictionary.TryGetValue("itemUrl", out itemUrl);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listItemDirectView);
                Assert.NotNull(listUrl);
                Assert.NotNull(listTitle);
                Assert.NotNull(listID);
                Assert.NotNull(itemTitle);
                Assert.NotNull(itemUrl);

            }
        }

    }
}
