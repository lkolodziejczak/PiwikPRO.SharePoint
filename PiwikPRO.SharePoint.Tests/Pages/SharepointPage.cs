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

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='pageCommandBarPublishButton']")]
        private IWebElement publishButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='pageCommandBarNewButton']")]
        private IWebElement newMenuItemFromHomePage;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='3']")]
        private IWebElement sitePageFromHomePageButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='template-panel-create-button']")]
        private IWebElement createPageButton;
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
            submitNewWikiPageButton.Click();
            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Ribbon.EditingTools.CPEditTab.EditAndCheckout.SaveAndPublish-Large")));
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
            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Ribbon.WebPartPage")));
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
            //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Ribbon.WebPartPage")));
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
        }
    }
}
