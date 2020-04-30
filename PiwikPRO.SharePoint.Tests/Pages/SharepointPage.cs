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
    class SharepointPage
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='newCommand']")]
        private IWebElement newMenuItem;
        
        [FindsBy(How = How.XPath, Using = "//i[@data-icon-name='ChevronDown']")]
        private IWebElement newMenuItemFromSiteContents;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='2']")]
        private IWebElement wikiPageButton;

        [FindsBy(How = How.XPath, Using = "//input[@name='ctl00$PlaceHolderMain$nameInput']")]
        private IWebElement newWikiPageNameInput;
        
        [FindsBy(How = How.XPath, Using = "//input[@name='ctl00$PlaceHolderMain$createButton']")]
        private IWebElement submitNewWikiPageButton;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='3']")]
        private IWebElement webPartPageButton;

        [FindsBy(How = How.XPath, Using = "//input[@id='onetidListTitle']")]
        private IWebElement newWebPartPageNameInput;

        [FindsBy(How = How.Id, Using = "onetidCreate")]
        private IWebElement submitNewWebPartPageButton;

        [FindsBy(How = How.Id, Using = "OverwriteCheckBox")]
        private IWebElement webPartPageOverwriteCheckbox;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='1']")]
        private IWebElement sitePageButton;
        
        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='2']")]
        private IWebElement sitePageFromSiteContentsButton;

        [FindsBy(How = How.XPath, Using = "//textarea[@data-automation-id='pageTitleInput']")]
        private IWebElement newSitePageNameInput;

        [FindsBy(How = How.XPath, Using = "//input[@data-automation-id='newslink-url']")]
        private IWebElement newsLinkPageNameInput;

        [FindsBy(How = How.ClassName, Using = "ms-TextField-field")]
        private IWebElement newListPageNameInput;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='pageCommandBarPublishButton']")]
        private IWebElement publishButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='newslink-post-button']")]
        private IWebElement newsLinkButton;

        [FindsBy(How = How.ClassName, Using = "ms-ListCreationPanel-CreateButton")]
        private IWebElement newListButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='pageCommandBarNewButton']")]
        private IWebElement newMenuItemFromHomePage;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='3']")]
        private IWebElement sitePageFromHomePageButton;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='5']")]
        private IWebElement newsLinkFromHomePageButton;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='1']")]
        private IWebElement newListFromHomePageButton;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='5']")]
        private IWebElement newSubsiteFromSiteContentsButton;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='1']")]
        private IWebElement newListFromSiteContentsButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='template-panel-create-button']")]
        private IWebElement createPageButton;
        
        [FindsBy(How = How.XPath, Using = "//i[@data-icon-name='CheckMark']")]
        private IWebElement showInLocalNavCheckbox;

        [FindsBy(How = How.Id, Using = "ctl00_PlaceHolderMain_idTitleDescSection_ctl01_TxtCreateSubwebTitle")]
        private IWebElement newSubsiteTitleInput;

        [FindsBy(How = How.Id, Using = "ctl00_PlaceHolderMain_idUrlSection_ctl01_TxtCreateSubwebName")]
        private IWebElement newSubsiteNameInput;

        [FindsBy(How = How.Id, Using = "ctl00_PlaceHolderMain_ctl00_RptControls_BtnCreateSubweb")]
        private IWebElement newSubsiteCreateButton;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='1']")]
        private IWebElement removeContextMenuButton;

        [FindsBy(How = How.ClassName, Using = "od-Dialog-actionsRight")]
        private IWebElement deleteListButton;

        [FindsBy(How = How.Id, Using = "O365_MainLink_Settings")]
        private IWebElement settingsButton;

        [FindsBy(How = How.Id, Using = "SuiteMenu_zz6_MenuItem_ViewAllSiteContents")]
        private IWebElement siteContentsButton;

        [FindsBy(How = How.XPath, Using = "(//i[@data-icon-name='CalculatorAddition'])[1]")]
        private IWebElement newItemButton;

        [FindsBy(How = How.ClassName, Using = "ms-TextField-field")]
        private IWebElement newItemInput;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='ReactClientFormSaveButton']")]
        private IWebElement saveItemButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='FieldRenderer-name']")]
        private IWebElement itemButton;

        public SharepointPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }
        public void WikiPageCreation()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='newCommand']")));
            newMenuItem.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            wikiPageButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@name='ctl00$PlaceHolderMain$nameInput']")));
            newWikiPageNameInput.Click();
            newWikiPageNameInput.SendKeys($"WikiPageTest{random}");
            Thread.Sleep(500);
            submitNewWikiPageButton.Click();
        }
        public void WebPartPageCreation()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='newCommand']")));
            newMenuItem.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            webPartPageButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='onetidListTitle']")));
            newWebPartPageNameInput.Click();
            newWebPartPageNameInput.SendKeys("WebPartPageTest");
            webPartPageOverwriteCheckbox.Click();
            submitNewWebPartPageButton.Click();
            Thread.Sleep(1000);
        }
        public void SitePageCreation()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='newCommand']")));
            newMenuItem.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            sitePageButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//textarea[@data-automation-id='pageTitleInput']")));
            newSitePageNameInput.SendKeys(Keys.Tab);
            newSitePageNameInput.Clear();
            newSitePageNameInput.SendKeys("Site Page Test");
            publishButton.Click();
            Thread.Sleep(1000);
        }
        public void SitePageFromSiteContentsCreation()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//i[@data-icon-name='ChevronDown']")));
            newMenuItemFromSiteContents.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            sitePageFromSiteContentsButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//textarea[@data-automation-id='pageTitleInput']")));
            newSitePageNameInput.SendKeys(Keys.Tab);
            newSitePageNameInput.Clear();
            newSitePageNameInput.SendKeys("Site Page Test");
            publishButton.Click();
            Thread.Sleep(1000);

        }
        public void SitePageFromHomePageCreation()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id='pageCommandBarNewButton']")));
            newMenuItemFromHomePage.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            sitePageFromHomePageButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id='template-panel-create-button']")));
            createPageButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//textarea[@data-automation-id='pageTitleInput']")));
            newSitePageNameInput.SendKeys(Keys.Tab);
            newSitePageNameInput.Clear();
            newSitePageNameInput.SendKeys("Site Page Test");
            Thread.Sleep(1000);
            publishButton.Click();
            Thread.Sleep(1000);
        }
        public void NewsLinkFromHomePageCreation()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id='pageCommandBarNewButton']")));
            newMenuItemFromHomePage.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            newsLinkFromHomePageButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@data-automation-id='newslink-url']")));
            newsLinkPageNameInput.Click();
            newsLinkPageNameInput.SendKeys("https://phkogifi.sharepoint.com/sites/PH/SitePages/Test-Page.aspx");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@data-automation-id='newslink-post-button']")));
            Thread.Sleep(1000);
            newsLinkButton.Click();
            Thread.Sleep(2500);
        }
        public void NewListFromSiteContentsCreation()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//i[@data-icon-name='ChevronDown']")));
            newMenuItemFromSiteContents.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            newListFromSiteContentsButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input.ms-TextField-field")));
            newListPageNameInput.Click();
            newListPageNameInput.SendKeys($"testlist{random}");
            showInLocalNavCheckbox.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@data-automationid='splitbuttonprimary']")));
            Thread.Sleep(1000);
            newListButton.Click();
            //Thread.Sleep(2500);
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
            //Thread.Sleep(2500);
            return $"testlist{random}";

        }

        public void NewSubsiteFromSiteContentsCreation()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//i[@data-icon-name='ChevronDown']")));
            newMenuItemFromSiteContents.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            newSubsiteFromSiteContentsButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ctl00_PlaceHolderMain_idTitleDescSection_ctl01_TxtCreateSubwebTitle")));
            newSubsiteTitleInput.Click();
            newSubsiteTitleInput.SendKeys($"testlist{random}");
            newSubsiteNameInput.Click();
            newSubsiteNameInput.SendKeys($"testlist{random}");
            Thread.Sleep(500);
            newSubsiteCreateButton.Click();
        }
        public void DeleteList(string listname)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("O365_MainLink_Settings")));
            settingsButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("SuiteMenu_zz6_MenuItem_ViewAllSiteContents")));
            siteContentsButton.Click();
            var row = wait.Until(x => x.FindElement(By.XPath($"//div[starts-with(@aria-label,'{listname}')]")));
            var element = row.FindElement(By.ClassName("od-FieldRenderer-dot"));
            element.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            removeContextMenuButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("od-Dialog-actionsRight")));
            deleteListButton.Click();
            Thread.Sleep(300);
        }
        public void NewListItemCreation()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@data-automationid='splitbuttonprimary']")));
            newItemButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("ms-TextField-field")));
            newItemInput.Click();
            newItemInput.SendKeys("testItem");
            saveItemButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@data-automationid='FieldRenderer-name']")));
            itemButton.Click();
            Thread.Sleep(500);
            
        }
    }
}
