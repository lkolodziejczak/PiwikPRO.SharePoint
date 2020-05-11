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
            string fileFilename = null;
            string userLogin = null;
            string fileId = null;
            string fileUrl = null;
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
                    dictionary.TryGetValue("fileUrl", out fileUrl);
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
            Assert.NotNull(fileUrl);
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
            string fileUrl = null;
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
                    dictionary.TryGetValue("fileUrl", out fileUrl);
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
            Assert.NotNull(fileUrl);
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
            string fileUrl = null;
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
                    dictionary.TryGetValue("fileUrl", out fileUrl);
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
            Assert.NotNull(fileUrl);
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
            string fileUrl = null;
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
                    dictionary.TryGetValue("fileUrl", out fileUrl);
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
            Assert.NotNull(fileUrl);
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

        [Test]
        public void ShareFolderFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderSharedFromTopMenu(shareToWho);

            Thread.Sleep(2500);

            object folderShared = null;
            string sharedWith = null;
            string whoShared = null;
            string contentType = null;
            string folderUrl = null;
            string folderTitle = null;
            string folderId = null;
            string typeOfShare = null;


            for (int i = 0; i < 20; i++)
            {
                folderShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderShared')");
                if (folderShared != null)
                {
                    var json = JsonConvert.SerializeObject(folderShared);
                    Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    Dictionary<string, string> dString = dictionary.ToDictionary(k => k.Key, k => Convert.ToString(k.Value));
                    dString.TryGetValue("sharedWith", out sharedWith);
                    dString.TryGetValue("whoShared", out whoShared);
                    dString.TryGetValue("contentType", out contentType);
                    dString.TryGetValue("folderUrl", out folderUrl);
                    dString.TryGetValue("folderTitle", out folderTitle);
                    dString.TryGetValue("folderId", out folderId);
                    dString.TryGetValue("typeOfShare", out typeOfShare);
                    break;
                }
                Thread.Sleep(500);
            }


            Assert.NotNull(folderShared);
            Assert.NotNull(sharedWith);
            Assert.NotNull(whoShared);
            Assert.NotNull(contentType);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(folderId);
            Assert.NotNull(typeOfShare);
        }

        [Test]
        public void ShareFolderFromGridMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderSharedFromGridMenu(shareToWho);

            Thread.Sleep(2500);

            object folderShared = null;
            string sharedWith = null;
            string whoShared = null;
            string contentType = null;
            string folderUrl = null;
            string folderTitle = null;
            string folderId = null;
            string typeOfShare = null;


            for (int i = 0; i < 20; i++)
            {
                folderShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderShared')");
                if (folderShared != null)
                {
                    var json = JsonConvert.SerializeObject(folderShared);
                    Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    Dictionary<string, string> dString = dictionary.ToDictionary(k => k.Key, k => Convert.ToString(k.Value));
                    dString.TryGetValue("sharedWith", out sharedWith);
                    dString.TryGetValue("whoShared", out whoShared);
                    dString.TryGetValue("contentType", out contentType);
                    dString.TryGetValue("folderUrl", out folderUrl);
                    dString.TryGetValue("folderTitle", out folderTitle);
                    dString.TryGetValue("folderId", out folderId);
                    dString.TryGetValue("typeOfShare", out typeOfShare);
                    break;
                }
                Thread.Sleep(500);
            }


            Assert.NotNull(folderShared);
            Assert.NotNull(sharedWith);
            Assert.NotNull(whoShared);
            Assert.NotNull(contentType);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(folderId);
            Assert.NotNull(typeOfShare);
        }

        [Test]
        public void ShareFolderFromContextMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderSharedFromContextMenu(shareToWho);

            Thread.Sleep(2500);

            object folderShared = null;
            string sharedWith = null;
            string whoShared = null;
            string contentType = null;
            string folderUrl = null;
            string folderTitle = null;
            string folderId = null;
            string typeOfShare = null;


            for (int i = 0; i < 20; i++)
            {
                folderShared = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderShared')");
                if (folderShared != null)
                {
                    var json = JsonConvert.SerializeObject(folderShared);
                    Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    Dictionary<string, string> dString = dictionary.ToDictionary(k => k.Key, k => Convert.ToString(k.Value));
                    dString.TryGetValue("sharedWith", out sharedWith);
                    dString.TryGetValue("whoShared", out whoShared);
                    dString.TryGetValue("contentType", out contentType);
                    dString.TryGetValue("folderUrl", out folderUrl);
                    dString.TryGetValue("folderTitle", out folderTitle);
                    dString.TryGetValue("folderId", out folderId);
                    dString.TryGetValue("typeOfShare", out typeOfShare);
                    break;
                }
                Thread.Sleep(500);
            }


            Assert.NotNull(folderShared);
            Assert.NotNull(sharedWith);
            Assert.NotNull(whoShared);
            Assert.NotNull(contentType);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(folderId);
            Assert.NotNull(typeOfShare);
        }

        [Test]
        public void FolderPinToTopFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderPinToTopFromTopMenu();

            Thread.Sleep(2500);

            object folderPinnedToTop = null;
            string foldername = null;
            string userLogin = null;
            string folderId = null;
            string folderUrl = null;
            string folderTitle = null;
            string whoPinned = null;

            for (int i = 0; i < 20; i++)
            {
                folderPinnedToTop = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderPinnedToTop')");
                if (folderPinnedToTop != null)
                {
                    var json = JsonConvert.SerializeObject(folderPinnedToTop);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("userLogin", out userLogin);
                    dictionary.TryGetValue("foldername", out foldername);
                    dictionary.TryGetValue("folderId", out folderId);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    dictionary.TryGetValue("folderTitle", out folderTitle);
                    dictionary.TryGetValue("whoPinned", out whoPinned);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderPinnedToTop);
            Assert.NotNull(foldername);
            Assert.NotNull(userLogin);
            Assert.NotNull(folderId);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(whoPinned);
        }

        [Test]
        public void FolderPinToTopFromContextMenu()
        {
            Thread.Sleep(3500);
            sharePointSite.FolderPinToTopFromContextMenu();

            Thread.Sleep(2500);

            object folderPinnedToTop = null;
            string foldername = null;
            string userLogin = null;
            string folderId = null;
            string folderUrl = null;
            string folderTitle = null;
            string whoPinned = null;

            for (int i = 0; i < 20; i++)
            {
                folderPinnedToTop = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderPinnedToTop')");
                if (folderPinnedToTop != null)
                {
                    var json = JsonConvert.SerializeObject(folderPinnedToTop);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("userLogin", out userLogin);
                    dictionary.TryGetValue("foldername", out foldername);
                    dictionary.TryGetValue("folderId", out folderId);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    dictionary.TryGetValue("folderTitle", out folderTitle);
                    dictionary.TryGetValue("whoPinned", out whoPinned);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderPinnedToTop);
            Assert.NotNull(foldername);
            Assert.NotNull(userLogin);
            Assert.NotNull(folderId);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(whoPinned);
        }

        [Test]
        public void FolderUnPinToTopFromTopMenu()
        {
            Thread.Sleep(2500);
            sharePointSite.FolderUnPinToTopFromTopMenu();

            Thread.Sleep(2500);

            object folderUnpinned = null;
            string foldername = null;
            string userLogin = null;
            string folderId = null;
            string folderUrl = null;
            string folderTitle = null;
            string whoUnpinned = null;

            for (int i = 0; i < 20; i++)
            {
                folderUnpinned = jse.ExecuteScript("return dataLayer.find(x => x.event === 'folderUnpinned')");
                if (folderUnpinned != null)
                {
                    var json = JsonConvert.SerializeObject(folderUnpinned);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("userLogin", out userLogin);
                    dictionary.TryGetValue("foldername", out foldername);
                    dictionary.TryGetValue("folderId", out folderId);
                    dictionary.TryGetValue("folderUrl", out folderUrl);
                    dictionary.TryGetValue("folderTitle", out folderTitle);
                    dictionary.TryGetValue("whoUnpinned", out whoUnpinned);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderUnpinned);
            Assert.NotNull(foldername);
            Assert.NotNull(userLogin);
            Assert.NotNull(folderId);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(whoUnpinned);
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
            string whoDeleted = null;

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
                    dictionary.TryGetValue("whoDeleted", out whoDeleted);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderDeleted);
            Assert.NotNull(folderSize);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(whoDeleted);
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
            string whoDeleted = null;

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
                    dictionary.TryGetValue("whoDeleted", out whoDeleted);
                    break;
                }
                Thread.Sleep(500);
            }

            Assert.NotNull(folderDeleted);
            Assert.NotNull(folderSize);
            Assert.NotNull(folderUrl);
            Assert.NotNull(folderTitle);
            Assert.NotNull(whoDeleted);
        }
    }
}
