using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [FindsBy(How = How.ClassName, Using = "heroButton_2ca50ba6")]
        private IWebElement dotShowActionsMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='pinItemCommand']")]
        private IWebElement filePinToTopFromContextMenu;

        [FindsBy(How = How.XPath, Using = "(//button[@data-automationid='editPinnedItemCommand'])[2]")]
        private IWebElement fileEditPinFromContextMenu;

        [FindsBy(How = How.XPath, Using = "(//button[@data-automationid='removePinnedItemCommand'])[1]")]
        private IWebElement fileUnPinToTopFromContextMenu;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='newCommand']")]
        private IWebElement newMenuItem;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='createFolderCommand']")]
        private IWebElement newFolderFromNewMenu;

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

        public void NewFolderFromTopMenu()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='newCommand']")));
            newMenuItem.Click();
            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='createFolderCommand']")));
            newFolderFromNewMenu.Click();
        }
    }
}
