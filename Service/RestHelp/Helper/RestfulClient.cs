using TKS.DTO.Core;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TKS.Service.Helper
{
    public class RestfulClient
    {
        public static ResponseContext SubmitRequest(RequestContext msg, string address)
        {
            var body = SimpleJson.SerializeObject(msg);
            var client = new RestClient(address);
            var request = new RestRequest("{id}", Method.POST);
            request.AddUrlSegment("id", "Execute");

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var response = client.Execute<ResponseContext>(request);

            if (response == null)
            {

                return new ResponseContext { Error = "请联系管理员，检查跨域文件是否存在" };

            }

            if (response.StatusCode != HttpStatusCode.OK)
            {

                return new ResponseContext
                {
                    Error = ConveterHttpStatusCode(response.StatusCode),
                    DetailError = response.ErrorException.Message
                };

            }

            if (!string.IsNullOrEmpty(response.Data.Error))
            {
                return new ResponseContext { Error = response.Data.Error, DetailError = response.Data.DetailError };
            }
            else
            {
                return response.Data;
            }

        }

        static string ConveterHttpStatusCode(HttpStatusCode status)
        {
            string result = string.Empty;
            switch (status)
            {
                case HttpStatusCode.Forbidden:
                    result = "403:请求被服务端拒绝";
                    break;
                case HttpStatusCode.NotFound:
                    result = "404:请求的资源不在服务器上";
                    break;
                case HttpStatusCode.BadRequest:
                    result = "400:服务器未能识别请求";
                    break;
                default:
                    result = status.ToString();
                    break;
            }

            return result;
        }
    }
}
