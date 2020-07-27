using Microsoft.SharePoint.Client;
using PiwikPRO.SharePoint.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PiwikPRO.SharePoint.WebJob
{
    public class PropertyBagOperations
    {
        public List<PropertyBagEntity> PropertyBagList;
        ISPLogger logger;
        ClientContext ctx;
        public PropertyBagOperations(ISPLogger _logger, ClientContext _ctx)
        {
            logger = _logger;
            ctx = _ctx;
            if (PropertyBagList == null)
            {
                PropertyBagList = new List<PropertyBagEntity>();
                GetPiwikAdminProperties();
            }
        }
        private void GetPiwikAdminProperties()
        {
            try
            {
                PropertyValues adminBag = ctx.Site.RootWeb.AllProperties;
                ctx.Load(adminBag);
                ctx.ExecuteQueryRetry();
                PropertyBagList.Add(new PropertyBagEntity("EnforceSslComunication", ConfigValues.PiwikPro_PropertyBag_EnforceSslComunication, Convert.ToString(adminBag[ConfigValues.PiwikPro_PropertyBag_EnforceSslComunication]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                PropertyBagList.Add(new PropertyBagEntity("Url", ConfigValues.PiwikPro_PropertyBag_ServiceUrl, Convert.ToString(adminBag[ConfigValues.PiwikPro_PropertyBag_ServiceUrl]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                PropertyBagList.Add(new PropertyBagEntity("ContainersUrl", ConfigValues.PiwikPro_PropertyBag_PiwikContainersUrl, Convert.ToString(adminBag[ConfigValues.PiwikPro_PropertyBag_PiwikContainersUrl]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                PropertyBagList.Add(new PropertyBagEntity("UseSha3", ConfigValues.PiwikPro_PropertyBag_Sha3, Convert.ToString(adminBag[ConfigValues.PiwikPro_PropertyBag_Sha3]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));

                PropertyBagList.Add(new PropertyBagEntity("TemplateSendUserIdEncoded", ConfigValues.PiwikPro_PropertyBag_TemplateSendUserIdEncoded, Convert.ToString(adminBag[ConfigValues.PiwikPro_PropertyBag_TemplateSendUserIdEncoded]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                PropertyBagList.Add(new PropertyBagEntity("TemplateDepartment", ConfigValues.PiwikPro_PropertyBag_TemplateDepartment, Convert.ToString(adminBag[ConfigValues.PiwikPro_PropertyBag_TemplateDepartment]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                PropertyBagList.Add(new PropertyBagEntity("TemplateJobTitle", ConfigValues.PiwikPro_PropertyBag_TemplateJobTitle, Convert.ToString(adminBag[ConfigValues.PiwikPro_PropertyBag_TemplateJobTitle]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                PropertyBagList.Add(new PropertyBagEntity("TemplateUserName", ConfigValues.PiwikPro_PropertyBag_TemplateUsername, Convert.ToString(adminBag[ConfigValues.PiwikPro_PropertyBag_TemplateUsername]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                PropertyBagList.Add(new PropertyBagEntity("TemplateOffice", ConfigValues.PiwikPro_PropertyBag_TemplateOffice, Convert.ToString(adminBag[ConfigValues.PiwikPro_PropertyBag_TemplateOffice]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
                PropertyBagList.Add(new PropertyBagEntity("TemplateSendExtendedUserinfo", ConfigValues.PiwikPro_PropertyBag_TemplateSendExtendedUserinfo, Convert.ToString(adminBag[ConfigValues.PiwikPro_PropertyBag_TemplateSendExtendedUserinfo]), PropertyBagEntity.PropertyLevelEnum.PiwikAdmin));
            }
            catch (Exception ex)
            {
                logger.WriteLog(Category.Unexpected, "Piwik GetPiwikAdminProperties", ex.Message);
            }
        }

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
