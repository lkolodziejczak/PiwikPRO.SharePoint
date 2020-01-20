using Microsoft.SharePoint.Administration;
using PiwikPRO.SharePoint.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.SP2013
{
    public class SPLogger : SPDiagnosticsServiceBase, ISPLogger
    {
        public string DiagnosticAreaName = "PiwikPRO";
        private SPLogger _Current;
        public SPLogger Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new SPLogger();
                }

                return _Current;
            }
        }

        public SPLogger() : base("Piwik Logging Service", SPFarm.Local)
        {

        }
        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            List<SPDiagnosticsArea> areas = new List<SPDiagnosticsArea>
        {
            new SPDiagnosticsArea(DiagnosticAreaName, new List<SPDiagnosticsCategory>
            {
                new SPDiagnosticsCategory("Unexpected", TraceSeverity.Unexpected, EventSeverity.Error),
                new SPDiagnosticsCategory("High", TraceSeverity.High, EventSeverity.Warning),
                new SPDiagnosticsCategory("Medium", TraceSeverity.Medium, EventSeverity.Information),
                new SPDiagnosticsCategory("Information", TraceSeverity.Verbose, EventSeverity.Information)
            })
        };

            return areas;
        }

        public void WriteLog(Category categoryName, string source, string errorMessage)
        {
            SPDiagnosticsCategory category = Logger.Current.Areas[DiagnosticAreaName].Categories[categoryName.ToString()];
            Logger.Current.WriteTrace(0, category, category.TraceSeverity, string.Concat(source, ": ", errorMessage));
        }
    }
}
