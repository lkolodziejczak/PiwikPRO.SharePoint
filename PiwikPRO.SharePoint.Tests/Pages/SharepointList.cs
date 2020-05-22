using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.Tests.Lists
{
    class SharepointList
    {
        private IWebDriver driver;
        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='quickEditommand']")]
        private IWebElement quickEditButton;

        [FindsBy(How = How.XPath, Using = "//button//span//i[@data-icon-name='CalculatorAddition']")]
        private IWebElement newButton;

        [FindsBy(How = How.XPath, Using = "//input[@type='text'][@aria-required='true'][@aria-invalid='false']")]
        private IWebElement inputText;

        [FindsBy(How = How.XPath, Using = "//button[@data-automationid='ReactClientFormSaveButton']")]
        private IWebElement saveButton;

        public SharepointList(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void QuickEdit()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automationid='quickEditommand']")));
            quickEditButton.Click();
            Thread.Sleep(2500);
            driver.Navigate().Refresh();
        }

        public void AddNewItem()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button//span//i[@data-icon-name='CalculatorAddition']")));
            newButton.Click();
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@type='text'][@aria-required='true'][@aria-invalid='false']")));
            inputText.SendKeys("listitemToTest" + DateTime.Now.ToString("hhmmss"));
            Thread.Sleep(1000);
            saveButton.Click();
        }
    }
}
