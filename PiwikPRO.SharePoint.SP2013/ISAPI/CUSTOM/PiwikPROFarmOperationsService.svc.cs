using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client.Services;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.SP2013
{
    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PiwikPROFarmOperationsService : IPiwikPROFarmOperationsService
    {
        public bool IsUserFarmAdmin()
        {
            //SPContext currentContext = SPContext.Current;
            // string uName = string.Empty;
            //uName = System.Web.HttpContext.Current.User.Identity.Name;
            // return IsFarmAdmin(uName);
            return SPFarm.Local.CurrentUserIsAdministrator(true);
        }

        public string UpdateFarmProperty(string propName, string propValue)
        {
            string returner = "true";
            try
            {
                //SPSecurity.RunWithElevatedPrivileges(delegate ()
                // {
                System.Web.HttpContext.Current.Items["FormDigestValidated"] = true;
                SPFarm farm = SPFarm.Local;
                farm.Properties[propName] = propValue;

                farm.Update();
                //});
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik UpdateFarmProperty", ex.Message);
                returner = ex.Message;
            }
            return returner;
        }

    public string UpdateFarmPropertyCentral(string propName, string propValue)
    {
        string returner = "true";
        try
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
             {
                 System.Web.HttpContext.Current.Items["FormDigestValidated"] = true;
                 Microsoft.SharePoint.Administration.SPAdministrationWebApplication centralWeb =
 SPAdministrationWebApplication.Local;
                 string centralAdminUrl = centralWeb.Sites[0].Url;

                 string callCommandUrl = centralAdminUrl + "/_vti_bin/CUSTOM/PiwikPROFarmOperationsServiceCentral.svc/UpdateFarmProperty?propName=" + propName + "&propValue=" + propValue;

                 ServicePointManager.Expect100Continue = true;
                 if (centralAdminUrl.ToString().Contains("https"))
                 {
                     ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                 }
                 HttpWebRequest HttpRequest = (HttpWebRequest)HttpWebRequest.Create(callCommandUrl);
                 HttpRequest.Method = "GET";
                 HttpRequest.Accept = "application/json; odata=verbose";
                 HttpRequest.ContentType = "application/json;odata=verbose";
                 HttpRequest.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
                     //HttpRequest.UseDefaultCredentials = true;
                     HttpRequest.Credentials = CredentialCache.DefaultNetworkCredentials;
                 HttpRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                 WebResponse response = HttpRequest.GetResponse();
                 string returnerXml = string.Empty;
                 using (var streamReader = new StreamReader(response.GetResponseStream()))
                 {
                     returnerXml = streamReader.ReadToEnd();
                 }

                 returner = returnerXml;
                 });
        }
        catch (Exception ex)
        {
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik UpdateFarmPropertyCentral", ex.Message);
                returner = ex.Message;
        }
        return returner;
    }

    public string GetFarmProperty(string propName)
    {
        string returner = "";
        try
        {
            // SPSecurity.RunWithElevatedPrivileges(delegate ()
            //{
            SPFarm farm = SPFarm.Local;
            returner = Convert.ToString(farm.Properties[propName]);

            // });
        }
        catch (Exception ex)
        {
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik GetFarmProperty", ex.Message);
                returner = ex.Message;
        }
        return returner;
    }

    private static bool IsFarmAdmin(string loginName)
    {
        bool isFarmAdmin = false;

        Microsoft.SharePoint.Administration.SPAdministrationWebApplication centralWeb =
SPAdministrationWebApplication.Local;
        string centralAdminUrl = centralWeb.Sites[0].Url;

        SPSecurity.RunWithElevatedPrivileges(delegate ()
        {
            using (SPSite site = new SPSite(centralAdminUrl))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPGroup adminGroup = web.SiteGroups["Farm Administrators"];

                    foreach (SPUser user in adminGroup.Users)
                    {
                        if (user.LoginName == loginName)
                        {
                            isFarmAdmin = true;
                            break;
                        }
                    }
                }
            }

        });

        return isFarmAdmin;
    }
}
}
