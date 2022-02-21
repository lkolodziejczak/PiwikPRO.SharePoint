using Microsoft.SharePoint.Client;
using PiwikPROSitesActivator.Shared;
using PiwikPROSitesActivator.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PiwikPROSitesActivator
{
    public class Configuration
    {
        //public static string ServiceSiteUrl = "https://lukaszsp.sharepoint.com/sites/piwikadmin";
    }
    class Program
    {
        static void Main(string[] args)
        {
            Logger.WriteLine("Start application, time: " + DateTime.Now.ToString());

            string urlSite = "";
            foreach (string line in System.IO.File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + @"Site.txt"))
            {
                urlSite = line;
            }

            if(!string.IsNullOrEmpty(urlSite))
            {
                Logger.WriteLine("Currently working on site url: " + urlSite);
                Logger.WriteLine("Press Y to proceed or N to change the url.");
                var keyClick = Console.ReadKey();
                if(keyClick.Key == ConsoleKey.Y)
                {

                }
                if (keyClick.Key == ConsoleKey.N)
                {
                    Logger.WriteLine("\nPlease put url to the Sharepoint site");
                    urlSite = Console.ReadLine();
                }
            }
            else
            {
                Logger.WriteLine("\nPlease put url to the Sharepoint site");
                urlSite = Console.ReadLine();
            }

            if (!string.IsNullOrEmpty(urlSite))
            {
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"Site.txt", urlSite);
                if (urlSite[urlSite.Length - 1] == '/')
                {
                    urlSite += "sites/piwikadmin";
                }
                else
                {
                    urlSite += "/sites/piwikadmin";
                }
            }
            else
            {
                Logger.WriteLine("\nSite url is empty, please restart the application and put the correct value.");
            }

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var authManager = new OfficeDevPnP.Core.AuthenticationManager();
            ClientContext ctx = authManager.GetWebLoginClientContext(urlSite);
            
                var pbo = new PropertyBagOperations(ctx);

                Config cfg = new Config();
                cfg.PiwikServiceUrl = pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ServiceUrl);

                var items = ctx.Web.Lists.GetByTitle("PiwikConfig").GetItems(CamlQuery.CreateAllItemsQuery());
                ctx.Load(items);
                ctx.ExecuteQueryRetry();

                cfg.PiwikClientID = items.FirstOrDefault(x => (string)x["Title"] == ConfigValues.PiwikPro_PropertyBag_ClientID)?["Value"]?.ToString();
                cfg.PiwikClientSecret = items.FirstOrDefault(x => (string)x["Title"] == ConfigValues.PiwikPro_PropertyBag_ClientSecret)?["Value"]?.ToString();

                var listProcessor = new ListProcessor(ctx);

                var command = new ProcessPiwikSitesCommand(ctx, listProcessor);
             command.ExecuteAsync();
            

            PiwikPROJobOperations pbjo = new PiwikPROJobOperations(ctx, cfg);

            pbjo.GetAllNewSitesAndOperate(true);
            pbjo.GetAllDeactivatingSitesAndOperate(true);
            pbjo.GetAllSettingsUpdatedSitesAndOperate();

            //splogger.WriteLog(Category.Information, "Piwik PRO Job", "Finished");
            //}
            Logger.WriteLine("\nFinished. Press any key to close the window.");
            Logger.SaveLog();
            Console.ReadKey();
        }
    }
}
