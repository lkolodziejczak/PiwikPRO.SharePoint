using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security;
using Microsoft.Online.SharePoint.TenantAdministration;
using System.Threading;
using System.Net;

namespace PiwikPRO.SharePoint.Shared.Helpers
{
    public class PiwikPROJobOperations
    {
        IConfiguration cfg;
        ISPLogger logger;
        public PiwikPROJobOperations(IConfiguration _configuration, ISPLogger _logger)
        {
            this.cfg = _configuration;
            this.logger = _logger;
        }

        public void GetAllNewSitesAndOperate(ClientContext clientContext, string spOnlineUserLogin, string spOnlineUserPassword, string adminTenantUrl)
        {
            try
            {
                ListProcessor sdlo = new ListProcessor(clientContext, cfg);

                //Get all Sites with status "New" and put it to Piwik
                foreach (ListItem item in sdlo.GetAllNewSites())
                {
                    //connect to service and create new site
                    PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl, cfg.PiwikOldApiToken, logger);

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
                            bool? ShouldTrackDocumentAddedGoal = null;
                            bool? ShouldTrackPageAddedGoal = null;
                            bool? ShouldTrackPageEditedGoal = null;
                            ClientContext contextToPropBag;


                            OfficeDevPnP.Core.AuthenticationManager authMan = new OfficeDevPnP.Core.AuthenticationManager();
                            contextToPropBag = authMan.GetAppOnlyAuthenticatedContext(valueUrl.Url, spOnlineUserLogin, spOnlineUserPassword);

                            if (!string.IsNullOrEmpty(spOnlineUserLogin))
                            {
                                contextToPropBag = authMan.GetAppOnlyAuthenticatedContext(valueUrl.Url, spOnlineUserLogin, spOnlineUserPassword);
                                SetEnablePropertyBagChange(adminTenantUrl, valueUrl.Url, spOnlineUserLogin, spOnlineUserPassword, logger);
                            }
                            else
                            {
                                contextToPropBag = new ClientContext(valueUrl.Url);
                            }


                            if (isSiteAlreadyOnPiwik)
                            {
                                //operations if site was active before
                            }
                            else
                            {
                                //copy template values from piwikadmin
                                foreach (PropertyBagEntity pbe in cfg.PropertyBagList)
                                {
                                    if (pbe.PropertyTitle.StartsWith("Template"))
                                    {
                                        CreateOrUpdateValueInPropertyBag(pbe.PropertyValue, contextToPropBag, pbe.PropertyName.Replace("template", ""));
                                    }
                                }
                            }

                            CreateOrUpdateValueInPropertyBag(idSite, contextToPropBag, ConfigValues.PiwikPro_PropertyBag_SiteId);
                            CreateOrUpdateValueInPropertyBag("true", contextToPropBag, ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive);

                            //set gdpr off
                            pso.SetSetGdprOffInPiwik(idSite);

                            //create/update values in propbag
                            PropertyValues currentBag = contextToPropBag.Site.RootWeb.AllProperties;
                            contextToPropBag.Load(currentBag);
                            contextToPropBag.ExecuteQueryRetry();

                            //Prepare goals
                            ShouldTrackDocumentAddedGoal = CheckIfValuesAreIntOrBoolAndReturn(Convert.ToString(currentBag[ConfigValues.PiwikPro_PropertyBag_ShouldTrackDocumentAddedGoal]));
                            ShouldTrackPageAddedGoal = CheckIfValuesAreIntOrBoolAndReturn(Convert.ToString(currentBag[ConfigValues.PiwikPro_PropertyBag_ShouldTrackPageAddedGoal]));
                            ShouldTrackPageEditedGoal = CheckIfValuesAreIntOrBoolAndReturn(Convert.ToString(currentBag[ConfigValues.PiwikPro_PropertyBag_ShouldTrackPageEditedGoal]));


                            //Add goals to Piwik site
                            AddGoalToPiwikAndWriteToPropertyBag(pso, ShouldTrackDocumentAddedGoal, idSite, ConfigValues.PiwikPro_PropertyBag_DocumentAddedGoalId, "Document added", currentBag, contextToPropBag);
                            AddGoalToPiwikAndWriteToPropertyBag(pso, ShouldTrackPageAddedGoal, idSite, ConfigValues.PiwikPro_PropertyBag_PageAddedGoalId, "Page added", currentBag, contextToPropBag);
                            AddGoalToPiwikAndWriteToPropertyBag(pso, ShouldTrackPageEditedGoal, idSite, ConfigValues.PiwikPro_PropertyBag_PageEditedGoalId, "Page edited", currentBag, contextToPropBag);

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
                    clientContext.ExecuteQueryRetry();
                }
            }
            catch (Exception expcc)
            {
                logger.WriteLog(Category.Unexpected, "Piwik GetAllNewSitesAndOperateOnFinish", expcc.Message);
            }
        }

        public void GetAllDeactivatingSitesAndOperate(ClientContext clientContext, string spOnlineUserLogin, string spOnlineUserPassword, string adminTenantUrl)
        {
            try
            {
                ListProcessor sdlo = new ListProcessor(clientContext, cfg);
                foreach (ListItem item in sdlo.GetAllDeactivatingSites())
                {
                    //connect to service and deactivate
                    PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl, cfg.PiwikOldApiToken, logger);
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
                    clientContext.ExecuteQueryRetry();


                    if (!string.IsNullOrEmpty(spOnlineUserLogin))
                    {
                        SetEnablePropertyBagChange(adminTenantUrl, valueUrl.Url, spOnlineUserLogin, spOnlineUserPassword, logger);

                        OfficeDevPnP.Core.AuthenticationManager authMan = new OfficeDevPnP.Core.AuthenticationManager();
                        using (ClientContext contextToPropBag = authMan.GetAppOnlyAuthenticatedContext(valueUrl.Url, spOnlineUserLogin, spOnlineUserPassword))
                        {
                            CreateOrUpdateValueInPropertyBag("false", contextToPropBag, ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive);
                        }
                    }
                    else
                    {
                        using (ClientContext contextToPropBag = new ClientContext(valueUrl.Url))
                        {
                            CreateOrUpdateValueInPropertyBag("false", contextToPropBag, ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive);
                        }

                    }
                }
            }
            catch (Exception expcc)
            {
                logger.WriteLog(Category.Unexpected, "Piwik GetAllNewSitesAndOperateOnFinish", expcc.Message);
            }
        }

        public void GetAllSettingsUpdatedPagesAndOperate(ClientContext clientContext, string spOnlineUserLogin, string spOnlineUserPassword, string adminTenantUrl)
        {
            try
            {
                ListProcessor sdlo = new ListProcessor(clientContext, cfg);
                foreach (ListItem item in sdlo.GetAllSettingsUpdatedSites())
                {
                    //connect to service and deactivate
                    PiwikPROServiceOperations pso = new PiwikPROServiceOperations(cfg.PiwikClientID, cfg.PiwikClientSecret, cfg.PiwikServiceUrl, cfg.PiwikOldApiToken, logger);
                    string idSite = string.Empty;
                    idSite = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]);
                    FieldUrlValue valueUrl = (FieldUrlValue)(item[ConfigValues.PiwikPro_SiteDirectory_Column_Url]);
                    string pwkChangedValues = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_PropChanged]);
                    ClientContext contextToPropBag = new ClientContext(valueUrl.Url);
                    if (!string.IsNullOrEmpty(spOnlineUserLogin))
                    {
                        SetEnablePropertyBagChange(adminTenantUrl, valueUrl.Url, spOnlineUserLogin, spOnlineUserPassword, logger);
                    }
                    PropertyValues currentBag = contextToPropBag.Site.RootWeb.AllProperties;
                    contextToPropBag.Load(currentBag);
                    contextToPropBag.ExecuteQueryRetry();

                    if (!string.IsNullOrEmpty(spOnlineUserLogin))
                    {
                        SetEnablePropertyBagChange(adminTenantUrl, valueUrl.Url, spOnlineUserLogin, spOnlineUserPassword, logger);
                    }

                    foreach (string pwkValue in pwkChangedValues.Split(';'))
                    {
                        if (pwkValue != null)
                        {
                            if (pwkValue.Split('|')[0] == "ShouldTrackDocumentAddedGoal" && pwkValue.Split('|')[1].ToLower() == "true")
                            {
                                AddGoalToPiwikAndWriteToPropertyBag(pso, true, idSite, ConfigValues.PiwikPro_PropertyBag_DocumentAddedGoalId, "Document added", currentBag, contextToPropBag);
                            }
                            if (pwkValue.Split('|')[0] == "ShouldTrackPageAddedGoal" && pwkValue.Split('|')[1].ToLower() == "true")
                            {
                                AddGoalToPiwikAndWriteToPropertyBag(pso, true, idSite, ConfigValues.PiwikPro_PropertyBag_PageAddedGoalId, "Page added", currentBag, contextToPropBag);
                            }
                            if (pwkValue.Split('|')[0] == "ShouldTrackPageEditedGoal" && pwkValue.Split('|')[1].ToLower() == "true")
                            {
                                AddGoalToPiwikAndWriteToPropertyBag(pso, true, idSite, ConfigValues.PiwikPro_PropertyBag_PageEditedGoalId, "Page edited", currentBag, contextToPropBag);
                            }
                        }
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_PropChanged] = "";

                    }
                    if (idSite.Contains("Error: "))
                    {
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Error;
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = idSite;
                    }
                    else
                    {
                        item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Active;
                    }

                    item.Update();
                    clientContext.ExecuteQueryRetry();
                }
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik GetAllSettingsUpdatedPagesAndOperateOnFinish", ex.Message);
            }
        }

        private static void AddPropBagValuesToIndexedProperties(ClientContext webToPropertyBag)
        {
            List<string> indexedProps = new List<string>();
            IEnumerable<string> indexedKeys = webToPropertyBag.Web.GetIndexedPropertyBagKeys();
            indexedProps = indexedKeys.ToList();
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_DocumentAddedGoalId, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_PageAddedGoalId, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_PageEditedGoalId, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_ShouldTrackDocumentAddedGoal, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_ShouldTrackPageAddedGoal, webToPropertyBag, indexedProps);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_ShouldTrackPageEditedGoal, webToPropertyBag, indexedProps);
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
                webToPropertyBag.Web.AddIndexedPropertyBagKey("key");
            }
        }

        private void AddGoalToPiwikAndWriteToPropertyBag(PiwikPROServiceOperations pso, bool? goalTrack, string idSite, string propBagName, string goalAction, PropertyValues currentBag, ClientContext webToPropertyBag)
        {
            try
            {
                int DocumentAddedGoalId = pso.AddGoalToPiwik(idSite, goalAction);
                webToPropertyBag.Web.SetPropertyBagValue(propBagName, Convert.ToString(DocumentAddedGoalId));
                webToPropertyBag.Web.Update();
                webToPropertyBag.ExecuteQueryRetry();
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik AddGoalToPiwikAndWriteToPropertyBag", ex.Message);
            }
        }

        private static void CreateOrUpdateValueInPropertyBag(string value, ClientContext webToPropertyBag, string propertyBagKey)
        {
            webToPropertyBag.Web.SetPropertyBagValue(propertyBagKey, value);
            webToPropertyBag.Web.Update();
            webToPropertyBag.ExecuteQueryRetry();
        }
        private bool? CheckIfValuesAreIntOrBoolAndReturn(string propBagValue)
        {
            if (propBagValue != null)
            {
                if (propBagValue == "0")
                {
                    return false;
                }
                if (propBagValue == "1")
                {
                    return true;
                }
                if (propBagValue == "false")
                {
                    return false;
                }
                if (propBagValue == "true")
                {
                    return true;
                }
            }
            return null;
        }

        private void SetEnablePropertyBagChange(string adminTenantUrl, string propertyBagContextUrl, string userLogin, string userPassword, ISPLogger logger)
        {
            try
            {
                // using (ClientContext tenantCtx = new ClientContext(adminTenantUrl))
                OfficeDevPnP.Core.AuthenticationManager authMan = new OfficeDevPnP.Core.AuthenticationManager();
                using (ClientContext tenantCtx = authMan.GetAppOnlyAuthenticatedContext(adminTenantUrl, userLogin, userPassword))
                {
                    var tenant = new Tenant(tenantCtx);
                    var siteProperties = tenant.GetSitePropertiesByUrl(propertyBagContextUrl, true);
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
    }
}
