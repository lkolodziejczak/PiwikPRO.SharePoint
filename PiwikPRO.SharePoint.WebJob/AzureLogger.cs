using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using PiwikPRO.SharePoint.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.WebJob
{
    public class AzureLogger : ISPLogger
    {
        public string DiagnosticAreaName = "PiwikPRO";

        public void WriteLog(Category categoryName, string source, string errorMessage)
        {
            //TelemetryConfiguration.Active.InstrumentationKey = DiagnosticAreaName;
            TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;

            var tc = new TelemetryClient();
            tc.TrackRequest(source + " : " + errorMessage, DateTimeOffset.UtcNow, new TimeSpan(0, 0, 3), "200", true);
            tc.TrackMetric(DiagnosticAreaName, 100);
            tc.TrackEvent(categoryName.ToString());

            tc.Flush();
        }
    }
}
