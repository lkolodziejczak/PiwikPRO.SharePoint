using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client;
using PiwikPRO.SharePoint.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiwikPRO.SharePoint.SP2013
{
    public class PropertyBagOperations
    {
        public List<PropertyBagEntity> PropertyBagList;

        public PropertyBagOperations()
        {
            if (PropertyBagList == null)
            {
                PropertyBagList = new List<PropertyBagEntity>();
                //GetFarmProperties();
                //GetPiwikAdminProperties();
                GetPiwikAdminListProperties();
            }
            //GetSiteCollectionProperties();
        }

        public PropertyBagOperations(string piwikAdminSiteUrl)
        {
            if (PropertyBagList == null)
            {
                PropertyBagList = new List<PropertyBagEntity>();
                PropertyBagList.Add(new PropertyBagEntity("PiwikAdminSiteUrl", ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl, piwikAdminSiteUrl, PropertyBagEntity.PropertyLevelEnum.Farm));

                //GetFarmProperties();
                GetPiwikAdminListProperties();
                //GetPiwikAdminProperties();
            }
            //GetSiteCollectionProperties();
        }

        private void GetJobProperties()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    //PropertyBagList.Add(new PropertyBagEntity("PiwikAdminSiteUrl", ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl, Convert.ToString(this.Properties[ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl]), PropertyBagEntity.PropertyLevelEnum.Farm));
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.Category.Unexpected, "Piwik GetFarmProperties", ex.Message);
                }
            });
        }

        private void GetFarmProperties()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    SPFarm farm = SPFarm.Local;
                    PropertyBagList.Add(new PropertyBagEntity("ClientID", ConfigValues.PiwikPro_PropertyBag_ClientID, Convert.ToString(farm.Properties[ConfigValues.PiwikPro_PropertyBag_ClientID]), PropertyBagEntity.PropertyLevelEnum.Farm));
                    PropertyBagList.Add(new PropertyBagEntity("ClientSecret", ConfigValues.PiwikPro_PropertyBag_ClientSecret, Convert.ToString(farm.Properties[ConfigValues.PiwikPro_PropertyBag_ClientSecret]), PropertyBagEntity.PropertyLevelEnum.Farm));
                    PropertyBagList.Add(new PropertyBagEntity("OldApiToken", ConfigValues.PiwikPro_PropertyBag_OldApiToken, Convert.ToString(farm.Properties[ConfigValues.PiwikPro_PropertyBag_OldApiToken]), PropertyBagEntity.PropertyLevelEnum.Farm));
                    PropertyBagList.Add(new PropertyBagEntity("PiwikAdminSiteUrl", ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl, Convert.ToString(farm.Properties[ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl]), PropertyBagEntity.PropertyLevelEnum.Farm));
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.Category.Unexpected, "Piwik GetFarmProperties", ex.Message);
                }
            });
        }
        private void GetPiwikAdminProperties()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(GetPropertyValueFromListByKey("piwik_adminsiteurl")))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        try
                        {
                            PropertyBagList.Add(new PropertyBagEntity("PiwikPro Site Directory Listname", ConfigValues.PiwikPro_PropertyBag_PiwikListName, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_PiwikListName]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("EnforceSslComunication", ConfigValues.PiwikPro_PropertyBag_EnforceSslComunication, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_EnforceSslComunication]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("Url", ConfigValues.PiwikPro_PropertyBag_ServiceUrl, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_ServiceUrl]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("ContainersUrl", ConfigValues.PiwikPro_PropertyBag_PiwikContainersUrl, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_PiwikContainersUrl]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("TrackerJSScriptURL", ConfigValues.PiwikPro_PropertyBag_TrackerJSScriptUrl, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_TrackerJSScriptUrl]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("UseSha3", ConfigValues.PiwikPro_PropertyBag_Sha3, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_Sha3]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));

                            PropertyBagList.Add(new PropertyBagEntity("TemplateSendUserIdEncoded", ConfigValues.PiwikPro_PropertyBag_TemplateSendUserIdEncoded, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_TemplateSendUserIdEncoded]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("TemplateShouldTrackDocumentAddedGoal", ConfigValues.PiwikPro_PropertyBag_TemplateShouldTrackDocumentAddedGoal, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_TemplateShouldTrackDocumentAddedGoal]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("TemplateShouldTrackPageAddedGoal", ConfigValues.PiwikPro_PropertyBag_TemplateShouldTrackPageAddedGoal, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_TemplateShouldTrackPageAddedGoal]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("TemplateShouldTrackPageEditedGoal", ConfigValues.PiwikPro_PropertyBag_TemplateShouldTrackPageEditedGoal, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_TemplateShouldTrackPageEditedGoal]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("TemplateDepartment", ConfigValues.PiwikPro_PropertyBag_TemplateDepartment, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_TemplateDepartment]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("TemplateJobTitle", ConfigValues.PiwikPro_PropertyBag_TemplateJobTitle, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_TemplateJobTitle]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("TemplateUserName", ConfigValues.PiwikPro_PropertyBag_TemplateUsername, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_TemplateUsername]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("TemplateOffice", ConfigValues.PiwikPro_PropertyBag_TemplateOffice, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_TemplateOffice]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                            PropertyBagList.Add(new PropertyBagEntity("TemplateSendExtendedUserinfo", ConfigValues.PiwikPro_PropertyBag_TemplateSendExtendedUserinfo, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_TemplateSendExtendedUserinfo]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog(Logger.Category.Unexpected, "Piwik GetPiwikAdminProperties", ex.Message);
                        }
                    }
                }
            });
        }

        private void GetPiwikAdminListProperties()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(GetPropertyValueFromListByKey("piwik_adminsiteurl")))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                            try
                            {
                                SPList oList = web.Lists.TryGetList(ConfigValues.PiwikPro_ConfigListName);
                            SPListItemCollection collListItems = oList.GetItems();

                            foreach(SPListItem item in collListItems)
                            {
                                if(item["Title"].Equals(ConfigValues.PiwikPro_PropertyBag_ClientID))
                                {
                                    PropertyBagList.Add(new PropertyBagEntity("ClientID", ConfigValues.PiwikPro_PropertyBag_ClientID, Convert.ToString(item["Value"]), PropertyBagEntity.PropertyLevelEnum.Farm));
                                }
                                if (item["Title"].Equals(ConfigValues.PiwikPro_PropertyBag_ClientSecret))
                                {
                                    PropertyBagList.Add(new PropertyBagEntity("ClientSecret", ConfigValues.PiwikPro_PropertyBag_ClientSecret, Convert.ToString(item["Value"]), PropertyBagEntity.PropertyLevelEnum.Farm));
                                }
                                if (item["Title"].Equals(ConfigValues.PiwikPro_PropertyBag_OldApiToken))
                                {
                                    PropertyBagList.Add(new PropertyBagEntity("OldApiToken", ConfigValues.PiwikPro_PropertyBag_OldApiToken, Convert.ToString(item["Value"]), PropertyBagEntity.PropertyLevelEnum.Farm));
                                }
                                   //PropertyBagList.Add(new PropertyBagEntity("ServiceUrl", ConfigValues.PiwikPro_PropertyBag_ServiceUrl, Convert.ToString(item["Value"]), PropertyBagEntity.PropertyLevelEnum.Farm));
                            }
                            PropertyBagList.Add(new PropertyBagEntity("ServiceUrl", ConfigValues.PiwikPro_PropertyBag_ServiceUrl, Convert.ToString(web.Properties[ConfigValues.PiwikPro_PropertyBag_ServiceUrl]), PropertyBagEntity.PropertyLevelEnum.Farm));
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog(Logger.Category.Unexpected, "Piwik GetPiwikAdminListProperties", ex.Message);
                        }
                    }
                }
            });
        }

        //private static void GetSiteCollectionProperties()
        //{

        //    //fill values of properties levels etc
        //    PropertyBagList.Add(new PropertyBagEntity("Department", "piwik_senddepartment", "true", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("DocumentAddedGoalId", "piwik_goaldocumentaddedid", "", PropertyBagEntity.PropertyLevelEnum.SiteCollection));

        //    // PropertyBagList.Add(new PropertyBagEntity("IsConnectedToMetaSite", "piwik_isconnectedtometasite", "", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("JobTitle", "piwik_sendjobtitle", "true", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("MetaSiteId", "piwik_metasitename", "", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("Office", "piwik_sendoffice", "true", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("PageAddedGoalId", "piwik_goalpageaddedid", "", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("PageEditedGoalId", "piwik_goalpageeditedid", "", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("SendExtendedUserinfo", "piwik_senduserextendedinfo", "true", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("SendUserIdEncoded", "piwik_senduserencoded", "true", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("ShouldTrackDocumentAddedGoal", "piwik_usegoaldocumentadded", "true", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("ShouldTrackPageAddedGoal", "piwik_usegoalpageadded", "true", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("ShouldTrackPageEditedGoal", "piwik_usegoalpageedited", "true", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("SiteId", "piwik_metasitenamestored", "", PropertyBagEntity.PropertyLevelEnum.SiteCollection));

        //    PropertyBagList.Add(new PropertyBagEntity("UserName", "piwik_sendusername", "true", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //    PropertyBagList.Add(new PropertyBagEntity("IsTrackingActive", "piwik_istrackingactive", "false", PropertyBagEntity.PropertyLevelEnum.SiteCollection));
        //}

        public string GetPropertyValueFromListByKey(string propertyKey)
        {
            return Convert.ToString(PropertyBagList.Where(x => x.PropertyName == propertyKey).FirstOrDefault().PropertyValue);
        }

        public string GetPropertyValueFromListByTitle(string propertyTitle)
        {
            return Convert.ToString(PropertyBagList.Where(x => x.PropertyTitle == propertyTitle).FirstOrDefault().PropertyValue);
        }
    }
}
