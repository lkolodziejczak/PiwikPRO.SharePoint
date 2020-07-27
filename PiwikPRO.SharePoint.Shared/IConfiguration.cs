namespace PiwikPRO.SharePoint.Shared
{
    public interface IConfiguration
    {
        string PiwikClientID { get; }
        string PiwikClientSecret { get; }
        string PiwikOldApiToken { get; }
        string PiwikServiceUrl { get; }
    }
}
