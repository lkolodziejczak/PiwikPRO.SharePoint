using System.Collections.Generic;
using System.Configuration;
using System.Security;
using Microsoft.SharePoint.Client;
using PiwikPRO.SharePoint.Shared;

namespace PiwikPRO.SharePoint.WebJob
{
    public class Configuration : Shared.IConfiguration
    {
        private PropertyBagOperations pbo;

        public Configuration(ISPLogger _logger, ClientContext _ctx)
        {
            //this.webApp = webApp;
            this.pbo = new PropertyBagOperations(_logger, _ctx);
        }

        public string ListName => ConfigurationManager.AppSettings["PiwikListName"];

        public string PiwikAdminSiteUrl => ConfigurationManager.AppSettings["PiwikAdminSiteUrl"];

        public string PiwikClientID => ConfigurationManager.AppSettings["PiwikClientID"];

        public string PiwikClientSecret => ConfigurationManager.AppSettings["PiwikClientSecret"];

        public string PiwikOldApiToken => ConfigurationManager.AppSettings["PiwikOldApiToken"];

        public string PiwikServiceUrl => ConfigurationManager.AppSettings["PiwikServiceUrl"];

        public List<PropertyBagEntity> PropertyBagList => new List<PropertyBagEntity>(pbo.PropertyBagList);
    }
}
