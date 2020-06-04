using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
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
        public string shareToWho = "lkolodz";
        public string sitePagesLibrary = "https://phkogifi.sharepoint.com/sites/PH/SitePages/Forms/ByAuthor.aspx";
        public string siteContents = "https://phkogifi.sharepoint.com/sites/PH/_layouts/15/viewlsts.aspx?view=14";
        public string homePage = "https://phkogifi.sharepoint.com/sites/PH/";
        public string listToTest = "https://phkogifi.sharepoint.com/sites/PH/Lists/testowa%20lista/AllItems.aspx";
        public string siteContentsClassicToTest = "https://phkogifi.sharepoint.com/sites/PH/ClassicExp/_layouts/15/viewlsts.aspx?view=14";

        //resources : to be changed on another language version of sharepoint:
        public const string resourceBackToClassicSharepoint = "Click or enter to return to classic SharePoint";
        public const string resourceCustomList = "Custom List";
        public const string resourceDocumentLibrary = "Document Library";

        public IWebDriver _webDriver;

        [OneTimeSetUp]
        public void SetUp()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            _webDriver = new ChromeDriver();
           
            _webDriver.Manage().Window.Maximize();

            _webDriver.Navigate().GoToUrl("http://phkogifi.sharepoint.com/");
        }

        [OneTimeTearDown]
        public void TearnDown()
        {
            _webDriver.Close();
            _webDriver.Quit();
        }
    }
}
