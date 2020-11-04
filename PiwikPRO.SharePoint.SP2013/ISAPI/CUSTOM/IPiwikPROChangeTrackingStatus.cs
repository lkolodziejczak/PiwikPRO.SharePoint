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
    interface IPiwikPROChangeTrackingStatus
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ChangeTrackingStatus?status={statusProp}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string ChangeTrackingStatus(string statusProp);
    }
}
