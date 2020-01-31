using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using PiwikPRO.SharePoint.Shared;

namespace PiwikPRO.SharePoint.SP2013
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class ERPiwikPROSiteCollDeleting : SPWebEventReceiver
    {
        /// <summary>
        /// A site is being deleted.
        /// </summary>
        public override void SiteDeleting(SPWebEventProperties properties)
        {
            try
            {
                SPWeb web = properties.Web as SPWeb;
                SPPropertyBag currentBag = web.Properties;
                if (currentBag.ContainsKey(ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive) && currentBag[ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive] != null)
                {
                    string siteId = currentBag[ConfigValues.PiwikPro_PropertyBag_SiteId];
                    string siteTitle = string.Empty;
                    PropertyBagOperations pbo = new PropertyBagOperations();
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        // using (SPWeb web = new SPSite(ConfigValues.PiwikPro_PiwikAdminSiteUrl).OpenWeb())
                       // using (SPWeb webz = new SPSite(pbo.GetPropertyValueFromListByKey("piwik_adminsiteurl")).OpenWeb())
                       //{
                            ClientContext context = new ClientContext(pbo.GetPropertyValueFromListByKey(pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl)));
                            Configuration cfg = new Configuration();
                            ListProcessor sdlo = new ListProcessor(context, cfg, new SPLogger());
                            ListItem item = sdlo.CheckIfElementIsAlreadyOnList(context.Web.ServerRelativeUrl);
                            if (item != null)
                            {
                                item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deleted;
                                siteTitle = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_Title]);
                                item.Update();
                            }
                        //}

                        PiwikPROServiceOperations pso = new PiwikPROServiceOperations(pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ClientID),
                            pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ClientSecret),
                            pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_OldApiToken),
                            pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ServiceUrl), new SPLogger());
                        pso.ChangeNameSiteInPiwik("Deleted - " + Convert.ToString(siteTitle), siteId);
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik WebDeleting", ex.Message);
            }

            base.SiteDeleting(properties);
        }


    }
}