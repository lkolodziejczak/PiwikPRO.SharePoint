using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace PiwikPRO.SharePoint.WebJob
{
    internal class Configuration
    {
        public string ClientId => ConfigurationManager.AppSettings["ClientId"];

        public string PiwikAdminSiteUrl => ConfigurationManager.AppSettings["PiwikAdminSiteUrl"];

        public StoreLocation StoreLocation => Enum.TryParse(ConfigurationManager.AppSettings["StoreLocation"], out StoreLocation value)
            ? value
            : StoreLocation.CurrentUser;

        public StoreName StoreName => Enum.TryParse(ConfigurationManager.AppSettings["StoreName"], out StoreName value)
            ? value
            : StoreName.My;

        public string Tenant => ConfigurationManager.AppSettings["Tenant"];

        public string Thumbprint => ConfigurationManager.AppSettings["Thumbprint"];
    }
}
