using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.Tests.ConfigServerTests
{
    [TestFixture]
    public class PiwikPROConfigServerTests
    {
        public const string ConfigServerURL = "https://piwikprosharepointfunctions2.azurewebsites.net/api/config";
        public const string tenantParameter = "tenant=phkogifi";
        public const string extensionParameter = "extension=valo";
        public const string tenant = "phkogifi";
        public const string extension = "valo";

        [Test]
        public void ComparePiwikCfgFromBlobWithPiwikConfigFromConfigServer()
        {
            Assert.AreEqual(GetMergedBlobFileContentFromAzureBlobStorage(ConfigurationManager.AppSettings["BlobFileName"],"",""), MakeRequest("", new Uri(ConfigServerURL), "GET"));
        }

        [Test]
        public void ComparePiwikCfgWithTenant()
        {
            Assert.AreEqual(GetMergedBlobFileContentFromAzureBlobStorage(ConfigurationManager.AppSettings["BlobFileName"], "", tenant), MakeRequest("", new Uri(ConfigServerURL + "?" + tenantParameter), "GET"));
        }

        [Test]
        public void ComparePiwikCfgWithExtension()
        {
            Assert.AreEqual(GetMergedBlobFileContentFromAzureBlobStorage(ConfigurationManager.AppSettings["BlobFileName"], extension, ""), MakeRequest("", new Uri(ConfigServerURL + "?" + extensionParameter), "GET"));
        }

        [Test]
        public void ComparePiwikCfgWithTenantAndExtension()
        {
            Assert.AreEqual(GetMergedBlobFileContentFromAzureBlobStorage(ConfigurationManager.AppSettings["BlobFileName"], extension, tenant), MakeRequest("", new Uri(ConfigServerURL + "?" + extensionParameter + "&" + tenantParameter), "GET"));
        }

        private string GetMergedBlobFileContentFromAzureBlobStorage(string configBlobFileName, string extension, string tenant)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureWebJobsStorage"]);
            CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = serviceClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerReference"]);
            CloudBlockBlob blob = container.GetBlockBlobReference(configBlobFileName);
            string contents = blob.DownloadTextAsync().Result;

            JObject configFile = JObject.Parse(contents);

            if(!string.IsNullOrEmpty(extension))
            {
                CloudBlockBlob extensionConfigBlob = container.GetBlockBlobReference($"extension-{extension}.json");
                string extensionConfig = extensionConfigBlob.DownloadTextAsync().Result;
                configFile.Merge(JObject.Parse(extensionConfig), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
            }

            if (!string.IsNullOrEmpty(tenant))
            {
                CloudBlockBlob extensionConfigBlob = container.GetBlockBlobReference($"tenant-{tenant}.json");
                string extensionConfig = extensionConfigBlob.DownloadTextAsync().Result;
                configFile.Merge(JObject.Parse(extensionConfig), new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
            }

            string returner = Convert.ToString(configFile).Replace(" ", "");
            
            return returner;
        }

        private string MakeRequest(string jsonString, Uri callCommandUrl, string method)
        {
            ServicePointManager.Expect100Continue = true;
            if (callCommandUrl.ToString().Contains("https"))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
            HttpWebRequest HttpRequest = (HttpWebRequest)HttpWebRequest.Create(callCommandUrl);
            HttpRequest.Method = method;
            HttpRequest.Accept = "text/xml";
            HttpRequest.ContentType = "application/vnd.api+json";
            HttpRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

            if (!string.IsNullOrEmpty(jsonString))
            {
                using (var writer = new StreamWriter(HttpRequest.GetRequestStream()))
                {
                    writer.Write(jsonString);
                }
            }
            WebResponse response = HttpRequest.GetResponse();
            string returnerXml = string.Empty;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                returnerXml = streamReader.ReadToEnd();
            }

            int start = returnerXml.IndexOf("{");
            int end = returnerXml.LastIndexOf("}") + 1;
            int length = end - start;

            // Get actual message
            string cleandJsonString = returnerXml.Substring(start, length);
            cleandJsonString = cleandJsonString.Replace("&amp;", "&").Replace(" ","");

            return cleandJsonString;
        }
    }
}
