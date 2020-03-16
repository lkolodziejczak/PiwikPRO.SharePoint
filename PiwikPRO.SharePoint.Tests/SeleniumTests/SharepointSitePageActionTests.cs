using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PiwikPRO.SharePoint.Tests.Pages;

namespace PiwikPRO.SharePoint.Tests.SeleniumTests
{
    class SharepointSitePageActionTests : TestBase
    {

        [SetUp]
        public void BeforeTest()
        {
            var loginPage = new LoginPage(_webDriver);
            var sharePointSite = new SharepointSitePage(_webDriver);
            loginPage.LoginToSharepoint("mzywicki@phkogifi.onmicrosoft.com", "SAqa@2018b44c");
            _webDriver.Navigate().GoToUrl("https://phkogifi.sharepoint.com/sites/PH/SitePages/Test-Page.aspx");
        }

        [Test]
        public void AddComment()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;

            sharePointSite.AddComment("test");

            Thread.Sleep(500);


            object commentItem = null;
            object userLogin = null;
            object pageUrl = null;
            object itemAuthorUserName = null;
            
            for (int i = 0; i < 50; i++)
            {
                commentItem = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentItem')");
                userLogin = jse.ExecuteScript("return dataLayer.find(x => x.event === 'userLogin')");
                pageUrl = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageUrl')");
                itemAuthorUserName = jse.ExecuteScript("return dataLayer.find(x => x.event === 'itemAuthorUserName')");
                if (commentItem != null & userLogin != null & pageUrl != null & itemAuthorUserName != null)
                    break;
                Thread.Sleep(500);
            }

            Assert.NotNull(commentItem);
            Assert.NotNull(userLogin);
            Assert.NotNull(pageUrl);
            Assert.NotNull(itemAuthorUserName);
           
        }

        [Test]
        public void LikePageAndUnLikePage()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;

            sharePointSite.ClickLikePage();

            sharePointSite.ClickUnlikePage();

            object likeItem = null;
            object unlikeItem = null;
            object userLogin = null;
            object pageUrl = null;
            object itemAuthorUserName = null;
            for (int i = 0; i < 10; i++)
            {
                likeItem = jse.ExecuteScript("return dataLayer.find(x => x.event === 'likeItem')");
                unlikeItem = jse.ExecuteScript("return dataLayer.find(x => x.event === 'unlikeItem')");
                userLogin = jse.ExecuteScript("return dataLayer.find(x => x.event === 'userLogin')");
                pageUrl = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageUrl')");
                itemAuthorUserName = jse.ExecuteScript("return dataLayer.find(x => x.event === 'itemAuthorUserName')");
                if (likeItem != null & unlikeItem != null & userLogin != null & pageUrl != null & itemAuthorUserName != null)
                    break;
                Thread.Sleep(100);
            }

            Assert.NotNull(likeItem);
            Assert.NotNull(unlikeItem);
            Assert.NotNull(userLogin);
            Assert.NotNull(pageUrl);
            Assert.NotNull(itemAuthorUserName);
        }
        [Test]
        public void LikeComment()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;

            sharePointSite.AddComment("test");

            sharePointSite.ClickLikeComment();
            Thread.Sleep(500);

            object likeComment = null;
            object commentAuthor = null;
            object userLogin = null;
            object commentUrl = null;
            for (int i = 0; i < 10; i++)
            {
                likeComment = jse.ExecuteScript("return dataLayer.find(x => x.event === 'likeComment')");
                commentAuthor = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentAuthor')");
                userLogin = jse.ExecuteScript("return dataLayer.find(x => x.event === 'userLogin')");
                commentUrl = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentUrl')");
                if (likeComment != null & commentAuthor != null & userLogin != null & commentUrl != null)
                    break;
                Thread.Sleep(500);
            }

            Assert.NotNull(likeComment);
            Assert.NotNull(commentAuthor);
            Assert.NotNull(userLogin);
            Assert.NotNull(commentUrl);
        }

        [Test]
        public void ReplyComment()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;

            sharePointSite.AddComment("test");

            sharePointSite.AddReplyComment("test");
            Thread.Sleep(500);
            object commentReply = null;
            object commentAuthor = null;
            object commentReplyUrl = null;
            object commentUrl = null;
            for (int i = 0; i < 10; i++)
            {
                commentReply = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentReply')");
                commentAuthor = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentAuthor')");
                commentReplyUrl = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentReplyUrl')");
                commentUrl = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentUrl')");
                if (commentReply != null & commentAuthor != null & commentReplyUrl != null & commentUrl != null)
                    break;
                Thread.Sleep(500);
            }

            Assert.NotNull(commentReply);
            Assert.NotNull(commentAuthor);
            Assert.NotNull(commentReplyUrl);
            Assert.NotNull(commentUrl);
        }

        [Test]
        public void ReplyCommentLike()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;

            sharePointSite.AddComment("test");

            sharePointSite.AddReplyComment("test");

            sharePointSite.ClickCommentReplyLike();
            Thread.Sleep(500);

            object likeComment = null;
            for (int i = 0; i < 10; i++)
            {
                likeComment = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentReply')");
                if (likeComment != null)
                    break;
                Thread.Sleep(500);
            }

            Assert.NotNull(likeComment);

        }

        [Test]
        public void ReplyCommentUnlike()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;

            sharePointSite.AddComment("test");

            sharePointSite.AddReplyComment("test");

            sharePointSite.ClickCommentReplyLike();

            sharePointSite.ClickCommentReplyUnlike();

            Thread.Sleep(500);

            object unlikeComment = null;
            for (int i = 0; i < 10; i++)
            {
                unlikeComment = jse.ExecuteScript("return dataLayer.find(x => x.event === 'commentReply')");
                if (unlikeComment != null)
                    break;
                Thread.Sleep(500);
            }

            Assert.NotNull(unlikeComment); 
        }

        [Test]
        public void PageViewed()
        {
            var sharePointSite = new SharepointSitePage(_webDriver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)_webDriver;
            Thread.Sleep(2000);

            object pageView = null;
            object pageUrl = null;
            object pageWebTitle = null;
            object userLogin = null;
            for (int i = 0; i < 10; i++)
            {
                pageView = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageView')");
                pageUrl = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageUrl')");
                pageWebTitle = jse.ExecuteScript("return dataLayer.find(x => x.event === 'pageWebTitle')");
                userLogin = jse.ExecuteScript("return dataLayer.find(x => x.event === 'userLogin')");
                if (pageView != null & pageUrl != null & pageWebTitle != null & userLogin != null)
                    break;
                Thread.Sleep(500);
            }

            Assert.NotNull(pageView); 
            Assert.NotNull(pageUrl); 
            Assert.NotNull(pageWebTitle); 
            Assert.NotNull(userLogin); 
        }
    }
}
