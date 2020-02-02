using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Xml;
using System.Web;

namespace PiwikPRO.SharePoint.Shared
{
    public class PiwikPROServiceOperations
    {
        public PiwikPROServiceOperations(string _clientId, string _clientSecret, string _serviceUrl, string _oldApiToken, ISPLogger _logger)
        {
            this.piwik_clientID = _clientId;
            this.piwik_clientSecret = _clientSecret;
            this.piwik_serviceUrl = _serviceUrl;
            this.piwik_oldapitoken = _oldApiToken;
            this.logger = _logger;
        }

        ISPLogger logger;
        private const string _apiAppsV2 = "/api/apps/v2";
        private const string _authToken = "/auth/token";
        private string _bearer;
        private string piwik_clientID;
        private string piwik_clientSecret;
        private string piwik_serviceUrl;
        private string piwik_oldapitoken;


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

                var callCommandUrl = new Uri(String.Format("https://{0}{1}\\{2}", piwik_serviceUrl, _apiAppsV2, siteID));

                string returnerXml = MakeRequest(jobj.ToString(), callCommandUrl, "PATCH");
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik SetSetGdprOffInPiwik", ex.Message);
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

        internal int AddGoalToPiwik(string siteId, string goalName)
        {
            try
            {
                string callCommand = String.Format("https://{0}"+
                    "/?module=API&format=XML" +
                    "&method=Goals.addGoal" +
                    "&idSite={1}" +
                    "&name={2}" +
                    "&matchAttribute=manually&pattern={3}" +
                    "&patternType=regex&caseSensitive=0&revenue=0&allowMultipleConversionsPerVisit=1&token_auth={4}", piwik_serviceUrl, siteId, HttpUtility.HtmlEncode(goalName), HttpUtility.HtmlEncode(".*"), piwik_oldapitoken);

                if (callCommand.ToString().Contains("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                }
                HttpWebRequest HttpRequest = (HttpWebRequest)HttpWebRequest.Create(callCommand);
                XmlDocument doc = new XmlDocument();
                WebResponse response = HttpRequest.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    doc.LoadXml(streamReader.ReadToEnd());

                    XmlNodeList error = doc.GetElementsByTagName("error");

                    if (error.Count > 0)
                    {
                        var message = error[0].Attributes["message"].Value;
                        //write to log
                    }
                }

                XmlNode newMetaSiteId = doc.GetElementsByTagName("result").Item(0);
                return Convert.ToInt32(newMetaSiteId.InnerText);
            }
            catch(Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik AddGoalToPiwik", ex.Message);
            }

            return -1;
        }

        internal void UpdateGoalToPiwik(string siteId, string goalName, string goalId, bool? goalActive)
        {
            try
            {
                string matchAttribute = "manually";
                if (goalActive == false)
                {
                    matchAttribute = "disabled";
                }
                string callCommand = String.Format("https://{0}" +
                    "?module=API&format=XML" +
                    "&method=Goals.updateGoal&idGoal={1}" +
                    "&idSite={2}" +
                    "&name={3}" +
                    "&matchAttribute={4}&pattern={5}" +
                    "&patternType=regex&caseSensitive=0&revenue=0&allowMultipleConversionsPerVisit=1&token_auth={6}", piwik_serviceUrl, goalId, siteId, HttpUtility.HtmlEncode(goalName), matchAttribute, HttpUtility.HtmlEncode(".*"), piwik_oldapitoken);

                if (callCommand.ToString().Contains("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                }
                HttpWebRequest HttpRequest = (HttpWebRequest)HttpWebRequest.Create(callCommand);
                XmlDocument doc = new XmlDocument();
                WebResponse response = HttpRequest.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    doc.LoadXml(streamReader.ReadToEnd());

                    XmlNodeList error = doc.GetElementsByTagName("error");

                    if (error.Count > 0)
                    {
                        var message = error[0].Attributes["message"].Value;
                        //write to log
                    }
                }

                XmlNode newMetaSiteId = doc.GetElementsByTagName("result").Item(0);
            }
            catch (Exception ex)
            {
                 logger.WriteLog(Category.Unexpected, "Piwik UpdateGoalToPiwik", ex.Message);
            }
        }

        #endregion

        #region Private Methods

        private string GetTokenBearer(string clientID, string clientSecret)
        {
            var callCommandUrl = new Uri(String.Format("https://{0}{1}", piwik_serviceUrl, _authToken));

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
                    new JProperty("appType", "sharepoint"),
                    new JProperty("name", siteName),
                    new JProperty("urls", arrayUrls))),
                    new JProperty("type", "ppms/app"))));

                var callCommandUrl = new Uri(String.Format("https://{0}{1}", piwik_serviceUrl, _apiAppsV2));

                string returnerXml = MakeRequest(jobj.ToString(), callCommandUrl, "POST");

                XmlDocument docXML = JsonConvert.DeserializeXmlNode(returnerXml, "root");

                resultSiteID = docXML.GetElementsByTagName("id")[0].InnerText;
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik AddSiteToPiwikService", ex.Message);
                return "Error: " + ex.Message;
            }

            return resultSiteID;
        }

        private string RemoveSiteFromPiwikService(string siteId, string tokenBearer)
        {
            string resultSiteID = string.Empty;
            try
            {
                var callCommandUrl = new Uri(String.Format("https://{0}{1}/{2}", piwik_serviceUrl, _apiAppsV2, siteId));
                resultSiteID = MakeRequest("", callCommandUrl, "DELETE");
               
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik RemoveSiteFromPiwikService", ex.Message);
                return "Error: " + ex.Message;
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

                var callCommandUrl = new Uri(String.Format("https://{0}{1}\\{2}", piwik_serviceUrl, _apiAppsV2, siteID));
                string returnerXml = MakeRequest(jobj.ToString(), callCommandUrl, "PATCH");
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik ChangeNameSiteInPiwikService", ex.Message);
            }
        }

        private string GetAllPiwikProSites(string tokenBearer)
        {
            string resultResponseXml = string.Empty;
            try
            {
                var callCommandUrl = new Uri(String.Format("https://{0}{1}", piwik_serviceUrl, _apiAppsV2));

                resultResponseXml = MakeRequest("", callCommandUrl, "GET");
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik GetAllPiwikProSites", ex.Message);
                return "Error: " + ex.Message;
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
