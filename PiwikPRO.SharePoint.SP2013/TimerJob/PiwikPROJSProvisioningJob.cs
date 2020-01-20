using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Utilities;
using PiwikPRO.SharePoint.Shared;
using PiwikPRO.SharePoint.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.SP2013
{
    class PiwikPROJSProvisioningJob : SPJobDefinition
    {
        public PiwikPROJSProvisioningJob(): base() { }
        public PiwikPROJSProvisioningJob(string jobName, SPService service, SPServer server, SPJobLockType targetType): base(jobName, service, server, targetType) { }
        public PiwikPROJSProvisioningJob(string jobName, SPWebApplication webApplication): base(jobName, webApplication, null, SPJobLockType.ContentDatabase) {
            this.Title = jobName;
        }
        public override void Execute(Guid contentDbId)
        {
            try {
                Configuration cfg = new Configuration();
                SPLogger splogger = new SPLogger();
                PiwikPROJobOperations pbjo = new PiwikPROJobOperations(cfg, splogger);

                pbjo.GetAllNewSitesAndOperate(new ClientContext(cfg.PiwikAdminSiteUrl),"","", "");
                pbjo.GetAllDeactivatingSitesAndOperate(new ClientContext(cfg.PiwikAdminSiteUrl), "", "", "");
                //pbjo.GetAllSettingsUpdatedPagesAndOperate(new ClientContext(cfg.PiwikAdminSiteUrl),"","", "");

            }
            catch (Exception ex) {
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik Execute TimerJob", ex.Message);
            }
        }
    }
}
