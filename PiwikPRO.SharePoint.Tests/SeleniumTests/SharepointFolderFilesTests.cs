using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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
            string whoPinned = null;
            string fileTitle = null;
            string fileId = null;
            string fileUniqueId = null;
            string fileName = null;
            string fileExt = null;
            string fileUrl = null;
            string documentlibraryUrl = null;
            string documentlibraryName = null;
            string objectType = null;

            for (int i = 0; i < 20; i++)
            {
                filePinnedToTop = jse.ExecuteScript("return dataLayer.find(x => x.event === 'filePinnedToTop')");
                if (filePinnedToTop != null)
                {
                    var json = JsonConvert.SerializeObject(filePinnedToTop);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("fileUniqueId", out fileUniqueId);
                    dictionary.TryGetValue("fileName", out fileName);
                    dictionary.TryGetValue("fileId", out fileId);
                    dictionary.TryGetValue("fileUrl", out fileUrl);
                    dictionary.TryGetValue("fileTitle", out fileTitle);
                    dictionary.TryGetValue("whoPinned", out whoPinned);
                    dictionary.TryGetValue("fileExt", out fileExt);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(filePinnedToTop);
            Assert.NotNull(fileUniqueId);
            Assert.NotNull(fileName);
            Assert.NotNull(fileId);
            Assert.NotNull(fileUrl);
            Assert.NotNull(fileTitle);
            Assert.NotNull(whoPinned);
            Assert.NotNull(fileExt);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(objectType);
        }

        [Test]
        public void FilePinToTopFromContextMenu()
        {
            Thread.Sleep(3500);
            sharePointSite.FilePinToTopFromContextMenu();

            Thread.Sleep(2500);

            object filePinnedToTop = null;
            string whoPinned = null;
            string fileTitle = null;
            string fileId = null;
            string fileUniqueId = null;
            string fileName = null;
            string fileExt = null;
            string fileUrl = null;
            string documentlibraryUrl = null;
            string documentlibraryName = null;
            string objectType = null;

            for (int i = 0; i < 20; i++)
            {
                filePinnedToTop = jse.ExecuteScript("return dataLayer.find(x => x.event === 'filePinnedToTop')");
                if (filePinnedToTop != null)
                {
                    var json = JsonConvert.SerializeObject(filePinnedToTop);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("fileUniqueId", out fileUniqueId);
                    dictionary.TryGetValue("fileName", out fileName);
                    dictionary.TryGetValue("fileId", out fileId);
                    dictionary.TryGetValue("fileUrl", out fileUrl);
                    dictionary.TryGetValue("fileTitle", out fileTitle);
                    dictionary.TryGetValue("whoPinned", out whoPinned);
                    dictionary.TryGetValue("fileExt", out fileExt);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(filePinnedToTop);
            Assert.NotNull(fileUniqueId);
            Assert.NotNull(fileName);
            Assert.NotNull(fileId);
            Assert.NotNull(fileUrl);
            Assert.NotNull(fileTitle);
            Assert.NotNull(whoPinned);
            Assert.NotNull(fileExt);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(objectType);
        }

        [Test]
        public void FileUnPinToTopFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FileUnPinToTopFromTopMenu();

            Thread.Sleep(2500);

            object fileUnpinned = null;
            string whoUnpinned = null;
            string fileTitle = null;
            string fileId = null;
            string fileUniqueId = null;
            string fileName = null;
            string fileExt = null;
            string fileUrl = null;
            string documentlibraryUrl = null;
            string documentlibraryName = null;
            string objectType = null;

            for (int i = 0; i < 20; i++)
            {
                fileUnpinned = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileUnpinned')");
                if (fileUnpinned != null)
                {
                    var json = JsonConvert.SerializeObject(fileUnpinned);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("fileUniqueId", out fileUniqueId);
                    dictionary.TryGetValue("fileName", out fileName);
                    dictionary.TryGetValue("fileId", out fileId);
                    dictionary.TryGetValue("fileUrl", out fileUrl);
                    dictionary.TryGetValue("fileTitle", out fileTitle);
                    dictionary.TryGetValue("whoUnpinned", out whoUnpinned);
                    dictionary.TryGetValue("fileExt", out fileExt);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(fileUnpinned);
            Assert.NotNull(fileUniqueId);
            Assert.NotNull(fileName);
            Assert.NotNull(fileId);
            Assert.NotNull(fileUrl);
            Assert.NotNull(fileTitle);
            Assert.NotNull(whoUnpinned);
            Assert.NotNull(fileExt);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(objectType);
        }

        [Test]
        public void FileUnPinToTopFromContextMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FileUnPinToTopFromContextMenu();

            Thread.Sleep(2500);

            object fileUnpinned = null;
            string whoUnpinned = null;
            string fileTitle = null;
            string fileId = null;
            string fileUniqueId = null;
            string fileName = null;
            string fileExt = null;
            string fileUrl = null;
            string documentlibraryUrl = null;
            string documentlibraryName = null;
            string objectType = null;

            for (int i = 0; i < 20; i++)
            {
                fileUnpinned = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileUnpinned')");
                if (fileUnpinned != null)
                {
                    var json = JsonConvert.SerializeObject(fileUnpinned);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("fileUniqueId", out fileUniqueId);
                    dictionary.TryGetValue("fileName", out fileName);
                    dictionary.TryGetValue("fileId", out fileId);
                    dictionary.TryGetValue("fileUrl", out fileUrl);
                    dictionary.TryGetValue("fileTitle", out fileTitle);
                    dictionary.TryGetValue("whoUnpinned", out whoUnpinned);
                    dictionary.TryGetValue("fileExt", out fileExt);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(fileUnpinned);
            Assert.NotNull(fileUniqueId);
            Assert.NotNull(fileName);
            Assert.NotNull(fileId);
            Assert.NotNull(fileUrl);
            Assert.NotNull(fileTitle);
            Assert.NotNull(whoUnpinned);
            Assert.NotNull(fileExt);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(objectType);
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
            string contentType = null;
            string folderUniqueId = null;
            string folderId = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;

            for (int i = 0; i < 20; i++)
            {
                folderCreated = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderCreatedOrUploaded')");
                if (folderCreated != null)
                {
                    var json = JsonConvert.SerializeObject(folderCreated);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("createdBy", out createdBy);
                    dictionary.TryGetValue("folderName", out folderName);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    dictionary.TryGetValue("contentType", out contentType);
                    dictionary.TryGetValue("folderUniqueId", out folderUniqueId);
                    dictionary.TryGetValue("folderId", out folderId);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderCreated);
            Assert.NotNull(createdBy);
            Assert.NotNull(folderName);
            Assert.NotNull(folderUrl);
            Assert.NotNull(contentType);
            Assert.NotNull(folderUniqueId);
            Assert.NotNull(folderId);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(objectType);
        }

        [Test]
        public void ShareFileFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FileSharedFromTopMenu(shareToWho);

            Thread.Sleep(2500);

            object fileShared = null;
            string filesize = null;
            string fileUniqueId = null;
            string fileName = null;
            string fileExt = null;
            string fileUrl = null;
            string folderName = null;
            string folderUrl = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;


            for (int i = 0; i < 20; i++)
            {
                fileShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileShared')");
                if (fileShared != null)
                {
                    var json = JsonConvert.SerializeObject(fileShared);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("filesize", out filesize);
                    dictionary.TryGetValue("fileUniqueId", out fileUniqueId);
                    dictionary.TryGetValue("fileName", out fileName);
                    dictionary.TryGetValue("fileExt", out fileExt);
                    dictionary.TryGetValue("fileUrl", out fileUrl);
                    dictionary.TryGetValue("folderName", out folderName);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(fileShared);
            Assert.NotNull(filesize);
            Assert.NotNull(fileUniqueId);
            Assert.NotNull(fileName);
            Assert.NotNull(fileExt);
            Assert.NotNull(fileUrl);
            Assert.NotNull(folderName);
            Assert.NotNull(folderUrl);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(objectType);
        }

        [Test]
        public void ShareFileFromContextMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FileSharedFromContextMenu(shareToWho);

            Thread.Sleep(2500);

            object fileShared = null;
            string filesize = null;
            string fileUniqueId = null;
            string fileName = null;
            string fileExt = null;
            string fileUrl = null;
            string folderName = null;
            string folderUrl = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;


            for (int i = 0; i < 20; i++)
            {
                fileShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileShared')");
                if (fileShared != null)
                {
                    var json = JsonConvert.SerializeObject(fileShared);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("filesize", out filesize);
                    dictionary.TryGetValue("fileUniqueId", out fileUniqueId);
                    dictionary.TryGetValue("fileName", out fileName);
                    dictionary.TryGetValue("fileExt", out fileExt);
                    dictionary.TryGetValue("fileUrl", out fileUrl);
                    dictionary.TryGetValue("folderName", out folderName);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(fileShared);
            Assert.NotNull(filesize);
            Assert.NotNull(fileUniqueId);
            Assert.NotNull(fileName);
            Assert.NotNull(fileExt);
            Assert.NotNull(fileUrl);
            Assert.NotNull(folderName);
            Assert.NotNull(folderUrl);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(objectType);
        }

        [Test]
        public void ShareFileFromGridMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FileSharedFromGridMenu(shareToWho);

            Thread.Sleep(2500);

            object fileShared = null;
            string filesize = null;
            string fileUniqueId = null;
            string fileName = null;
            string fileExt = null;
            string fileUrl = null;
            string folderName = null;
            string folderUrl = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;


            for (int i = 0; i < 20; i++)
            {
                fileShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileShared')");
                if (fileShared != null)
                {
                    var json = JsonConvert.SerializeObject(fileShared);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("filesize", out filesize);
                    dictionary.TryGetValue("fileUniqueId", out fileUniqueId);
                    dictionary.TryGetValue("fileName", out fileName);
                    dictionary.TryGetValue("fileExt", out fileExt);
                    dictionary.TryGetValue("fileUrl", out fileUrl);
                    dictionary.TryGetValue("folderName", out folderName);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(fileShared);
            Assert.NotNull(filesize);
            Assert.NotNull(fileUniqueId);
            Assert.NotNull(fileName);
            Assert.NotNull(fileExt);
            Assert.NotNull(fileUrl);
            Assert.NotNull(folderName);
            Assert.NotNull(folderUrl);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(objectType);
        }

        [Test]
        public void ShareFolderFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderSharedFromTopMenu(shareToWho);

            Thread.Sleep(2500);

            object folderShared = null;
            string contentType = null;
            string folderUrl = null;
            string folderTitle = null;
            string folderId = null;
            string folderUniqueId = null;
            string folderName = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;
            string authorId = null;


            for (int i = 0; i < 20; i++)
            {
                folderShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderShared')");
                if (folderShared != null)
                {
                    var json = JsonConvert.SerializeObject(folderShared);
                    Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    Dictionary<string, string> dString = dictionary.ToDictionary(k => k.Key, k => Convert.ToString(k.Value));
                    dString.TryGetValue("contentType", out contentType);
                    dString.TryGetValue("folderUrl", out folderUrl);
                    dString.TryGetValue("folderTitle", out folderTitle);
                    dString.TryGetValue("folderId", out folderId);
                    dString.TryGetValue("folderUniqueId", out folderUniqueId);
                    dString.TryGetValue("folderName", out folderName);
                    dString.TryGetValue("documentlibraryName", out documentlibraryName);
                    dString.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dString.TryGetValue("objectType", out objectType);
                    dString.TryGetValue("authorId", out authorId);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderShared);
            Assert.NotNull(contentType);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(folderId);
            Assert.NotNull(folderUniqueId);
            Assert.NotNull(folderName);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(objectType);
            Assert.NotNull(authorId);
        }

        [Test]
        public void ShareFolderFromGridMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderSharedFromGridMenu(shareToWho);

            Thread.Sleep(2500);

            object folderShared = null;
            string contentType = null;
            string folderUrl = null;
            string folderTitle = null;
            string folderId = null;
            string folderUniqueId = null;
            string folderName = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;
            string authorId = null;


            for (int i = 0; i < 20; i++)
            {
                folderShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderShared')");
                if (folderShared != null)
                {
                    var json = JsonConvert.SerializeObject(folderShared);
                    Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    Dictionary<string, string> dString = dictionary.ToDictionary(k => k.Key, k => Convert.ToString(k.Value));
                    dString.TryGetValue("contentType", out contentType);
                    dString.TryGetValue("folderUrl", out folderUrl);
                    dString.TryGetValue("folderTitle", out folderTitle);
                    dString.TryGetValue("folderId", out folderId);
                    dString.TryGetValue("folderUniqueId", out folderUniqueId);
                    dString.TryGetValue("folderName", out folderName);
                    dString.TryGetValue("documentlibraryName", out documentlibraryName);
                    dString.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dString.TryGetValue("objectType", out objectType);
                    dString.TryGetValue("authorId", out authorId);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderShared);
            Assert.NotNull(contentType);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(folderId);
            Assert.NotNull(folderUniqueId);
            Assert.NotNull(folderName);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(objectType);
            Assert.NotNull(authorId);
        }

        [Test]
        public void ShareFolderFromContextMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderSharedFromContextMenu(shareToWho);

            Thread.Sleep(2500);

            object folderShared = null;
            string contentType = null;
            string folderUrl = null;
            string folderTitle = null;
            string folderId = null;
            string folderUniqueId = null;
            string folderName = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;
            string authorId = null;


            for (int i = 0; i < 20; i++)
            {
                folderShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderShared')");
                if (folderShared != null)
                {
                    var json = JsonConvert.SerializeObject(folderShared);
                    Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    Dictionary<string, string> dString = dictionary.ToDictionary(k => k.Key, k => Convert.ToString(k.Value));
                    dString.TryGetValue("contentType", out contentType);
                    dString.TryGetValue("folderUrl", out folderUrl);
                    dString.TryGetValue("folderTitle", out folderTitle);
                    dString.TryGetValue("folderId", out folderId);
                    dString.TryGetValue("folderUniqueId", out folderUniqueId);
                    dString.TryGetValue("folderName", out folderName);
                    dString.TryGetValue("documentlibraryName", out documentlibraryName);
                    dString.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dString.TryGetValue("objectType", out objectType);
                    dString.TryGetValue("authorId", out authorId);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderShared);
            Assert.NotNull(contentType);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(folderId);
            Assert.NotNull(folderUniqueId);
            Assert.NotNull(folderName);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(objectType);
            Assert.NotNull(authorId);
        }

        [Test]
        public void FolderPinToTopFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderPinToTopFromTopMenu();

            Thread.Sleep(2500);

            object folderPinnedToTop = null;
            string foldername = null;
            string folderId = null;
            string folderUrl = null;
            string whoPinned = null;
            string folderUniqueId = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;

            for (int i = 0; i < 20; i++)
            {
                folderPinnedToTop = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderPinnedToTop')");
                if (folderPinnedToTop != null)
                {
                    var json = JsonConvert.SerializeObject(folderPinnedToTop);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("folderName", out foldername);
                    dictionary.TryGetValue("folderId", out folderId);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    dictionary.TryGetValue("whoPinned", out whoPinned);
                    dictionary.TryGetValue("folderUniqueId", out folderUniqueId);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderPinnedToTop);
            Assert.NotNull(foldername);
            Assert.NotNull(folderId);
            Assert.NotNull(folderUrl);
            Assert.NotNull(whoPinned);
            Assert.NotNull(folderUniqueId);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(objectType);
        }

        [Test]
        public void FolderPinToTopFromContextMenu()
        {
            Thread.Sleep(3500);
            sharePointSite.FolderPinToTopFromContextMenu();

            Thread.Sleep(2500);

            object folderPinnedToTop = null;
            string foldername = null;
            string folderId = null;
            string folderUrl = null;
            string whoPinned = null;
            string folderUniqueId = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;

            for (int i = 0; i < 20; i++)
            {
                folderPinnedToTop = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderPinnedToTop')");
                if (folderPinnedToTop != null)
                {
                    var json = JsonConvert.SerializeObject(folderPinnedToTop);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("folderName", out foldername);
                    dictionary.TryGetValue("folderId", out folderId);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    dictionary.TryGetValue("whoPinned", out whoPinned);
                    dictionary.TryGetValue("folderUniqueId", out folderUniqueId);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderPinnedToTop);
            Assert.NotNull(foldername);
            Assert.NotNull(folderId);
            Assert.NotNull(folderUrl);
            Assert.NotNull(whoPinned);
            Assert.NotNull(folderUniqueId);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(objectType);
        }

        [Test]
        public void FolderUnPinFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderUnPinToTopFromTopMenu();

            Thread.Sleep(2500);

            object folderPinnedToTop = null;
            string foldername = null;
            string folderId = null;
            string folderUrl = null;
            string folderTitle = null;
            string whoUnpinned = null;
            string folderUniqueId = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;

            for (int i = 0; i < 20; i++)
            {
                folderPinnedToTop = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderUnpinned')");
                if (folderPinnedToTop != null)
                {
                    var json = JsonConvert.SerializeObject(folderPinnedToTop);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("folderName", out foldername);
                    dictionary.TryGetValue("folderId", out folderId);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    dictionary.TryGetValue("folderTitle", out folderTitle);
                    dictionary.TryGetValue("whoUnpinned", out whoUnpinned);
                    dictionary.TryGetValue("folderUniqueId", out folderUniqueId);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("objectType", out objectType);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderPinnedToTop);
            Assert.NotNull(foldername);
            Assert.NotNull(folderId);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(whoUnpinned);
            Assert.NotNull(folderUniqueId);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(objectType);
        }

        [Test]
        public void FolderDeletedFromContextMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderDeletedContextMenu();

            Thread.Sleep(2500);

            object folderDeleted = null;
            string folderSize = null;
            string folderUrl = null;
            string folderTitle = null;
            string whoCreated = null;
            string folderName = null;
            string folderUrl2 = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;
            string folderModified = null;
            string parentFolderName = null;
            string parentFolderUrl = null;

            for (int i = 0; i < 20; i++)
            {
                folderDeleted = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderDeleted')");
                if (folderDeleted != null)
                {
                    var json = JsonConvert.SerializeObject(folderDeleted);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("folderSize", out folderSize);
                    dictionary.TryGetValue("FolderUrl", out folderUrl);
                    dictionary.TryGetValue("folderTitle", out folderTitle);
                    dictionary.TryGetValue("whoCreated", out whoCreated);
                    dictionary.TryGetValue("folderName", out folderName);
                    dictionary.TryGetValue("folderUrl", out folderUrl2);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("objectType", out objectType);
                    dictionary.TryGetValue("folderModified", out folderModified);
                    dictionary.TryGetValue("parentFolderName", out parentFolderName);
                    dictionary.TryGetValue("parentFolderUrl", out parentFolderUrl);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderDeleted);
            Assert.NotNull(folderSize);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(whoCreated);
            Assert.NotNull(folderName);
            Assert.NotNull(folderUrl2);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(folderModified);
            Assert.NotNull(objectType);
            //Assert.NotNull(parentFolderName);
            //Assert.NotNull(parentFolderUrl);
        }

        [Test]
        public void FolderDeletedFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderDeletedFromTopMenu();

            Thread.Sleep(2500);

            object folderDeleted = null;
            string folderSize = null;
            string folderUrl = null;
            string folderTitle = null;
            string whoCreated = null;
            string folderName = null;
            string folderUrl2 = null;
            string documentlibraryName = null;
            string documentlibraryUrl = null;
            string objectType = null;
            string folderModified = null;
            string parentFolderName = null;
            string parentFolderUrl = null;

            for (int i = 0; i < 20; i++)
            {
                folderDeleted = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderDeleted')");
                if (folderDeleted != null)
                {
                    var json = JsonConvert.SerializeObject(folderDeleted);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("folderSize", out folderSize);
                    dictionary.TryGetValue("FolderUrl", out folderUrl);
                    dictionary.TryGetValue("folderTitle", out folderTitle);
                    dictionary.TryGetValue("whoCreated", out whoCreated);
                    dictionary.TryGetValue("folderName", out folderName);
                    dictionary.TryGetValue("folderUrl", out folderUrl2);
                    dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                    dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                    dictionary.TryGetValue("objectType", out objectType);
                    dictionary.TryGetValue("folderModified", out folderModified);
                    dictionary.TryGetValue("parentFolderName", out parentFolderName);
                    dictionary.TryGetValue("parentFolderUrl", out parentFolderUrl);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderDeleted);
            Assert.NotNull(folderSize);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(whoCreated);
            Assert.NotNull(folderName);
            Assert.NotNull(folderUrl2);
            Assert.NotNull(documentlibraryName);
            Assert.NotNull(documentlibraryUrl);
            Assert.NotNull(folderModified);
            Assert.NotNull(objectType);
            //Assert.NotNull(parentFolderName);
            //Assert.NotNull(parentFolderUrl);
        }
        [Test]
        public void FileUpload()
        {
            _webDriver.Navigate().GoToUrl(documentList);
            Thread.Sleep(1000);
            sharePointSite.FileUpload();
            Thread.Sleep(1500);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object fileUploaded = null;
                string userID = null;
                string libraryName = null;
                string filesize = null;
                string folderName = null;
                string fileUniqueId = null;

                for (int i = 0; i < 30; i++)
                {
                    fileUploaded = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileUploaded')");
                    if (fileUploaded != null)
                    {
                        var json = JsonConvert.SerializeObject(fileUploaded);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("userID", out userID);
                        dictionary.TryGetValue("libraryName", out libraryName);
                        dictionary.TryGetValue("filesize", out filesize);
                        dictionary.TryGetValue("folderName", out folderName);
                        dictionary.TryGetValue("fileUniqueId", out fileUniqueId);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(fileUploaded);
                Assert.NotNull(userID);
                Assert.NotNull(libraryName);
                Assert.NotNull(filesize);
                Assert.NotNull(folderName);
                Assert.NotNull(fileUniqueId);
            }
        }
        [Test]
        public void FileUploadDragAndDrop()
        {
            _webDriver.Navigate().GoToUrl(documentList);
            Thread.Sleep(1000);
            sharePointSite.FileUploadDragAndDrop();
            Thread.Sleep(1500);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object fileUploaded = null;
                string userID = null;
                string libraryName = null;
                string filesize = null;
                string folderName = null;
                string fileUniqueId = null;

                for (int i = 0; i < 30; i++)
                {
                    fileUploaded = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileUploaded')");
                    if (fileUploaded != null)
                    {
                        var json = JsonConvert.SerializeObject(fileUploaded);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("userID", out userID);
                        dictionary.TryGetValue("libraryName", out libraryName);
                        dictionary.TryGetValue("filesize", out filesize);
                        dictionary.TryGetValue("folderName", out folderName);
                        dictionary.TryGetValue("fileUniqueId", out fileUniqueId);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(fileUploaded);
                Assert.NotNull(userID);
                Assert.NotNull(libraryName);
                Assert.NotNull(filesize);
                Assert.NotNull(folderName);
                Assert.NotNull(fileUniqueId);
            }
        }
        [Test]
        public void FileDeleted()
        {
            _webDriver.Navigate().GoToUrl(documentList);
            Thread.Sleep(1000);
            sharePointSite.FileDeleted();
            Thread.Sleep(1500);
            {
                var sharePointSite = new SharepointSitePage(_webDriver);
                IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
                Thread.Sleep(2000);

                object fileDeleted = null;
                string deletionDate = null;
                string documentlibraryName = null;
                string documentlibraryUrl = null;
                string fileExt = null;
                string fileName = null;
                string fileUniqueId = null;
                string fileUrl = null;
                string filesize = null;
                string folderName = null;
                string folderUrl = null;
                string objectType = null;
                string title = null;
                string userID = null;
                string whoCreated = null;

                for (int i = 0; i < 30; i++)
                {
                    fileDeleted = jse.ExecuteScript("return dataLayer.find(x => x.event === 'fileDeleted')");
                    if (fileDeleted != null)
                    {
                        var json = JsonConvert.SerializeObject(fileDeleted);
                        Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        dictionary.TryGetValue("deletionDate", out deletionDate);
                        dictionary.TryGetValue("documentlibraryName", out documentlibraryName);
                        dictionary.TryGetValue("documentlibraryUrl", out documentlibraryUrl);
                        dictionary.TryGetValue("fileExt", out fileExt);
                        dictionary.TryGetValue("fileName", out fileName);
                        dictionary.TryGetValue("fileUniqueId", out fileUniqueId);
                        dictionary.TryGetValue("fileUrl", out fileUrl);
                        dictionary.TryGetValue("filesize", out filesize);
                        dictionary.TryGetValue("folderName", out folderName);
                        dictionary.TryGetValue("folderUrl", out folderUrl);
                        dictionary.TryGetValue("objectType", out objectType);
                        dictionary.TryGetValue("title", out title);
                        dictionary.TryGetValue("userID", out userID);
                        dictionary.TryGetValue("whoCreated", out whoCreated);
                        break;
                    }
                    Thread.Sleep(100);
                }
                Assert.NotNull(fileDeleted);
                Assert.NotNull(deletionDate);
                Assert.NotNull(documentlibraryName);
                Assert.NotNull(documentlibraryUrl);
                Assert.NotNull(fileExt);
                Assert.NotNull(folderName);
                Assert.NotNull(fileName);
                Assert.NotNull(fileUniqueId);
                Assert.NotNull(fileUrl);
                Assert.NotNull(filesize);
                Assert.NotNull(folderName);
                Assert.NotNull(folderUrl);
                Assert.NotNull(objectType);
                //Assert.NotNull(title);
                Assert.NotNull(userID);
                Assert.NotNull(whoCreated);


            }
        }
    }
}
