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
        public PiwikPROJSProvisioningJob() : base() { }
        public PiwikPROJSProvisioningJob(string jobName, SPService service, SPServer server, SPJobLockType targetType) : base(jobName, service, server, targetType) { }
        public PiwikPROJSProvisioningJob(string jobName, SPWebApplication webApplication) : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            this.Title = jobName;
        }
        public override void Execute(Guid contentDbId)
        {
            try
            {
                Configuration cfg = new Configuration(Convert.ToString(this.Properties[ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl]));
                SPLogger splogger = new SPLogger();
                using (var ctx = new ClientContext(Convert.ToString(this.Properties[ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl])))
                {
                    PiwikPROJobOperations pbjo = new PiwikPROJobOperations(ctx, cfg, splogger);

                    pbjo.GetAllNewSitesAndOperate();
                    pbjo.GetAllDeactivatingSitesAndOperate();
                    pbjo.GetAllSettingsUpdatedSitesAndOperate();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik Execute TimerJob", ex.Message);
            }
        }
    }
}
