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


            object fileFilename = null;
            object userLogin = null;
            object fileId = null;
            object fileRelativeUrl = null;
            object fileTitle = null;
            object whoPinned = null;

            for (int i = 0; i < 20; i++)
            {
                fileFilename = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileFilename')");
                userLogin = jse.ExecuteScript("return dataLayer.find(x => x.event === 'userLogin')");
                fileId = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileId')");
                fileRelativeUrl = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileRelativeUrl')");
                fileTitle = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileTitle')");
                whoPinned = jse.ExecuteScript("return dataLayer.find(x => x.event === 'whoPinned')");
                if (fileFilename != null && userLogin != null && fileId != null && fileRelativeUrl != null && fileTitle != null && whoPinned != null)
                    break;
                Thread.Sleep(500);
            }

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

            object fileFilename = null;
            object userLogin = null;
            object fileId = null;
            object fileRelativeUrl = null;
            object fileTitle = null;
            object whoPinned = null;

            for (int i = 0; i < 20; i++)
            {
                fileFilename = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileFilename')");
                userLogin = jse.ExecuteScript("return dataLayer.find(x => x.event === 'userLogin')");
                fileId = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileId')");
                fileRelativeUrl = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileRelativeUrl')");
                fileTitle = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileTitle')");
                whoPinned = jse.ExecuteScript("return dataLayer.find(x => x.event === 'whoPinned')");
                if (fileFilename != null && userLogin != null && fileId != null && fileRelativeUrl != null && fileTitle != null && whoPinned != null)
                    break;
                Thread.Sleep(500);
            }

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


            object fileFilename = null;
            object userLogin = null;
            object fileId = null;
            object fileRelativeUrl = null;
            object fileTitle = null;
            object whoUnpinned = null;

            for (int i = 0; i < 20; i++)
            {
                fileFilename = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileFilename')");
                userLogin = jse.ExecuteScript("return dataLayer.find(x => x.event === 'userLogin')");
                fileId = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileId')");
                fileRelativeUrl = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileRelativeUrl')");
                fileTitle = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileTitle')");
                whoUnpinned = jse.ExecuteScript("return dataLayer.find(x => x.event === 'whoUnpinned')");
                if (fileFilename != null && userLogin != null && fileId != null && fileRelativeUrl != null && fileTitle != null && whoUnpinned != null)
                    break;
                Thread.Sleep(500);
            }

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

            object fileFilename = null;
            object userLogin = null;
            object fileId = null;
            object fileRelativeUrl = null;
            object fileTitle = null;
            object whoUnpinned = null;

            for (int i = 0; i < 20; i++)
            {
                fileFilename = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileFilename')");
                userLogin = jse.ExecuteScript("return dataLayer.find(x => x.event === 'userLogin')");
                fileId = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileId')");
                fileRelativeUrl = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileRelativeUrl')");
                fileTitle = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileTitle')");
                whoUnpinned = jse.ExecuteScript("return dataLayer.find(x => x.event === 'whoUnpinned')");
                if (fileFilename != null && userLogin != null && fileId != null && fileRelativeUrl != null && fileTitle != null && whoUnpinned != null)
                    break;
                Thread.Sleep(500);
            }

            Assert.NotNull(fileFilename);
            Assert.NotNull(userLogin);
            Assert.NotNull(fileId);
            Assert.NotNull(fileRelativeUrl);
            Assert.NotNull(fileTitle);
            Assert.NotNull(whoUnpinned);
        }
    }
}
