using TKS.DTO.Core;
using TKS.DTO.Models;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace IamConsol
{
    class Program
    {
        static InvokeType currentInvokeType = InvokeType.normal;
        static object _lock = new object();
        static void Main(string[] args)
        {
            List<ExampleData> ls = new List<ExampleData>();
            ls.Add(new ExampleData { Name = "test", Age = 10 });

            

            Dictionary<string, ParamDataItem> param = new Dictionary<string, ParamDataItem>();
            //参数的顺序非常重要，是后台方法的参数顺序
            DateTime s = DateTime.Now;
            for (int i = 0; i < 200; i++)
            {
                //send<string>(new SubmitContext
                //{
                //    FuncType = "RESTfulS.MYLPA_APPLY",
                //    MethodName = "Test",
                //    Parameters = null
                //},i);
                send<string>(new SubmitContext
                {
                    FuncType = "RESTfuls2.TEST2Handler",
                    MethodName = "Test",
                    Parameters = null
                }, i);
            }
            TimeSpan e = s.Subtract(DateTime.Now);
            Console.WriteLine("执行时间"+e.ToString());
           
            Console.ReadLine();
            Console.WriteLine(total.ToString() + "次调用成功");
            Console.ReadLine();
        }
       static  int total = 0;
        static void send<T>(SubmitContext _submitContext,int index)
        {
            var client = new RestClient("http://localhost:5621/RESTfulService.svc/Rest/");
            RequestContext context = new RequestContext();
            context.FuncType = _submitContext.FuncType;
            context.MethodName = _submitContext.MethodName;
            context.UserName = _submitContext.UserName;
            context.Data = string.Empty;
            context.TInvokeType = _submitContext.TInvokeType;
            currentInvokeType = _submitContext.TInvokeType;
            context.AuthKey = _submitContext.AuthKey;
            if (_submitContext.Parameters != null)
            {
                foreach (var item in _submitContext.Parameters)
                {

                    string json = "\"" + item.Key + "\":" + AnalyzeData(item.Value);
                    context.Data += json + ",";
                }
                context.Data = context.Data.Remove(context.Data.LastIndexOf(','));

                context.Data = "{" + context.Data + "}";
            }
            var re = SimpleJson.SerializeObject(context);

            var request = new RestRequest("{id}", Method.POST);
            request.AddUrlSegment("id", "Execute");

            request.AddParameter("application/json", re, ParameterType.RequestBody);

            var asyncHandle4 = client.ExecuteAsync<ResponseContext>(request, response =>
            {
                if (response == null)
                {

                    System.Diagnostics.Debug.Write("请联系管理员，检查跨域文件是否存在");
                    return;
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {


                    System.Diagnostics.Debug.Write(ConveterHttpStatusCode(response.StatusCode));
                    return;
                }

                if (!string.IsNullOrEmpty(response.Data.Error))
                {

                    System.Diagnostics.Debug.Write(response.Data.Error + response.Data.DetailError);
                }
                else
                {
                    T result;

                    try
                    {
                         result = SimpleJson.DeserializeObject<T>(response.Data.Data);
                         lock (_lock)
                         {
                             total++;
                             Console.WriteLine("第" + (index + 1).ToString() + "次调用" + result);
                         }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write("返回的结果类型转换错误" + typeof(T).ToString());
                    }
                }
            });
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

        static string AnalyzeData(object data)
        {
            ParamDataItem item = (ParamDataItem)data;

            string functype = "\"_FuncType\":\"" + item._FuncType + "\",";
            string methodName = "\"_MethodName\":\"" + item._MethodName + "\",";

            StringBuilder json = new StringBuilder();
            string result = string.Empty;
            if (item.Data == null)
            {
                result = "{" + functype + methodName + "\"Data\":null}";
            }
            else
            {
                Type tt = item.Data.GetType();

                if (typeof(ICollection).IsAssignableFrom(tt))
                {
                    IList datas = (IList)item.Data;

                    foreach (var tmpdata in datas)
                    {
                        var BaseModeltmp = tmpdata as IBaseModel;
                        if (BaseModeltmp != null)
                        {
                            json.Append(BaseModeltmp.GetJson());
                        }
                        else
                        {
                            json.Append(ConvertValue(tmpdata));
                        }
                        json.Append(",");
                    }

                    var resultjson = json.ToString();
                    if (resultjson.Length > 0)
                    {
                        resultjson = resultjson.Substring(0, resultjson.LastIndexOf(','));
                    }
                    result = "[" + resultjson + "]";

                }
                else
                {

                    var BaseModeltmp = item.Data as IBaseModel;
                    if (BaseModeltmp != null)
                    {
                        json.Append(BaseModeltmp.GetJson());
                    }
                    else
                    {
                        json.Append(ConvertValue(item.Data));
                    }
                    result = json.ToString();
                }

                if (currentInvokeType != InvokeType.normal)
                {
                    result = "{" + functype + methodName + "\"Data\":" + result + "}";
                }
            }

            return result;
        }

        static string ConvertValue(object value)
        {
            Type T = value.GetType();
            string result = string.Empty;
            switch (T.Name)
            {
                case Types._int16:
                case Types._int32:
                case Types._int64:
                case Types._float:
                case Types._double:
                case Types._decimal:
                    result = value.ToString();
                    break;
                case Types._bool:
                    result = value.ToString().ToLower();
                    break;
                default:
                    result = "\"" + value.ToString() + "\"";
                    break;
            }

            return result;
        }

        class Types
        {
            public const string _int16 = "Int16";
            public const string _int32 = "Int32";
            public const string _int64 = "Int64";
            public const string _float = "Single";
            public const string _double = "Double";
            public const string _decimal = "Decimal";
            public const string _bool = "Boolean";

        }


    }

    public class ParamDataItem
    {
        /// <summary>
        /// 方法类型
        /// </summary>
        public string _FuncType
        {
            get;
            set;
        }

        /// <summary>
        /// 方法名
        /// </summary>
        public string _MethodName
        {
            get;
            set;
        }

        public object Data
        {
            get;
            set;
        }
    }





    public class SubmitContext
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string AuthKey
        {
            get;
            set;
        }
        /// <summary>
        /// 需要实例化的类型
        /// </summary>
        public string FuncType
        {
            get;
            set;
        }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName
        {
            get;
            set;
        }

        /// <summary>
        /// 调用类型
        /// </summary>
        public InvokeType TInvokeType
        {
            get;
            set;
        }
        /// <summary>
        /// 参数
        /// 添加参数的顺序必需和方法的参数顺序一致
        /// </summary>
        public Dictionary<string, ParamDataItem> Parameters
        {
            get;
            set;
        }
    }
}
