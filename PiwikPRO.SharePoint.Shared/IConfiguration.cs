using System.Collections.Generic;

namespace PiwikPRO.SharePoint.Shared
{
    public interface IConfiguration
    {
        //string ListUrl { get; }
        string ListName { get; }
        string PiwikAdminSiteUrl { get; }
        string PiwikClientID { get; }
        string PiwikClientSecret { get; }
        string PiwikOldApiToken { get; }
        string PiwikServiceUrl { get; }
        List<PropertyBagEntity> PropertyBagList { get; }
    }
}
