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

        [FindsBy(How = How.Id, Using = "ctl00_PlaceHolderMain_nameInput")]
        private IWebElement newPageNameInput;

        [FindsBy(How = How.XPath, Using = "//i[@data-icon-name='CheckMark']")]
        private IWebElement showInLocalNavCheckbox;

        [FindsBy(How = How.ClassName, Using = "ms-ListCreationPanel-CreateButton")]
        private IWebElement newListButton;

        [FindsBy(How = How.Id, Using = "ctl00_PlaceHolderMain_createButton")]
        private IWebElement newPageButton;

        [FindsBy(How = How.Id, Using = "Ribbon.EditingTools.CPEditTab.EditAndCheckout.SaveEdit-SelectedItem")]
        private IWebElement saveOnRibbonPageButton;

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

        [FindsBy(How = How.Id, Using = "WPQ1_ListTitleViewSelectorMenu_Container_surfaceopt1")]
        private IWebElement allPagesButton;

        [FindsBy(How = How.Id, Using = "btnShare")]
        private IWebElement shareButton;

        [FindsBy(How = How.XPath, Using = "//a[@title='Files']")]
        private IWebElement ribbonFiles;

        [FindsBy(How = How.XPath, Using = "//a[contains(@id, 'NewFolder-Large')]")]
        private IWebElement newFolderButtonFromRibbon;

        public SharepointClassic(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }
        public void ListItemDirectView()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Thread.Sleep(3000);
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("tbody > tr > td > div > a")));
            Thread.Sleep(1000);
            element.Click();
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
            Thread.Sleep(1500);
            newFolderCreateButton.Click();
            Thread.Sleep(500);
            return $"testfolder{random}";
        }

        public string FolderCreatedFromRibbon()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@title='Files']")));
            ribbonFiles.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(@id, 'NewFolder-Large')]")));
            newFolderButtonFromRibbon.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ccfc_folderNameInput_0_onetidIOFile")));
            newFolderNameInput.Click();
            newFolderNameInput.SendKeys($"testfolder{random}");
            Thread.Sleep(1500);
            newFolderCreateButton.Click();
            Thread.Sleep(500);
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
        public string PageCreated()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            newButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_PlaceHolderMain_nameInput")));
            newPageNameInput.Click();
            newPageNameInput.SendKeys($"testPage{random}");
            Thread.Sleep(500);
            newPageButton.Click();
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Ribbon.EditingTools.CPEditTab.EditAndCheckout.SaveEdit-SelectedItem")));
            //saveOnRibbonPageButton.Click();
            Thread.Sleep(3000);
            return $"testPage{random}";
        }
        public string PageCreatedForPageAdditionalNeeds()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            newButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ctl00_PlaceHolderMain_nameInput")));
            newPageNameInput.Click();
            newPageNameInput.SendKeys($"testPage{random}");
            Thread.Sleep(500);
            newPageButton.Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Ribbon.EditingTools.CPEditTab.EditAndCheckout.SaveEdit-SelectedItem")));
            saveOnRibbonPageButton.Click();
            Thread.Sleep(3000);
            return $"testPage{random}";
        }
        public void PageSharedContextMenu(string pageName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            allPagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//a[starts-with(@aria-label,'{pageName}')]")));
            IWebElement page = wait.Until(x => x.FindElement(By.XPath($"//a[starts-with(@aria-label,'{pageName}')]")));
            Actions action = new Actions(driver);
            action.ContextClick(page).Perform();
            IWebElement shareMenuButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@aria-label='Share']")));
            action.MoveToElement(shareMenuButton);
            shareMenuButton.Click();
            IWebElement peoplePickerDiv = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan")));
            action.MoveToElement(peoplePickerDiv);
            peoplePickerDiv.Click();
            IWebElement peoplePickerInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan_EditorInput")));
            action.MoveToElement(peoplePickerInput);
            Thread.Sleep(500);
            peoplePickerInput.SendKeys("mzywicki");
            IWebElement peoplePickerResultContainer = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan_AutoFillDiv")));
            peoplePickerResultContainer.Click();
            shareButton.Click();
            Thread.Sleep(3000);
        }
        public void PageSharedThreeDotsMenu(string pageName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            allPagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//a[starts-with(@aria-label,'{pageName}')]")));
            IWebElement page = wait.Until(x => x.FindElement(By.XPath($"//a[starts-with(@aria-label,'{pageName}')]")));
            Actions action = new Actions(driver);
            IWebElement row = wait.Until(x => x.FindElement(By.XPath($"//a[starts-with(@aria-label,'{pageName}')]/../../..")));
            IWebElement threeDotsButton = row.FindElement(By.XPath(".//a[contains(@id,'calloutLaunchPoint')]"));
            threeDotsButton.Click();
            IWebElement popUp =  wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@id,'callout-content')]")));
            IWebElement threeDotsFromPopUp = popUp.FindElement(By.ClassName("js-callout-ecbActionDownArrow"));
            threeDotsFromPopUp.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-contextmenu-list")));
            IWebElement menu = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@aria-label='Share']")));
            action.MoveToElement(menu);
            menu.Click();
            IWebElement peoplePickerDiv = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan")));
            action.MoveToElement(peoplePickerDiv);
            peoplePickerDiv.Click();
            IWebElement peoplePickerInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan_EditorInput")));
            action.MoveToElement(peoplePickerInput);
            Thread.Sleep(500);
            peoplePickerInput.SendKeys("mzywicki");
            IWebElement peoplePickerResultContainer = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan_AutoFillDiv")));
            peoplePickerResultContainer.Click();
            shareButton.Click();
            Thread.Sleep(3000);
        }
        public void PageCopyLinkFromShareMenu(string pageName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            allPagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//a[starts-with(@aria-label,'{pageName}')]")));
            IWebElement page = wait.Until(x => x.FindElement(By.XPath($"//a[starts-with(@aria-label,'{pageName}')]")));
            Actions action = new Actions(driver);
            action.ContextClick(page).Perform();
            IWebElement shareMenuButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@aria-label='Share']")));
            action.MoveToElement(shareMenuButton);
            shareMenuButton.Click();
            Thread.Sleep(1500);
            IWebElement getLinkButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("lnkGetLnkDlg")));
            action.MoveToElement(getLinkButton);

            getLinkButton.Click();
            Thread.Sleep(3000);
        }
        public void PageCopyLinkFromContextMenu(string pageName)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            allPagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//a[starts-with(@aria-label,'{pageName}')]")));
            IWebElement page = wait.Until(x => x.FindElement(By.XPath($"//a[starts-with(@aria-label,'{pageName}')]")));
            Actions action = new Actions(driver);
            action.ContextClick(page).Perform();
            IWebElement getLinkButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@aria-label='Get a link']")));
            action.MoveToElement(getLinkButton);
            getLinkButton.Click();
            Thread.Sleep(3000);
        }
        public void FolderSharedContextMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            string folderName = this.FolderCreated();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            //allPagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//a[starts-with(@aria-label,'{folderName}')]")));
            IWebElement folder = wait.Until(x => x.FindElement(By.XPath($"//a[starts-with(@aria-label,'{folderName}')]")));
            Actions action = new Actions(driver);
            action.ContextClick(folder).Perform();
            IWebElement shareMenuButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@aria-label='Share']")));
            action.MoveToElement(shareMenuButton);
            shareMenuButton.Click();
            IWebElement peoplePickerDiv = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan")));
            action.MoveToElement(peoplePickerDiv);
            peoplePickerDiv.Click();
            IWebElement peoplePickerInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan_EditorInput")));
            action.MoveToElement(peoplePickerInput);
            Thread.Sleep(500);
            peoplePickerInput.SendKeys("mzywicki");
            IWebElement peoplePickerResultContainer = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan_AutoFillDiv")));
            peoplePickerResultContainer.Click();
            shareButton.Click();
            Thread.Sleep(3000);
        }
        public void FolderSharedThreeDotsMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            string folderName = this.FolderCreated();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("QCB1_Button1")));
            //allPagesButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//a[starts-with(@aria-label,'{folderName}')]")));
            IWebElement page = wait.Until(x => x.FindElement(By.XPath($"//a[starts-with(@aria-label,'{folderName}')]")));
            Actions action = new Actions(driver);
            IWebElement row = wait.Until(x => x.FindElement(By.XPath($"//a[starts-with(@aria-label,'{folderName}')]/../../..")));
            IWebElement threeDotsButton = row.FindElement(By.XPath(".//a[contains(@id,'calloutLaunchPoint')]"));
            threeDotsButton.Click();
            IWebElement threeDotsFromPopUp = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".js-callout-ecbActionDownArrow")));
            threeDotsFromPopUp.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-contextmenu-list")));
            IWebElement menu = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@aria-label='Share']")));
            action.MoveToElement(menu);
            menu.Click();
            IWebElement peoplePickerDiv = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan")));
            action.MoveToElement(peoplePickerDiv);
            peoplePickerDiv.Click();
            IWebElement peoplePickerInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan_EditorInput")));
            action.MoveToElement(peoplePickerInput);
            Thread.Sleep(500);
            peoplePickerInput.SendKeys("mzywicki");
            IWebElement peoplePickerResultContainer = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("peoplePicker_TopSpan_AutoFillDiv")));
            peoplePickerResultContainer.Click();
            shareButton.Click();
            Thread.Sleep(3000);
        }
    }
}
