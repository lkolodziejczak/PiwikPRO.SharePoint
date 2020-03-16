using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PiwikPRO.SharePoint.Tests.Pages;

namespace PiwikPRO.SharePoint.Tests.SeleniumTests
{
    class LoginTests : TestBase
    {
        [Test]
        public void LoginSuccess()       
        {
            var loginPage = new LoginPage(_webDriver);

            loginPage.LoginToSharepoint("mzywicki@phkogifi.onmicrosoft.com", "SAqa@2018b44c");
        }
    }
}
