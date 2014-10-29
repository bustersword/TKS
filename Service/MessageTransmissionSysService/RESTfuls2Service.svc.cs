using TKS.DTO.Core;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TKS.Service;
namespace RESTfuls2
{
    public class RESTfuls2Service : TKS.Service.Helper.IRESTfulService
    {

        public ResponseContext ExecuteData(RequestContext _request)
        {

            TKS.Service.RestHelper helper = new TKS.Service.RestHelper();
            var r = helper.ExecuteData(_request, "RESTfuls2");
            return r;
        }


        public ResponseContext ExecuteDataForQuery(RequestContextForQuery _request)
        {
            TKS.Service.RestHelperForQuery helper = new TKS.Service.RestHelperForQuery();
            var r = helper.ExecuteData(_request, "RESTfuls2");
            return r;
        }
    }
}
