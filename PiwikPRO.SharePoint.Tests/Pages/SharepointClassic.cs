using AngleSharp.Dom;
using Microsoft.SharePoint.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.Tests.Classic
{
    class SharepointClassic
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='newCommand']")]
        private IWebElement newMenuItem;

        [FindsBy(How = How.Id, Using = "idHomePageNewItem")]
        private IWebElement newItemButton;

        [FindsBy(How = How.XPath, Using = "//input[starts-with(@id,'Title')]")]
        private IWebElement newItemInput;

        [FindsBy(How = How.Id, Using = "ctl00_ctl30_g_7f37dd2c_dbc8_4d5a_9b71_366ccb1e5409_ctl00_toolBarTbl_RightRptControls_ctl00_ctl00_diidIOSaveItem")]
        private IWebElement saveItemButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='pageCommandBarNewButton']")]
        private IWebElement newMenuItemFromHomePage;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='1']")]
        private IWebElement newListFromHomePageButton;

        [FindsBy(How = How.ClassName, Using = "ms-TextField-field")]
        private IWebElement newListPageNameInput;

        [FindsBy(How = How.XPath, Using = "//i[@data-icon-name='CheckMark']")]
        private IWebElement showInLocalNavCheckbox;

        [FindsBy(How = How.ClassName, Using = "ms-ListCreationPanel-CreateButton")]
        private IWebElement newListButton;

        [FindsBy(How = How.CssSelector, Using = ".LeftNav-subLinksClip .LeftNav-notificationLink .LeftNav-linkText")]
        private IWebElement classicViewButton;

        [FindsBy(How = How.Id, Using = "QCB1_Button1")]
        private IWebElement newButton;

        [FindsBy(How = How.Id, Using = "js-newdocWOPI-divFolder-WPQ1")]
        private IWebElement newFolderButton;

        [FindsBy(How = How.Id, Using = "js-newdocWOPI-divWord-WPQ1")]
        private IWebElement newWordFileButton;

        [FindsBy(How = How.Id, Using = "ccfc_folderNameInput_0_onetidIOFile")]
        private IWebElement newFolderNameInput;

        [FindsBy(How = How.Id, Using = "csfd_createButton_toolBarTbl_RightRptControls_diidIOSaveItem")]
        private IWebElement newFolderCreateButton;

        [FindsBy(How = How.XPath, Using = "//a[starts-with(@aria-label, 'Document')]")]
        private IWebElement fileItem;

        public SharepointClassic(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void SwitchToClassicExp()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".LeftNav-subLinksClip .LeftNav-notificationLink .LeftNav-linkText")));
            classicViewButton.Click();
        }
        public void ListItemDirectView()
        {
            //string listName = this.NewListFromHomePageCreation();
            //this.NewListItemCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idHomePageNewItem")));
            //classicViewButton.Click();
            Thread.Sleep(3000);
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("tbody > tr > td > div > a")));
            Thread.Sleep(1000);
            element.Click();
            //var attachmentItem = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#idAttachmentsTable > tbody > tr > td > span > a")));
            //attachmentItem.Click();
            //Thread.Sleep(1000);
            // driver.Navigate().Back();
            Thread.Sleep(1000);
        }
        public void ListItemAttachmentView()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            this.ListItemDirectView();
            var attachmentItem = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#idAttachmentsTable > tbody > tr > td > span > a")));
            attachmentItem.Click();
            Thread.Sleep(1000);
            driver.Navigate().Back();
            Thread.Sleep(1000);
            }
        public string NewListItemCreation()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            string listItemName = "testItem";
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idHomePageNewItem")));
            newItemButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[starts-with(@id,'Title')]")));
            newItemInput.Click();
            newItemInput.SendKeys($"{listItemName}{random}");
            saveItemButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("idHomePageNewItem")));
            Thread.Sleep(500);
            return $"{listItemName}{random}";
        }
        public string NewListFromHomePageCreation()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id='pageCommandBarNewButton']")));
            newMenuItemFromHomePage.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            newListFromHomePageButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-TextField-field")));
            newListPageNameInput.Click();
            newListPageNameInput.SendKeys($"testlist{random}");
            showInLocalNavCheckbox.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@data-automationid='splitbuttonprimary']")));
            Thread.Sleep(1000);
            newListButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("od-ItemsScopeItemContent-header")));
            //Thread.Sleep(2500);
            return $"testlist{random}";
        }

        public string FolderCreated()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            newButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cojs-newdocWOPI-WPQ1_callout-content")));
            newFolderButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ccfc_folderNameInput_0_onetidIOFile")));
            newFolderNameInput.Click();
            newFolderNameInput.SendKeys($"testfolder{random}");
            Thread.Sleep(300);
            newFolderCreateButton.Click();
            Thread.Sleep(1500);
            return $"testfolder{random}";
        }
        public void FileCreated()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            string folderName = this.FolderCreated();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var folder = wait.Until(x => x.FindElement(By.XPath($"//a[starts-with(@aria-label,'{folderName}')]")));
            folder.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            newButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("cojs-newdocWOPI-WPQ1_callout-content")));
            newWordFileButton.Click();
            Thread.Sleep(1500);
            driver.Navigate().Back();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
        }
        public void FileOpened()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            driver.Navigate().Refresh();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            fileItem.Click();
            driver.Navigate().Back();
            Thread.Sleep(3000);
        }
    }
}
