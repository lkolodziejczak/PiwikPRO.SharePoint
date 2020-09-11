using Microsoft.SharePoint.Administration;
using PiwikPRO.SharePoint.Shared;
using System;
using System.Collections.Generic;

namespace PiwikPRO.SharePoint.SP2013
{
    class Configuration : IConfiguration
    {
        private readonly SPWebApplication webApp;
        private PropertyBagOperations pbo;

        public Configuration()
        {
            this.pbo = new PropertyBagOperations();
        }

        public Configuration(string piwikAdminSiteUrl)
        {
            this.pbo = new PropertyBagOperations(piwikAdminSiteUrl);
        }

        public string ListName => pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_PiwikListName);
        public string PiwikAdminSiteUrl => pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl);
        public string PiwikClientID => pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ClientID);
        public string PiwikClientSecret => pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ClientSecret);
        public string PiwikOldApiToken => pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_OldApiToken);
        public string PiwikServiceUrl => pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_ServiceUrl);
        public List<PropertyBagEntity> PropertyBagList => new List<PropertyBagEntity>(pbo.PropertyBagList);

    }
}
