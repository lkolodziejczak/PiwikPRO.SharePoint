using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client.Services;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.SP2013
{
    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PiwikPROFarmOperationsServiceCentral : IPiwikPROFarmOperationsServiceCentral
    { 
        public string UpdateFarmProperty(string propName, string propValue)
        {
            string returner = "true";
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                 {
                System.Web.HttpContext.Current.Items["FormDigestValidated"] = true;
                SPFarm farm = SPFarm.Local;
                farm.Properties[propName] = propValue;
               
                farm.Update();
                });
            }
            catch(Exception ex) {
                Logger.WriteLog(Logger.Category.Unexpected, "Piwik UpdateFarmPropertyFromCentral", ex.Message);
                returner = ex.Message;
            }
            return returner;
        }
    }
}
