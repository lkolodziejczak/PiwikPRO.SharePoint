using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Threading;

namespace PiwikPRO.SharePoint.Tests.Pages
{
    public class LoginPage
    {
        private IWebDriver driver;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#i0116")]
        private IWebElement emailInput;

        [FindsBy(How = How.CssSelector, Using = "#idSIButton9")]
        private IWebElement nextButton;

        [FindsBy(How = How.XPath, Using = "//input[@name='passwd']")]
        private IWebElement passwordInput;

        [FindsBy(How = How.CssSelector, Using = "#idBtn_Back")]
        private IWebElement staySingedInNoButton;


        public void TypeEmail(string email)
        {
            emailInput.SendKeys(email);
        }

        public void TypePassword(string password)
        {
            passwordInput.SendKeys(password);
        }

        public void ClickNextButton()
        {
            nextButton.Click();
        }

        public void ClickStaySingedInNoButton()
        {
            staySingedInNoButton.Click();
        }

        public void LoginToSharepoint(string login, string password)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#i0116"))); //waiting for "next" button
            TypeEmail(login);
            ClickNextButton();
            TypePassword(password);
            Thread.Sleep(1000);
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#i0118"))); //waiting for "next" button
            ClickNextButton();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#idBtn_Back"))); //waiting for "No" button
            ClickStaySingedInNoButton();

        }
    }
}
