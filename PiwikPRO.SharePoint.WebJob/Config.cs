using PiwikPRO.SharePoint.Shared;

namespace PiwikPRO.SharePoint.WebJob
{
    public class Config : IConfiguration
    {
        public string PiwikClientID { get; set; }

        public string PiwikClientSecret { get; set; }

        public string PiwikOldApiToken { get; set; }

        public string PiwikServiceUrl { get; set; }
    }
}
