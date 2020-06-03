using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using PiwikPRO.SharePoint.Tests.Classic;
using PiwikPRO.SharePoint.Tests.Pages;
using System.Collections.Generic;
using System.Threading;

namespace PiwikPRO.SharePoint.Tests.SeleniumTests
{
    class SharepointClassicTests : TestBase
    {
        SharepointClassic sharePointSite;
        IJavaScriptExecutor jse;
        [OneTimeSetUp]
        public void BeforeTest()
        {
            var loginPage = new LoginPage(_webDriver);
            sharePointSite = new SharepointClassic(_webDriver);
            jse = (IJavaScriptExecutor)_webDriver;
            loginPage.LoginToSharepoint(sharepointUserToTest, sharepointUserPasswordToTest);
        }
        [SetUp]
        public void ForEachTest()
        {
            _webDriver.Navigate().GoToUrl(homePageClassic);
            Thread.Sleep(1500);
        }
        [Test]
        public void ListItemDirectView()
        {
            _webDriver.Navigate().GoToUrl(listToTestClassic);

            sharePointSite.ListItemDirectView();
            {
                var sharePointSite = new SharepointClassic(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listItemDirectView = null;
                string itemTitle = null;
                string itemUrl = null;
                string listID = null;
                string listTitle = null;
                string listUrl = null;
                string userID = null;
                for (int i = 0; i < 30; i++)
                {
                    listItemDirectView = jse.ExecuteScript("return dataLayer.find(x => x.event === 'listItemDirectView')");
                    if (listItemDirectView != null)
                    {
                        var json = JsonConvert.SerializeObject(listItemDirectView);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("itemTitle", out itemTitle);
                        dictionary.TryGetValue("itemUrl", out itemUrl);
                        dictionary.TryGetValue("listID", out listID);
                        dictionary.TryGetValue("listTitle", out listTitle);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("userID", out userID);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listItemDirectView);
                Assert.NotNull(itemTitle);
                Assert.NotNull(itemUrl);
                Assert.NotNull(listID);
                Assert.NotNull(listTitle);
                Assert.NotNull(listUrl);
                Assert.NotNull(userID);
            }
        }
        [Test]
        public void PageViewEnrichment()
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
        [Test]
        public void ListViewEnrichment()
        {
            _webDriver.Navigate().GoToUrl(listToTestClassic);
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
        public void FolderCreated()
        {
            _webDriver.Navigate().GoToUrl(documentListClassic);
            sharePointSite.FolderCreated();
            Thread.Sleep(1500);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object folderCreated = null;
                string contentType = null;
                string createdBy = null;
                string folderId = null;
                string folderName = null;
                string folderTitle = null;
                string folderUniqueId = null;
                string folderUrl = null;
                string userID = null;

                for (int i = 0; i < 30; i++)
                {
                    folderCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderCreated')");
                    if (folderCreated != null)
                    {
                        var json = JsonConvert.SerializeObject(folderCreated);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("contentType", out contentType);
                        dictionary.TryGetValue("createdBy", out createdBy);
                        dictionary.TryGetValue("folderId", out folderId);
                        dictionary.TryGetValue("folderName", out folderName);
                        dictionary.TryGetValue("folderTitle", out folderTitle);
                        dictionary.TryGetValue("folderUniqueId", out folderUniqueId);
                        dictionary.TryGetValue("folderUrl", out folderUrl);
                        dictionary.TryGetValue("userID", out userID);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(folderCreated);
                Assert.NotNull(contentType);
                Assert.NotNull(createdBy);
                Assert.NotNull(folderId);
                Assert.NotNull(folderName);
                //Assert.NotNull(folderTitle);
                Assert.NotNull(folderUniqueId);
                Assert.NotNull(folderUrl);
                Assert.NotNull(userID);
            }
        }
        [Test]
        public void FileOpened()
        {
            _webDriver.Navigate().GoToUrl(documentListClassic);
            sharePointSite.FileCreated();
            sharePointSite.FileOpened();

            Thread.Sleep(1500);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object fileOpenedOrEdited = null;
                string documentlibraryName = null;
                string documentlibraryUrl = null;
                string fileExt = null;
                string fileName = null;
                string fileUrl = null;
                string folderName = null;
                string folderUrl = null;
                string objectType = null;
                string userID = null;

                for (int i = 0; i < 30; i++)
                {
                    fileOpenedOrEdited = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileOpenedOrEdited')");
                    if (fileOpenedOrEdited != null)
                    {
                        var json = JsonConvert.SerializeObject(fileOpenedOrEdited);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                        dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                        dictionary.TryGetValue("fileExt", out fileExt);
                        dictionary.TryGetValue("fileName", out fileName);
                        dictionary.TryGetValue("fileUrl", out fileUrl);
                        dictionary.TryGetValue("folderName", out folderName);
                        dictionary.TryGetValue("folderUrl", out folderUrl);
                        dictionary.TryGetValue("objectType", out objectType);
                        dictionary.TryGetValue("userID", out userID);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(fileOpenedOrEdited);
                Assert.NotNull(documentlibraryName);
                Assert.NotNull(documentlibraryUrl);
                Assert.NotNull(fileExt);
                Assert.NotNull(fileName);
                Assert.NotNull(fileUrl);
                Assert.NotNull(folderName);
                Assert.NotNull(folderUrl);
                Assert.NotNull(objectType);
                Assert.NotNull(userID);
            }
        }
        [Test]
        public void ListItemAttachmentView()
        {
            _webDriver.Navigate().GoToUrl(listToTestClassic);

            sharePointSite.ListItemAttachmentView();
            {
                var sharePointSite = new SharepointClassic(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object listItemAttachmentView = null;
                string itemTitle = null;
                string itemUrl = null;
                string listId = null;
                string whoViewed = null;
                string listUrl = null;
                string userID = null;
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
                        dictionary.TryGetValue("whoViewed", out whoViewed);
                        dictionary.TryGetValue("listUrl", out listUrl);
                        dictionary.TryGetValue("userID", out userID);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(listItemAttachmentView);
                Assert.NotNull(itemTitle);
                Assert.NotNull(itemUrl);
                Assert.NotNull(listId);
                Assert.NotNull(whoViewed);
                Assert.NotNull(listUrl);
                Assert.NotNull(userID);
            }
        }
    }
}
