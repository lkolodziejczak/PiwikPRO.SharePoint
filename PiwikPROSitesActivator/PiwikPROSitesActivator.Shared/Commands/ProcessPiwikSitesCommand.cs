using CamlexNET;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiwikPROSitesActivator.Shared
{
    public sealed class ProcessPiwikSitesCommand
    {
        private readonly ClientContext ctx;
        private readonly ListProcessor listProcessor;

        public ProcessPiwikSitesCommand(ClientContext ctx, ListProcessor listProcessor)
        {
            this.ctx = ctx;
            this.listProcessor = listProcessor;
        }

        public async Task ExecuteAsync()
        {
            var sitesToAdd = await FindSitesByStatusAsync(PiwikStatus.Adding);
            foreach (var site in sitesToAdd)
            {
                listProcessor.AddOrUpdateElementInList(site.Title, ConfigValues.PiwikPro_SiteDirectory_Column_Status_New, site.Url, "", site.Url, "");
                Logger.WriteLine("\nAdding site to Piwik PRO Site Directory list. : " + site.Url);
                await SetAsProcessedAsync(site.Url, PiwikStatus.Adding);
            }
            
            var sitesToDelete = await FindSitesByStatusAsync(PiwikStatus.Deleting);
            foreach (var site in sitesToDelete)
            {
                listProcessor.AddOrUpdateElementInList(site.Title, ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating, site.Url, "", site.Url, "");
                Logger.WriteLine("\nRemoving site from Piwik PRO Site Directory list. : " + site.Url);
                await SetAsProcessedAsync(site.Url, PiwikStatus.Deleting);
            }
        }

        private async Task<IEnumerable<(string Title, string Url)>> FindSitesByStatusAsync(PiwikStatus status)
        {
            var sites = new List<(string Title, string Url)>();

            KeywordQuery searchQuery = new KeywordQuery(ctx)
            {
                QueryText = $"Title:PiwikStatus AND ${status}",
                TrimDuplicates = true,
                RowLimit = 500
            };
            searchQuery.SelectProperties.Add("SPWebUrl");
            SearchExecutor executor = new SearchExecutor(ctx);
            ClientResult<ResultTableCollection> results = executor.ExecuteQuery(searchQuery);
            ctx.ExecuteQueryRetry();

            foreach (var result in results.Value[0].ResultRows)
            {
                using (ClientContext webCtx = ctx.Clone((string)result["SPWebUrl"]))
                {
                    webCtx.Load(webCtx.Web, x => x.Title, x => x.Url);
                    List list = webCtx.Web.Lists.GetByTitle("PiwikSettings");
                    CamlQuery camlQuery = Camlex.Query()
                        .Where(x => (string)x["Title"] == "PiwikStatus" && (string)x["Value"] == status.ToString())
                        .ViewFields(x => x["Id"])
                        .Take(1)
                        .ToCamlQuery();
                    ListItemCollection items = list.GetItems(camlQuery);
                    webCtx.Load(items);
                    webCtx.ExecuteQueryRetry();

                    if (items.Any())
                    {
                        sites.Add((webCtx.Web.Title, webCtx.Web.Url));
                    }
                }
            }

            if(results.Value[0].ResultRows.Count() == 0 && status == PiwikStatus.Adding)
            {
                Logger.WriteLine("\nNo sites found to register in Piwik PRO Analytics. Please try again in some minutes.");

                Logger.WriteLine("\nChecking if there are sites to add in manual activation file...");

                foreach (string line in System.IO.File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + @"Exceptional_SitesToManualActivation.txt"))
                {
                    sites.Add((line.Split(';')[0], line.Split(';')[1]));

                    Logger.WriteLine("\nAdding site to activate: " + line.Split(';')[0] + ", url: "+ line.Split(';')[1]);
                }
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"Exceptional_SitesToManualActivation.txt", string.Empty);
            }



            if (results.Value[0].ResultRows.Count() == 0 && status == PiwikStatus.Deleting)
            {
                Logger.WriteLine("\nNo sites found to deactivate in Piwik PRO Analytics. Please try again in some minutes.");

                Logger.WriteLine("\nChecking if there are sites to deactivate in manual deactivation file...");

                foreach (string line in System.IO.File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + @"Exceptional_SitesToManualDeactivation.txt"))
                {
                    sites.Add((line.Split(';')[0], line.Split(';')[1]));

                    Logger.WriteLine("\nAdding site to deacivate: " + line.Split(';')[0] + ", url: " + line.Split(';')[1]);
                }
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"Exceptional_SitesToManualDeactivation.txt", string.Empty);
            }

            return sites;
        }

        private async Task SetAsProcessedAsync(string siteUrl, PiwikStatus initialStatus)
        {
            using (ClientContext webCtx = ctx.Clone(siteUrl))
            {
                List list = webCtx.Web.Lists.GetByTitle("PiwikSettings");
                CamlQuery camlQuery = Camlex.Query()
                    .Where(x => (string)x["Title"] == "PiwikStatus" && (string)x["Value"] == initialStatus.ToString())
                    .Take(1)
                    .ToCamlQuery();
                ListItemCollection items = list.GetItems(camlQuery);
                webCtx.Load(items);
                webCtx.ExecuteQueryRetry();

                foreach (ListItem item in items)
                {
                    item["Value"] = PiwikStatus.Processed.ToString();
                    item.Update();
                }
                webCtx.ExecuteQueryRetry();
            }
        }
    }

    public enum PiwikStatus
    {
        Processed,
        Adding,
        Deleting
    }
}
