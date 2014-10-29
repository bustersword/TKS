using TKS.DTO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace TKS.Service.Helper
{

    [ServiceContract]
    public interface IRESTfulService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/Rest/Execute", Method = "*", ResponseFormat = WebMessageFormat.Json)]
        ResponseContext ExecuteData(RequestContext _request);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Rest/ExecuteForQuery", Method = "*", ResponseFormat = WebMessageFormat.Json)]
        ResponseContext ExecuteDataForQuery(RequestContextForQuery _request);
    }

}
