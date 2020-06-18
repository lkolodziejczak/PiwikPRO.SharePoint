using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using PiwikPRO.SharePoint.Tests.SeleniumTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.Tests.Pages
{
    class SharepointClassicLists : TestBase
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = "//a[@name='add an app']")]
        private IWebElement addAnAppButton;

        [FindsBy(How = How.XPath, Using = "//div[@Title='" + resourceCustomList + "']")]
        private IWebElement addAList;

        [FindsBy(How = How.XPath, Using = "//div[@Title='" + resourceDocumentLibrary + "']")]
        private IWebElement addALib;

        [FindsBy(How = How.XPath, Using = "//input[@id='onetidListTitle']")]
        private IWebElement inputTextCreateList;

        [FindsBy(How = How.XPath, Using = "//a[@aria-label='" + resourceBackToClassicSharepoint + "']")]
        private IWebElement returnToClassicButton;

        [FindsBy(How = How.XPath, Using = "//input[@value='Cancel']")]
        private IWebElement inputCancelButton;

        [FindsBy(How = How.XPath, Using = "//button[@id='O365_MainLink_Settings']")]
        private IWebElement settingsButton;

        [FindsBy(How = How.XPath, Using = "//a[@id='SuiteMenu_LibrarySettings']")]
        private IWebElement listSettingsButton;

        [FindsBy(How = How.XPath, Using = "//a[@id='ctl00_PlaceHolderMain_ctl10_RptControls_onetidListEdit2']")]
        private IWebElement listDeleteButton;

        public SharepointClassicLists(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void CreateClassicListFromSiteContents()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@aria-label='"+ resourceBackToClassicSharepoint+"']")));
            returnToClassicButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@name='add an app']")));
            addAnAppButton.Click();
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@Title='" + resourceCustomList + "']")));
            addAList.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//IFrame[contains(@id,'DlgFrame')]")));
            string frameElementXpath = "//IFrame[contains(@id,'DlgFrame')]";
            IWebElement f = driver.FindElement(By.XPath(frameElementXpath));
            driver.SwitchTo().Frame(f);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='onetidListTitle']")));
            inputTextCreateList.SendKeys(DateTime.Now.ToString("hhmm") + "ClassicTestList");
            Thread.Sleep(500);
            inputTextCreateList.SendKeys(Keys.Enter);
        }

        public void CreateClassicListFromSiteContentsAdvancedMode()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@aria-label='" + resourceBackToClassicSharepoint + "']")));
            returnToClassicButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@name='add an app']")));
            addAnAppButton.Click();
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@Title='" + resourceCustomList + "']")));
            addAList.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//IFrame[contains(@id,'DlgFrame')]")));
            string frameElementXpath = "//IFrame[contains(@id,'DlgFrame')]";
            IWebElement f = driver.FindElement(By.XPath(frameElementXpath));
            driver.SwitchTo().Frame(f);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='onetidListTitle']")));
            Thread.Sleep(1000);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("NavigateToFullPage()");
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='onetidListTitle']")));
            inputTextCreateList.SendKeys(DateTime.Now.ToString("hhmm") + "ClassicTestLibAdv");
            Thread.Sleep(500);
            inputTextCreateList.SendKeys(Keys.Enter);
        }
        public void DeleteClassicList()
        {
            CreateClassicListFromSiteContentsAdvancedMode();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@id='O365_MainLink_Settings']")));
            settingsButton.Click();
            Thread.Sleep(1500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@id='SuiteMenu_LibrarySettings']")));
            listSettingsButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@id='ctl00_PlaceHolderMain_ctl10_RptControls_onetidListEdit2']")));
            listDeleteButton.Click();
            Thread.Sleep(1000);
            driver.SwitchTo().Alert().SendKeys(Keys.Enter);

        }

        public void CreateClassicLibraryFromSiteContents()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@aria-label='" + resourceBackToClassicSharepoint + "']")));
            returnToClassicButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@name='add an app']")));
            addAnAppButton.Click();
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@Title='" + resourceDocumentLibrary + "']")));
            addALib.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//IFrame[contains(@id,'DlgFrame')]")));
            string frameElementXpath = "//IFrame[contains(@id,'DlgFrame')]";
            IWebElement f = driver.FindElement(By.XPath(frameElementXpath));
            driver.SwitchTo().Frame(f);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='onetidListTitle']")));
            inputTextCreateList.SendKeys(DateTime.Now.ToString("hhmm") + "ClassicTestLib");
            Thread.Sleep(500);
            inputTextCreateList.SendKeys(Keys.Enter);
        }

        public void CreateClassicLibraryFromSiteContentsAdvancedMode()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@aria-label='" + resourceBackToClassicSharepoint + "']")));
            returnToClassicButton.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@name='add an app']")));
            addAnAppButton.Click();
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@Title='" + resourceDocumentLibrary + "']")));
            addALib.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//IFrame[contains(@id,'DlgFrame')]")));
            string frameElementXpath = "//IFrame[contains(@id,'DlgFrame')]";
            IWebElement f = driver.FindElement(By.XPath(frameElementXpath));
            driver.SwitchTo().Frame(f);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='onetidListTitle']")));
            Thread.Sleep(1000);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("NavigateToFullPage()");
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='onetidListTitle']")));
            inputTextCreateList.SendKeys(DateTime.Now.ToString("hhmm") + "ClassicTestLib");
            Thread.Sleep(500);
            inputTextCreateList.SendKeys(Keys.Enter);
        }
    }
}
