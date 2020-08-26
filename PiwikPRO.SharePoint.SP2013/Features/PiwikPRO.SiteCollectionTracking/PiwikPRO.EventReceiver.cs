using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Web.UI;
using System.Web;
using Microsoft.SharePoint.Utilities;
using System.Threading;
using PiwikPRO.SharePoint.Shared;
using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace PiwikPRO.SharePoint.SP2013.Features.PiwikPRO.SiteCollectionTracking
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("749bc6cf-3385-4a00-823e-1468a2d7d5aa")]
    public class PiwikPROEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite osite = ((SPSite)properties.Feature.Parent);

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (SPSite elevatedSite = new SPSite(osite.ID))
                    {
                        var web = elevatedSite.RootWeb;
                        RemoveEventReceiverFromList(web);
                        SPList list = web.GetList(web.ServerRelativeUrl + "/_catalogs/masterpage/");
                        bool oldWebAllowUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;

                        foreach (SPListItem item in list.Items)
                        {
                            if (item.File.CheckedOutByUser != null)
                            {
                                Page page = HttpContext.Current.Handler as Page;
                                SPLongOperation operation = new SPLongOperation(page);
                                operation.Begin();
                                try
                                {
                                    operation.End("javascript:if(!alert('Cannot activate the feature of tracking, master page: " + Convert.ToString(item["Name"]) + " is checked out by user: " + item.File.CheckedOutByUser.Name + " (" + item.File.CheckedOutByUser.LoginName + ").')) document.location = window.location.href.substring(0, window.location.href.indexOf('_layouts')) + '_catalogs/masterpage/Forms/AllItems.aspx';", SPRedirectFlags.Static, HttpContext.Current, null);
                                }
                                catch (ThreadAbortException ex)
                                {
                                    Logger.WriteLog(Logger.Category.Information, "Piwik Master page is checked out: " + Convert.ToString(item["Name"]), ex.Message);
                                }
                                catch (Exception ex) { throw ex; }
                            }
                        }

                        foreach (SPListItem item in list.Items)
                        {
                            if (Convert.ToString(item["Name"]).ToLower().EndsWith(".master"))
                            {
                                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                                EventDisabler eventDisabler = new EventDisabler();
                                SPFile file = item.File;
                                byte[] byteArrayOfFile = file.OpenBinary();
                                if (byteArrayOfFile.Length > 0)
                                {
                                    string strFileContentsBefore = enc.GetString(byteArrayOfFile);
                                    //check if javascript exists 
                                    PropertyBagOperations pbos = new PropertyBagOperations();
                                    string trackerJSScriptUrl = pbos.GetPropertyValueFromListByKey("piwik_trackerjsscripturl");
                                    if (!strFileContentsBefore.ToLower().Contains(trackerJSScriptUrl.ToLower()))
                                    {
                                        if (item.File.CheckOutType == SPFile.SPCheckOutType.None)
                                        {
                                            SPFileLevel oldLevel = item.File.Level;
                                            string newStr = AddJSRefToFile(strFileContentsBefore, trackerJSScriptUrl);
                                            byte[] byteArrayFileContentsAfter = null;
                                            if (!newStr.Equals(""))
                                            {
                                                //after binary to string there are ??? chars at the first line, this is the action to replace it: 
                                                if (newStr.Substring(0, 3).Equals("???"))
                                                {
                                                    newStr = newStr.Replace("???", "");
                                                }
                                                byteArrayFileContentsAfter = enc.GetBytes(newStr);
                                                eventDisabler.DisableEvents();
                                            }

                                            if (list.ForceCheckout)
                                            {
                                                item.File.CheckOut();
                                            }

                                            item.File.SaveBinary(byteArrayFileContentsAfter);
                                            item.File.Update();

                                            if (list.ForceCheckout)
                                            {
                                                if (item.File.Level == SPFileLevel.Checkout)
                                                {
                                                    if (oldLevel == SPFileLevel.Published)
                                                    {
                                                        item.File.CheckIn("", SPCheckinType.MajorCheckIn);
                                                    }
                                                    else
                                                    {
                                                        item.File.CheckIn("", SPCheckinType.MinorCheckIn);
                                                    }
                                                }
                                            }

                                            try
                                            {
                                                if (oldLevel == SPFileLevel.Published)
                                                {
                                                    if (item.File.DocumentLibrary.EnableModeration)
                                                        item.File.Approve("");
                                                    else
                                                        item.File.Publish("");
                                                }
                                            }
                                            catch (Exception exp)
                                            {
                                                Logger.WriteLog(Logger.Category.Unexpected, "Piwik FeatureActivating publish or approve master", exp.Message);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        CreateOrUpdateValueInPropertyBag("false", web, web.Properties, ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive);

                        web.AllowUnsafeUpdates = oldWebAllowUnsafe;
                    }
                    bool ifWasntDeactivatingAndActive = true;
                    PropertyBagOperations pbo = new PropertyBagOperations();
                    Configuration cfg = new Configuration();

                    using (SPSite elevatedSite = new SPSite(cfg.PiwikAdminSiteUrl))
                    {
                        ClientContext context = new ClientContext(pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl));
                        ListProcessor sdlo = new ListProcessor(context, new SPLogger());
                        ifWasntDeactivatingAndActive = sdlo.AddOrUpdateElementInList(osite.RootWeb.Title, ConfigValues.PiwikPro_SiteDirectory_Column_Status_New, osite.RootWeb.Url, "", osite.RootWeb.ServerRelativeUrl, "");
                    }
                    if (!ifWasntDeactivatingAndActive)
                    {
                        using (SPSite elevatedSite = new SPSite(osite.ID))
                        {
                            var web = elevatedSite.RootWeb;
                            CreateOrUpdateValueInPropertyBag("true", web, web.Properties, ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Logger.WriteLog(Logger.Category.Unexpected, "Piwik FeatureActivating Full in elevated", exc.Message);
                }
            });
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite osite = ((SPSite)properties.Feature.Parent);
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite elevatedSite = new SPSite(osite.ID))
                {
                    try
                    {
                        var web = elevatedSite.RootWeb;
                        RemoveEventReceiverFromList(web);
                        SPList list = web.GetList(web.ServerRelativeUrl + "/_catalogs/masterpage/");
                        bool oldAllowUnsafe = web.AllowUnsafeUpdates;
                        web.AllowUnsafeUpdates = true;

                        foreach (SPListItem item in list.Items)
                        {
                            if (item.File.CheckedOutByUser != null)
                            {
                                Page page = HttpContext.Current.Handler as Page;
                                SPLongOperation operation = new SPLongOperation(page);
                                operation.Begin();
                                try
                                {
                                    operation.End("javascript:if(!alert('Cannot deactivate the feature of tracking, master page: " + Convert.ToString(item["Name"]) + " is checked out by user: " + item.File.CheckedOutByUser.Name + " (" + item.File.CheckedOutByUser.LoginName + ").')) document.location = window.location.href.substring(0, window.location.href.indexOf('_layouts')) + '_catalogs/masterpage/Forms/AllItems.aspx';", SPRedirectFlags.Static, HttpContext.Current, null);
                                }
                                catch (ThreadAbortException ex)
                                {
                                    Logger.WriteLog(Logger.Category.Unexpected, "Piwik Master page is checked out: " + Convert.ToString(item["Name"]), ex.Message);
                                }
                                catch (Exception ex) { throw ex; }
                            }
                        }

                        foreach (SPListItem item in list.Items)
                        {
                            if (Convert.ToString(item["Name"]).ToLower().EndsWith(".master"))
                            {
                                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                                EventDisabler eventDisabler = new EventDisabler();
                                SPFile file = item.File;
                                byte[] byteArrayOfFile = file.OpenBinary();
                                if (byteArrayOfFile.Length > 0)
                                {
                                    string strFileContentsBefore = enc.GetString(byteArrayOfFile);
                                    //check if javascript exists 
                                    PropertyBagOperations pbos = new PropertyBagOperations();
                                    string trackerJSScriptUrl = pbos.GetPropertyValueFromListByKey("piwik_trackerjsscripturl");
                                    if (strFileContentsBefore.ToLower().Contains(trackerJSScriptUrl.ToLower()))
                                    {
                                        if (item.File.CheckOutType == SPFile.SPCheckOutType.None)
                                        {
                                            SPFileLevel oldLevel = item.File.Level;
                                            string newStr = RemoveJSFromFile(strFileContentsBefore, trackerJSScriptUrl);
                                            byte[] byteArrayFileContentsAfter = null;
                                            if (!newStr.Equals(""))
                                            {
                                                //after binary to string there are ??? chars at the first line, this is the action to replace it: 
                                                if (newStr.Substring(0, 3).Equals("???"))
                                                {
                                                    newStr = newStr.Replace("???", "");
                                                }
                                                byteArrayFileContentsAfter = enc.GetBytes(newStr);
                                                eventDisabler.DisableEvents();
                                            }

                                            if (list.ForceCheckout)
                                            {
                                                item.File.CheckOut();
                                            }

                                            item.File.SaveBinary(byteArrayFileContentsAfter);
                                            item.File.Update();

                                            if (list.ForceCheckout)
                                            {
                                                if (item.File.Level == SPFileLevel.Checkout)
                                                {
                                                    if (oldLevel == SPFileLevel.Published)
                                                    {
                                                        item.File.CheckIn("", SPCheckinType.MajorCheckIn);
                                                    }
                                                    else
                                                    {
                                                        item.File.CheckIn("", SPCheckinType.MinorCheckIn);
                                                    }
                                                }
                                            }

                                            try
                                            {
                                                if (oldLevel == SPFileLevel.Published)
                                                {
                                                    if (item.File.DocumentLibrary.EnableModeration)
                                                        item.File.Approve("");
                                                    else
                                                        item.File.Publish("");
                                                }
                                            }
                                            catch (Exception exp)
                                            {
                                                Logger.WriteLog(Logger.Category.Unexpected, "Piwik FeatureActivating publish or approve master", exp.Message);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        web.AllowUnsafeUpdates = oldAllowUnsafe;

                        PropertyBagOperations pbo = new PropertyBagOperations();
                        Configuration cfg = new Configuration();

                        using (SPSite elevatedSiteAdmin = new SPSite(cfg.PiwikAdminSiteUrl))
                        {
                            ClientContext context = new ClientContext(pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl));
                            ListProcessor sdlo = new ListProcessor(context, new SPLogger());
                            sdlo.AddOrUpdateElementInList(osite.RootWeb.Title, ConfigValues.PiwikPro_SiteDirectory_Column_Status_Deactivating, osite.RootWeb.Url, "", osite.RootWeb.ServerRelativeUrl, "");
                        }
                    }
                    catch (Exception exc)
                    {
                        Logger.WriteLog(Logger.Category.Unexpected, "Piwik FeatureDeactivating Full in elevated", exc.Message);
                    }
                }
            });
        }
        private string AddJSRefToFile(string strFileContentsBefore, string trackerJSScriptUrl)
        {
            string newStr = string.Empty;

            if (strFileContentsBefore.Contains("<head runat=\"server\">\r\n\t"))
            {
                newStr = ReplaceFirst(strFileContentsBefore, "<head runat=\"server\">\r\n\t", "<head runat=\"server\">\r\n\t<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>\r\n\t");
            }
            else
            {
                if (strFileContentsBefore.Contains("<head runat=\"server\">\r\n"))
                {
                    newStr = ReplaceFirst(strFileContentsBefore, "<head runat=\"server\">\r\n", "<head runat=\"server\">\r\n<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>\r\n");
                }
                if (strFileContentsBefore.Contains("<head>"))
                {
                    newStr = ReplaceFirst(strFileContentsBefore, "<head>", "<head>\r\n<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>\r\n");
                }
            }

            return newStr;
        }

        private string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        private string RemoveJSFromFile(string strFileContentsBefore, string trackerJSScriptUrl)
        {
            string newStr = string.Empty;

            if (strFileContentsBefore.Contains("<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>\r\n\t"))
            {
                newStr = strFileContentsBefore.Replace("<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>\r\n\t", "");
            }
            else
            {
                if (strFileContentsBefore.Contains("<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>\r\n"))
                {
                    newStr = strFileContentsBefore.Replace("<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>\r\n", "");
                }
                else
                {
                    newStr = strFileContentsBefore.Replace("<script language=\"javascript\" src=\"" + trackerJSScriptUrl + "\"></script>", "");
                }
            }

            return newStr;
        }

        private void RemoveEventReceiverFromList(SPWeb web)
        {
            SPList listM = web.GetList(web.ServerRelativeUrl + "/_catalogs/masterpage/");
            bool oldAllowUnsafeUpdated = web.AllowUnsafeUpdates;
            try
            {
                web.AllowUnsafeUpdates = true;

                SPEventReceiverDefinitionCollection erdc = listM.EventReceivers;
                List<SPEventReceiverDefinition> eventsToDelete = new List<SPEventReceiverDefinition>();

                foreach (SPEventReceiverDefinition erd in erdc)
                {
                    if (erd.Type == SPEventReceiverType.ItemUpdated)
                    {
                        eventsToDelete.Add(erd);
                        break;
                    }
                }
                foreach (SPEventReceiverDefinition er in eventsToDelete)
                {
                    er.Delete();
                }
                listM.Update();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.Category.Unexpected, "RemoveEventReceiverFromList", ex.Message);
            }
            finally
            {
                // Remember to set AllowUnsafeUpdates to false again when you're done. 
                web.AllowUnsafeUpdates = oldAllowUnsafeUpdated;
            }

        }

        private static void CreateOrUpdateValueInPropertyBag(string value, SPWeb webToPropertyBag, SPPropertyBag currentBag, string propertyBagKey)
        {
            if (currentBag.ContainsKey(propertyBagKey) && currentBag[propertyBagKey] != null)
            {
                currentBag[propertyBagKey] = value;
                currentBag.Update();
                webToPropertyBag.Properties.Update();
                webToPropertyBag.Update();
            }
            else
            {
                currentBag.Add(propertyBagKey, value);
                currentBag.Update();
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed. 

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties) 
        //{ 
        //} 


        // Uncomment the method below to handle the event raised before a feature is uninstalled. 

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties) 
        //{ 
        //} 

        // Uncomment the method below to handle the event raised when a feature is upgrading. 

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters) 
        //{ 
        //} 
    }

    public class EventDisabler : SPEventReceiverBase
    {
        public void DisableEvents()
        {
            DisableEventFiring();
        }
        public void EnableEvents()
        {
            EnableEventFiring();
        }
    }
}