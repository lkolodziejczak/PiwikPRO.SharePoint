using Microsoft.Azure.WebJobs;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using PiwikPRO.SharePoint.Shared;
using PiwikPRO.SharePoint.Shared.Helpers;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.WebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.

        public static void ExecuteTimer([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo timer, TextWriter log)
        {
            OfficeDevPnP.Core.AuthenticationManager authMan = new OfficeDevPnP.Core.AuthenticationManager();
            using (ClientContext ctx = authMan.GetAppOnlyAuthenticatedContext(ConfigurationManager.AppSettings["PiwikAdminSiteUrl"], ConfigurationManager.AppSettings["PiwikAzureAppKey"], ConfigurationManager.AppSettings["PiwikAzureAppSecret"]))
            {
                Functions f = new Functions();

                AzureLogger splogger = new AzureLogger();
                splogger.WriteLog(Category.Information, "Piwik PRO Job", "Started");
                Configuration cfg = new Configuration(splogger, ctx);
                PiwikPROJobOperations pbjo = new PiwikPROJobOperations(cfg, splogger);

                pbjo.GetAllNewSitesAndOperate(ctx, ConfigurationManager.AppSettings["PiwikAzureAppKey"], ConfigurationManager.AppSettings["PiwikAzureAppSecret"], ConfigurationManager.AppSettings["PiwikAdminTenantSiteUrl"]);
                pbjo.GetAllDeactivatingSitesAndOperate(ctx, ConfigurationManager.AppSettings["PiwikAzureAppKey"], ConfigurationManager.AppSettings["PiwikAzureAppSecret"], ConfigurationManager.AppSettings["PiwikAdminTenantSiteUrl"]);

                splogger.WriteLog(Category.Information, "Piwik PRO Job", "Finished");
            }
        }
    }
}
