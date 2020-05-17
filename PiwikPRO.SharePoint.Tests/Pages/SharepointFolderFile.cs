using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [FindsBy(How = How.CssSelector, Using = "div[role='dialog'] input")]
        private IWebElement folderfromTopMenuTextInput;

        [FindsBy(How = How.CssSelector, Using = ".od-SendLink-email input")]
        private IWebElement shareWindowInput;

        [FindsBy(How = How.ClassName, Using = "ms-Button--primary")]
        private IWebElement shareWindowSendButton;

        [FindsBy(How = How.XPath, Using = "//div[@data-automationid='DetailsRowCell']//i")]
        private IWebElement listItemCellIcon;

        [FindsBy(How = How.XPath, Using = "//div[@data-automationid='itemCard'][contains(@aria-label,'Folder')]//span[@role='checkbox']")]
        private IWebElement folderPinnedToTopItem;

        [FindsBy(How = How.XPath, Using = "(//button[@data-automationid='deleteCommand'])[2]")]
        private IWebElement folderDeleteContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='deleteCommand']")]
        private IWebElement folderDeleteTopMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='confirmbutton']")]
        private IWebElement folderDeleteContextMenuConfirm;

        public SharepointFolderFile(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void FilePinToTopFromTopMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement fileIconElem = GetFileElementFromList();
            fileIconElem.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='pinItemCommand']")));
            filePinToTopFromTopMenu.Click();
        }

        public void FilePinToTopFromContextMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement fileIconElem = GetFileElementFromList();
            fileIconElem.Click();
            Thread.Sleep(500);
            foreach (var item in driver.FindElements(By.XPath("//button[@data-automationid='FieldRender-DotDotDot']")))
            {
                if (item.Displayed)
                {
                    item.Click();
                    break;
                }
            }
            Thread.Sleep(500);
            filePinToTopFromContextMenu.Click();
        }

        public void FileUnPinToTopFromTopMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement fileIconElem = GetFileElementFromList();
            fileIconElem.Click();
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='editPinnedItemCommand']")));
            fileEditPinToTopFromTopMenu.Click();

            Thread.Sleep(500);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='removePinnedItemCommand']")));
            fileUnPinToTopFromTopMenu.Click();
        }

        public void FileUnPinToTopFromContextMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement fileIconElem = GetFileElementFromList();
            fileIconElem.Click();
            Thread.Sleep(500);
            foreach (var item in driver.FindElements(By.XPath("//button[@data-automationid='FieldRender-DotDotDot']")))
            {
                if (item.Displayed)
                {
                    item.Click();
                    break;
                }
            }
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
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[role='dialog'] input")));
        folderfromTopMenuTextInput.SendKeys("FolderToTest" + DateTime.Now.ToString("hhmmss"));
            Thread.Sleep(500);
            folderfromTopMenuTextInput.SendKeys(Keys.Enter);
        }

        public void FileSharedFromTopMenu(string shareToWho)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement fileIconElem = GetFileElementFromList();
            fileIconElem.Click();
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='shareCommand']")));
            fileShareFromTopMenu.Click();
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("shareFrame"));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".od-SendLink-email input")));
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
            IWebElement fileIconElem = GetFileElementFromList();
            fileIconElem.Click();
            Thread.Sleep(500);
            foreach (var item in driver.FindElements(By.XPath("//button[@data-automationid='FieldRender-DotDotDot']")))
            {
                if (item.Displayed)
                {
                    item.Click();
                    break;
                }
            }
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@title='Share the selected item with other people']//button[@data-automationid='shareCommand']")));
            Thread.Sleep(300);
            fileShareFromContextMenu.Click();
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("shareFrame"));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".od-SendLink-email input")));
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
            IWebElement fileIconElem = GetFileElementFromList();
            fileIconElem.Click();
            Thread.Sleep(500);

            foreach (var item in driver.FindElements(By.XPath("//button[@data-automationid='FieldRender-ShareHero']")))
            {
                if (item.Displayed)
                {
                    item.Click();
                    break;
                }
            }
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("shareFrame"));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".od-SendLink-email input")));
            Thread.Sleep(300);
            shareWindowInput.SendKeys(shareToWho);
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Suggestions-itemButton")));
            shareWindowInput.SendKeys(Keys.Enter);
            Thread.Sleep(1500);

            shareWindowSendButton.Click();
            driver.SwitchTo().DefaultContent();
        }

        public void FolderSharedFromTopMenu(string shareToWho)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement folderIconElem = GetFolderElementFromList();
            folderIconElem.Click();
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='shareCommand']")));
            fileShareFromTopMenu.Click();
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("shareFrame"));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".od-SendLink-email input")));
            Thread.Sleep(300);
            shareWindowInput.SendKeys(shareToWho);
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Suggestions-itemButton")));
            shareWindowInput.SendKeys(Keys.Enter);
            Thread.Sleep(1500);

            shareWindowSendButton.Click();
            driver.SwitchTo().DefaultContent();
        }

        public void FolderSharedFromContextMenu(string shareToWho)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement folderIconElem = GetFolderElementFromList();
            folderIconElem.Click();
            Thread.Sleep(500);
            foreach (var item in driver.FindElements(By.XPath("//button[@data-automationid='FieldRender-DotDotDot']")))
            {
                if (item.Displayed)
                {
                    item.Click();
                    break;
                }
            }
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@title='Share the selected item with other people']//button[@data-automationid='shareCommand']")));
            Thread.Sleep(300);
            fileShareFromContextMenu.Click();
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("shareFrame"));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".od-SendLink-email input")));
            Thread.Sleep(300);
            shareWindowInput.SendKeys(shareToWho);
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Suggestions-itemButton")));
            shareWindowInput.SendKeys(Keys.Enter);
            Thread.Sleep(1500);

            shareWindowSendButton.Click();
            driver.SwitchTo().DefaultContent();
        }

        public void FolderSharedFromGridMenu(string shareToWho)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement folderIconElem = GetFolderElementFromList();
            folderIconElem.Click();
            Thread.Sleep(500);

            foreach (var item in driver.FindElements(By.XPath("//button[@data-automationid='FieldRender-ShareHero']")))
            {
                if (item.Displayed)
                {
                    item.Click();
                    break;
                }
            }
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("shareFrame"));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".od-SendLink-email input")));
            Thread.Sleep(300);
            shareWindowInput.SendKeys(shareToWho);
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ms-Suggestions-itemButton")));
            shareWindowInput.SendKeys(Keys.Enter);
            Thread.Sleep(1500);

            shareWindowSendButton.Click();
            driver.SwitchTo().DefaultContent();
        }

        public void FolderPinToTopFromTopMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement fileIconElem = GetFolderElementFromList();
            fileIconElem.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='pinItemCommand']")));
            filePinToTopFromTopMenu.Click();
        }

        public void FolderPinToTopFromContextMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement fileIconElem = GetFolderElementFromList();
            fileIconElem.Click();
            Thread.Sleep(500);
            foreach (var item in driver.FindElements(By.XPath("//button[@data-automationid='FieldRender-DotDotDot']")))
            {
                if (item.Displayed)
                {
                    item.Click();
                    break;
                }
            }
            Thread.Sleep(500);
            filePinToTopFromContextMenu.Click();
        }

        public void FolderUnPinToTopFromTopMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@data-automationid='itemCard'][contains(@aria-label,'Folder')]")));
            folderPinnedToTopItem.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='editPinnedItemCommand']")));
            fileEditPinToTopFromTopMenu.Click();

            Thread.Sleep(500);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='removePinnedItemCommand']")));
            fileUnPinToTopFromTopMenu.Click();
        }

        public void FolderDeletedContextMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement fileIconElem = GetFolderElementFromList();
            fileIconElem.Click();
            Thread.Sleep(500);
            foreach (var item in driver.FindElements(By.XPath("//button[@data-automationid='FieldRender-DotDotDot']")))
            {
                if (item.Displayed)
                {
                    item.Click();
                    break;
                }
            }
            Thread.Sleep(500);
            folderDeleteContextMenu.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='confirmbutton']")));
            folderDeleteContextMenuConfirm.Click();
        }

        public void FolderDeletedFromTopMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement fileIconElem = GetFolderElementFromList();
            fileIconElem.Click();
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='deleteCommand']")));
            folderDeleteTopMenu.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='confirmbutton']")));
            folderDeleteContextMenuConfirm.Click();
        }

        private IWebElement GetFolderElementFromList()
        {
            IWebElement elemToReturn = null;
            foreach (var item in driver.FindElements(By.XPath("//div[@data-automationid='DetailsRowCell']//i")))
            {
                if (item.GetAttribute("aria-label") != null)
                {
                    if (item.GetAttribute("aria-label").ToLower().Equals("folder"))
                    {
                        elemToReturn = item;
                        break;
                    }
                }
            }
            return elemToReturn;
        }

        private IWebElement GetFileElementFromList()
        {
            IWebElement elemToReturn = null;
            ReadOnlyCollection<IWebElement> itemsColl = driver.FindElements(By.XPath("//div[@data-automationid='DetailsRowCell']//i"));

            foreach (IWebElement item in itemsColl)
            {
                if (item.GetAttribute("aria-label") != null)
                {
                    if (!item.GetAttribute("aria-label").ToLower().Equals("folder") && !item.GetAttribute("aria-label").ToLower().Equals("aspx")
                        && !item.GetAttribute("aria-label").ToLower().Equals("html") && !item.GetAttribute("aria-label").ToLower().Equals("this item is new"))
                    {
                        elemToReturn = item;
                        break;
                    }
                }
            }
            return elemToReturn;
        }

        private IWebElement GetPageElementFromList()
        {
            IWebElement elemToReturn = null;
            foreach (var item in driver.FindElements(By.XPath("//div[@data-automationid='DetailsRowCell']//i")))
            {
                if (item.GetAttribute("aria-label") != null)
                {
                    if ((item.GetAttribute("aria-label").ToLower().Equals("aspx") || item.GetAttribute("aria-label").ToLower().Equals("html")) && !item.GetAttribute("aria-label").ToLower().Equals("this item is new"))
                    {
                        elemToReturn = item;
                        break;
                    }
                }
            }
            return elemToReturn;
        }
    }
}
