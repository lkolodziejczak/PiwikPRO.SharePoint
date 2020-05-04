using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.Tests.Pages
{
    class SharepointFolderFile
    {
        private IWebDriver driver;
        [FindsBy(How = How.XPath, Using = "//div[@data-automationid='DetailsRowCheck'][1]")]
        private IWebElement fileGrid;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='pinItemCommand']")]
        private IWebElement filePinToTopFromTopMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='editPinnedItemCommand']")]
        private IWebElement fileEditPinToTopFromTopMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='removePinnedItemCommand']")]
        private IWebElement fileUnPinToTopFromTopMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='FieldRender-DotDotDot']")]
        private IWebElement dotShowActionsMenu;

        [FindsBy(How = How.XPath, Using = "(//button[@data-automationid='pinItemCommand'])[1]")]
        private IWebElement filePinToTopFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='shareCommand']")]
        private IWebElement fileShareFromTopMenu;

        [FindsBy(How = How.XPath, Using = "//li[@title='Share the selected item with other people']//button[@data-automationid='shareCommand']")]
        private IWebElement fileShareFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='FieldRender-ShareHero']")]
        private IWebElement fileShareFromGridMenu;

        [FindsBy(How = How.XPath, Using = "(//button[@data-automationid='editPinnedItemCommand'])[2]")]
        private IWebElement fileEditPinFromContextMenu;

        [FindsBy(How = How.XPath, Using = "(//button[@data-automationid='removePinnedItemCommand'])[1]")]
        private IWebElement fileUnPinToTopFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='newCommand']")]
        private IWebElement newMenuItem;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='createFolderCommand']")]
        private IWebElement newFolderFromNewMenu;

        [FindsBy(How = How.XPath, Using = "//input[@placeholder='Enter your folder name']")]
        private IWebElement folderfromTopMenuTextInput;

        [FindsBy(How = How.XPath, Using = "//input[@placeholder='Enter a name or email address']")]
        private IWebElement shareWindowInput;

        [FindsBy(How = How.ClassName, Using = "ms-Button--primary")]
        private IWebElement shareWindowSendButton;

        public SharepointFolderFile(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
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
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='FieldRender-DotDotDot']")));
            dotShowActionsMenu.Click();
            Thread.Sleep(200);
            filePinToTopFromContextMenu.Click();
        }

        public void FileUnPinToTopFromTopMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            fileGrid.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='editPinnedItemCommand']")));
            fileEditPinToTopFromTopMenu.Click();

            Thread.Sleep(500);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='removePinnedItemCommand']")));
            fileUnPinToTopFromTopMenu.Click();
        }

        public void FileUnPinToTopFromContextMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            fileGrid.Click();
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='FieldRender-DotDotDot']")));
            dotShowActionsMenu.Click();
            Thread.Sleep(500);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//button[@data-automationid='editPinnedItemCommand'])[2]")));
            fileEditPinFromContextMenu.Click();

            Thread.Sleep(500);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("(//button[@data-automationid='removePinnedItemCommand'])[1]")));
            fileUnPinToTopFromContextMenu.Click();
        }

        public void NewFolderFromTopMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='newCommand']")));
            newMenuItem.Click();
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='createFolderCommand']")));
            newFolderFromNewMenu.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@placeholder='Enter your folder name']")));
            folderfromTopMenuTextInput.SendKeys("FolderToTest"+DateTime.Now.ToString("hhmmss"));
            Thread.Sleep(500);
            folderfromTopMenuTextInput.SendKeys(Keys.Enter);
        }

        public void FileSharedFromTopMenu(string shareToWho)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            fileGrid.Click();
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='FieldRender-DotDotDot']")));
            dotShowActionsMenu.Click();
            Thread.Sleep(500);
            fileShareFromTopMenu.Click();
            driver.SwitchTo().Frame("shareFrame");
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@placeholder='Enter a name or email address']")));
            Thread.Sleep(300);
            shareWindowInput.SendKeys(shareToWho);
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Suggestions-itemButton")));
            shareWindowInput.SendKeys(Keys.Enter);
            Thread.Sleep(1500);

            shareWindowSendButton.Click();
            driver.SwitchTo().DefaultContent();
        }

        public void FileSharedFromContextMenu(string shareToWho)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            fileGrid.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='FieldRender-DotDotDot']")));
            dotShowActionsMenu.Click();
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@title='Share the selected item with other people']//button[@data-automationid='shareCommand']")));
            Thread.Sleep(300);
            fileShareFromContextMenu.Click();
            driver.SwitchTo().Frame("shareFrame");
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@placeholder='Enter a name or email address']")));
            Thread.Sleep(300);
            shareWindowInput.SendKeys(shareToWho);
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Suggestions-itemButton")));
            shareWindowInput.SendKeys(Keys.Enter);
            Thread.Sleep(1500);

            shareWindowSendButton.Click();
            driver.SwitchTo().DefaultContent();
        }
        public void FileSharedFromGridMenu(string shareToWho)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            fileGrid.Click();
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='FieldRender-ShareHero']")));
            fileShareFromGridMenu.Click();
            driver.SwitchTo().Frame("shareFrame");
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@placeholder='Enter a name or email address']")));
            Thread.Sleep(300);
            shareWindowInput.SendKeys(shareToWho);
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Suggestions-itemButton")));
            shareWindowInput.SendKeys(Keys.Enter);
            Thread.Sleep(1500);

            shareWindowSendButton.Click();
            driver.SwitchTo().DefaultContent();
        }
    }
}
