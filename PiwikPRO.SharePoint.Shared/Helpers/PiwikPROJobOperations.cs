using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace PiwikPRO.SharePoint.Shared.Helpers
{
    public class PiwikPROJobOperations
    {
        private readonly ClientContext context;
        private readonly IConfiguration cfg;
        private readonly ISPLogger logger;

        public PiwikPROJobOperations(ClientContext context, IConfiguration configuration, ISPLogger logger)
        {
            this.context = context;
            this.cfg = configuration;
            this.logger = logger;
        }

        public void GetAllSettingsUpdatedSitesAndOperate()
        {
            try
            {
                ListProcessor sdlo = new ListProcessor(context, logger);

                //Get all Sites with status "Settings updated" and put it to Piwik
                foreach (ListItem item in sdlo.GetAllNewSettingsUpdatedSites())
                {
                    //connect to service and create new site
                    PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl, logger);

                    FieldUrlValue valueUrl = (FieldUrlValue)(item[ConfigValues.PiwikPro_SiteDirectory_Column_Url]);

                    string idSite = string.Empty;
                    idSite = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]);
                    if (!string.IsNullOrEmpty(idSite))
                    {
                        try
                        {
                            //add tag manager json
                            pso.AddTagManagerJSONFile(idSite, context);

                            //publish tag manager
                            pso.PublishLastVersionOfTagManager(idSite);
                        }
                        catch (Exception exp)
                        {
                            item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = exp.Message;
                            logger.WriteLog(Category.Unexpected, "Piwik GetAllNewSettingsUpdatedSites Inside", exp.Message);
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
                logger.WriteLog(Category.Unexpected, "Piwik GetAllSettingsUpdatedSitesAndOperate", expcc.Message);
            }
        }

        public void GetAllNewSitesAndOperate()
        {
            try
            {
                ListProcessor sdlo = new ListProcessor(context, logger);

                //Get all Sites with status "New" and put it to Piwik
                foreach (ListItem item in sdlo.GetAllNewSites())
                {
                    //connect to service and create new site
                    PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl, logger);

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
                    }
                    else
                    {
                        idSite = pso.AddSiteToPiwik(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_Title]), valueUrl.Url);
                    }

                    if (idSite.Contains("Error: "))
                    {
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Error;
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = idSite;
                    }
                    else
                    {
                        try
                        {
                            ClientContext contextToPropBag = context.Clone(valueUrl.Url);

                            try
                            {
                                SetEnablePropertyBagChange(valueUrl.Url, logger);
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

                            //add tag manager json
                            pso.AddTagManagerJSONFile(idSite, context);

                            //publish tag manager
                            pso.PublishLastVersionOfTagManager(idSite);

                            //create/update values in propbag
                            PropertyValues currentBag = contextToPropBag.Site.RootWeb.AllProperties;
                            contextToPropBag.Load(currentBag);
                            contextToPropBag.ExecuteQueryRetry();

                            AddPropBagValuesToIndexedProperties(contextToPropBag);
                        }
                        catch (Exception exp)
                        {
                            item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = exp.Message;
                            logger.WriteLog(Category.Unexpected, "Piwik GetAllNewSitesAndOperate", exp.Message);
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
                logger.WriteLog(Category.Unexpected, "Piwik GetAllNewSitesAndOperateOnFinish", expcc.Message);
            }
        }

        public void GetAllDeactivatingSitesAndOperate()
        {
            try
            {
                ListProcessor sdlo = new ListProcessor(context, logger);
                foreach (ListItem item in sdlo.GetAllDeactivatingSites())
                {
                    //connect to service and deactivate
                    PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl, logger);
                    string idSite = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]);
                    FieldUrlValue valueUrl = (FieldUrlValue)(item[ConfigValues.PiwikPro_SiteDirectory_Column_Url]);
                    //idSite = pso.RemoveSiteFromPiwik(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]));
                    pso.ChangeNameSiteInPiwik("Inactive - " + Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_Title]), idSite);
                    if (idSite.Contains("Error: "))
                    {
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Error;
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = idSite;
                    }
                    else
                    {
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_NoActive;
                    }

                    item.Update();
                    context.ExecuteQueryRetry();

                    using (ClientContext contextToPropBag = context.Clone(valueUrl.Url))
                    {
                        CreateOrUpdateValueInPropertyBag("false", contextToPropBag, ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive);
                    }
                }
            }
            catch (Exception expcc)
            {
                logger.WriteLog(Category.Unexpected, "Piwik GetAllNewSitesAndOperateOnFinish", expcc.Message);
            }
        }

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

        private void SetEnablePropertyBagChange(string siteUrl, ISPLogger logger)
        {
            try
            {
                using (ClientContext tenantCtx = context.Clone(context.Web.GetTenantAdministrationUrl()))
                {
                    var tenant = new Tenant(tenantCtx);
                    var siteProperties = tenant.GetSitePropertiesByUrl(siteUrl, true);
                    tenant.Context.Load(siteProperties);
                    tenant.Context.ExecuteQueryRetry();

                    siteProperties.DenyAddAndCustomizePages = DenyAddAndCustomizePagesStatus.Disabled;
                    var operation = siteProperties.Update();
                    tenant.Context.Load(operation, i => i.IsComplete, i => i.PollingInterval);
                    tenant.Context.ExecuteQueryRetry();

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
                                logger.WriteLog(Category.Unexpected, "Piwik Set DenyAddAndCustomizePagesStatus inside site", webEx.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik Set DenyAddAndCustomizePagesStatus inside site Full method", ex.Message);
            }
        }

        public bool CheckIfPageIsAlreadyOnPiwik(string idSite)
        {
            PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl, logger);
            return pso.CheckIfPageIsAlreadyOnPiwik(idSite);
        }
    }
}
