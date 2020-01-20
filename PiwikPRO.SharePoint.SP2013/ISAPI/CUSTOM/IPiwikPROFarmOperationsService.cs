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
    interface IPiwikPROFarmOperationsService
    {
        [OperationContract]
        [WebGet(UriTemplate = "IsUserFarmAdmin",
            ResponseFormat = WebMessageFormat.Json)]
        bool IsUserFarmAdmin();

        [OperationContract]
        [WebGet(UriTemplate = "UpdateFarmPropertyCentral?propName={propName}&propValue={propValue}",
ResponseFormat = WebMessageFormat.Json)]
        string UpdateFarmPropertyCentral(string propName, string propValue);

        [OperationContract]
        [WebGet(UriTemplate = "UpdateFarmProperty?propName={propName}&propValue={propValue}",
    ResponseFormat = WebMessageFormat.Json)]
        string UpdateFarmProperty(string propName, string propValue);

        [OperationContract]
        [WebGet(UriTemplate = "GetFarmProperty?propName={propName}",
    ResponseFormat = WebMessageFormat.Json)]
        string GetFarmProperty(string propName);
    }
}
