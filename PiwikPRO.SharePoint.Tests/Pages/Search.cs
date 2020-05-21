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
    class Search
    {
        private IWebDriver driver;
        [FindsBy(How = How.XPath, Using = "//input[@type='search'][@role='combobox']")]
        private IWebElement searchInput;

        public Search(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void SearchFromTopSearch()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//input[@type='search'][@role='combobox']")));
            searchInput.SendKeys("a");
            Thread.Sleep(500);
            searchInput.SendKeys(Keys.Enter);
        }
    }
}
