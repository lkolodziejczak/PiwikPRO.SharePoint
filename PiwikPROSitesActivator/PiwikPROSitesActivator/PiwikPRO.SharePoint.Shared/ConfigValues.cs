namespace PiwikPRO.SharePoint.Shared
{
    public class ConfigValues
    {
        public const string PiwikPro_JobName = "Piwik PRO Job";
        public const string PiwikPro_SiteDirectoryListName = "Piwik Pro Site Directory";
        public const string PiwikPro_ConfigListName = "PiwikConfig";

        //property bag keys
        public const string PiwikPro_PropertyBag_SiteId = "piwik_metasitenamestored";
        public const string PiwikPro_PropertyBag_Department = "piwik_senddepartment";
        public const string PiwikPro_PropertyBag_EnforceSslComunication = "piwik_enforcessl";
        public const string PiwikPro_PropertyBag_JobTitle = "piwik_sendjobtitle";
        public const string PiwikPro_PropertyBag_MetaSiteId = "piwik_metasitename";
        public const string PiwikPro_PropertyBag_Office = "piwik_sendoffice";
        public const string PiwikPro_PropertyBag_Username = "piwik_sendusername";
        public const string PiwikPro_PropertyBag_SendExtendedUserinfo = "piwik_senduserextendedinfo";
        public const string PiwikPro_PropertyBag_SendUserIdEncoded = "piwik_senduserencoded";
        public const string PiwikPro_PropertyBag_PiwikIsTrackingActive = "piwik_istrackingactive";
        public const string PiwikPro_PropertyBag_ServiceUrl = "piwik_serviceurl";
        public const string PiwikPro_PropertyBag_ClientID = "piwik_clientid";
        public const string PiwikPro_PropertyBag_ClientSecret = "piwik_clientsecret";
        public const string PiwikPro_PropertyBag_OldApiToken = "piwik_oldapitoken";
        public const string PiwikPro_PropertyBag_PiwikContainersUrl = "piwik_containersurl";
        public const string PiwikPro_PropertyBag_Sha3 = "piwik_sha3";

        //property bag template keys
        public const string PiwikPro_PropertyBag_TemplateDepartment = "piwik_templatesenddepartment";
        public const string PiwikPro_PropertyBag_TemplateJobTitle = "piwik_templatesendjobtitle";
        public const string PiwikPro_PropertyBag_TemplateUsername = "piwik_templatesendusername";
        public const string PiwikPro_PropertyBag_TemplateOffice = "piwik_templatesendoffice";
        public const string PiwikPro_PropertyBag_TemplateSendExtendedUserinfo = "piwik_templatesenduserextendedinfo";
        public const string PiwikPro_PropertyBag_TemplateSendUserIdEncoded = "piwik_templatesenduserencoded";

        //list site directory fields
        public const string PiwikPro_SiteDirectory_Column_Title = "Title";
        public const string PiwikPro_SiteDirectory_Column_Status = "pwk_status";
        public const string PiwikPro_SiteDirectory_Column_Url = "pwk_url";
        public const string PiwikPro_SiteDirectory_Column_ErrorLog = "pwk_errormessage";
        public const string PiwikPro_SiteDirectory_Column_SiteID = "pwk_siteId";
        public const string PiwikPro_SiteDirectory_Column_PropChanged = "pwk_propchanged";

        //field status values
        public const string PiwikPro_SiteDirectory_Column_Status_Active = "Active";
        public const string PiwikPro_SiteDirectory_Column_Status_New = "New";
        public const string PiwikPro_SiteDirectory_Column_Status_NoActive = "No active";
        public const string PiwikPro_SiteDirectory_Column_Status_Deactivating = "Deactivating";
        public const string PiwikPro_SiteDirectory_Column_Status_Error = "Error";
        public const string PiwikPro_SiteDirectory_Column_Status_SettingsUpdated = "Settings updated";
        public const string PiwikPro_SiteDirectory_Column_Status_Deleted = "Deleted";

        //old - not used
        public const string PiwikPro_PropertyBag_AdminSiteUrl = "piwik_adminsiteurl";
        public const string PiwikPro_PropertyBag_PiwikListName = "piwik_listname";
        public const string PiwikPro_PropertyBag_TrackerJSScriptUrl = "piwik_trackerjsscripturl";
        public const string PiwikPro_PropertyBag_TemplateShouldTrackDocumentAddedGoal = "piwik_templateusegoaldocumentadded";
        public const string PiwikPro_PropertyBag_TemplateShouldTrackPageAddedGoal = "piwik_templateusegoalpageadded";
        public const string PiwikPro_PropertyBag_TemplateShouldTrackPageEditedGoal = "piwik_templateusegoalpageedited";
    }
}
