using Microsoft.SharePoint.Administration;
using System;

namespace PiwikPRO.SharePoint.Shared.OnPremise
{
    class Configuration : IConfiguration
    {
        private readonly SPWebApplication webApp;
        private PropertyBagOperations pbo;

        public Configuration()
        {
            //this.webApp = webApp;
            this.pbo = new PropertyBagOperations();
        }

       // public string ListUrl => (string)webApp.Properties["ListUrl"];
        public string ListName => pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_PiwikListName);
        public string PiwikAdminSiteUrl => pbo.GetPropertyValueFromListByKey(ConfigValues.PiwikPro_PropertyBag_AdminSiteUrl);
    }
}
