using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PiwikPRO.SharePoint.Tests.Pages;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace PiwikPRO.SharePoint.Tests.SeleniumTests
{
    class TestBase
    {
        public string sharepointUserToTest = "mzywicki@phkogifi.onmicrosoft.com";
        public string sharepointUserPasswordToTest = "SAqa@2018b44c";
        public string testPageUrl = "https://phkogifi.sharepoint.com/sites/PH/SitePages/Test-Page.aspx";
        public string documentListWithFileUrl = "https://phkogifi.sharepoint.com/Shared%20Documents/Forms/AllItems.aspx";

        public IWebDriver _webDriver;

        [SetUp]
        public void SetUp()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            _webDriver = new ChromeDriver();
           
            _webDriver.Manage().Window.Maximize();

            _webDriver.Navigate().GoToUrl("http://phkogifi.sharepoint.com/");
        }

        [TearDown]
        public void TearnDown()
        {
            _webDriver.Close();
            _webDriver.Quit();
        }
    }
}
