using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace PiwikPRO.SharePoint.Tests.Pages
{
    class SharepointSitePage
    {
        private IWebDriver driver;

        public SharepointSitePage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//i[@data-icon-name='Like']")]
        private IWebElement likePageButton;

        [FindsBy(How = How.XPath, Using = "//i[@data-icon-name='LikeSolid']")]
        private IWebElement unlikePageButton;

        [FindsBy(How = How.XPath, Using = "//div[@id='sp-comment-input']")]
        private IWebElement commentInput;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='sp-comment-post']")]
        private IWebElement addCommentButton;

        [FindsBy(How = How.XPath, Using = "(//i[@data-icon-name='Like'])")]
        private IWebElement likeCommentButton;

        [FindsBy(How = How.XPath, Using = "//i[@data-icon-name='LikeSolid']")]
        private IWebElement unlikeCommentButton;

        [FindsBy(How = How.XPath, Using = "(//button[@data-automation-id='comment-reply-button'])[1]")]
        private IWebElement commentReplyButton;

        [FindsBy(How = How.XPath, Using = "(//div[starts-with(@id,'sp-comment-input-reply')])[1]")]
        private IWebElement commentReplyInput;

        [FindsBy(How = How.XPath, Using = "(//button[@aria-label='Like the comment.'])[2]")]
        private IWebElement commentReplyLikeButton;

        [FindsBy(How = How.XPath, Using = "(//button)[35]")]
        private IWebElement commentReplyUnlikeButton;

        [FindsBy(How = How.XPath, Using = "(//button[starts-with(@data-automation-id,'sp-comment-reply-post-')])[1]")]
        private IWebElement commentReplySubmitButton;

        public void ClickLikePage()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//i[@data-icon-name='Like']")));
            likePageButton.Click();
        }

        public void ClickUnlikePage()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//i[@data-icon-name='LikeSolid']")));
            unlikePageButton.Click();
        }

        public void AddComment(string comment)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='sp-comment-input']")));
            commentInput.Click();
            commentInput.SendKeys(comment);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id='sp-comment-post']")));

            addCommentButton.Click();
        }

        public void ClickLikeComment()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//i[@data-icon-name='Like'])[1]")));
            likeCommentButton.Click();
        }

        public void ClickUnlikeComment()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//i[@data-icon-name='LikeSolid']")));

            unlikeCommentButton.Click();
        }

        public void AddReplyComment(string replyComment)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            commentReplyButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//div[starts-with(@id,'sp-comment-input-reply')])[1]")));
            commentReplyInput.SendKeys(replyComment);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//button[starts-with(@data-automation-id,'sp-comment-reply-post-')])[1]")));
            commentReplySubmitButton.Click();
        }

        public void ClickCommentReplyLike()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//button)[35]")));
            commentReplyLikeButton.Click();
        }

        public void ClickCommentReplyUnlike()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//button)[35]")));
            commentReplyUnlikeButton.Click();
        }
    }
}
