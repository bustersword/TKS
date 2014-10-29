using System;
using System.Net;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections;
using RestSharp.Deserializers;
using TKS.DTO.Core;

using System.Text;
namespace TKS.Common
{
    /// <summary>
    /// 客户端请求类
    /// </summary>
    public class RESTFulClient
    {
        public static string RestFulAddress
        {
            get;
            set;
        }

        //当前调用的方式，只有在使用SubmitContext参数的时候才能选择，否则默认为普通方式
        static InvokeType currentInvokeType = InvokeType.normal;

        #region 一般请求

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="_submitContext">请求的上下文</param>
        /// <param name="call">回调</param>
        public static void SubmitRequest<T>(SubmitContext _submitContext, Action<T> call)
        {

            var client = new RestClient(RestFulAddress);
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
                //foreach (var item in _submitContext.Parameters)
                //{
                //    DateTime dt = DateTime.Now;
                //    string json = "\"" + item.Key + "\":" + SimpleJson.SerializeObject(item.Value);
                //    context.Data += json + ",";
                //    DateTime dt2 = DateTime.Now;
                //    System.Diagnostics.Debug.WriteLine(dt2.Subtract(dt));
                //}
                context.Data = context.Data.Remove(context.Data.LastIndexOf(','));

                context.Data = "{" + context.Data + "}";
            }

            Send<T>(call, client, context);
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="funcType">后台方法的类型</param>
        /// <param name="methodName">后台方法名</param>
        /// <param name="param">参数实体</param>
        /// <param name="call">回调</param>
        public static void SubmitRequest<T>(string funcType, string methodName, Action<T> call, BaseModel param)
        {

            var client = new RestClient(RestFulAddress);
            RequestContext context = new RequestContext();
            context.FuncType = funcType;
            context.MethodName = methodName;
            context.UserName = "";
            context.Data = string.Empty;
            context.TInvokeType = InvokeType.normal;
            currentInvokeType = InvokeType.normal;
            context.AuthKey = "";

            if (param != null)
            {
                string json = "\"param\":" + param.GetJson();

                context.Data = "{" + json + "}";
            }

            Send<T>(call, client, context);
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="funcType">后台方法的类型</param>
        /// <param name="methodName">后台方法名</param>
        /// <param name="call">回调</param>
        /// <param name="param">参数</param>
        public static void SubmitRequest<T>(string funcType, string methodName, Action<T> call, params string[] param)
        {

            var client = new RestClient(RestFulAddress);
            RequestContext context = new RequestContext();
            context.FuncType = funcType;
            context.MethodName = methodName;
            context.UserName = "";
            context.Data = string.Empty;
            context.TInvokeType = InvokeType.normal;
            currentInvokeType = InvokeType.normal;
            context.AuthKey = "";

            if (param != null)
            {
                string json = "";
                for (int i = 0; i < param.Length; i++)
                {
                    json += "\"" + i.ToString() + "\":" + ConvertValue(param[i]);
                    json += ",";
                }
                json = json.Remove(json.LastIndexOf(','));
                context.Data = "{" + json + "}";
            }

            Send<T>(call, client, context);
        }

        /// <summary>
        /// 执行请求（无参数）
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="funcType">后台方法的类型</param>
        /// <param name="methodName"></param>
        /// <param name="call"></param>
        public static void SubmitRequest<T>(string funcType, string methodName, Action<T> call)
        {

            var client = new RestClient(RestFulAddress);
            RequestContext context = new RequestContext();
            context.FuncType = funcType;
            context.MethodName = methodName;
            context.UserName = "";
            context.Data = string.Empty;
            context.TInvokeType = InvokeType.normal;
            currentInvokeType = InvokeType.normal;
            context.AuthKey = "";

            Send<T>(call, client, context);
        }

        #endregion

        #region 仅针对查询的请求操作

        public static void SubmitRequestOnlyForQuery<T>(string selectSqlString, int pageNum, int pageSize, Action<T, int> call, BaseModel param)
        {

            var client = new RestClient(RestFulAddress);
            RequestContextForQuery context = new RequestContextForQuery();
            context.PageSize = pageSize;
            context.PageNum = pageNum;
            context.UserName = "";
            context.AuthKey = "";
            context.PrameterData = param.GetJson();
            context.SqlString = selectSqlString;
            SendForQuery<T>(call, client, context);
        }

        #endregion

        #region 发送请求核心操作
        private static void SendForQuery<T>(Action<T, int> call, RestClient client, RequestContextForQuery context)
        {
            var serializeString = SimpleJson.SerializeObject(context);
            Byte[] b = System.Text.Encoding.UTF8.GetBytes(serializeString);
            if (b.Length > 4194304)
            {
                // Message.WarnMessage("请求提交的数据大小超出限制", null, null);
                throw new Exception("请求提交的数据大小超出限制");

            }

            var request = new RestRequest("{id}", Method.POST);
            request.AddUrlSegment("id", "ExecuteForQuery");
            request.AddParameter("application/json", serializeString, ParameterType.RequestBody);


            client.Authenticator = OAuth1Authenticator.ForAccessToken(
                "key", "secret",
                "oauthtoken", "oauthtokenverify", "verify"
                );

            SendCore<T>(call, client, request);
        }

        private static void Send<T>(Action<T> call, RestClient client, RequestContext context)
        {
            var re = SimpleJson.SerializeObject(context);
            Byte[] b = System.Text.Encoding.UTF8.GetBytes(re);
            if (b.Length > 4194304)
            {
                throw new Exception("请求提交的数据大小超出限制");

            }

            var request = new RestRequest("{id}", Method.POST);
            request.AddUrlSegment("id", "Execute");
            request.AddParameter("application/json", re, ParameterType.RequestBody);


            client.Authenticator = OAuth1Authenticator.ForAccessToken(
                "key", "secret",
                "oauthtoken", "oauthtokenverify", "verify"
                );

            SendCore<T>(call, client, request);
        }

        private static void SendCore<T>(Action<T, int> call, RestClient client, RestRequest request)
        {
            var asyncHandle = client.ExecuteAsync<ResponseContext>(request, response =>
            {
                if (response == null)
                {
                    CommonBusy.IsNotBusy();
                    throw new Exception("请求提交的数据大小超出限制");

                }

                if (response.StatusCode != HttpStatusCode.OK)
                {

                    CommonBusy.IsNotBusy();
                    throw new Exception(ConveterHttpStatusCode(response.StatusCode)
                        + response.ErrorException.Message);

                }

                if (!string.IsNullOrEmpty(response.Data.Error))
                {
                    CommonBusy.IsNotBusy();
                    throw new Exception(response.Data.Error + response.Data.DetailError);
                }
                else
                {
                    T result;

                    try
                    {
                        result = SimpleJson.DeserializeObject<T>(response.Data.Data);

                        if (call != null)
                        {
                            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
                            {
                                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    call(result, response.Data.TotalNum);
                                });
                            }
                            else
                            {
                                call(result, response.Data.TotalNum);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonBusy.IsNotBusy();
                        throw new Exception("返回的结果类型转换错误" + typeof(T).ToString()+ex.Message );
                    }
                }
            });
        }
        private static void SendCore<T>(Action<T> call, RestClient client, RestRequest request)
        {
            var asyncHandle = client.ExecuteAsync<ResponseContext>(request, response =>
            {
                if (response == null)
                {
                    CommonBusy.IsNotBusy();
                    throw new Exception("请联系管理员，检查跨域文件是否存在");
                    
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {

                    CommonBusy.IsNotBusy();
                    throw new Exception(ConveterHttpStatusCode(response.StatusCode)
                       + response.ErrorException);
                  
                }

                if (!string.IsNullOrEmpty(response.Data.Error))
                {
                    CommonBusy.IsNotBusy();
                    throw new Exception(response.Data.Error+response.Data.DetailError);
                }
                else
                {
                    T result;

                    try
                    {
                        result = SimpleJson.DeserializeObject<T>(response.Data.Data);

                        if (call != null)
                        {
                            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
                            {
                                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    call(result);
                                });
                            }
                            else
                            {
                                call(result);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonBusy.IsNotBusy();
                        throw new Exception("返回的结果类型转换错误" + typeof(T).ToString()+ ex.Message );
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
                case HttpStatusCode.InternalServerError:
                    result = "500:服务器上发生了一般错误";
                    break;
                default:
                    result = status.ToString();
                    break;
            }

            return result;
        }
        #endregion

        #region 序列化数据
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
        #endregion

        #region 反序列化数据（暂不用）
        /// <summary>
        /// 将json数据转换为单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        static T GetEntity<T>(string data) where T : BaseModel, new()
        {

            List<T> results = GetEntities<T>(data);
            if (results.Count == 0)
            {
                throw new Exception("指定的json字符串未能转换为实体类" + typeof(T).Name);
            }

            return results[0];
        }

        /// <summary>
        /// 将json数据转换为实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        static List<T> GetEntities<T>(string data) where T : BaseModel, new()
        {
            Type tt = typeof(T);
            if (!tt.IsSubclassOf(typeof(BaseModel)))
            {
                throw new Exception("传入的类型必须继承自BaseModel");
            }
            JsonDeserializer json = new JsonDeserializer();
            List<Dictionary<string, object>> res = new List<Dictionary<string, object>>();
            res = json.Deserialize<List<Dictionary<string, object>>>(new RestResponse { Content = data });

            List<T> models = new List<T>();

            for (int i = 0; i < res.Count; i++)
            {
                T temp = new T();
                ((BaseModel)temp).SetDic(res[i]);
                models.Add(temp);
            }
            return models;
        }
        #endregion
    }

    /// <summary>
    /// 参数描述
    /// </summary>
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

    /// <summary>
    /// 提交的上下文
    /// </summary>
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
