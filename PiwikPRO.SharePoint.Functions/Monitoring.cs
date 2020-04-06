using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace PiwikPRO.SharePoint.Functions
{
    public class Monitoring
    {
        public string DiagnosticAreaName = "PiwikPROConfigServer";
        public void WriteLog(string eventName, string source, string errorMessage)
        {
            TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;

            var tc = new TelemetryClient();
            tc.TrackRequest(source + " : " + errorMessage, DateTimeOffset.UtcNow, new TimeSpan(0, 0, 3), "200", true);
            tc.TrackMetric(DiagnosticAreaName, 100);
            tc.TrackEvent(eventName);

            tc.Flush();
        }
    }
}
