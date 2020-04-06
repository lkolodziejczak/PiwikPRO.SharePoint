using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.Functions
{
    public static class GetConfig
    {
        [FunctionName("GetConfig")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "config")] HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            Monitoring monitoring = new Monitoring();
            JObject piwikConfig = new JObject();

            if (req == null)
                throw new ArgumentNullException(nameof(req));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            try
            {
                string configJson = File.ReadAllText(Path.Combine(context.FunctionAppDirectory, "Configs", "config.json"));
                piwikConfig.Merge(JObject.Parse(configJson), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
            }
            catch (FileNotFoundException ex)
            {
                monitoring.WriteLog("Storage", "GetConfig Storage exception - base config", ex.Message);
            }

            if (!string.IsNullOrEmpty(req.Query["version"]))
            {
                Version version = new Version(req.Query["version"]);
                string[] configFiles = Directory.GetFiles(Path.Combine(context.FunctionAppDirectory, "Configs"), "version-*");
                foreach (var configFile in configFiles)
                {
                    var match = Regex.Match(configFile, @"version-(.*)-(.*).json", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    if (match.Success)
                    {
                        if (!string.IsNullOrWhiteSpace(match.Groups[1].Value) && version < new Version(match.Groups[1].Value) ||
                            !string.IsNullOrWhiteSpace(match.Groups[2].Value) && version > new Version(match.Groups[2].Value))
                            continue;

                        string configJson = File.ReadAllText(configFile);
                        piwikConfig.Merge(JObject.Parse(configJson), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
                    }
                }
            }

            if (req.Query["extension"] != StringValues.Empty)
            {
                foreach (string extension in req.Query["extension"])
                {
                    try
                    {
                        string configJson = File.ReadAllText(Path.Combine(context.FunctionAppDirectory, "Configs", $"extension-{extension}.json"));
                        piwikConfig.Merge(JObject.Parse(configJson), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
                    }
                    catch (FileNotFoundException ex)
                    {
                        monitoring.WriteLog("Storage", "GetConfig Storage exception - extension", ex.Message);
                    }
                }
            }

            if (!string.IsNullOrEmpty(req.Query["tenant"]))
            {
                try
                {
                    string configJson = File.ReadAllText(Path.Combine(context.FunctionAppDirectory, "Configs", $"tenant-{req.Query["tenant"]}.json"));
                    piwikConfig.Merge(JObject.Parse(configJson), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
                }
                catch (FileNotFoundException ex)
                {
                    monitoring.WriteLog("Storage", "GetConfig Storage exception - tenant", ex.Message);
                }
            }

            req.HttpContext.Response.Headers["Cache-Control"] = "max-age=86400";

            return new OkObjectResult(piwikConfig.ToString(Formatting.None));
        }
    }
}