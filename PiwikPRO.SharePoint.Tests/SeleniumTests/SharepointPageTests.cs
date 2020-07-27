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
                string pageId = null;
                string pageUrl = null;
                string typeOfContent = null;
                string listId = null;
                string pageName = null;
                string userDisplayName = null;
                string userID = null;
                for (int i = 0; i < 30; i++)
                {
                    pageCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageCreated')");
                    if (pageCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(pageCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("pageName", out pageName);
                        dictionary.TryGetValue("userDisplayName", out userDisplayName);
                        dictionary.TryGetValue("userID", out userID);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageCreated);
                Assert.NotNull(pageId);
                Assert.NotNull(pageUrl);
                Assert.NotNull(typeOfContent);
                Assert.NotNull(listId);
                Assert.NotNull(pageName);
                Assert.NotNull(userDisplayName);
                Assert.NotNull(userID);
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
                string pageId = null;
                string pageUrl = null;
                string typeOfContent = null;
                string listId = null;
                string pageName = null;
                string userDisplayName = null;
                string userID = null;
                for (int i = 0; i < 30; i++)
                {
                    pageCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageCreated')");
                    if (pageCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(pageCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("pageName", out pageName);
                        dictionary.TryGetValue("userDisplayName", out userDisplayName);
                        dictionary.TryGetValue("userID", out userID);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageCreated);
                Assert.NotNull(pageId);
                Assert.NotNull(pageUrl);
                Assert.NotNull(typeOfContent);
                Assert.NotNull(listId);
                Assert.NotNull(pageName);
                Assert.NotNull(userDisplayName);
                Assert.NotNull(userID);
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
                string pageId = null;
                string pageUrl = null;
                string typeOfContent = null;
                string listId = null;
                string pageName = null;
                string userDisplayName = null;
                string userID = null;
                for (int i = 0; i < 30; i++)
                {
                    pageCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageCreated')");
                    if (pageCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(pageCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("pageName", out pageName);
                        dictionary.TryGetValue("userDisplayName", out userDisplayName);
                        dictionary.TryGetValue("userID", out userID);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageCreated);
                Assert.NotNull(pageId);
                Assert.NotNull(pageUrl);
                Assert.NotNull(typeOfContent);
                Assert.NotNull(listId);
                Assert.NotNull(pageName);
                Assert.NotNull(userDisplayName);
                Assert.NotNull(userID);
            }
        }

        [Test]
        public void SitePageEdited()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.SitePageFromHomePageCreation();

            sharePointSite.SitePageEdited();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageCreated = null;
                string pageId = null;
                string pageUrl = null;
                string typeOfContent = null;
                string listId = null;
                string pageName = null;
                string userDisplayName = null;
                string userID = null;
                for (int i = 0; i < 30; i++)
                {
                    pageCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageEdited')");
                    if (pageCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(pageCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("pageName", out pageName);
                        dictionary.TryGetValue("userDisplayName", out userDisplayName);
                        dictionary.TryGetValue("userID", out userID);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageCreated);
                Assert.NotNull(pageId);
                Assert.NotNull(pageUrl);
                Assert.NotNull(typeOfContent);
                Assert.NotNull(listId);
                Assert.NotNull(pageName);
                Assert.NotNull(userDisplayName);
                Assert.NotNull(userID);
            }
        }

        [Test]
        public void NewsPageEdited()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            string newsPostUrl = NewsPostFromHomePageCreation();

            sharePointSite.NewsPageEdited(newsPostUrl);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object newsEdited = null;
                string pageId = null;
                string pageUrl = null;
                string typeOfContent = null;
                string listId = null;
                string pageName = null;
                string userDisplayName = null;
                string userID = null;
                for (int i = 0; i < 30; i++)
                {
                    newsEdited = jse.ExecuteScript("return dataLayer.find(x => x.event === 'newsEdited')");
                    if (newsEdited != null)
                    {
                        var json = JsonConvert.SerializeObject(newsEdited);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("pageName", out pageName);
                        dictionary.TryGetValue("userDisplayName", out userDisplayName);
                        dictionary.TryGetValue("userID", out userID);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(newsEdited);
                Assert.NotNull(pageId);
                Assert.NotNull(pageUrl);
                Assert.NotNull(typeOfContent);
                Assert.NotNull(listId);
                Assert.NotNull(pageName);
                Assert.NotNull(userDisplayName);
                Assert.NotNull(userID);
            }
        }

        [Test]
        public string NewsPostFromHomePageCreation()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);
            string pageUrl = null;
            sharePointSite.NewsPostFromHomePageCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageCreated = null;
                string pageId = null;
                
                string typeOfContent = null;
                string listId = null;
                string pageName = null;
                string userDisplayName = null;
                string userID = null;
                for (int i = 0; i < 30; i++)
                {
                    pageCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'newsPost')");
                    if (pageCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(pageCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("pageName", out pageName);
                        dictionary.TryGetValue("userDisplayName", out userDisplayName);
                        dictionary.TryGetValue("userID", out userID);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageCreated);
                Assert.NotNull(pageId);
                Assert.NotNull(pageUrl);
                Assert.NotNull(typeOfContent);
                Assert.NotNull(listId);
                Assert.NotNull(pageName);
                Assert.NotNull(userDisplayName);
                Assert.NotNull(userID);
            }
            return pageUrl;
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
                //Assert.NotNull(createdBy);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(listId);
                //Assert.NotNull(documentTemplateUrl);
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
                //Assert.NotNull(createdBy);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(listId);
                //Assert.NotNull(documentTemplateUrl);
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
            sharePointSite.ListItemDirectView();

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
        [Test]
        public void NewDocumentLibraryFromHomePageCreation()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.NewDocumentLibraryFromHomePageCreation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object documentLibraryCreated = null;
                string createdBy = null;
                string listTitle = null;
                string listUrl = null;
                string listId = null;
                string documentTemplateUrl = null;

                for (int i = 0; i < 30; i++)
                {
                    documentLibraryCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'documentLibraryCreated')");
                    if (documentLibraryCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(documentLibraryCreated);
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
                Assert.NotNull(documentLibraryCreated);
                //Assert.NotNull(createdBy);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(listId);
               // Assert.NotNull(documentTemplateUrl);
            }
        }
        public void Not_Used_PageEditedFirstSaveFromTopBar()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PageEditedFirstSaveFromTopBar(sitePagesLibrary);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageEdited = null;
                string whoEdited = null;
                string pageTitle = null;
                string typeOfContent = null;
                for (int i = 0; i < 30; i++)
                {
                    pageEdited = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageEdited')");
                    if (pageEdited != null)
                    {
                        var json = JsonConvert.SerializeObject(pageEdited);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoEdited", out whoEdited);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageEdited);
                Assert.NotNull(whoEdited);
                Assert.NotNull(pageTitle);
                Assert.NotNull(typeOfContent);
            }
        }

        public void Not_Used_PageEditedSecondSaveFromTopBar()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PageEditedSecondSaveFromTopBar(sitePagesLibrary);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageEdited = null;
                string whoEdited = null;
                string pageTitle = null;
                string typeOfContent = null;
                for (int i = 0; i < 30; i++)
                {
                    pageEdited = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageEdited')");
                    if (pageEdited != null)
                    {
                        var json = JsonConvert.SerializeObject(pageEdited);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoEdited", out whoEdited);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageEdited);
                Assert.NotNull(whoEdited);
                Assert.NotNull(pageTitle);
                Assert.NotNull(typeOfContent);
            }
        }

        [Test]
        public void PageEditedSaveAndPublishFromTopBar()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PageEditedSaveAndPublishFromTopBar(sitePagesLibrary);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageEdited = null;
                string whoEdited = null;
                string pageTitle = null;
                string typeOfContent = null;
                for (int i = 0; i < 30; i++)
                {
                    pageEdited = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageEdited')");
                    if (pageEdited != null)
                    {
                        var json = JsonConvert.SerializeObject(pageEdited);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoEdited", out whoEdited);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageEdited);
                Assert.NotNull(whoEdited);
                Assert.NotNull(pageTitle);
                Assert.NotNull(typeOfContent);
            }
        }

        public void Not_Used_PageEditedThirdSaveFromTopBar()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PageEditedThirdSaveFromTopBar(sitePagesLibrary);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageEdited = null;
                string whoEdited = null;
                string pageTitle = null;
                string typeOfContent = null;
                for (int i = 0; i < 30; i++)
                {
                    pageEdited = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageEdited')");
                    if (pageEdited != null)
                    {
                        var json = JsonConvert.SerializeObject(pageEdited);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoEdited", out whoEdited);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageEdited);
                Assert.NotNull(whoEdited);
                Assert.NotNull(pageTitle);
                Assert.NotNull(typeOfContent);
            }
        }

        public void Not_Used_PageEditedFourthStopEditingSaveFromTopBar()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PageEditedFourthStopEditingSaveFromTopBar(sitePagesLibrary);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageEdited = null;
                string whoEdited = null;
                string pageTitle = null;
                string typeOfContent = null;
                for (int i = 0; i < 30; i++)
                {
                    pageEdited = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageEdited')");
                    if (pageEdited != null)
                    {
                        var json = JsonConvert.SerializeObject(pageEdited);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoEdited", out whoEdited);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("typeOfContent", out typeOfContent);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageEdited);
                Assert.NotNull(whoEdited);
                Assert.NotNull(pageTitle);
                Assert.NotNull(typeOfContent);
            }
        }

        [Test]
        public void PromoteAddPageToNavigation()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PromoteAddPageToNavigation();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageAddToNavigation = null;
                string whoAddedToNavigation = null;
                string fileUrl = null;
                string fileTitle = null;
                string fileId = null;
                for (int i = 0; i < 30; i++)
                {
                    pageAddToNavigation = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageAddToNavigation')");
                    if (pageAddToNavigation != null)
                    {
                        var json = JsonConvert.SerializeObject(pageAddToNavigation);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoAddedToNavigation", out whoAddedToNavigation);
                        dictionary.TryGetValue("fileUrl", out fileUrl);
                        dictionary.TryGetValue("fileTitle", out fileTitle);
                        dictionary.TryGetValue("fileId", out fileId);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageAddToNavigation);
                Assert.NotNull(whoAddedToNavigation);
                Assert.NotNull(fileUrl);
                Assert.NotNull(fileTitle);
                Assert.NotNull(fileId);
            }
        }
        [Test]
        public void PromoteAddPageToNavigationExistingPage()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PromoteAddPageToNavigationExistingPage();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageAddToNavigation = null;
                string whoAddedToNavigation = null;
                string fileUrl = null;
                string fileTitle = null;
                string fileId = null;
                for (int i = 0; i < 30; i++)
                {
                    pageAddToNavigation = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageAddToNavigation')");
                    if (pageAddToNavigation != null)
                    {
                        var json = JsonConvert.SerializeObject(pageAddToNavigation);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoAddedToNavigation", out whoAddedToNavigation);
                        dictionary.TryGetValue("fileUrl", out fileUrl);
                        dictionary.TryGetValue("fileTitle", out fileTitle);
                        dictionary.TryGetValue("fileId", out fileId);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageAddToNavigation);
                Assert.NotNull(whoAddedToNavigation);
                Assert.NotNull(fileUrl);
                Assert.NotNull(fileTitle);
                Assert.NotNull(fileId);
            }
        }
        [Test]
        public void PromotePostAsNews()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PromotePostAsNews();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pagePromoteAsNews = null;
                string pageUrl = null;
                string pageTitle = null;
                string pageId = null;
                string whoPromoted = null;
                for (int i = 0; i < 30; i++)
                {
                    pagePromoteAsNews = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pagePromoteAsNews')");
                    if (pagePromoteAsNews != null)
                    {
                        var json = JsonConvert.SerializeObject(pagePromoteAsNews);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("whoPromoted", out whoPromoted);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pagePromoteAsNews);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageTitle);
                Assert.NotNull(pageId);
                Assert.NotNull(whoPromoted);
            }
        }
        [Test]
        public void PromotePostAsNewsExistingPage()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PromotePostAsNewsExistingPage();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pagePromoteAsNews = null;
                string pageUrl = null;
                string pageTitle = null;
                string pageId = null;
                string whoPromoted = null;
                for (int i = 0; i < 30; i++)
                {
                    pagePromoteAsNews = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pagePromoteAsNews')");
                    if (pagePromoteAsNews != null)
                    {
                        var json = JsonConvert.SerializeObject(pagePromoteAsNews);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("whoPromoted", out whoPromoted);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pagePromoteAsNews);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageTitle);
                Assert.NotNull(pageId);
                Assert.NotNull(whoPromoted);
            }
        }
        [Test]
        public void PromoteSendByEmail()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PromoteSendByEmail();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pagePromoteEmail = null;
                string pageUrl = null;
                string pageTitle = null;
                string pageId = null;
                string whoPromoted = null;
                for (int i = 0; i < 30; i++)
                {
                    pagePromoteEmail = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pagePromoteEmail')");
                    if (pagePromoteEmail != null)
                    {
                        var json = JsonConvert.SerializeObject(pagePromoteEmail);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("whoPromoted", out whoPromoted);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pagePromoteEmail);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageTitle);
                Assert.NotNull(pageId);
                Assert.NotNull(whoPromoted);
            }
        }
        [Test]
        public void PromoteSendByEmailExistingPage()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PromoteSendByEmailExistingPage();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pagePromoteEmail = null;
                string pageUrl = null;
                string pageTitle = null;
                string pageId = null;
                string whoPromoted = null;
                for (int i = 0; i < 30; i++)
                {
                    pagePromoteEmail = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pagePromoteEmail')");
                    if (pagePromoteEmail != null)
                    {
                        var json = JsonConvert.SerializeObject(pagePromoteEmail);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("whoPromoted", out whoPromoted);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pagePromoteEmail);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageTitle);
                Assert.NotNull(pageId);
                Assert.NotNull(whoPromoted);
            }
        }
        [Test]
        public void PromoteSaveAsPageTemplate()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PromoteSaveAsPageTemplate();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pagePromoteSaveAsTemplate = null;
                string pageUrl = null;
                string pageTitle = null;
                string pageId = null;
                string whoPromoted = null;
                for (int i = 0; i < 30; i++)
                {
                    pagePromoteSaveAsTemplate = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pagePromoteSaveAsTemplate')");
                    if (pagePromoteSaveAsTemplate != null)
                    {
                        var json = JsonConvert.SerializeObject(pagePromoteSaveAsTemplate);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("whoPromoted", out whoPromoted);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pagePromoteSaveAsTemplate);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageTitle);
                Assert.NotNull(pageId);
                Assert.NotNull(whoPromoted);
            }
        }
        [Test]
        public void PromoteSaveAsPageTemplateExistingPage()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PromoteSaveAsPageTemplateExistingPage();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pagePromoteSaveAsTemplate = null;
                string pageUrl = null;
                string pageTitle = null;
                string pageId = null;
                string whoPromoted = null;
                for (int i = 0; i < 30; i++)
                {
                    pagePromoteSaveAsTemplate = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pagePromoteSaveAsTemplate')");
                    if (pagePromoteSaveAsTemplate != null)
                    {
                        var json = JsonConvert.SerializeObject(pagePromoteSaveAsTemplate);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("whoPromoted", out whoPromoted);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pagePromoteSaveAsTemplate);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageTitle);
                Assert.NotNull(pageId);
                Assert.NotNull(whoPromoted);
            }
        }
        [Test]
        public void PromoteCopyAddress()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PromoteCopyAddress();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pagePromoteCopyLink = null;
                string pageUrl = null;
                string pageTitle = null;
                string pageId = null;
                string whoPromoted = null;
                for (int i = 0; i < 30; i++)
                {
                    pagePromoteCopyLink = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pagePromoteCopyLink')");
                    if (pagePromoteCopyLink != null)
                    {
                        var json = JsonConvert.SerializeObject(pagePromoteCopyLink);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("whoPromoted", out whoPromoted);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pagePromoteCopyLink);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageTitle);
                Assert.NotNull(pageId);
                Assert.NotNull(whoPromoted);
            }
        }
   
        public void PromoteCopyAddressExistingPage()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            sharePointSite.PromoteCopyAddressExistingPage();
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pagePromoteCopyLink = null;
                string pageUrl = null;
                string pageTitle = null;
                string pageId = null;
                string whoPromoted = null;
                for (int i = 0; i < 30; i++)
                {
                    pagePromoteCopyLink = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pagePromoteCopyLink')");
                    if (pagePromoteCopyLink != null)
                    {
                        var json = JsonConvert.SerializeObject(pagePromoteCopyLink);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("whoPromoted", out whoPromoted);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pagePromoteCopyLink);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageTitle);
                Assert.NotNull(pageId);
                Assert.NotNull(whoPromoted);
            }
        }
        [Test]
        public void PageCopyLink()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            string pageName = sharePointSite.SitePageFromHomePageCreation();
            _webDriver.Navigate().GoToUrl(sitePagesLibrary);
            sharePointSite.PageCopyLink(pageName);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object fileFolderPageCopyLink = null;
                string whoCopied = null;
                string elUrl = null;
                string elTitle = null;
                string elId = null;
                string elUniqueId = null;
                string objectType = null;
                for (int i = 0; i < 30; i++)
                {
                    fileFolderPageCopyLink = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileFolderPageCopyLink')");
                    if (fileFolderPageCopyLink != null)
                    {
                        var json = JsonConvert.SerializeObject(fileFolderPageCopyLink);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoCopied", out whoCopied);
                        dictionary.TryGetValue("elUrl", out elUrl);
                        dictionary.TryGetValue("elTitle", out elTitle);
                        dictionary.TryGetValue("elId", out elId);
                        dictionary.TryGetValue("elUniqueId", out elUniqueId);
                        dictionary.TryGetValue("objectType", out objectType);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(fileFolderPageCopyLink);
                Assert.NotNull(whoCopied);
                Assert.NotNull(elUrl);
                Assert.NotNull(elTitle);
                Assert.NotNull(elId);
                Assert.NotNull(elUniqueId);
                Assert.NotNull(objectType);
            }
        }
        [Test]
        public void PagePinToTopAndUnpinned()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            string pagename = sharePointSite.SitePageFromHomePageCreation();
            _webDriver.Navigate().GoToUrl(sitePagesLibrary);
            sharePointSite.PagePinToTopAndUnpinned(pagename);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pagePinnedToTop = null;
                string whoPinned = null;
                string pageUrlPinnedToTop = null;
                string pageNamePinnedToTop = null;
                string pageTitlePinnedToTop = null;
                string pageIdPinnedToTop = null;

                object pageUnpinned = null;
                string whoUnpinned = null;
                string pageUrlUnpinned = null;
                string pageNameUnpinned = null;
                string pageTitleUnpinned = null;
                string pageIdUnpinned = null;
                for (int i = 0; i < 30; i++)
                {
                    pagePinnedToTop = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pagePinnedToTop')");
                    if (pagePinnedToTop != null)
                    {
                        var json = JsonConvert.SerializeObject(pagePinnedToTop);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoPinned", out whoPinned);
                        dictionary.TryGetValue("pageUrl", out pageUrlPinnedToTop);
                        dictionary.TryGetValue("pageName", out pageNamePinnedToTop);
                        dictionary.TryGetValue("pageTitle", out pageTitlePinnedToTop);
                        dictionary.TryGetValue("pageId", out pageIdPinnedToTop);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Thread.Sleep(500);
                for (int i = 0; i < 30; i++)
                {
                    pageUnpinned = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageUnpinned')");
                    if (pageUnpinned != null)
                    {
                        var json = JsonConvert.SerializeObject(pageUnpinned);
                        Dictionary<string, string> dictionary2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary2.TryGetValue("whoUnpinned", out whoUnpinned);
                        dictionary2.TryGetValue("pageUrl", out pageUrlUnpinned);
                        dictionary2.TryGetValue("pageName", out pageNameUnpinned);
                        dictionary2.TryGetValue("pageTitle", out pageTitleUnpinned);
                        dictionary2.TryGetValue("pageId", out pageIdUnpinned);
                        break;
                    }
                    Thread.Sleep(100);
                }

                Assert.NotNull(pagePinnedToTop);
                Assert.NotNull(whoPinned);
                Assert.NotNull(pageUrlPinnedToTop);
                Assert.NotNull(pageNamePinnedToTop);
                Assert.NotNull(pageTitlePinnedToTop);
                Assert.NotNull(pageIdPinnedToTop);

                Assert.NotNull(pageUnpinned);
                Assert.NotNull(whoUnpinned);
                Assert.NotNull(pageUrlUnpinned);
                Assert.NotNull(pageNameUnpinned);
                Assert.NotNull(pageTitleUnpinned);
                Assert.NotNull(pageIdUnpinned);
            }
        }
        [Test]
        public void PageMakeHomePage()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            string pagename = sharePointSite.SitePageFromHomePageCreation();
            _webDriver.Navigate().GoToUrl(sitePagesLibrary);
            sharePointSite.PageMakeHomePage(pagename);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageMakeHomePage = null;
                string whoSetHomePage = null;
                string pageUrl = null;
                string pageName = null;
                string pageTitle = null;
                string pageId = null;
                for (int i = 0; i < 30; i++)
                {
                    pageMakeHomePage = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageMakeHomePage')");
                    if (pageMakeHomePage != null)
                    {
                        var json = JsonConvert.SerializeObject(pageMakeHomePage);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoSetHomePage", out whoSetHomePage);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageName", out pageName);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageId", out pageId);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageMakeHomePage);
                Assert.NotNull(whoSetHomePage);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageName);
                Assert.NotNull(pageTitle);
                Assert.NotNull(pageId);
            }
        }
        [Test]
        public void PageDelete()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            string pagename = sharePointSite.SitePageFromHomePageCreation();
            _webDriver.Navigate().GoToUrl(sitePagesLibrary);
            sharePointSite.PageDelete(pagename);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageDeleted = null;
                string pageWhoCreated = null;
                string pageName = null;
                string filesize = null;
                string deletionDate = null;
                string pageTitle = null;
                string pageUrl = null;
                for (int i = 0; i < 30; i++)
                {
                    pageDeleted = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageDeleted')");
                    if (pageDeleted != null)
                    {
                        var json = JsonConvert.SerializeObject(pageDeleted);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("pageWhoCreated", out pageWhoCreated);
                        dictionary.TryGetValue("pageName", out pageName);
                        dictionary.TryGetValue("filesize", out filesize);
                        dictionary.TryGetValue("deletionDate", out deletionDate);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageDeleted);
                Assert.NotNull(pageWhoCreated);
                Assert.NotNull(pageName);
                Assert.NotNull(filesize);
                Assert.NotNull(deletionDate);
                Assert.NotNull(pageTitle);
                Assert.NotNull(pageUrl);
            }
        }
        [Test]
        public void ListItemShare()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);
            sharePointSite.NewListFromHomePageCreation();
            Thread.Sleep(1000);
            ;
            sharePointSite.ListItemShare(sharePointSite.NewListItemCreation());

            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object itemShared = null;
                string whoShared = null;
                string listUrl = null;
                string listTitle = null;
                string listId = null;
                string itemTitle = null;
                string itemUrl = null;

                for (int i = 0; i < 30; i++)
                {
                    itemShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'itemShared')");
                    if (itemShared != null)
                    {
                        var json = JsonConvert.SerializeObject(itemShared);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("whoShared", out whoShared);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("itemTitle", out itemTitle);
                        dictionary.TryGetValue("itemUrl", out itemUrl);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(itemShared);
                Assert.NotNull(whoShared);
                Assert.NotNull(listUrl);
                Assert.NotNull(listTitle);
                Assert.NotNull(listId);
                Assert.NotNull(itemTitle);
                Assert.NotNull(itemUrl);

            }
        }
        [Test]
        public void PageView()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageViewEnrichment = null;
                string jobTitle = null;
                string pageID = null;
                string pageUrl = null;
                string pageWebId = null;
                string pageWebTitle = null;
                string sitecolectionID = null;
                string sitecolletionTitle = null;
                string userDepartment = null;
                string userID = null;
                string userOffice = null;

                for (int i = 0; i < 30; i++)
                {
                    pageViewEnrichment = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageViewEnrichment')");
                    if (pageViewEnrichment != null)
                    {
                        var json = JsonConvert.SerializeObject(pageViewEnrichment);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("jobTitle", out jobTitle);
                        dictionary.TryGetValue("pageID", out pageID);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageWebId", out pageWebId);
                        dictionary.TryGetValue("pageWebTitle", out pageWebTitle);
                        dictionary.TryGetValue("sitecolectionID", out sitecolectionID);
                        dictionary.TryGetValue("sitecolletionTitle", out sitecolletionTitle);
                        dictionary.TryGetValue("userDepartment", out userDepartment);
                        dictionary.TryGetValue("userID", out userID);
                        dictionary.TryGetValue("userOffice", out userOffice);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageViewEnrichment);
                //Assert.NotNull(jobTitle);
                Assert.NotNull(pageID);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageWebId);
                Assert.NotNull(pageWebTitle);
                Assert.NotNull(sitecolectionID);
                Assert.NotNull(sitecolletionTitle);
                //Assert.NotNull(userDepartment);
                Assert.NotNull(userID);
                //Assert.NotNull(userOffice);

            }
        }
        [Test]
        public void ListView()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            sharePointSite.NewListFromHomePageCreation();
            //Thread.Sleep(1000);
            //sharePointSite.NewListItemCreation();
            Thread.Sleep(1500);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listViewEnrichment = null;
                string jobTitle = null;
                string listUrl = null;
                string listWebId = null;
                string listWebTitle = null;
                string sitecolectionID = null;
                string sitecolletionTitle = null;
                string userDepartment = null;
                string userID = null;
                string userOffice = null;

                for (int i = 0; i < 30; i++)
                {
                    listViewEnrichment = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listViewEnrichment')");
                    if (listViewEnrichment != null)
                    {
                        var json = JsonConvert.SerializeObject(listViewEnrichment);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("jobTitle", out jobTitle);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("listWebId", out listWebId);
                        dictionary.TryGetValue("listWebTitle", out listWebTitle);
                        dictionary.TryGetValue("sitecolectionID", out sitecolectionID);
                        dictionary.TryGetValue("sitecolletionTitle", out sitecolletionTitle);
                        dictionary.TryGetValue("userDepartment", out userDepartment);
                        dictionary.TryGetValue("userID", out userID);
                        dictionary.TryGetValue("userOffice", out userOffice);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listViewEnrichment);
                //Assert.NotNull(jobTitle);
                //Assert.NotNull(pageID);
                Assert.NotNull(listUrl);
                Assert.NotNull(listWebId);
                Assert.NotNull(listWebTitle);
                Assert.NotNull(sitecolectionID);
                Assert.NotNull(sitecolletionTitle);
                //Assert.NotNull(userDepartment);
                Assert.NotNull(userID);
                //Assert.NotNull(userOffice);

            }
        }
        [Test]
        public void PageShared()
        {
            _webDriver.Navigate().GoToUrl(homePage);
            Thread.Sleep(1500);

            string pagename = sharePointSite.SitePageFromHomePageCreation();
            _webDriver.Navigate().GoToUrl(sitePagesLibrary);
            sharePointSite.PageShared(pagename);

            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object pageShared = null;
                string userID = null;
                string sharedWith = null;
                string contentType = null;
                string pageUrl = null;
                string pageName = null;
                string pageTitle = null;
                string pageId = null;
                string typeOfShare = null;

                for (int i = 0; i < 30; i++)
                {
                    pageShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageShared')");
                    if (pageShared != null)
                    {
                        var json = JsonConvert.SerializeObject(pageShared);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("userID", out userID);
                        dictionary.TryGetValue("sharedWith", out sharedWith);
                        dictionary.TryGetValue("contentType", out contentType);
                        dictionary.TryGetValue("pageUrl", out pageUrl);
                        dictionary.TryGetValue("pageName", out pageName);
                        dictionary.TryGetValue("pageTitle", out pageTitle);
                        dictionary.TryGetValue("pageId", out pageId);
                        dictionary.TryGetValue("typeOfShare", out typeOfShare);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(pageShared);
                Assert.NotNull(userID);
                //Assert.NotNull(sharedWith);
                Assert.NotNull(contentType);
                Assert.NotNull(pageUrl);
                Assert.NotNull(pageName);
                //Assert.NotNull(pageTitle);
                Assert.NotNull(pageId);
                Assert.NotNull(typeOfShare);
                Assert.NotNull(userID);
            }
        }
        [Test]
        public void ListItemAttachmentView()
        {
            _webDriver.Navigate().GoToUrl(listToTest);
            sharePointSite.ListItemAttachmentView();

            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listItemAttachmentView = null;
                string itemTitle = null;
                string itemUrl = null;
                string listId = null;
                string listUrl = null;
                string userID = null;
                string whoViewed = null;


                for (int i = 0; i < 30; i++)
                {
                    listItemAttachmentView = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listItemAttachmentView')");
                    if (listItemAttachmentView != null)
                    {
                        var json = JsonConvert.SerializeObject(listItemAttachmentView);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("itemTitle", out itemTitle);
                        dictionary.TryGetValue("itemUrl", out itemUrl);
                        dictionary.TryGetValue("listId", out listId);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("userID", out userID);
                        dictionary.TryGetValue("whoViewed", out whoViewed);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listItemAttachmentView);
                Assert.NotNull(itemTitle);
                Assert.NotNull(itemUrl);
                Assert.NotNull(listId);
                Assert.NotNull(listUrl);
                Assert.NotNull(userID);
                Assert.NotNull(whoViewed);
            }
        }

    }
}
