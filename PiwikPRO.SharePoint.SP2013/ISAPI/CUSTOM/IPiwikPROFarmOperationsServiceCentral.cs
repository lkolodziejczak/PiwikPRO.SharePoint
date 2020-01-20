using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace PiwikPRO.SharePoint.SP2013
{
    [ServiceContract]
    interface IPiwikPROFarmOperationsServiceCentral
    {
        [OperationContract]
        [WebGet(UriTemplate = "UpdateFarmProperty?propName={propName}&propValue={propValue}",
    ResponseFormat = WebMessageFormat.Json)]
        string UpdateFarmProperty(string propName, string propValue);
    }
}
