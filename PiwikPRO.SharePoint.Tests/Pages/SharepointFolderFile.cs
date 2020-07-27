using AutoItX3Lib;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        [FindsBy(How = How.XPath, Using = "//li[@role='presentation']//button[@data-automationid='shareCommand']")]
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

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='uploadCommand']")]
        private IWebElement uploadButton;

        [FindsBy(How = How.XPath, Using = "//button[@aria-posinset='1']")]
        private IWebElement uploadFileButton;

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
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@role='presentation']//button[@data-automationid='shareCommand']")));
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
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[@role='presentation']//button[@data-automationid='shareCommand']")));
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
        public string FileUpload()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            string tempPath = Path.GetTempPath();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='uploadCommand']")));
            //var tempFile = File.CreateText($"{tempPath}TempFile{random}.txt");
            //tempFile.WriteLine("testFile");
            string tempfile = $"{tempPath}TempFile{random}.txt";
            File.WriteAllText(tempfile, "testfile");
            uploadButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@aria-posinset='1']")));
            uploadFileButton.Click();
            AutoItX3 autoIt = new AutoItX3();
            autoIt.WinActivate("Otwieranie");
            Thread.Sleep(2000);
            //FileOpen(@ScriptDir & "\TempFile.txt", 1);
            autoIt.Send(tempfile);
            Thread.Sleep(2000);
            autoIt.Send("{ENTER}");
            Thread.Sleep(2000);
            File.Delete(tempfile);
            return $"TempFile{random}.txt";
        }
        public void FileUploadDragAndDrop()
        {
            Random rnd = new Random();
            int random = rnd.Next(999999);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Thread.Sleep(2000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@aria-label='List of folders, files or items']")));
            IWebElement droparea = driver.FindElement(By.XPath("//div[@aria-label='List of folders, files or items']"));
            DropFile(droparea, @"C:\test.LOG");
        }
        const string JS_DROP_FILE = "for(var b=arguments[0],k=arguments[1],l=arguments[2],c=b.ownerDocument,m=0;;){var e=b.getBoundingClientRect(),g=e.left+(k||e.width/2),h=e.top+(l||e.height/2),f=c.elementFromPoint(g,h);if(f&&b.contains(f))break;if(1<++m)throw b=Error('Element not interractable'),b.code=15,b;b.scrollIntoView({behavior:'instant',block:'center',inline:'center'})}var a=c.createElement('INPUT');a.setAttribute('type','file');a.setAttribute('style','position:fixed;z-index:2147483647;left:0;top:0;');a.onchange=function(){var b={effectAllowed:'all',dropEffect:'none',types:['Files'],files:this.files,setData:function(){},getData:function(){},clearData:function(){},setDragImage:function(){}};window.DataTransferItemList&&(b.items=Object.setPrototypeOf([Object.setPrototypeOf({kind:'file',type:this.files[0].type,file:this.files[0],getAsFile:function(){return this.file},getAsString:function(b){var a=new FileReader;a.onload=function(a){b(a.target.result)};a.readAsText(this.file)}},DataTransferItem.prototype)],DataTransferItemList.prototype));Object.setPrototypeOf(b,DataTransfer.prototype);['dragenter','dragover','drop'].forEach(function(a){var d=c.createEvent('DragEvent');d.initMouseEvent(a,!0,!0,c.defaultView,0,0,0,g,h,!1,!1,!1,!1,0,null);Object.setPrototypeOf(d,null);d.dataTransfer=b;Object.setPrototypeOf(d,DragEvent.prototype);f.dispatchEvent(d)});a.parentElement.removeChild(a)};c.documentElement.appendChild(a);a.getBoundingClientRect();return a;";
        static void DropFile(IWebElement target, string filePath, double offsetX = 0, double offsetY = 0)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            IWebDriver driver = ((RemoteWebElement)target).WrappedDriver;
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;

            IWebElement input = (IWebElement)jse.ExecuteScript(JS_DROP_FILE, target, offsetX, offsetY);
            input.SendKeys(filePath);
        }
        public void FileDeleted()
        {
            string fileName = this.FileUpload();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[starts-with(@aria-label,'{fileName}')]")));
            var row = wait.Until(x => x.FindElement(By.XPath($"//div[starts-with(@aria-label,'{fileName}')]")));
            var element = row.FindElement(By.XPath(".//button[@data-automationid='FieldRender-DotDotDot']"));
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
            Thread.Sleep(200);
            element.Click();
            IWebElement deleteButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='deleteCommand']")));
            deleteButton.Click();
            IWebElement confirmationDeleteButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//button[@data-automationid='confirmbutton']")));
            confirmationDeleteButton.Click();
            Thread.Sleep(1500);
        }
    }
}
