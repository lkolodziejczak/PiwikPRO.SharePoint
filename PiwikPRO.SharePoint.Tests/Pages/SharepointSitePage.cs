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

        [FindsBy(How = How.XPath, Using = "//button[@title='Like this page']")]
        private IWebElement likePageButton;

        [FindsBy(How = How.XPath, Using = "//button[@title='Unlike this page']")]
        private IWebElement unlikePageButton;

        [FindsBy(How = How.XPath, Using = "//div[@id='sp-comment-input']")]
        private IWebElement commentInput;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='sp-comment-post']")]
        private IWebElement addCommentButton;

        [FindsBy(How = How.XPath, Using = "//button[@aria-label='Like the comment.']")]
        private IWebElement likeCommentButton;

        [FindsBy(How = How.XPath, Using = "//button[@aria-label='Unlike the comment.']")]
        private IWebElement unlikeCommentButton;

        [FindsBy(How = How.XPath, Using = "(//button[@data-automation-id='comment-reply-button'])[1]")]
        private IWebElement commentReplyButton;

        [FindsBy(How = How.XPath, Using = "(//div[@aria-label='Add a comment.'])[2]")]
        private IWebElement commentReplyInput;

        [FindsBy(How = How.XPath, Using = "(//button[@aria-label='Like the comment.'])[2]")]
        private IWebElement commentReplyLikeButton;

        [FindsBy(How = How.XPath, Using = "(//button)[35]")]
        private IWebElement commentReplyUnlikeButton;

        [FindsBy(How = How.XPath, Using = "(//button[@aria-label='Post'])[2]")]
        private IWebElement commentReplySubmitButton;

        [FindsBy(How = How.XPath, Using = "//div[@data-automationid='DetailsRowCheck'][1]")]
        private IWebElement fileGrid;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='pinItemCommand']")]
        private IWebElement filePinToTopFromTopMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='editPinnedItemCommand']")]
        private IWebElement fileEditPinToTopFromTopMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='removePinnedItemCommand']")]
        private IWebElement fileUnPinToTopFromTopMenu;
        
        [FindsBy(How = How.ClassName, Using = "heroButton_2ca50ba6")]
        private IWebElement dotShowActionsMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='pinItemCommand']")]
        private IWebElement filePinToTopFromContextMenu;

        [FindsBy(How = How.XPath, Using = "(//button[@data-automationid='editPinnedItemCommand'])[2]")]
        private IWebElement fileEditPinFromContextMenu;

        [FindsBy(How = How.XPath, Using = "(//button[@data-automationid='removePinnedItemCommand'])[1]")]
        private IWebElement fileUnPinToTopFromContextMenu;

        public void ClickLikePage()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@title='Like this page']")));
            likePageButton.Click();
        }

        public void ClickUnlikePage()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@title='Unlike this page']")));
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
            likeCommentButton.Click();
        }

        public void ClickUnlikeComment()
        {
            unlikeCommentButton.Click();
        }

        public void AddReplyComment(string replyComment)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            commentReplyButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//div[@aria-label='Add a comment.'])[2]")));
            commentReplyInput.SendKeys(replyComment);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//button[@aria-label='Post'])[2]")));
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

        public void FilePinToTopFromTopMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@data-automationid='DetailsRowCheck'][1]")));
            fileGrid.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='pinItemCommand']")));
            filePinToTopFromTopMenu.Click();
        }

        public void FilePinToTopFromContextMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            fileGrid.Click();
            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            //dotShowActionsMenu.Click();
            driver.FindElements(By.CssSelector(".heroButton_2ca50ba6"))[1].Click();
            WebDriverWait wait3 = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            filePinToTopFromContextMenu.Click();
        }

        public void FileUnPinToTopFromTopMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@data-automationid='DetailsRowCheck'][1]")));
            fileGrid.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='editPinnedItemCommand']")));
            fileEditPinToTopFromTopMenu.Click();

            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='removePinnedItemCommand']")));
            fileUnPinToTopFromTopMenu.Click();
        }

        public void FileUnPinToTopFromContextMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            fileGrid.Click();
            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            driver.FindElements(By.CssSelector(".heroButton_2ca50ba6"))[1].Click();
            //dotShowActionsMenu2.Click();
            WebDriverWait wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//button[@data-automationid='editPinnedItemCommand'])[1]")));
            fileEditPinFromContextMenu.Click();

            WebDriverWait wait3 = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//button[@data-automationid='removePinnedItemCommand'])[1]")));
            fileUnPinToTopFromContextMenu.Click();
        }

    }
}
