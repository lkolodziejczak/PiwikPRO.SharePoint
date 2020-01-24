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
        [WebInvoke(Method = "POST", UriTemplate = "UpdateFarmProperty?propName={propName}&propValue={propValue}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateFarmProperty(string propName, string propValue);
    }
}
