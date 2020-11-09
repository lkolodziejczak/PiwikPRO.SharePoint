using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Services;
using Microsoft.SharePoint.WebControls;
using PiwikPRO.SharePoint.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.SP2013
{
    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PiwikPROChangeTrackingStatusService : IPiwikPROChangeTrackingStatus
    {
        public string ChangeTrackingStatus(string statusProp)
        {
            string returner = "true";
            try
            {
                if (SPContext.Current.Web.CurrentUser.IsSiteAdmin)
                {
                    string siteToTrackUrl = SPContext.Current.Site.Url;
                    //string siteToTrackRelativeUrl = SPContext.Current.Site.ServerRelativeUrl;
                    string siteToTrackTitle = SPContext.Current.Site.RootWeb.Title;
                    string piwikAdminSiteUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, System.Web.HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("_vti_bin")) + "sites/piwikadmin";

                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        ClientContext context = new ClientContext(piwikAdminSiteUrl);
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                        ListProcessor sdlo = new ListProcessor(context, new SPLogger());
                        if (statusProp == "0")
                        {
                            sdlo.AddOrUpdateElementInList(siteToTrackTitle, ConfigValues.PiwikPro_SiteDirectory_Column_Status_New, siteToTrackUrl, "", siteToTrackUrl, "");
                        }
                        if (statusProp == "1")
                        {
                            sdlo.AddOrUpdateElementInList(siteToTrackTitle, ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating, siteToTrackUrl, "", siteToTrackUrl, "");
                        }
                    });
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik ChangeTrackingStatus", ex.Message);
                returner = ex.Message;
            }
            return returner;
        }   
    }
}