using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace PiwikPRO.SharePoint.Tests
{
    [TestFixture("Chrome")]
    public abstract class SeleniumTestFixture : IDisposable
    {
        private readonly string _driverName;
        bool disposed = false;

        protected IWebDriver Driver { get; private set; }

        public SeleniumTestFixture(string driverName)
        {
            _driverName = driverName;
        }

        [SetUp]
        public virtual void SetUp()
        {
            switch (_driverName)
            {
                case "Chrome":
                    Driver = new ChromeDriver();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (Driver != null)
            {
                Driver.Dispose();
                Driver = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (Driver != null)
                {
                    Driver.Dispose();
                    Driver = null;
                }
            }

            disposed = true;
        }
    }
}
