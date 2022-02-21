using PiwikPROSitesActivator.Shared;

namespace PiwikPROSitesActivator
{
    public class Config : IConfiguration
    {
        public string PiwikClientID { get; set; }

        public string PiwikClientSecret { get; set; }

        public string PiwikServiceUrl { get; set; }
    }
}
