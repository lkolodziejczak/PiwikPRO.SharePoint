using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace PiwikPROSitesActivator.Shared.Helpers
{
    public class PiwikPROJobOperations
    {
        private readonly ClientContext context;
        private readonly IConfiguration cfg;

        public PiwikPROJobOperations(ClientContext context, IConfiguration configuration)
        {
            this.context = context;
            this.cfg = configuration;
        }

        public void GetAllSettingsUpdatedSitesAndOperate()
        {
            try
            {
                ListProcessor sdlo = new ListProcessor(context);

                //Get all Sites with status "Settings updated" and put it to Piwik
                foreach (ListItem item in sdlo.GetAllNewSettingsUpdatedSites())
                {
                    //connect to service and create new site
                    PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl);

                    FieldUrlValue valueUrl = (FieldUrlValue)(item[ConfigValues.PiwikPro_SiteDirectory_Column_Url]);

                    string idSite = string.Empty;
                    idSite = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]);
                    if (!string.IsNullOrEmpty(idSite))
                    {
                        try
                        {
                            Thread.Sleep(5000);
                            pso.SetSharepointIntegrationOnInPiwik(idSite);

                            Thread.Sleep(5000);
                            //add tag manager json
                            Logger.WriteLine("\nAdding tag manager for site in Piwik PRO Analytics...");
                            pso.AddTagManagerJSONFile(idSite, context);

                            Thread.Sleep(20000);

                            //publish tag manager
                            Logger.WriteLine("\nPublishing tag manager in Piwik PRO Analytics...");
                            pso.PublishLastVersionOfTagManager(idSite);
                            Logger.WriteLine("\nDone.");
                        }
                        catch (Exception exp)
                        {
                            item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = exp.Message;
                            Logger.WriteLine("\nPiwik GetAllNewSettingsUpdatedSites Inside: " + exp.Message);
                        }

                        item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Active;
                    }
                    else
                    {
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = "id of site is empty!";
                    }

                    item.Update();
                    context.ExecuteQueryRetry();
                }
            }
            catch (Exception expcc)
            {
                Logger.WriteLine("\nPiwik GetAllSettingsUpdatedSitesAndOperate: " + expcc.Message);
            }
        }

        public void GetAllNewSitesAndOperate(bool isSPOnline = false)
        {
            try
            {
                ListProcessor sdlo = new ListProcessor(context);

                //Get all Sites with status "New" and put it to Piwik
                foreach (ListItem item in sdlo.GetAllNewSites())
                {
                    //connect to service and create new site
                    PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl);

                    FieldUrlValue valueUrl = (FieldUrlValue)(item[ConfigValues.PiwikPro_SiteDirectory_Column_Url]);

                    string idSite = string.Empty;
                    bool isSiteAlreadyOnPiwik = false;
                    if (!string.IsNullOrEmpty(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID])))
                    {
                        isSiteAlreadyOnPiwik = pso.CheckIfPageIsAlreadyOnPiwik(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]));
                    }
                    if (isSiteAlreadyOnPiwik)
                    {
                        idSite = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]);
                        pso.ChangeNameSiteInPiwik(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_Title]), idSite);
                        Logger.WriteLine("\nFound the site in Piwik PRO Analytics. Making changes: " + valueUrl.Url);
                    }
                    else
                    {
                        Logger.WriteLine("\nCreating new site in Piwik PRO Analytics: "+ valueUrl.Url);
                        idSite = pso.AddSiteToPiwik(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_Title]), valueUrl.Url);
                    }

                    if (idSite.Contains("Error: "))
                    {
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Error;
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = idSite;
                        Logger.WriteLine("\nThere is an error during updates in Piwik PRO Analytics: " + valueUrl.Url);
                    }
                    else
                    {
                        try
                        {
                            ClientContext contextToPropBag = context.Clone(valueUrl.Url);

                            try
                            {
                                SetEnablePropertyBagChange(valueUrl.Url);
                            }
                            catch
                            {
                                //in case on premise
                            }

                            CreateOrUpdateValueInPropertyBag(idSite, contextToPropBag, ConfigValues.PiwikPro_PropertyBag_SiteId);
                            CreateOrUpdateValueInPropertyBag("true", contextToPropBag, ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive);

                            //set gdpr off
                            pso.SetSetGdprOffInPiwik(idSite);

                            //set sp integration on
                            pso.SetSharepointIntegrationOnInPiwik(idSite);

                            try
                            {
                                Thread.Sleep(5000);
                                //add tag manager json
                                Logger.WriteLine("\nEnabling Tag Manager for the site in Piwik PRO...");
                                pso.AddTagManagerJSONFile(idSite, context);

                                Thread.Sleep(20000);
                                //publish tag manager
                                Logger.WriteLine("\nPublishing the Tag Manager snapshot.");
                                pso.PublishLastVersionOfTagManager(idSite);
                                Logger.WriteLine("\nDone.");
                            }
                            catch (Exception exp)
                            {
                                item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = exp.Message;
                                Logger.WriteLine("\nThere was a problem with importing the Tag Manager container snapshot: " + exp.Message);
                            }

                            //create/update values in propbag
                            PropertyValues currentBag = contextToPropBag.Site.RootWeb.AllProperties;
                            contextToPropBag.Load(currentBag);
                            contextToPropBag.ExecuteQueryRetry();

                            AddPropBagValuesToIndexedProperties(contextToPropBag);

                            //commented because not required yet
                            //if (isSPOnline)
                            //{
                                //AddCustomAction("ListTrackingCommandSet", contextToPropBag, "a0a0acea-cd3c-454b-9376-9cd0e98f5847", "ListTrackingCommandSet", "ListTrackingCommandSet", "Adds ListTrackingCommandSet to the site", "ClientSideExtension.ApplicationCustomizer");
                                //AddCustomAction("TrackingApplicationCustomizer", contextToPropBag, "2ff5e374-69cb-4645-9083-b6317019705b", "TrackingApplicationCustomizer", "TrackingApplicationCustomizer", "Adds TrackingApplicationCustomizer to the site", "ClientSideExtension.ApplicationCustomizer");
                            //}
                        }
                        catch (Exception exp)
                        {
                            item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = exp.Message;
                            Logger.WriteLine("\nPiwik GetAllNewSitesAndOperate: " + exp.Message);
                        }

                        item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Active;
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID] = idSite;

                    }

                    item.Update();
                    context.ExecuteQueryRetry();
                }
            }
            catch (Exception expcc)
            {
                Logger.WriteLine("\nPiwik GetAllNewSitesAndOperateOnFinish: " + expcc.Message);
            }
        }

        public void GetAllDeactivatingSitesAndOperate(bool isSPOnline = false)
        {
            try
            {
                ListProcessor sdlo = new ListProcessor(context);
                foreach (ListItem item in sdlo.GetAllDeactivatingSites())
                {
                    //connect to service and deactivate
                    PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl);
                    string idSite = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]);
                    FieldUrlValue valueUrl = (FieldUrlValue)(item[ConfigValues.PiwikPro_SiteDirectory_Column_Url]);
                    //idSite = pso.RemoveSiteFromPiwik(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]));
                    pso.ChangeNameSiteInPiwik("Inactive - " + Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_Title]), idSite);
                    if (idSite.Contains("Error: "))
                    {
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Error;
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = idSite;
                        Logger.WriteLine("\nThere was a problem with deactivating the site in Piwik PRO Analytics: " + valueUrl.Url);
                    }
                    else
                    {
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_NoActive;
                        Logger.WriteLine("\nThe site has been deactivated: " + valueUrl.Url);
                    }

                    item.Update();
                    context.ExecuteQueryRetry();

                    using (ClientContext contextToPropBag = context.Clone(valueUrl.Url))
                    {
                        CreateOrUpdateValueInPropertyBag("false", contextToPropBag, ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive);
                        //commented because not required yet
                        //if (isSPOnline)
                        //{
                        //    DeleteCustomAction("ListTrackingCommandSet", contextToPropBag);
                        //    DeleteCustomAction("TrackingApplicationCustomizer", contextToPropBag);
                        //    DeleteCustomAction("PiwikPRO.SharePoint365.Tracking", contextToPropBag);
                        //}
                    }
                }
            }
            catch (Exception expcc)
            {
                Logger.WriteLine("\nPiwik GetAllNewSitesAndOperateOnFinish: " + expcc.Message);
            }
        }

        private static void DeleteCustomAction(string customActionName, ClientContext ctx)
        {
            var userCustomActions = ctx.Site.UserCustomActions;
            ctx.Load(userCustomActions);
            ctx.ExecuteQueryRetry();

            for (int i = userCustomActions.Count - 1; i >= 0; i--)
            {
                if (userCustomActions[i].Name == customActionName)
                {
                    userCustomActions[i].DeleteObject();
                    ctx.ExecuteQueryRetry();
                }
            }
        }

        //private static void AddCustomAction(string customActionName, ClientContext ctx, string guid, string spfxExtName, string spfxExtTitle, string spfxExtDescription, string spfxExtLocation)
        //{
        //    bool checkIfExists = false;

        //    var userCustomActions = ctx.Site.UserCustomActions;
        //    ctx.Load(userCustomActions);
        //    ctx.ExecuteQueryRetry();

        //    for (int i = userCustomActions.Count - 1; i >= 0; i--)
        //    {
        //        if (userCustomActions[i].Name == customActionName)
        //        {
        //            checkIfExists = true;
        //        }
        //    }

        //    if(!checkIfExists)
        //    {
        //        Guid spfxExtension_GlobalHeaderID = new Guid(guid);

        //        UserCustomAction userCustomAction = ctx.Site.UserCustomActions.Add();
        //        userCustomAction.Name = spfxExtName;
        //        userCustomAction.Title = spfxExtTitle;
        //        userCustomAction.Description = spfxExtDescription;
        //        userCustomAction.Location = spfxExtLocation;
        //        userCustomAction.ClientSideComponentId = spfxExtension_GlobalHeaderID;

        //        ctx.Site.Context.ExecuteQueryRetry();
        //    }
        //}

        private static void AddPropBagValuesToIndexedProperties(ClientContext webToPropertyBag)
        {
            List<string> indexedProps = new List<string>();
            IEnumerable<string> indexedKeys = webToPropertyBag.Web.GetIndexedPropertyBagKeys();
            indexedProps = indexedKeys.ToList();
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_SiteId, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_Department, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_EnforceSslComunication, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_JobTitle, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_MetaSiteId, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_Office, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_SendExtendedUserinfo, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_SendUserIdEncoded, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive, webToPropertyBag, indexedProps);
            webToPropertyBag.Web.Update();
            webToPropertyBag.ExecuteQueryRetry();
        }

        private static void CheckIfIsIndexedPropertyAndAdd(string propBagKey, ClientContext webToPropertyBag, List<string> indexedProps)
        {
            if (!indexedProps.Contains(propBagKey))
            {
                webToPropertyBag.Web.AddIndexedPropertyBagKey(propBagKey);
            }
        }

        private static void CreateOrUpdateValueInPropertyBag(string value, ClientContext webToPropertyBag, string propertyBagKey)
        {
            webToPropertyBag.Web.SetPropertyBagValue(propertyBagKey, value);
            webToPropertyBag.Web.Update();
            webToPropertyBag.ExecuteQueryRetry();
        }

        private void SetEnablePropertyBagChange(string siteUrl)
        {
            try
            {
                var authManager = new OfficeDevPnP.Core.AuthenticationManager();
                using (ClientContext tenantCtx = authManager.GetWebLoginClientContext(context.Web.GetTenantAdministrationUrl()))

                //using (ClientContext tenantCtx = context.Clone(context.Web.GetTenantAdministrationUrl()))
                {
                    var tenant = new Tenant(tenantCtx);
                    var siteProperties = tenant.GetSitePropertiesByUrl(siteUrl, true);
                    tenant.Context.Load(siteProperties);
                    tenant.Context.ExecuteQueryRetry();

                    siteProperties.DenyAddAndCustomizePages = DenyAddAndCustomizePagesStatus.Disabled;
                    var operation = siteProperties.Update();
                    tenant.Context.Load(operation, i => i.IsComplete, i => i.PollingInterval);
                    tenant.Context.ExecuteQueryRetry();

                    Logger.WriteLine("\nSet no script on false.");

                    // this is necessary, because the setting is not immediately reflected after ExecuteQuery
                    while (!operation.IsComplete)
                    {
                        Thread.Sleep(operation.PollingInterval);
                        operation.RefreshLoad();
                        if (!operation.IsComplete)
                        {
                            try
                            {
                                tenant.Context.ExecuteQueryRetry();
                            }
                            catch (WebException webEx)
                            {
                                Logger.WriteLine("\nPiwik Set DenyAddAndCustomizePagesStatus inside site: " + webEx.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine("\nPiwik Set DenyAddAndCustomizePagesStatus inside site Full method: "+ ex.Message);
            }
        }

        public bool CheckIfPageIsAlreadyOnPiwik(string idSite)
        {
            PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl);
            return pso.CheckIfPageIsAlreadyOnPiwik(idSite);
        }
    }
}
