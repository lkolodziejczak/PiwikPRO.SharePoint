﻿using Newtonsoft.Json;
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
            Thread.Sleep(2500);
            sharePointSite.FilePinToTopFromTopMenu();

            Thread.Sleep(2500);

            object filePinnedToTop = null;
            string fileFilename = null;
            string userLogin = null;
            string fileId = null;
            string fileRelativeUrl = null;
            string fileTitle = null;
            string whoPinned = null;

            for (int i = 0; i < 20; i++)
            {
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
                    break;
                }
                Thread.Sleep(500);
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
            Thread.Sleep(3500);
            sharePointSite.FilePinToTopFromContextMenu();

            Thread.Sleep(2500);

            object filePinnedToTop = null;
            string fileFilename = null;
            string userLogin = null;
            string fileId = null;
            string fileRelativeUrl = null;
            string fileTitle = null;
            string whoPinned = null;

            for (int i = 0; i < 20; i++)
            {
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
                    break;
                }
                Thread.Sleep(500);
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
            Thread.Sleep(2500);
            sharePointSite.FileUnPinToTopFromTopMenu();

            Thread.Sleep(2500);

            object fileUnpinned = null;
            string fileFilename = null;
            string userLogin = null;
            string fileId = null;
            string fileRelativeUrl = null;
            string fileTitle = null;
            string whoUnpinned = null;

            for (int i = 0; i < 20; i++)
            {
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
                    break;
                }
                Thread.Sleep(500);
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
            Thread.Sleep(2500);
            sharePointSite.FileUnPinToTopFromContextMenu();

            Thread.Sleep(2500);

            object fileUnpinned = null;
            string fileFilename = null;
            string userLogin = null;
            string fileId = null;
            string fileRelativeUrl = null;
            string fileTitle = null;
            string whoUnpinned = null;

            for (int i = 0; i < 20; i++)
            {
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
                    break;
                }
                Thread.Sleep(500);
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
        public void NewFolderFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.NewFolderFromTopMenu();

            Thread.Sleep(2500);

            object folderCreated = null;
            string createdBy = null;
            string folderName = null;
            string folderUrl = null;

            for (int i = 0; i < 20; i++)
            {
                folderCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderCreated')");
                if (folderCreated != null)
                {
                    var json = JsonConvert.SerializeObject(folderCreated);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("createdBy", out createdBy);
                    dictionary.TryGetValue("folderName", out folderName);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderCreated);
            Assert.NotNull(createdBy);
            Assert.NotNull(folderName);
            Assert.NotNull(folderUrl);
        }

        [Test]
        public void ShareFileFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FileSharedFromTopMenu(shareToWho);

            Thread.Sleep(2500);

            object fileShared = null;
            string whoCreated = null;
            string whoShared = null;
            string filename = null;
            string fileExt = null;
            string docLocalizaton = null;


            for (int i = 0; i < 20; i++)
            {
                fileShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileShared')");
                if (fileShared != null)
                {
                    var json = JsonConvert.SerializeObject(fileShared);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("whoCreated", out whoCreated);
                    dictionary.TryGetValue("whoShared", out whoShared);
                    dictionary.TryGetValue("filename", out filename);
                    dictionary.TryGetValue("fileExt", out fileExt);
                    dictionary.TryGetValue("docLocalizaton", out docLocalizaton);
                    break;
                }
                Thread.Sleep(500);
            }


            Assert.NotNull(fileShared);
            Assert.NotNull(whoCreated);
            Assert.NotNull(whoShared);
            Assert.NotNull(filename);
            Assert.NotNull(fileExt);
            Assert.NotNull(docLocalizaton);
        }

        [Test]
        public void ShareFileFromContextMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FileSharedFromContextMenu(shareToWho);

            Thread.Sleep(2500);

            object fileShared = null;
            string whoCreated = null;
            string whoShared = null;
            string filename = null;
            string fileExt = null;
            string docLocalizaton = null;


            for (int i = 0; i < 20; i++)
            {
                fileShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileShared')");
                if (fileShared != null)
                {
                    var json = JsonConvert.SerializeObject(fileShared);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("whoCreated", out whoCreated);
                    dictionary.TryGetValue("whoShared", out whoShared);
                    dictionary.TryGetValue("filename", out filename);
                    dictionary.TryGetValue("fileExt", out fileExt);
                    dictionary.TryGetValue("docLocalizaton", out docLocalizaton);
                    break;
                }
                Thread.Sleep(500);
            }


            Assert.NotNull(fileShared);
            Assert.NotNull(whoCreated);
            Assert.NotNull(whoShared);
            Assert.NotNull(filename);
            Assert.NotNull(fileExt);
            Assert.NotNull(docLocalizaton);
        }

        [Test]
        public void ShareFileFromGridMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FileSharedFromGridMenu(shareToWho);

            Thread.Sleep(2500);

            object fileShared = null;
            string whoCreated = null;
            string whoShared = null;
            string filename = null;
            string fileExt = null;
            string docLocalizaton = null;


            for (int i = 0; i < 20; i++)
            {
                fileShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileShared')");
                if (fileShared != null)
                {
                    var json = JsonConvert.SerializeObject(fileShared);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("whoCreated", out whoCreated);
                    dictionary.TryGetValue("whoShared", out whoShared);
                    dictionary.TryGetValue("filename", out filename);
                    dictionary.TryGetValue("fileExt", out fileExt);
                    dictionary.TryGetValue("docLocalizaton", out docLocalizaton);
                    break;
                }
                Thread.Sleep(500);
            }


            Assert.NotNull(fileShared);
            Assert.NotNull(whoCreated);
            Assert.NotNull(whoShared);
            Assert.NotNull(filename);
            Assert.NotNull(fileExt);
            Assert.NotNull(docLocalizaton);
        }
    }
}
