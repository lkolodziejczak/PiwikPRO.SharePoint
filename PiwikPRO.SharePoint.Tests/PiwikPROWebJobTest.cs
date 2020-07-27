using Microsoft.SharePoint.Client;
using NUnit.Framework;
using PiwikPRO.SharePoint.Shared;
using PiwikPRO.SharePoint.Shared.Helpers;
using PiwikPRO.SharePoint.WebJob;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;

namespace PiwikPRO.SharePoint.Tests.WebJobTests
{
    [TestFixture]
    public class PiwikPROWebJobTest
    {
        ClientContext ctx;
        public const string PiwikListName = "Piwik Pro Site Directory";
        string PiwikAdminSiteUrl = "https://kogifidev3.sharepoint.com/";
        string PiwikAdminTenantSiteUrl = "https://kogifidev3-admin.sharepoint.com/";
        string PiwikAzureAppKey = "5000d8d3-f22c-46ac-a7ba-98938c7a0fb4";
        string PiwikAzureAppSecret = "2C/4jwkseGIpqzr3ZzddUXq2PBHp7Jd6SC4gQLYU0fo=";
        string JobExecuteFile = @"C:\Users\d3\Desktop\PiwikPRO.SharePoint\PiwikPRO.SharePoint.WebJob\bin\Debug\net472\PiwikPRO.SharePoint.WebJob.exe";

        string testedSiteName = "test from application";
        string testedStatus = "New";
        string testedRelativeUrl = "/sites/ptrack1";
        string testedFullUrl = "https://kogifidev3.sharepoint.com/sites/ptrack1";

        [Test]
        public void Test1AddItemToList()
        {
            Assert.AreEqual(true, TestAddItemToList());
        }

        [Test]
        public void Test2CheckIfItemHasBeenAddedToList()
        {
            Assert.AreEqual(true, CheckIfItemHasBeenAddedToListReturnTrue());
        }

        [Test]
        public void Test3ExecuteJobCommandsForActivate()
        {
            Assert.AreEqual(true, ExecuteJobCommands());
        }

        [Test]
        public void Test4CheckIfSiteIsAddedToPiwik()
        {
            Assert.AreEqual(true, CheckIfSiteIsAddedToPiwik());
        }
        [Test]
        public void Test5ChangeStatusToDeactivatingOfItemInPiwikAdminList()
        {
            Assert.AreEqual(true, ChangeStatusOfItemFromPiwikAdminList(ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating));
        }

        [Test]
        public void Test6CheckIfItemHasDeactivatingStatusOnList()
        {
            Assert.AreEqual(true, CheckIfItemHasSpecificStatusOnList(ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating));
        }

        [Test]
        public void Test7ExecuteJobCommandsForDeactivating()
        {
            Assert.AreEqual(true, ExecuteJobCommands());
        }

        [Test]
        public void Test8CheckIfItemHasNoActiveStatusOnList()
        {
            Assert.AreEqual(true, CheckIfItemHasSpecificStatusOnList(ConfigValues.PiwikPro_SiteDirectory_Column_Status_NoActive));
        }

        [Test]
        public void Test9CheckIfSiteIsInactiveInPiwik()
        {
            Assert.AreEqual(true, CheckIfSiteIsInactiveInPiwik());
        }

        [Test]
        public void Test10RemoveSiteFromPiwik()
        {
            Assert.AreEqual(true, RemoveSiteFromPiwik());
        }

        [Test]
        public void Test11RemoveItemFromPiwikAdminList()
        {
            Assert.AreEqual(true, RemoveItemFromPiwikAdminList());
        }

        public bool RemoveItemFromPiwikAdminList()
        {
            bool returner = false;

            ListProcessor sdlo = CreateListProcessorObject();
            ListItem itemReturn = sdlo.CheckIfElementIsAlreadyOnList(testedRelativeUrl);
            if (itemReturn != null)
            {
                itemReturn.DeleteObject();
                ctx.ExecuteQueryRetry();
                returner = true;
            }

            return returner;
        }

        public bool ChangeStatusOfItemFromPiwikAdminList(string status)
        {
            bool returner = false;
            ListProcessor sdlo = CreateListProcessorObject();
            ListItem itemReturn = sdlo.CheckIfElementIsAlreadyOnList(testedRelativeUrl);
            if (itemReturn != null)
            {
                itemReturn[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = status;
                itemReturn.Update();
                ctx.ExecuteQueryRetry();
                returner = true;
            }

            return returner;
        }

        public bool RemoveSiteFromPiwik()
        {
            bool returner = false;
            string idSite = GetItemSiteIdAddedToList();
            if (!string.IsNullOrEmpty(idSite))
            {
                AzureLogger splogger = new AzureLogger();
                splogger.WriteLog(Category.Information, "Piwik PRO Job", "Started");
                Config cfg = ExecuteConfiguration();

                PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl, cfg.PiwikOldApiToken, splogger);
                pso.RemoveSiteFromPiwik(idSite);
                returner = true;
            }
            return returner;
        }

        public bool CheckIfSiteIsAddedToPiwik()
        {
            AzureLogger splogger = new AzureLogger();
            Config cfg = ExecuteConfiguration();
            PiwikPROJobOperations pbjo = new PiwikPROJobOperations(ctx, cfg, splogger);

            string idSite = GetItemSiteIdAddedToList();
            if (!string.IsNullOrEmpty(idSite))
            {
                return pbjo.CheckIfPageIsAlreadyOnPiwik(idSite);
            }
            else
            {
                return false;
            }
        }

        public bool CheckIfSiteIsInactiveInPiwik()
        {
            AzureLogger splogger = new AzureLogger();
            Config cfg = ExecuteConfiguration();
            PiwikPROJobOperations pbjo = new PiwikPROJobOperations(ctx, cfg, splogger);

            return pbjo.CheckIfPageIsAlreadyOnPiwik("Inactive - " + testedSiteName);
        }

        public string GetItemSiteIdAddedToList()
        {
            string idReturner = string.Empty;
            ListProcessor sdlo = CreateListProcessorObject();
            ListItem itemReturn = sdlo.CheckIfElementIsAlreadyOnList(testedRelativeUrl);
            if (itemReturn != null)
            {
                idReturner = Convert.ToString(itemReturn[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]);
            }
            return idReturner;
        }

        public bool CheckIfItemHasBeenAddedToListReturnTrue()
        {
            bool idReturner = false;
            ListProcessor sdlo = CreateListProcessorObject();
            ListItem itemReturn = sdlo.CheckIfElementIsAlreadyOnList(testedRelativeUrl);
            if (itemReturn != null)
            {
                idReturner = true;
            }
            return idReturner;
        }

        public bool CheckIfItemHasSpecificStatusOnList(string status)
        {
            bool returner = false;
            ListProcessor sdlo = CreateListProcessorObject();
            ListItem itemReturn = sdlo.CheckIfElementIsAlreadyOnList(testedRelativeUrl);
            if (itemReturn != null)
            {
                if (Convert.ToString(itemReturn[ConfigValues.PiwikPro_SiteDirectory_Column_Status]).Equals(status))
                {
                    returner = true;
                }
            }
            return returner;
        }

        public bool ExecuteJobCommands()
        {
            bool returner = false;
            OfficeDevPnP.Core.AuthenticationManager authMan = new OfficeDevPnP.Core.AuthenticationManager();
            using (ClientContext ctx = authMan.GetAppOnlyAuthenticatedContext(PiwikAdminSiteUrl, PiwikAzureAppKey, PiwikAzureAppSecret))
            {
                AzureLogger splogger = new AzureLogger();
                splogger.WriteLog(Category.Information, "Piwik PRO Job", "Started");
                Config cfg = ExecuteConfiguration();
                PiwikPROJobOperations pbjo = new PiwikPROJobOperations(ctx, cfg, splogger);

                pbjo.GetAllNewSitesAndOperate();
                pbjo.GetAllDeactivatingSitesAndOperate();
                returner = true;
                splogger.WriteLog(Category.Information, "Piwik PRO Job", "Finished");
            }
            return returner;
        }

        public void TestStartJob()
        {
            using (Process myProcess = new Process())
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = JobExecuteFile;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
            }
        }
        public bool TestAddItemToList()
        {
            ListProcessor sdlo = CreateListProcessorObject();
            sdlo.AddOrUpdateElementInList(testedSiteName, testedStatus, testedFullUrl, "", testedRelativeUrl, "");

            return true;
        }

        private ListProcessor CreateListProcessorObject()
        {
            ListProcessor sdlo;
            OfficeDevPnP.Core.AuthenticationManager authMan = new OfficeDevPnP.Core.AuthenticationManager();
            using (ctx = authMan.GetAppOnlyAuthenticatedContext(PiwikAdminSiteUrl, PiwikAzureAppKey, PiwikAzureAppSecret))
            {
                AzureLogger splogger = new AzureLogger();
                sdlo = new ListProcessor(ctx, splogger);
            }
            return sdlo;
        }

        private Config ExecuteConfiguration()
        {
            AzureLogger splogger = new AzureLogger();
            var pbo = new PropertyBagOperations(splogger, ctx);
            Config cfg = new Config();
            cfg.PiwikServiceUrl = pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ServiceUrl);

            var items = ctx.Web.Lists.GetByTitle("PiwikConfig").GetItems(CamlQuery.CreateAllItemsQuery());
            ctx.Load(items);
            ctx.ExecuteQueryRetry();

            cfg.PiwikClientID = items.FirstOrDefault(x => (string)x["Title"] == ConfigValues.PiwikPro_PropertyBag_ClientID)?["Value"]?.ToString();
            cfg.PiwikClientSecret = items.FirstOrDefault(x => (string)x["Title"] == ConfigValues.PiwikPro_PropertyBag_ClientSecret)?["Value"]?.ToString();
            cfg.PiwikOldApiToken = items.FirstOrDefault(x => (string)x["Title"] == ConfigValues.PiwikPro_PropertyBag_OldApiToken)?["Value"]?.ToString();

            return cfg;
        }
    }
}
