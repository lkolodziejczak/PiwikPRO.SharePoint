using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PiwikPRO.SharePoint.Tests.Pages;

namespace PiwikPRO.SharePoint.Tests.SeleniumTests
{
    class SharepointSitePageActionTests : TestBase
    {
        [OneTimeSetUp]
        public void BeforeTest()
        {
            var loginPage = new LoginPage(_webDriver);
            var sharePointSite = new SharepointSitePage(_webDriver);
            loginPage.LoginToSharepoint(sharepointUserToTest, sharepointUserPasswordToTest);
        }
        [SetUp]
        public void ForEachTest()
        {
            _webDriver.Navigate().GoToUrl(testPageUrl);
            Thread.Sleep(1500);
        }
        public static void WaitForLoad(IWebDriver driver, int timeoutSec = 15)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }

        [Test]
        public void AddComment()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
            sharePointSite.AddComment("test");
           
            object commentItem = null;
            string authorDisplayName = null;
            string itemAuthorUserName = null;
            string itemUrl = null;
            string commentUrl = null;
            string itemContentTypeName = null;
            string itemContentTypeId = null;
            string pageAuthorDepartment = null;
            string pageAuthorOffice = null;
            string pageAuthorJobTitle = null;
            string userID = null;
            string userDisplayName = null;

            for (int i = 0; i < 30; i++)
            {
                commentItem = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentItem')");
                if (commentItem != null)
                {
                    var json = JsonConvert.SerializeObject(commentItem);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("authorDisplayName", out authorDisplayName);
                    dictionary.TryGetValue("itemAuthorUserName", out itemAuthorUserName);
                    dictionary.TryGetValue("itemUrl", out itemUrl);
                    dictionary.TryGetValue("commentUrl", out commentUrl);
                    dictionary.TryGetValue("itemContentTypeName", out itemContentTypeName);
                    dictionary.TryGetValue("itemContentTypeId", out itemContentTypeId);
                    dictionary.TryGetValue("pageAuthorDepartment", out pageAuthorDepartment);
                    dictionary.TryGetValue("pageAuthorOffice", out pageAuthorOffice);
                    dictionary.TryGetValue("pageAuthorJobTitle", out pageAuthorJobTitle);
                    dictionary.TryGetValue("userID", out userID);
                    dictionary.TryGetValue("userDisplayName", out userDisplayName);
                    break;
                }
                Thread.Sleep(100);
            }
            Assert.NotNull(commentItem);
            //Assert.NotNull(authorDisplayName);
            //Assert.NotNull(itemAuthorUserName);
            Assert.NotNull(itemUrl);
            Assert.NotNull(commentUrl);
            Assert.NotNull(itemContentTypeName);
            Assert.NotNull(itemContentTypeId);
            //Assert.NotNull(pageAuthorDepartment);
            //Assert.NotNull(pageAuthorOffice);
            //Assert.NotNull(pageAuthorJobTitle);
            Assert.NotNull(userID);
            Assert.NotNull(userDisplayName);
        }

        [Test]
        public void LikePageAndUnLikePage()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
            Thread.Sleep(1000);
            sharePointSite.ClickLikePage();
            Thread.Sleep(2500);
            sharePointSite.ClickUnlikePage();
            Thread.Sleep(2500);

            object likeItem = null;
            string likeauthorDisplayName = null;
            string likeitemUniqueId = null;
            string likeitemAuthorUserName = null;
            string likeitemContentTypeName = null;
            string likeitemUrl = null;
            string likeitemContentTypeId = null;
            string likepageAuthorDepartment = null;
            string likepageAuthorOffice = null;
            string likepageAuthorJobTitle = null;
            string likeuserID = null;
            string likeuserDisplayName = null;

            object unlikeItem = null;
            string unlikeauthorDisplayName = null;
            string unlikeitemUniqueId = null;
            string unlikeitemAuthorUserName = null;
            string unlikeitemContentTypeName = null;
            string unlikeitemUrl = null;
            string unlikeitemContentTypeId = null;
            string unlikepageAuthorDepartment = null;
            string unlikepageAuthorOffice = null;
            string unlikepageAuthorJobTitle = null;
            string unlikeuserID = null;
            string unlikeuserDisplayName = null;



            for (int i = 0; i < 30; i++)
            {
                likeItem = jse.ExecuteScript("return dataLayer.find(x => x.event === 'likeItem')");
                if (likeItem != null )
                {
                    var jsonLike = JsonConvert.SerializeObject(likeItem);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonLike);
                    dictionary.TryGetValue("authorDisplayName", out likeauthorDisplayName);
                    dictionary.TryGetValue("itemUniqueId", out likeitemUniqueId);
                    dictionary.TryGetValue("itemAuthorUserName", out likeitemAuthorUserName);
                    dictionary.TryGetValue("itemContentTypeName", out likeitemContentTypeName);
                    dictionary.TryGetValue("itemUrl", out likeitemUrl);
                    dictionary.TryGetValue("itemContentTypeId", out likeitemContentTypeId);
                    dictionary.TryGetValue("pageAuthorDepartment", out likepageAuthorDepartment);
                    dictionary.TryGetValue("pageAuthorOffice", out likepageAuthorOffice);
                    dictionary.TryGetValue("pageAuthorJobTitle", out likepageAuthorJobTitle);
                    dictionary.TryGetValue("userID", out likeuserID);
                    dictionary.TryGetValue("userDisplayName", out likeuserDisplayName);

                }
                Thread.Sleep(300);
            }
            Thread.Sleep(1500);
            for (int i = 0; i < 30; i++)
            {
                unlikeItem = jse.ExecuteScript("return dataLayer.find(x => x.event === 'unlikeItem')");
                if (unlikeItem != null)
                {
                    var jsonUnlike = JsonConvert.SerializeObject(unlikeItem);
                    Dictionary<string, string> dictionary2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonUnlike);
                    dictionary2.TryGetValue("authorDisplayName", out unlikeauthorDisplayName);
                    dictionary2.TryGetValue("itemUniqueId", out unlikeitemUniqueId);
                    dictionary2.TryGetValue("itemAuthorUserName", out unlikeitemAuthorUserName);
                    dictionary2.TryGetValue("itemContentTypeName", out unlikeitemContentTypeName);
                    dictionary2.TryGetValue("itemUrl", out unlikeitemUrl);
                    dictionary2.TryGetValue("itemContentTypeId", out unlikeitemContentTypeId);
                    dictionary2.TryGetValue("pageAuthorDepartment", out unlikepageAuthorDepartment);
                    dictionary2.TryGetValue("pageAuthorOffice", out unlikepageAuthorOffice);
                    dictionary2.TryGetValue("pageAuthorJobTitle", out unlikepageAuthorJobTitle);
                    dictionary2.TryGetValue("userID", out unlikeuserID);
                    dictionary2.TryGetValue("userDisplayName", out unlikeuserDisplayName);

                    break;
                }
                Thread.Sleep(300);
            }
            Assert.NotNull(likeItem);
            Assert.NotNull(likeauthorDisplayName);
            Assert.NotNull(likeitemUniqueId);
            //Assert.NotNull(likeitemAuthorUserName);
            Assert.NotNull(likeitemContentTypeName);
            Assert.NotNull(likeitemUrl);
            Assert.NotNull(likeitemContentTypeId);
            //Assert.NotNull(likepageAuthorDepartment);
            //Assert.NotNull(likepageAuthorOffice);
            //Assert.NotNull(likepageAuthorJobTitle);
            Assert.NotNull(likeuserID);
            Assert.NotNull(likeuserDisplayName);


            Assert.NotNull(unlikeItem);
            Assert.NotNull(unlikeauthorDisplayName);
            Assert.NotNull(unlikeitemUniqueId);
            //Assert.NotNull(unlikeitemAuthorUserName);
            Assert.NotNull(unlikeitemContentTypeName);
            Assert.NotNull(unlikeitemUrl);
            Assert.NotNull(unlikeitemContentTypeId);
            //Assert.NotNull(unlikepageAuthorDepartment);
            //Assert.NotNull(unlikepageAuthorOffice);
            //Assert.NotNull(unlikepageAuthorJobTitle);
            Assert.NotNull(unlikeuserID);
            Assert.NotNull(unlikeuserDisplayName);

        }

        [Test]
        public void LikeCommentAndUnLikeComment()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
            Thread.Sleep(1000);
            sharePointSite.ClickLikeComment();
            Thread.Sleep(2500);
            sharePointSite.ClickUnlikeComment();
            Thread.Sleep(2500);

            object likeComment = null;
            string likeauthorDisplayName = null;
            string likeitemUniqueId = null;
            string likeitemAuthorUserName = null;
            string likeitemContentTypeName = null;
            string likeitemUrl = null;
            string likeitemContentTypeId = null;
            string likepageAuthorDepartment = null;
            string likepageAuthorOffice = null;
            string likepageAuthorJobTitle = null;
            string likeuserID = null;
            string likeuserDisplayName = null;

            object unlikeComment = null;
            string unlikeauthorDisplayName = null;
            string unlikeitemUniqueId = null;
            string unlikeitemAuthorUserName = null;
            string unlikeitemContentTypeName = null;
            string unlikeitemUrl = null;
            string unlikeitemContentTypeId = null;
            string unlikepageAuthorDepartment = null;
            string unlikepageAuthorOffice = null;
            string unlikepageAuthorJobTitle = null;
            string unlikeuserID = null;
            string unlikeuserDisplayName = null;



            for (int i = 0; i < 30; i++)
            {
                likeComment = jse.ExecuteScript("return dataLayer.find(x => x.event === 'likeComment')");
                if (likeComment != null)
                {
                    var jsonLike = JsonConvert.SerializeObject(likeComment);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonLike);
                    dictionary.TryGetValue("authorDisplayName", out likeauthorDisplayName);
                    dictionary.TryGetValue("itemUniqueId", out likeitemUniqueId);
                    dictionary.TryGetValue("itemAuthorUserName", out likeitemAuthorUserName);
                    dictionary.TryGetValue("itemContentTypeName", out likeitemContentTypeName);
                    dictionary.TryGetValue("itemUrl", out likeitemUrl);
                    dictionary.TryGetValue("itemContentTypeId", out likeitemContentTypeId);
                    dictionary.TryGetValue("pageAuthorDepartment", out likepageAuthorDepartment);
                    dictionary.TryGetValue("pageAuthorOffice", out likepageAuthorOffice);
                    dictionary.TryGetValue("pageAuthorJobTitle", out likepageAuthorJobTitle);
                    dictionary.TryGetValue("userID", out likeuserID);
                    dictionary.TryGetValue("userDisplayName", out likeuserDisplayName);

                }
                Thread.Sleep(300);
            }
            Thread.Sleep(1500);
            for (int i = 0; i < 30; i++)
            {
                unlikeComment = jse.ExecuteScript("return dataLayer.find(x => x.event === 'unlikeComment')");
                if (unlikeComment != null)
                {
                    var jsonUnlike = JsonConvert.SerializeObject(unlikeComment);
                    Dictionary<string, string> dictionary2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonUnlike);
                    dictionary2.TryGetValue("authorDisplayName", out unlikeauthorDisplayName);
                    dictionary2.TryGetValue("itemUniqueId", out unlikeitemUniqueId);
                    dictionary2.TryGetValue("itemAuthorUserName", out unlikeitemAuthorUserName);
                    dictionary2.TryGetValue("itemContentTypeName", out unlikeitemContentTypeName);
                    dictionary2.TryGetValue("itemUrl", out unlikeitemUrl);
                    dictionary2.TryGetValue("itemContentTypeId", out unlikeitemContentTypeId);
                    dictionary2.TryGetValue("pageAuthorDepartment", out unlikepageAuthorDepartment);
                    dictionary2.TryGetValue("pageAuthorOffice", out unlikepageAuthorOffice);
                    dictionary2.TryGetValue("pageAuthorJobTitle", out unlikepageAuthorJobTitle);
                    dictionary2.TryGetValue("userID", out unlikeuserID);
                    dictionary2.TryGetValue("userDisplayName", out unlikeuserDisplayName);

                    break;
                }
                Thread.Sleep(300);
            }
            Assert.NotNull(likeComment);
            Assert.NotNull(likeauthorDisplayName);
            Assert.NotNull(likeitemUniqueId);
            //Assert.NotNull(likeitemAuthorUserName);
            Assert.NotNull(likeitemContentTypeName);
            Assert.NotNull(likeitemUrl);
            Assert.NotNull(likeitemContentTypeId);
            //Assert.NotNull(likepageAuthorDepartment);
            //Assert.NotNull(likepageAuthorOffice);
            //Assert.NotNull(likepageAuthorJobTitle);
            Assert.NotNull(likeuserID);
            Assert.NotNull(likeuserDisplayName);


            Assert.NotNull(unlikeComment);
            Assert.NotNull(unlikeauthorDisplayName);
            Assert.NotNull(unlikeitemUniqueId);
            //Assert.NotNull(unlikeitemAuthorUserName);
            Assert.NotNull(unlikeitemContentTypeName);
            Assert.NotNull(unlikeitemUrl);
            Assert.NotNull(unlikeitemContentTypeId);
            //Assert.NotNull(unlikepageAuthorDepartment);
            //Assert.NotNull(unlikepageAuthorOffice);
            //Assert.NotNull(unlikepageAuthorJobTitle);
            Assert.NotNull(unlikeuserID);
            Assert.NotNull(unlikeuserDisplayName);

        }


        [Test]
        public void ReplyComment()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
            sharePointSite.AddComment("test");
            WaitForLoad(_webDriver);
            sharePointSite.AddReplyComment("test");
            Thread.Sleep(500);
            object commentReply = null;
            string authorDisplayName = null;
            string itemUniqueId = null;
            string itemAuthorUserName = null;
            string itemUrl = null;
            string commentAuthor = null;
            string commentUrl = null;
            string commentReplyUrl = null;
            string itemContentTypeName = null;
            string itemContentTypeId = null;
            string pageAuthorDepartment = null;
            string pageAuthorOffice = null;
            string pageAuthorJobTitle = null;
            string userID = null;
            string userDisplayName = null;

            for (int i = 0; i < 30; i++)
            {
                commentReply = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentReply')");
                if (commentReply != null)
                {
                    var json = JsonConvert.SerializeObject(commentReply);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("authorDisplayName", out authorDisplayName);
                    dictionary.TryGetValue("itemUniqueId", out itemUniqueId);
                    dictionary.TryGetValue("itemAuthorUserName", out itemAuthorUserName);
                    dictionary.TryGetValue("itemUrl", out itemUrl);
                    dictionary.TryGetValue("commentAuthor", out commentAuthor);
                    dictionary.TryGetValue("commentUrl", out commentUrl);
                    dictionary.TryGetValue("commentReplyUrl", out commentReplyUrl);
                    dictionary.TryGetValue("itemContentTypeName", out itemContentTypeName);
                    dictionary.TryGetValue("itemContentTypeId", out itemContentTypeId);
                    dictionary.TryGetValue("pageAuthorDepartment", out pageAuthorDepartment);
                    dictionary.TryGetValue("pageAuthorOffice", out pageAuthorOffice);
                    dictionary.TryGetValue("pageAuthorJobTitle", out pageAuthorJobTitle);
                    dictionary.TryGetValue("userID", out userID);
                    dictionary.TryGetValue("userDisplayName", out userDisplayName);
  
                }
                Thread.Sleep(100);
            }
            Assert.NotNull(authorDisplayName);
            Assert.NotNull(itemUniqueId);
            Assert.NotNull(itemAuthorUserName);
            Assert.NotNull(itemUrl);
            Assert.NotNull(commentAuthor);
            Assert.NotNull(commentUrl);
            Assert.NotNull(commentReplyUrl);
            Assert.NotNull(itemContentTypeName);
            Assert.NotNull(itemContentTypeId);
            //Assert.NotNull(pageAuthorDepartment);
            //Assert.NotNull(pageAuthorOffice);
            //Assert.NotNull(pageAuthorJobTitle);
            Assert.NotNull(userID);
            Assert.NotNull(userDisplayName);
        }
    }
}
