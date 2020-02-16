using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using System;
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
            if (req == null)
                throw new ArgumentNullException(nameof(req));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config["AzureWebJobsStorage"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("piwikblob");

            CloudBlockBlob baseConfigBlob = container.GetBlockBlobReference("piwik-config.json");

            string configJson = await baseConfigBlob.DownloadTextAsync();
            JObject piwikConfig = JObject.Parse(configJson);

            if (req.Query["extension"] != StringValues.Empty)
            {
                foreach (string extension in req.Query["extension"])
                {
                    try
                    {
                        CloudBlockBlob extensionConfigBlob = container.GetBlockBlobReference($"extension-{extension}.json");
                        string extensionConfig = await extensionConfigBlob.DownloadTextAsync();
                        piwikConfig.Merge(JObject.Parse(extensionConfig), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
                    }
                    catch (StorageException)
                    {
                        // Ignore
                    }
                }
            }

            if (!string.IsNullOrEmpty(req.Query["tenant"]))
            {
                try
                {
                    CloudBlockBlob tenantConfigBlob = container.GetBlockBlobReference($"tenant-{req.Query["tenant"]}.json");
                    string tenantConfigJson = await tenantConfigBlob.DownloadTextAsync();
                    piwikConfig.Merge(JObject.Parse(tenantConfigJson), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
                }
                catch (StorageException)
                {
                    // Ignore
                }
            }

            req.HttpContext.Response.Headers["Cache-Control"] = "max-age=86400";
            return new OkObjectResult(piwikConfig.ToString());
        }
    }
}