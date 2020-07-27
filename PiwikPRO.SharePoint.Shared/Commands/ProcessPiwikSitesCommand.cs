using CamlexNET;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.Shared.Commands
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
                await SetAsProcessedAsync(site.Url, PiwikStatus.Adding);
            }

            var sitesToDelete = await FindSitesByStatusAsync(PiwikStatus.Deleting);
            foreach (var site in sitesToDelete)
            {
                listProcessor.AddOrUpdateElementInList(site.Title, ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating, site.Url, "", site.Url, "");
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
            searchQuery.SelectProperties.Add("WebUrl");
            SearchExecutor executor = new SearchExecutor(ctx);
            ClientResult<ResultTableCollection> results = executor.ExecuteQuery(searchQuery);
            await ctx.ExecuteQueryRetryAsync();

            foreach (var result in results.Value[0].ResultRows)
            {
                using (ClientContext webCtx = ctx.Clone((string)result["WebUrl"]))
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
                    await webCtx.ExecuteQueryRetryAsync();

                    if (items.Any())
                    {
                        sites.Add((webCtx.Web.Title, webCtx.Web.Url));
                    }
                }
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
                await webCtx.ExecuteQueryRetryAsync();

                foreach (ListItem item in items)
                {
                    item["Value"] = PiwikStatus.Processed.ToString();
                    item.Update();
                }
                await webCtx.ExecuteQueryRetryAsync();
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
