using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.Shared.OnPremise
{
    class PiwikPROJSProvisioningJob : SPJobDefinition
    {
        public PiwikPROJSProvisioningJob(): base() { }
        public PiwikPROJSProvisioningJob(string jobName, SPService service, SPServer server, SPJobLockType targetType): base(jobName, service, server, targetType) { }
        public PiwikPROJSProvisioningJob(string jobName, SPWebApplication webApplication): base(jobName, webApplication, null, SPJobLockType.ContentDatabase) {
            this.Title = jobName;
        }
        public override void Execute(Guid contentDbId)
        {
            try {
                PropertyBagOperations pbo = new PropertyBagOperations();
                    // using (SPWeb web = new SPSite(ConfigValues.PiwikPro_PiwikAdminSiteUrl).OpenWeb())
                    // using (SPWeb web = new SPSite(pbo.GetPropertyValueFromListByKey("piwik_adminsiteurl")).OpenWeb()) 
                    // {
                    Configuration cfg = new Configuration();
                    ClientContext clientContext = new ClientContext(pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ServiceUrl));
                    ListProcessor sdlo = new ListProcessor(clientContext, cfg);

                        //Get all Sites with status "New" and put it to Piwik
                        foreach (ListItem item in sdlo.GetAllNewSites())
                        {
                            //connect to service and create new site
                            PiwikPROServiceOperations pso = new PiwikPROServiceOperations(pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ClientID),
                                pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ClientSecret),
                                pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ServiceUrl),
                                pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_OldApiToken));
                            SPFieldUrlValue valueUrl = new SPFieldUrlValue(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_Url]));
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

                                    using (SPWeb webToPropertyBag = new SPSite(valueUrl.Url).OpenWeb())
                                    {
                                        bool oldAllowUnsafeUpdates = webToPropertyBag.AllowUnsafeUpdates;
                                        webToPropertyBag.AllowUnsafeUpdates = true;

                                        //create/update values in propbag
                                        SPPropertyBag currentBag = webToPropertyBag.Properties;
                                        if (isSiteAlreadyOnPiwik)
                                        {
                                            //operations if site was active before
                                        }
                                        else
                                        {
                                            //copy template values from piwikadmin
                                            foreach (PropertyBagEntity pbe in pbo.PropertyBagList)
                                            {
                                                if(pbe.PropertyTitle.StartsWith("Template"))
                                                {
                                                    CreateOrUpdateValueInPropertyBag(pbe.PropertyValue, webToPropertyBag, currentBag, pbe.PropertyName.Replace("template",""));
                                                }
                                            }
                                        }

                                        CreateOrUpdateValueInPropertyBag(idSite, webToPropertyBag, currentBag, ConfigValues.PiwikPro_PropertyBag_SiteId);
                                        CreateOrUpdateValueInPropertyBag("true", webToPropertyBag, currentBag, ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive);

                                        //set gdpr off
                                        pso.SetSetGdprOffInPiwik(idSite);

                                        //Prepare goals
                                        ShouldTrackDocumentAddedGoal = CheckIfValuesAreIntOrBoolAndReturn(currentBag[ConfigValues.PiwikPro_PropertyBag_ShouldTrackDocumentAddedGoal]);
                                        ShouldTrackPageAddedGoal = CheckIfValuesAreIntOrBoolAndReturn(currentBag[ConfigValues.PiwikPro_PropertyBag_ShouldTrackPageAddedGoal]);
                                        ShouldTrackPageEditedGoal = CheckIfValuesAreIntOrBoolAndReturn(currentBag[ConfigValues.PiwikPro_PropertyBag_ShouldTrackPageEditedGoal]);


                                        //Add goals to Piwik site
                                        AddGoalToPiwikAndWriteToPropertyBag(pso, ShouldTrackDocumentAddedGoal, idSite, ConfigValues.PiwikPro_PropertyBag_DocumentAddedGoalId, "Document added", currentBag, webToPropertyBag);
                                        AddGoalToPiwikAndWriteToPropertyBag(pso, ShouldTrackPageAddedGoal, idSite, ConfigValues.PiwikPro_PropertyBag_PageAddedGoalId, "Page added", currentBag, webToPropertyBag);
                                        AddGoalToPiwikAndWriteToPropertyBag(pso, ShouldTrackPageEditedGoal, idSite, ConfigValues.PiwikPro_PropertyBag_PageEditedGoalId, "Page edited", currentBag, webToPropertyBag);

                                        AddPropBagValuesToIndexedProperties(webToPropertyBag);

                                        webToPropertyBag.AllowUnsafeUpdates = oldAllowUnsafeUpdates;
                                    }
                                }
                                catch(Exception exp)
                                {
                                    item[ConfigValues.PiwikPro_SiteDirectory_Column_ErrorLog] = exp.Message;
                                }

                                item[ConfigValues.PiwikPro_SiteDirectory_Column_Status] = ConfigValues.PiwikPro_SiteDirectory_Column_Status_Active;
                                item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID] = idSite;
                            }

                            item.Update();
                            //PiwikPROServiceOperations pso = new PiwikPROServiceOperations();
                            // pso.RemoveSiteFromPiwik("4206f53d-c12a-4d63-a87c-677760ea58d8");
                        }

                        //Get all sites with status "Deactivating" and remove from Piwik
                        foreach (ListItem item in sdlo.GetAllDeactivatingSites())
                        {
                            //connect to service and deactivate
                            PiwikPROServiceOperations pso = new PiwikPROServiceOperations();
                            string idSite = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]);
                            SPFieldUrlValue valueUrl = new SPFieldUrlValue(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_Url]));
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

                            using (SPWeb webToPropertyBag = new SPSite(valueUrl.Url).OpenWeb())
                            {
                                bool oldAllowUnsafeUpdates = webToPropertyBag.AllowUnsafeUpdates;
                                webToPropertyBag.AllowUnsafeUpdates = true;

                                //create/update values in propbag
                                SPPropertyBag currentBag = webToPropertyBag.Properties;
                                CreateOrUpdateValueInPropertyBag("false", webToPropertyBag, currentBag, ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive);

                                webToPropertyBag.AllowUnsafeUpdates = oldAllowUnsafeUpdates;
                            }

                        }

                        //Get all sites with status "Settings updated" and update goals in Piwik
                        foreach (ListItem item in sdlo.GetAllSettingsUpdatedSites())
                        {
                            //connect to service and deactivate
                            PiwikPROServiceOperations pso = new PiwikPROServiceOperations();
                            string idSite = string.Empty;
                            idSite = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]);
                            SPFieldUrlValue valueUrl = new SPFieldUrlValue(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_Url]));
                            string pwkChangedValues = Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_PropChanged]);
                            using (SPWeb webToPropertyBag = new SPSite(valueUrl.Url).OpenWeb())
                            {
                                SPPropertyBag currentBag = webToPropertyBag.Properties;

                                foreach (string pwkValue in pwkChangedValues.Split(';'))
                                {
                                    if(pwkValue != null)
                                    {
                                        if(pwkValue.Split('|')[0]== "ShouldTrackDocumentAddedGoal" && pwkValue.Split('|')[1].ToLower() == "true")
                                        {
                                                AddGoalToPiwikAndWriteToPropertyBag(pso, true, idSite, ConfigValues.PiwikPro_PropertyBag_DocumentAddedGoalId, "Document added", currentBag, webToPropertyBag);
                                        }
                                        if (pwkValue.Split('|')[0] == "ShouldTrackPageAddedGoal" && pwkValue.Split('|')[1].ToLower() == "true")
                                        {
                                                AddGoalToPiwikAndWriteToPropertyBag(pso, true, idSite, ConfigValues.PiwikPro_PropertyBag_PageAddedGoalId, "Page added", currentBag, webToPropertyBag);
                                        }
                                        if (pwkValue.Split('|')[0] == "ShouldTrackPageEditedGoal" && pwkValue.Split('|')[1].ToLower() == "true")
                                        {
                                                AddGoalToPiwikAndWriteToPropertyBag(pso, true, idSite, ConfigValues.PiwikPro_PropertyBag_PageEditedGoalId, "Page edited", currentBag, webToPropertyBag);
                                        }
                                    }
                                }
                                item[ConfigValues.PiwikPro_SiteDirectory_Column_PropChanged] = "";

                                //pso.UpdateGoalToPiwik(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]), "Document added", currentBag[ConfigValues.PiwikPro_PropertyBag_DocumentAddedGoalId], CheckIfValuesAreIntOrBoolAndReturn(currentBag[ConfigValues.PiwikPro_PropertyBag_ShouldTrackDocumentAddedGoal]));
                                //pso.UpdateGoalToPiwik(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]), "Page added", currentBag[ConfigValues.PiwikPro_PropertyBag_PageAddedGoalId], CheckIfValuesAreIntOrBoolAndReturn(currentBag[ConfigValues.PiwikPro_PropertyBag_ShouldTrackPageAddedGoal]));
                                //pso.UpdateGoalToPiwik(Convert.ToString(item[ConfigValues.PiwikPro_SiteDirectory_Column_SiteID]), "Page edited", currentBag[ConfigValues.PiwikPro_PropertyBag_PageEditedGoalId], CheckIfValuesAreIntOrBoolAndReturn(currentBag[ConfigValues.PiwikPro_PropertyBag_ShouldTrackPageEditedGoal]));

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
                        }


                    //}

            } catch (Exception ex) {
               // Logger.WriteLog(Logger.Category.Unexpected, "Piwik Execute TimerJob", ex.Message);
            }
        }

        private static void AddPropBagValuesToIndexedProperties(SPWeb webToPropertyBag)
        {
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_DocumentAddedGoalId, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_PageAddedGoalId, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_PageEditedGoalId, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_ShouldTrackDocumentAddedGoal, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_ShouldTrackPageAddedGoal, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_ShouldTrackPageEditedGoal, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_SiteId, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_Department, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_EnforceSslComunication, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_JobTitle, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_MetaSiteId, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_Office, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_SendExtendedUserinfo, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_SendUserIdEncoded, webToPropertyBag);
            CheckIfIsIndexedPropertyAndAdd(ConfigValues.PiwikPro_PropertyBag_PiwikIsTrackingActive, webToPropertyBag);
            webToPropertyBag.Update();
        }

        private static void CheckIfIsPropertyBagExistsAndAdd(string propBagKey, string value, SPWeb webToPropertyBag)
        {
            if (!webToPropertyBag.Properties.ContainsKey(propBagKey))
            {
                webToPropertyBag.Properties.Add(propBagKey, value);
            }
        }

        private static void CheckIfIsIndexedPropertyAndAdd(string propBagKey, SPWeb webToPropertyBag)
        {
            if (!webToPropertyBag.IndexedPropertyKeys.Contains(propBagKey))
            {
                webToPropertyBag.IndexedPropertyKeys.Add(propBagKey);
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

        private void AddGoalToPiwikAndWriteToPropertyBag(PiwikPROServiceOperations pso, bool? goalTrack, string idSite, string propBagName, string goalAction, SPPropertyBag currentBag, SPWeb webToPropertyBag)
        {
            try
            {
               // if (goalTrack != null && goalTrack == true)
               // {
                   // int DocumentAddedGoalId = pso.AddGoalToPiwik(idSite, goalAction);

                    if (currentBag.ContainsKey(propBagName) && currentBag[propBagName] != null)
                    {
                        if(String.IsNullOrEmpty(currentBag[propBagName]))
                        {
                            int DocumentAddedGoalId = pso.AddGoalToPiwik(idSite, goalAction);
                            currentBag[propBagName] = Convert.ToString(DocumentAddedGoalId);
                            currentBag.Update();
                        }
                       // currentBag[propBagName] = Convert.ToString(DocumentAddedGoalId);
                       // currentBag.Update();
                       // webToPropertyBag.Properties.Update();
                       // webToPropertyBag.Update();
                    }
                    else
                    {
                        int DocumentAddedGoalId = pso.AddGoalToPiwik(idSite, goalAction);
                        currentBag.Add(propBagName, Convert.ToString(DocumentAddedGoalId));
                        currentBag.Update();
                    }
               // }
            }
            catch(Exception ex)
            {
                //Logger.WriteLog(Logger.Category.Unexpected, "Piwik AddGoalToPiwikAndWriteToPropertyBag", ex.Message);
            }
        }

        private bool? CheckIfValuesAreIntOrBoolAndReturn(string propBagValue)
        {
            if (propBagValue != null)
            {
                if(propBagValue == "0")
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
    }
}
