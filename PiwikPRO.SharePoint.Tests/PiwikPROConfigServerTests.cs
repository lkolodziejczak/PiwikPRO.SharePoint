using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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

        [Test]
        public void ComparePiwikCfgFromBlobWithPiwikConfigFromConfigServer()
        {
            Assert.AreEqual(GetBlobFileContentFromAzureBlobStorage(ConfigurationManager.AppSettings["BlobFileName"]), MakeRequest("", new Uri(ConfigServerURL), "GET"));
        }

        private string GetBlobFileContentFromAzureBlobStorage(string blobFileName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureWebJobsStorage"]);
            CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = serviceClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerReference"]);
            CloudBlockBlob blob = container.GetBlockBlobReference(blobFileName);
            string contents = blob.DownloadTextAsync().Result;

            contents = contents.Replace(" ", "");

            return contents;
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
