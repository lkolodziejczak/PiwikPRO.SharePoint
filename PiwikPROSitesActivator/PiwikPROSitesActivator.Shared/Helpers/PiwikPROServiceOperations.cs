using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Xml;
using System.Web;
using Microsoft.SharePoint.Client;

namespace PiwikPROSitesActivator.Shared
{
    public class PiwikPROServiceOperations
    {
        public PiwikPROServiceOperations(string _clientId, string _clientSecret, string _serviceUrl)
        {
            this.piwik_clientID = _clientId;
            this.piwik_clientSecret = _clientSecret;
            this.piwik_serviceUrl = _serviceUrl;
        }

        private const string _apiAppsV2 = "/api/apps/v2";
        private const string _authToken = "/auth/token";
        private string _bearer;
        private string piwik_clientID;
        private string piwik_clientSecret;
        private string piwik_serviceUrl;


        #region Public Methods

        public string AddSiteToPiwik(string siteName, string url)
        {
            if (string.IsNullOrEmpty(_bearer))
            {
                _bearer = GetTokenBearer(piwik_clientID, piwik_clientSecret);
            }
            return AddSiteToPiwikService(siteName, url, _bearer);
        }

        public void SetSetGdprOffInPiwik(string siteID)
        {
            try
            {
                //JSON should look like: "{\n\"data\": {\n\"attributes\":{\n\n\"gdpr\":false\n},\n\"type\":\"ppms/app\",\n\"id\":\"" + siteID + "\"\n}\n}\n";
                JObject jobj = new JObject(
                    new JProperty("data",
                        new JObject(
                            new JProperty("attributes",
                                new JObject(
                                    new JProperty("gdpr", false))),
                                    new JProperty("type", "ppms/app"),
                                    new JProperty("id", siteID))));

                var callCommandUrl = new Uri(String.Format("{0}{1}\\{2}", piwik_serviceUrl, _apiAppsV2, siteID));

                string returnerXml = MakeRequest(jobj.ToString(), callCommandUrl, "PATCH");
            }
            catch (Exception ex)
            {
                Logger.WriteLine("\nPiwik SetSetGdprOffInPiwik: " + ex.Message);
            }
        }

        public void SetSharepointIntegrationOnInPiwik(string siteID)
        {
            try
            {
                //JSON should look like: "{\n\"data\": {\n\"attributes\":{\n\n\"gdpr\":false\n},\n\"type\":\"ppms/app\",\n\"id\":\"" + siteID + "\"\n}\n}\n";
                JObject jobj = new JObject(
                    new JProperty("data",
                        new JObject(
                            new JProperty("attributes",
                                new JObject(
                                    new JProperty("sharepointIntegration", true))),
                                    new JProperty("type", "ppms/app"),
                                    new JProperty("id", siteID))));

                var callCommandUrl = new Uri(String.Format("{0}{1}\\{2}", piwik_serviceUrl, _apiAppsV2, siteID));

                if (string.IsNullOrEmpty(_bearer))
                {
                    _bearer = GetTokenBearer(piwik_clientID, piwik_clientSecret);
                }

                string returnerXml = MakeRequest(jobj.ToString(), callCommandUrl, "PATCH");
            }
            catch (Exception ex)
            {
                Logger.WriteLine("\nPiwik SetSharepointIntegrationOn: " + ex.Message);
            }
        }

        public void PublishLastVersionOfTagManager(string siteID)
        {
            try
            {
                // https://example.piwik.pro/api/tag/v1/{app_id}/versions/draft/publish
                var callCommandUrl = new Uri(String.Format("{0}{1}{2}{3}", piwik_serviceUrl, "/api/tag/v1/", siteID, "/versions/draft/publish"));

                ServicePointManager.Expect100Continue = true;
                if (callCommandUrl.ToString().Contains("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                }
                HttpWebRequest HttpRequest = (HttpWebRequest)HttpWebRequest.Create(callCommandUrl);
                HttpRequest.Method = "POST";
                HttpRequest.Accept = "*/*";
                HttpRequest.ContentType = "application/vnd.api+json";
                HttpRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                if (String.IsNullOrEmpty(_bearer))
                {
                    _bearer = GetTokenBearer(piwik_clientID, piwik_clientSecret);
                }
                if (!String.IsNullOrEmpty(_bearer))
                {
                    HttpRequest.PreAuthenticate = true;
                    HttpRequest.Headers.Add("Authorization", _bearer);
                }

                WebResponse response = HttpRequest.GetResponse();
                string returnerXml = string.Empty;
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    returnerXml = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine("\nPiwik PublishLastVersionOfTagManager: " + ex.Message);
            }
        }

        public void AddTagManagerJSONFile(string siteID, ClientContext clientContext)
        {
            //https://example.piwik.pro/api/tag/v1/{app_id}/versions/import-file
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            // var callCommandUrl = new Uri(String.Format("{0}{1}{2}{3}", piwik_serviceUrl, "/api/tag/v1/", "cb1789d0-c7fa-4060-a4ed-62aafccc7f81", "/versions/import-file"));
            var callCommandUrl = new Uri(String.Format("{0}{1}{2}{3}", piwik_serviceUrl, "/api/tag/v1/", siteID, "/versions/import-file"));
            ServicePointManager.Expect100Continue = true;
            if (callCommandUrl.ToString().Contains("https"))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
            HttpWebRequest HttpRequest = (HttpWebRequest)HttpWebRequest.Create(callCommandUrl);
            HttpRequest.Method = "POST";
            HttpRequest.Accept = "*/*";
            HttpRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            HttpRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            if (String.IsNullOrEmpty(_bearer))
            {
                _bearer = GetTokenBearer(piwik_clientID, piwik_clientSecret);
            }
            HttpRequest.PreAuthenticate = true;
            HttpRequest.Headers.Add("Authorization", _bearer);
            Stream memStream = new System.IO.MemoryStream();
            var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            var endBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--");

            string filePath = clientContext.Url + "/Style%20Library/PROD/tagmanager.json";

            Uri filename = new Uri(filePath);
            string server = filename.AbsoluteUri.Replace(filename.AbsolutePath, "");
            string serverrelative = filename.AbsolutePath;

            var result = clientContext.Web.GetFileByServerRelativeUrl(serverrelative).OpenBinaryStream();


            clientContext.ExecuteQueryRetry();
            using (var stream = result.Value)
            { 
                string header =
      "Content-Disposition: form-data; name=\"file\"; filename=\"" + Path.GetFileName(filePath) + "\"\r\n" +
      "Content-Type: application/json\r\n\r\n";

            memStream.Write(boundarybytes, 0, boundarybytes.Length);
            var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

            memStream.Write(headerbytes, 0, headerbytes.Length);

            using (var fileStream = stream)
            {
                var buffer = new byte[1024];
                var bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }
            }
       


            memStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                HttpRequest.ContentLength = memStream.Length;

                using (Stream requestStream = HttpRequest.GetRequestStream())
                {
                    memStream.Position = 0;
                    byte[] tempBuffer = new byte[memStream.Length];
                    memStream.Read(tempBuffer, 0, tempBuffer.Length);
                    memStream.Close();
                    requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                }
            }

            WebResponse response = HttpRequest.GetResponse();
                string returnerXml = string.Empty;
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    returnerXml = streamReader.ReadToEnd();
                }
        }

        public string RemoveSiteFromPiwik(string siteid)
        {
            if (string.IsNullOrEmpty(_bearer))
            {
                _bearer = GetTokenBearer(piwik_clientID, piwik_clientSecret);
            }
            return RemoveSiteFromPiwikService(siteid, _bearer);
        }

        public void ChangeNameSiteInPiwik(string siteName, string siteid)
        {
            if (string.IsNullOrEmpty(_bearer))
            {
                _bearer = GetTokenBearer(piwik_clientID, piwik_clientSecret);
            }
            ChangeNameSiteInPiwikService(siteid, _bearer, siteName);
        }

        public string GetAllPiwikSites()
        {
            if (string.IsNullOrEmpty(_bearer))
            {
                _bearer = GetTokenBearer(piwik_clientID, piwik_clientSecret);
            }
            return GetAllPiwikProSites(_bearer);
        }

        public bool CheckIfPageIsAlreadyOnPiwik(string idSite)
        {
            string allSitesResponse = GetAllPiwikSites();
            if (allSitesResponse.Contains(idSite))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Private Methods

        private string GetTokenBearer(string clientID, string clientSecret)
        {
            var callCommandUrl = new Uri(String.Format("{0}{1}", piwik_serviceUrl, _authToken));

            //JSON should look like: "{\"grant_type\":\"client_credentials\",\"client_id\":\"" + clientID + "\",\"client_secret\":\"" + clientSecret + "\"}";
            JObject jobj = new JObject(
                new JProperty("grant_type", "client_credentials"),
                new JProperty("client_id", clientID),
                new JProperty("client_secret", clientSecret));
            string bearerJson = MakeRequest(jobj.ToString(), callCommandUrl, "POST");

            var joobject = JObject.Parse(bearerJson);
            string stdd = joobject["access_token"].ToString();
            stdd.Replace("{", string.Empty).Replace("}", string.Empty);
            if (!string.IsNullOrEmpty(stdd))
            {
                return "Bearer" + " " + stdd;
            }
            else
            {
                return "";
            }
        }

        private string AddSiteToPiwikService(string siteName, string urls, string tokenBearer)
        {
            string resultSiteID = string.Empty;
            try
            {
                //JSON should look like: "{\n\"data\":{\n\"attributes\":{\"appType\":\"sharepoint\",\n\"name\":\"" + siteName + "\",\n\"urls\":[\n\"" + urls + "\"]},\n\"type\":\"ppms/app\"\n}\n}";
                JArray arrayUrls = new JArray();
                arrayUrls.Add(urls);
                JObject oUrls = new JObject();
                oUrls["urls"] = arrayUrls;

                JObject jobj = new JObject(
    new JProperty("data",
        new JObject(
            new JProperty("attributes",
                new JObject(
                    new JProperty("appType", "web"),
                    new JProperty("name", siteName),
                    new JProperty("urls", arrayUrls))),
                    new JProperty("type", "ppms/app"))));

                var callCommandUrl = new Uri(String.Format("{0}{1}", piwik_serviceUrl, _apiAppsV2));

                string returnerXml = MakeRequest(jobj.ToString(), callCommandUrl, "POST");

                XmlDocument docXML = JsonConvert.DeserializeXmlNode(returnerXml, "root");

                resultSiteID = docXML.GetElementsByTagName("id")[0].InnerText;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Piwik AddSiteToPiwikService"+ ex.Message);
                return "Error: AddSiteToPiwikService: " + ex.Message;
            }

            return resultSiteID;
        }

        private string RemoveSiteFromPiwikService(string siteId, string tokenBearer)
        {
            string resultSiteID = string.Empty;
            try
            {
                var callCommandUrl = new Uri(String.Format("{0}{1}/{2}", piwik_serviceUrl, _apiAppsV2, siteId));
                resultSiteID = MakeRequest("", callCommandUrl, "DELETE");
               
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Piwik RemoveSiteFromPiwikService: "+ ex.Message);
                return "Error: RemoveSiteFromPiwikService: " + ex.Message;
            }

            return resultSiteID;
        }

        private void ChangeNameSiteInPiwikService(string siteID, string tokenBearer, string siteName)
        {
            try
            {
                //JSON should look like: "{\n\"data\": {\n\"attributes\":{\n\n\"name\":\"" + siteName + "\""+"\n},\n\"type\":\"ppms/app\",\n\"id\":\"" + siteID + "\"\n}\n}";

                JObject jobj = new JObject(
    new JProperty("data",
        new JObject(
            new JProperty("attributes",
                new JObject(
                    new JProperty("name", siteName))),
                new JProperty("type", "ppms/app"),
                new JProperty("id", siteID))));

                var callCommandUrl = new Uri(String.Format("{0}{1}\\{2}", piwik_serviceUrl, _apiAppsV2, siteID));
                string returnerXml = MakeRequest(jobj.ToString(), callCommandUrl, "PATCH");
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Piwik ChangeNameSiteInPiwikService: "+  ex.Message);
            }
        }

        private string GetAllPiwikProSites(string tokenBearer)
        {
            string resultResponseXml = string.Empty;
            try
            {
                var callCommandUrl = new Uri(String.Format("{0}{1}", piwik_serviceUrl, _apiAppsV2));

                resultResponseXml = MakeRequest("", callCommandUrl, "GET");
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Piwik GetAllPiwikProSites: "+ex.Message);
                return "Error: GetAllPiwikProSites: " + ex.Message;
            }

            return resultResponseXml;
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
            if (!String.IsNullOrEmpty(_bearer))
            {
                HttpRequest.PreAuthenticate = true;
                HttpRequest.Headers.Add("Authorization", _bearer);
            }
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

            return returnerXml;
        }

        #endregion
    }
}
