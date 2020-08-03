using Microsoft.Azure.WebJobs;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using PiwikPRO.SharePoint.Shared;
using PiwikPRO.SharePoint.Shared.Commands;
using PiwikPRO.SharePoint.Shared.Helpers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.WebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.

        public static async Task ExecuteTimer([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo timer, TextWriter log)
        {
            Configuration configuration = new Configuration();
            AuthenticationManager authMan = new AuthenticationManager();
            using (ClientContext ctx = authMan.GetAzureADAppOnlyAuthenticatedContext(
                configuration.PiwikAdminSiteUrl,
                configuration.ClientId,
                configuration.Tenant,
                configuration.StoreName,
                configuration.StoreLocation,
                configuration.Thumbprint))
            {
                AzureLogger splogger = new AzureLogger();
                splogger.WriteLog(Category.Information, "Piwik PRO Job", "Started");

                var pbo = new PropertyBagOperations(splogger, ctx);

                Config cfg = new Config();
                cfg.PiwikServiceUrl = pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ServiceUrl);

                var items = ctx.Web.Lists.GetByTitle("PiwikConfig").GetItems(CamlQuery.CreateAllItemsQuery());
                ctx.Load(items);
                await ctx.ExecuteQueryRetryAsync();

                cfg.PiwikClientID = items.FirstOrDefault(x => (string)x["Title"] == ConfigValues.PiwikPro_PropertyBag_ClientID)?["Value"]?.ToString();
                cfg.PiwikClientSecret = items.FirstOrDefault(x => (string)x["Title"] == ConfigValues.PiwikPro_PropertyBag_ClientSecret)?["Value"]?.ToString();

                var listProcessor = new ListProcessor(ctx, splogger);

                var command = new ProcessPiwikSitesCommand(ctx, listProcessor);
                await command.ExecuteAsync();

                PiwikPROJobOperations pbjo = new PiwikPROJobOperations(ctx, cfg, splogger);

                pbjo.GetAllNewSitesAndOperate();
                pbjo.GetAllDeactivatingSitesAndOperate();

                splogger.WriteLog(Category.Information, "Piwik PRO Job", "Finished");
            }
        }
    }
}
