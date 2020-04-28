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
    class SharepointFolderFilesTests : TestBase
    {
        SharepointFolderFile sharePointSite;
        IJavaScriptExecutor jse;
       [SetUp]
        public void BeforeTest()
        {
            var loginPage = new LoginPage(_webDriver);
            sharePointSite = new SharepointFolderFile(_webDriver);
            jse = (IJavaScriptExecutor)_webDriver;
            loginPage.LoginToSharepoint(sharepointUserToTest, sharepointUserPasswordToTest);
            _webDriver.Navigate().GoToUrl(documentListWithFileUrl);
        }

        [Test]
        public void FilePinToTopFromTopMenu()
        {
            sharePointSite.FilePinToTopFromTopMenu();

            Thread.Sleep(1500);

            object filePinnedToTop = null;
            string fileFilename = null;
            string userLogin = null;
            string fileId = null;
            string fileRelativeUrl = null;
            string fileTitle = null;
            string whoPinned = null;

            filePinnedToTop = jse.ExecuteScript("return dataLayer.find(x => x.event === 'filePinnedToTop')");
            if (filePinnedToTop != null)
            {
                var json = JsonConvert.SerializeObject(filePinnedToTop);
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                dictionary.TryGetValue("userLogin", out userLogin);
                dictionary.TryGetValue("fileFilename", out fileFilename);
                dictionary.TryGetValue("fileId", out fileId);
                dictionary.TryGetValue("fileRelativeUrl", out fileRelativeUrl);
                dictionary.TryGetValue("fileTitle", out fileTitle);
                dictionary.TryGetValue("whoPinned", out whoPinned);
            }

            Assert.NotNull(filePinnedToTop);
            Assert.NotNull(fileFilename);
            Assert.NotNull(userLogin);
            Assert.NotNull(fileId);
            Assert.NotNull(fileRelativeUrl);
            Assert.NotNull(fileTitle);
            Assert.NotNull(whoPinned);
        }

        [Test]
        public void FilePinToTopFromContextMenu()
        {
            sharePointSite.FilePinToTopFromContextMenu();

            Thread.Sleep(1500);

            object filePinnedToTop = null;
            string fileFilename = null;
            string userLogin = null;
            string fileId = null;
            string fileRelativeUrl = null;
            string fileTitle = null;
            string whoPinned = null;

            filePinnedToTop = jse.ExecuteScript("return dataLayer.find(x => x.event === 'filePinnedToTop')");
            if (filePinnedToTop != null)
            {
                var json = JsonConvert.SerializeObject(filePinnedToTop);
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                dictionary.TryGetValue("userLogin", out userLogin);
                dictionary.TryGetValue("fileFilename", out fileFilename);
                dictionary.TryGetValue("fileId", out fileId);
                dictionary.TryGetValue("fileRelativeUrl", out fileRelativeUrl);
                dictionary.TryGetValue("fileTitle", out fileTitle);
                dictionary.TryGetValue("whoPinned", out whoPinned);
            }

            Assert.NotNull(filePinnedToTop);
            Assert.NotNull(fileFilename);
            Assert.NotNull(userLogin);
            Assert.NotNull(fileId);
            Assert.NotNull(fileRelativeUrl);
            Assert.NotNull(fileTitle);
            Assert.NotNull(whoPinned);
        }

        [Test]
        public void FileUnPinToTopFromTopMenu()
        {
            sharePointSite.FileUnPinToTopFromTopMenu();

            Thread.Sleep(1500);

            object fileUnpinned = null;
            string fileFilename = null;
            string userLogin = null;
            string fileId = null;
            string fileRelativeUrl = null;
            string fileTitle = null;
            string whoUnpinned = null;

            fileUnpinned = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileUnpinned')");
            if (fileUnpinned != null)
            {
                var json = JsonConvert.SerializeObject(fileUnpinned);
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                dictionary.TryGetValue("userLogin", out userLogin);
                dictionary.TryGetValue("fileFilename", out fileFilename);
                dictionary.TryGetValue("fileId", out fileId);
                dictionary.TryGetValue("fileRelativeUrl", out fileRelativeUrl);
                dictionary.TryGetValue("fileTitle", out fileTitle);
                dictionary.TryGetValue("whoUnpinned", out whoUnpinned);
            }

            Assert.NotNull(fileUnpinned);
            Assert.NotNull(fileFilename);
            Assert.NotNull(userLogin);
            Assert.NotNull(fileId);
            Assert.NotNull(fileRelativeUrl);
            Assert.NotNull(fileTitle);
            Assert.NotNull(whoUnpinned);
        }

        [Test]
        public void FileUnPinToTopFromContextMenu()
        {
            sharePointSite.FileUnPinToTopFromContextMenu();

            Thread.Sleep(1500);

            object fileUnpinned = null;
            string fileFilename = null;
            string userLogin = null;
            string fileId = null;
            string fileRelativeUrl = null;
            string fileTitle = null;
            string whoUnpinned = null;

            fileUnpinned = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileUnpinned')");
            if (fileUnpinned != null)
            {
                var json = JsonConvert.SerializeObject(fileUnpinned);
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                dictionary.TryGetValue("userLogin", out userLogin);
                dictionary.TryGetValue("fileFilename", out fileFilename);
                dictionary.TryGetValue("fileId", out fileId);
                dictionary.TryGetValue("fileRelativeUrl", out fileRelativeUrl);
                dictionary.TryGetValue("fileTitle", out fileTitle);
                dictionary.TryGetValue("whoUnpinned", out whoUnpinned);
            }

            Assert.NotNull(fileUnpinned);
            Assert.NotNull(fileFilename);
            Assert.NotNull(userLogin);
            Assert.NotNull(fileId);
            Assert.NotNull(fileRelativeUrl);
            Assert.NotNull(fileTitle);
            Assert.NotNull(whoUnpinned);
        }
    }
}
