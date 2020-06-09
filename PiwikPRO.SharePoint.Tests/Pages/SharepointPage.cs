using AngleSharp.Dom;
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

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='pageCommandBarSaveTemplateButton']")]
        private IWebElement saveAsTemplateButton;

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

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='2']")]
        private IWebElement newDocumentLibraryFromHomePageButton;

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

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='shareCommand']")]
        private IWebElement shareFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='promoteAddToNav']")]
        private IWebElement addPageToNavigationButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='promotePostToNews']")]
        private IWebElement promoteAsNewsButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='promoteEmail']")]
        private IWebElement promoteSendByEmailButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='savePageAsTemplate']")]
        private IWebElement promoteSavePageAsTemplateButton;

        [FindsBy(How = How.XPath, Using = "//input[@data-automation-id='sendByEmailDialogPeoplePicker']")]
        private IWebElement peoplePickerInput;

        [FindsBy(How = How.XPath, Using = "//input[starts-with(@class,'ms-BasePicker-input')]")]
        private IWebElement peoplePickerForItemShareInput;

        [FindsBy(How = How.ClassName, Using = "ms-PeoplePicker-personaContent")]
        private IWebElement resultForPeoplePicker;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='sendByEmailDialogSendButton']")]
        private IWebElement sendByEmailButton;

        [FindsBy(How = How.ClassName, Using = "ms-Button--primary")]
        private IWebElement sendButton;

        [FindsBy(How = How.XPath, Using = "//button[contains(@class,'copyButton_')]")]
        private IWebElement copyAddressButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automation-id='promoteButton']")]
        private IWebElement promoteButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='copyLinkCommand']")]
        private IWebElement copyLinkFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='editPinnedItemCommand']")]
        private IWebElement editPinFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='removePinnedItemCommand']")]
        private IWebElement unpinFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='pinItemCommand']")]
        private IWebElement pinItemFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='12']")]
        private IWebElement makeHomePageFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='deleteCommand']")]
        private IWebElement deleteFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='switchLayoutCommand']")]
        private IWebElement switchViewOnSitePagesButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='confirmbutton']")]
        private IWebElement confirmDeletionButton;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='switchViewCommand_cf47a07c-e5d4-4f3d-9cb9-a57ba4002b42']")]
        private IWebElement showAllPagesButton;

        [FindsBy(How = How.XPath, Using = "//li[contains(@id,'WikiPageTab-title')]")]
        private IWebElement startRibbonPageLi;

        [FindsBy(How = How.XPath, Using = "//a[contains(@aria-describedby,'Edit_ToolTip')]")]
        private IWebElement editRibbonPage;

        [FindsBy(How = How.XPath, Using = "//a[contains(@aria-describedby,'SaveAndStop_ToolTip')]")]
        private IWebElement saveFirstRibbonPage;

        public SharepointPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }
        public string WikiPageCreation()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='newCommand']")));
            newMenuItem.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            wikiPageButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@name='ctl00$PlaceHolderMain$nameInput']")));
            newWikiPageNameInput.Click();
            newWikiPageNameInput.SendKeys($"WikiPageTest{random}");
            Thread.Sleep(500);
            submitNewWikiPageButton.Click();
            return $"WikiPageTest{random}";
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
        public string SitePageFromHomePageCreation()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
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
            newSitePageNameInput.SendKeys($"SitePageTest{random}");
            Thread.Sleep(1000);
            publishButton.Click();
            Thread.Sleep(1000);
            return $"SitePageTest{random}";
        }

        public void PageEditedFirstSaveFromTopBar(string sitePagesLibrary)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl(sitePagesLibrary);
            WikiPageCreation();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[contains(@id,'WikiPageTab-title')]")));
            startRibbonPageLi.Click();
           // wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(@aria-describedby,'Edit_ToolTip')]")));
            //editRibbonPage.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(@aria-describedby,'SaveAndStop_ToolTip')]")));
            saveFirstRibbonPage.Click();
        }
        public void ReloadPageClickPromoteButton()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().Refresh();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id='promoteButton']")));
            promoteButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
        }
        public void PromoteAddPageToNavigation()
        {
            this.SitePageFromHomePageCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
            addPageToNavigationButton.Click();
            Thread.Sleep(1000);
        }
        public void PromoteAddPageToNavigationExistingPage()
        {
            this.SitePageFromHomePageCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
            this.ReloadPageClickPromoteButton();
            addPageToNavigationButton.Click();
            Thread.Sleep(1000);
        }
        public void PromotePostAsNews()
        {
            this.SitePageFromHomePageCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
            promoteAsNewsButton.Click();
            Thread.Sleep(1000);
        }
        public void PromotePostAsNewsExistingPage()
        {
            this.SitePageFromHomePageCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
            this.ReloadPageClickPromoteButton();
            promoteAsNewsButton.Click();
            Thread.Sleep(1000);
        }
        public void PromoteSendByEmail()
        {
            this.SitePageFromHomePageCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
            promoteSendByEmailButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@data-automation-id='sendByEmailDialogPeoplePicker']")));
            peoplePickerInput.Click();
            peoplePickerInput.SendKeys("mzywicki@phkogifi.onmicrosoft.com");
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-PeoplePicker-pickerPersona")));
            resultForPeoplePicker.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@data-automation-id='sendByEmailDialogSendButton']")));
            sendByEmailButton.Click();
            Thread.Sleep(1000);
        }
        public void PromoteSendByEmailExistingPage()
        {
            this.SitePageFromHomePageCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
            this.ReloadPageClickPromoteButton();
            promoteSendByEmailButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@data-automation-id='sendByEmailDialogPeoplePicker']")));
            peoplePickerInput.Click();
            peoplePickerInput.SendKeys("mzywicki@phkogifi.onmicrosoft.com");
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-PeoplePicker-pickerPersona")));
            resultForPeoplePicker.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@data-automation-id='sendByEmailDialogSendButton']")));
            sendByEmailButton.Click();
            Thread.Sleep(1000);
        }
        public void PromoteSaveAsPageTemplate()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            this.SitePageFromHomePageCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
            promoteSavePageAsTemplateButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//textarea[@data-automation-id='pageTitleInput']")));
            newSitePageNameInput.SendKeys(Keys.Tab);
            newSitePageNameInput.Clear();
            newSitePageNameInput.SendKeys($"Site Page Template Test {random}");
            Thread.Sleep(1000);
            saveAsTemplateButton.Click();
            Thread.Sleep(2000);
        }
        public void PromoteSaveAsPageTemplateExistingPage()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            this.SitePageFromHomePageCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
            this.ReloadPageClickPromoteButton();
            promoteSavePageAsTemplateButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//textarea[@data-automation-id='pageTitleInput']")));
            newSitePageNameInput.SendKeys(Keys.Tab);
            newSitePageNameInput.Clear();
            newSitePageNameInput.SendKeys($"Site Page Template Test {random}");
            Thread.Sleep(1000);
            saveAsTemplateButton.Click();
            Thread.Sleep(2000);
        }
        public void PromoteCopyAddress()
        {
            this.SitePageFromHomePageCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
            Thread.Sleep(500);
            copyAddressButton.Click();
            Thread.Sleep(500);
            copyAddressButton.Click();
            Thread.Sleep(2000);
        }
        public void PromoteCopyAddressExistingPage()
        {
            this.SitePageFromHomePageCreation();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Panel-navigation")));
            this.ReloadPageClickPromoteButton();
            Thread.Sleep(500);
            copyAddressButton.Click();
            Thread.Sleep(500);
            copyAddressButton.Click();
            Thread.Sleep(2000);
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
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-TextField-field")));
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
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("od-ItemsScopeItemContent-header")));
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
        public string NewListItemCreation()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            string listItemName = "testItem";
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@data-automationid='splitbuttonprimary']")));
            newItemButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("ms-TextField-field")));
            newItemInput.Click();
            newItemInput.SendKeys(listItemName);
            saveItemButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@data-automationid='FieldRenderer-name']")));
            Thread.Sleep(500);
            return listItemName;
        }
        public void ListItemDirectView()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@data-automationid='FieldRenderer-name']")));
            itemButton.Click();
            Thread.Sleep(500);
        }
        public void ListItemShare(string listItemName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@data-automationid='FieldRenderer-name']")));
            var row = wait.Until(x => x.FindElement(By.XPath($"//div[starts-with(@aria-label,'{listItemName}')]")));
            var element = row.FindElement(By.XPath(".//button[@data-automationid='FieldRender-DotDotDot']"));
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
            Thread.Sleep(1500);
            element.Click();
            shareFromContextMenu.Click();
            Thread.Sleep(1500);
            driver.SwitchTo().Frame("shareFrame");
            Thread.Sleep(500);
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("//input[@role='textbox']")));
            peoplePickerForItemShareInput.Click();
            peoplePickerForItemShareInput.SendKeys("mzywicki@phkogifi.onmicrosoft.com");
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-PeoplePicker-pickerPersona")));
            resultForPeoplePicker.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("ms-Button--primary")));
            sendButton.Click();
            driver.SwitchTo().DefaultContent();
            Thread.Sleep(1500);
        }
        public string NewDocumentLibraryFromHomePageCreation()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id='pageCommandBarNewButton']")));
            newMenuItemFromHomePage.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-ContextualMenu-Callout")));
            newDocumentLibraryFromHomePageButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-TextField-field")));
            newListPageNameInput.Click();
            newListPageNameInput.SendKeys($"testdocumentLibrary{random}");
            showInLocalNavCheckbox.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@data-automationid='splitbuttonprimary']")));
            Thread.Sleep(1000);
            newListButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("od-ItemsScopeItemContent-header")));
            //Thread.Sleep(2500);
            return $"testlist{random}";

        }
        public void PageCopyLink(string pageName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='newCommand']")));
            switchViewOnSitePagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='switchViewCommand_cf47a07c-e5d4-4f3d-9cb9-a57ba4002b42']")));
            showAllPagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[starts-with(@aria-label,'{pageName}')]")));
            var row = wait.Until(x => x.FindElement(By.XPath($"//div[starts-with(@aria-label,'{pageName}')]")));
            var element = row.FindElement(By.XPath(".//button[@data-automationid='FieldRender-DotDotDot']"));
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
            Thread.Sleep(200);
            element.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='copyLinkCommand']")));
            copyLinkFromContextMenu.Click();
            Thread.Sleep(1500);

        }
        public void PagePinToTopAndUnpinned(string pageName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='newCommand']")));
            switchViewOnSitePagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='switchViewCommand_cf47a07c-e5d4-4f3d-9cb9-a57ba4002b42']")));
            showAllPagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[starts-with(@aria-label,'{pageName}')]")));
            var row = wait.Until(x => x.FindElement(By.XPath($"//div[starts-with(@aria-label,'{pageName}')]")));
            var element = row.FindElement(By.XPath(".//button[@data-automationid='FieldRender-DotDotDot']"));
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
            Thread.Sleep(200);
            element.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='copyLinkCommand']")));
            pinItemFromContextMenu.Click();
            Thread.Sleep(1500);
            var spotlightContainer = wait.Until(x => x.FindElement(By.XPath($"//div[starts-with(@class,'spotlightContainer')]")));
            var elPinned = spotlightContainer.FindElement(By.XPath($".//div[starts-with(@aria-label,'{pageName}')]"));
            var threeDotMenuButton = elPinned.FindElement(By.XPath($".//span[@data-automationid='splitbuttonprimary']"));
            action.MoveToElement(elPinned).Perform();
            Thread.Sleep(200);
            threeDotMenuButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='editPinnedItemCommand']")));
            editPinFromContextMenu.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='removePinnedItemCommand']")));
            unpinFromContextMenu.Click();
            Thread.Sleep(1000);
        }

        public void PageMakeHomePage(string pageName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='newCommand']")));
            switchViewOnSitePagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='switchViewCommand_cf47a07c-e5d4-4f3d-9cb9-a57ba4002b42']")));
            showAllPagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[starts-with(@aria-label,'{pageName}')]")));
            var row = wait.Until(x => x.FindElement(By.XPath($"//div[starts-with(@aria-label,'{pageName}')]")));
            var element = row.FindElement(By.XPath(".//button[@data-automationid='FieldRender-DotDotDot']"));
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
            Thread.Sleep(200);
            element.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@aria-posinset='15']")));
            makeHomePageFromContextMenu.Click();
            Thread.Sleep(1500);
        }
        public void PageDelete(string pageName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='newCommand']")));
            switchViewOnSitePagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='switchViewCommand_cf47a07c-e5d4-4f3d-9cb9-a57ba4002b42']")));
            showAllPagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[starts-with(@aria-label,'{pageName}')]")));
            var row = wait.Until(x => x.FindElement(By.XPath($"//div[starts-with(@aria-label,'{pageName}')]")));
            var element = row.FindElement(By.XPath(".//button[@data-automationid='FieldRender-DotDotDot']"));
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
            Thread.Sleep(200);
            element.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='deleteCommand']")));
            deleteFromContextMenu.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@data-automationid='confirmbutton']")));
            confirmDeletionButton.Click();
            Thread.Sleep(1500);
        }
    }
}
