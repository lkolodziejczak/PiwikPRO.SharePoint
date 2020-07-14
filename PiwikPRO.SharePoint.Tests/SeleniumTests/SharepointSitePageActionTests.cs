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
            string itemUrl = null;
            string itemContentTypeName = null;
            string commentUrl = null;
            string itemAuthorUserName = null;
            string itemUniqueId = null;
            string itemContentTypeId = null;

            for (int i = 0; i < 30; i++)
            {
                commentItem = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentItem')");
                if (commentItem != null)
                {
                    var json = JsonConvert.SerializeObject(commentItem);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("itemContentTypeName", out itemContentTypeName);
                    dictionary.TryGetValue("commentUrl", out commentUrl);
                    dictionary.TryGetValue("itemAuthorUserName", out itemAuthorUserName);
                    dictionary.TryGetValue("itemUrl", out itemUrl);
                    dictionary.TryGetValue("itemUniqueId", out itemUniqueId);
                    dictionary.TryGetValue("itemContentTypeId", out itemContentTypeId);
                    break;
                }
                Thread.Sleep(100);
            }
            Assert.NotNull(commentItem);
            Assert.NotNull(itemContentTypeName);
            Assert.NotNull(commentUrl);
            Assert.NotNull(itemAuthorUserName);
            Assert.NotNull(itemUrl);
            Assert.NotNull(itemUniqueId);
            Assert.NotNull(itemContentTypeId);
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
            string likeItemContentTypeName = null;
            string likeItemAuthorUserName = null;
            string likeItemUrl = null;
            string itemUniqueId = null;
            string itemContentTypeId = null;

            object unlikeItem = null;
            string unlikeItemContentTypeName = null;
            string unlikeItemContentTypeId = null;
            string unlikeItemUniqueId = null;
            string unlikeItemAuthorUserName = null;
            string unlikeItemUrl = null;
            
            for (int i = 0; i < 30; i++)
            {
                likeItem = jse.ExecuteScript("return dataLayer.find(x => x.event === 'likeItem')");
                if (likeItem != null )
                {
                    var jsonLike = JsonConvert.SerializeObject(likeItem);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonLike);
                    dictionary.TryGetValue("itemContentTypeName", out likeItemContentTypeName);
                    dictionary.TryGetValue("itemContentTypeId", out itemContentTypeId);
                    dictionary.TryGetValue("itemAuthorUserName", out likeItemAuthorUserName);
                    dictionary.TryGetValue("itemUrl", out likeItemUrl);
                    dictionary.TryGetValue("itemUniqueId", out itemUniqueId);
                    break;
                }
                Thread.Sleep(300);
            }
            Thread.Sleep(500);
            for (int i = 0; i < 30; i++)
            {
                unlikeItem = jse.ExecuteScript("return dataLayer.find(x => x.event === 'unlikeItem')");
                if (unlikeItem != null)
                {
                    var jsonUnlike = JsonConvert.SerializeObject(unlikeItem);
                    Dictionary<string, string> dictionary2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonUnlike);
                    dictionary2.TryGetValue("itemContentTypeName", out unlikeItemContentTypeName);
                    dictionary2.TryGetValue("itemContentTypeId", out unlikeItemContentTypeId);
                    dictionary2.TryGetValue("itemUniqueId", out unlikeItemUniqueId);
                    dictionary2.TryGetValue("itemAuthorUserName", out unlikeItemAuthorUserName);
                    dictionary2.TryGetValue("itemUrl", out unlikeItemUrl);

                    break;
                }
                Thread.Sleep(300);
            }
            Assert.NotNull(likeItem);
            Assert.NotNull(likeItemContentTypeName);
            Assert.NotNull(itemContentTypeId);
            Assert.NotNull(likeItemAuthorUserName);
            Assert.NotNull(likeItemUrl);
            Assert.NotNull(itemUniqueId);

            Assert.NotNull(unlikeItem);
            Assert.NotNull(unlikeItemContentTypeName);
            Assert.NotNull(unlikeItemContentTypeId);
            Assert.NotNull(unlikeItemAuthorUserName);
            Assert.NotNull(unlikeItemUniqueId);
            Assert.NotNull(unlikeItemUrl);
        }

        [Test]
        public void LikeComment()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
            sharePointSite.AddComment("test");
            Thread.Sleep(1500);
            sharePointSite.ClickLikeComment();
            Thread.Sleep(2500);

            object likeComment = null;
            string commentAuthor = null;
            string commentUrl = null;
            string itemUrl = null;
            string itemAuthorUserName = null;
            string itemUniqueId = null;
            string itemContentTypeName = null;
            string itemContentTypeId = null;
            for (int i = 0; i < 30; i++)
            {
                likeComment = jse.ExecuteScript("return dataLayer.find(x => x.event === 'likeComment')");
                if (likeComment != null)
                {
                    var json = JsonConvert.SerializeObject(likeComment);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("itemUniqueId", out itemUniqueId);
                    dictionary.TryGetValue("commentAuthor", out commentAuthor);
                    dictionary.TryGetValue("commentUrl", out commentUrl);
                    dictionary.TryGetValue("itemUrl", out itemUrl);
                    dictionary.TryGetValue("itemAuthorUserName", out itemAuthorUserName);
                    dictionary.TryGetValue("itemContentTypeName", out itemContentTypeName);
                    dictionary.TryGetValue("itemContentTypeId", out itemContentTypeId);
                    break;
                }
                Thread.Sleep(500);
            }
            Assert.NotNull(itemUniqueId);
            Assert.NotNull(likeComment);
            Assert.NotNull(commentAuthor);
            Assert.NotNull(commentUrl);
            Assert.NotNull(itemUrl);
            Assert.NotNull(itemAuthorUserName);
            Assert.NotNull(itemContentTypeName);
            Assert.NotNull(itemContentTypeId);
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
            string commentAuthor = null;
            string commentReplyUrl = null;
            string commentUrl = null;
            string itemUniqueId = null;
            string itemAuthorUserName = null;
            string itemUrl = null;
            string itemContentTypeName = null;
            string itemContentTypeId = null;
            for (int i = 0; i < 30; i++)
            {
                commentReply = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentReply')");
                if (commentReply != null)
                {
                    var json = JsonConvert.SerializeObject(commentReply);
                    Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    dictionary.TryGetValue("commentAuthor", out commentAuthor);
                    dictionary.TryGetValue("commentReplyUrl", out commentReplyUrl);
                    dictionary.TryGetValue("commentUrl", out commentUrl);
                    dictionary.TryGetValue("itemUniqueId", out itemUniqueId);
                    dictionary.TryGetValue("itemAuthorUserName", out itemAuthorUserName);
                    dictionary.TryGetValue("itemUrl", out itemUrl);
                    dictionary.TryGetValue("itemContentTypeName", out itemContentTypeName);
                    dictionary.TryGetValue("itemContentTypeId", out itemContentTypeId);
                    break;
                }
                Thread.Sleep(100);
            }
            Assert.NotNull(commentReply);
            Assert.NotNull(commentAuthor);
            Assert.NotNull(commentReplyUrl);
            Assert.NotNull(commentUrl);
            Assert.NotNull(itemUniqueId);
            Assert.NotNull(itemAuthorUserName);
            Assert.NotNull(itemUrl);
            Assert.NotNull(itemContentTypeName);
            Assert.NotNull(itemContentTypeId);
        }
    }
}
