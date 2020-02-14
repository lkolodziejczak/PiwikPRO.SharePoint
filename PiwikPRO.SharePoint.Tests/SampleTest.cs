using NUnit.Framework;
using System.Threading;

namespace PiwikPRO.SharePoint.Tests
{
    public class SampleTest : SeleniumTestFixture
    {
        public SampleTest(string driverName) : base(driverName)
        {
        }

        [Test]
        public void Test()
        {
            Driver.Url = "https://www.google.pl";
            Thread.Sleep(10000);
        }
    }
}
