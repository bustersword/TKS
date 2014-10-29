// ***********************************************************************
// Assembly         : RestHelper
// Author           : Peter.Zhao
// Created          : 12-20-2013
//
// Last Modified By : Peter.Zhao
// Last Modified On : 12-20-2013
// ***********************************************************************
// <copyright file="RestHelper.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using TKS.DTO.Core;
using TKS.Service.Helper;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

/// <summary>
/// The RestHelper namespace.
/// </summary>
namespace TKS.Service
{

    /// <summary>
    /// General operation
    /// </summary>
    public class RestHelper   
    {
        /// <summary>
        /// Executes the data.
        /// </summary>
        /// <param name="_request">The _request.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>ResponseContext.</returns>
        /// <exception cref="System.Exception">
        /// 未找到指定的类型： + _request.FuncType
        /// or
        /// 未找到指定的方法： + _request.MethodName
        /// or
        /// 未找到指定的方法： + _request.MethodName</exception>
        public ResponseContext ExecuteData(RequestContext _request,string assemblyName)
        {
            //错误
            string _error = string.Empty;
            //详细错误
            string _detailError = string.Empty;
            //返回的数据
            string data = string.Empty;
            //是否本地执行
            bool isCurrentHandler = false;
            try
            {
                //路由
                Dictionary<string, RouteItem> routeList = ConfigHelper.GetRouteItems();

                //路由列表中存在对应的通道，判断通道的地址有无，无则本地处理，有则将消息发送
                //到对应的地址进行处理
                string address = string.Empty;
                if (routeList.Keys.Contains(_request.FuncType))
                {
                    address = routeList[_request.FuncType].RouteAddress;
                    if (string.IsNullOrEmpty(address))
                        isCurrentHandler = true;
                }
                else
                {
                    isCurrentHandler = true;
                }

                if (isCurrentHandler)
                {

                    AssemblyHelper helper = new AssemblyHelper(assemblyName);
                    var ob = helper.Instance.CreateInstance(_request.FuncType);

                    if (ob == null)
                    {
                        throw new Exception("未找到指定的类型：" + _request.FuncType);
                    }
                    Dictionary<string, string> resjson = new Dictionary<string, string>();
                    JsonDeserializer json = new JsonDeserializer();
                    if (!string.IsNullOrEmpty(_request.Data))
                        resjson = json.Deserialize<Dictionary<string, string>>(new RestResponse { Content = _request.Data });

                    Type TT = ob.GetType();
                    MethodInfo method;
                    switch (_request.TInvokeType)
                    {
                        case InvokeType.dependOnClass:
                            {
                                method = TT.GetMethod(_request.MethodName);
                                if (method == null)
                                {
                                    throw new Exception("未找到指定的方法：" + _request.MethodName);
                                }
                                object o = method.Invoke(ob, new object[] { resjson });
                                data = AnalyzeData(o);
                            }
                            break;
                        case InvokeType.normal:
                        default:
                            {
                                object[] paras = resjson.Select(p => p.Value).ToArray();
                                Type[] types = new Type[paras.Length];
                                for (int i = 0; i < paras.Length; i++)
                                {
                                    types[i] = typeof(string);
                                }
                                method = TT.GetMethod(_request.MethodName, types);
                                if (method == null)
                                {
                                    throw new Exception("未找到指定的方法：" + _request.MethodName);
                                }
                                object o = method.Invoke(ob, paras);
                                data = AnalyzeData(o);
                            }
                            break;
                    }
                }
                else
                {
                    //路由到对应地址
                    return RestfulClient.SubmitRequest(_request, address);

                }
            }
            catch (TargetInvocationException ex)
            {
                _error = "后台执行请求的方法[" + _request.MethodName + "]出现异常，详细信息请点击详细按钮。";
                if (ex.InnerException != null)
                {
                    if (ex.InnerException is APPException)
                    {
                        _error = ex.InnerException.Message;
                    }
                    else
                    {
                        _detailError = ex.InnerException.Message + ex.InnerException.StackTrace;
                    }
                }
            }
           
            catch (Exception ex)
            {
                _error = "查询异常，详细信息请点击详细按钮查看.";
                _detailError = ex.Message;
                if (ex.InnerException != null)
                    _detailError += Environment.NewLine + ex.InnerException.Message + ex.InnerException.StackTrace;

            }
        


           // var request = WebOperationContext.Current.IncomingRequest;
            //var args = request.UriTemplateMatch.WildcardPathSegments;
            //var oauth = request.Headers["Authorization"];

            return new ResponseContext { Data = data, Error = _error, DetailError = _detailError };
        }


        /// <summary>
        /// Analyzes the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        string AnalyzeData(object data)
        {
            if (data == null)
                return ConvertValue("");
            string jsonresult = string.Empty;
            StringBuilder json = new StringBuilder();
            Type tt = data.GetType();

            if (typeof(ICollection).IsAssignableFrom(tt))
            {
                IList datas = (IList)data;

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
                    resultjson = resultjson.Substring(0, resultjson.Length - 1);
                }
                jsonresult = "[" + resultjson + "]";

            }
            else
            {
                var BaseModeltmp = data as IBaseModel;
                if (BaseModeltmp != null)
                {
                    json.Append(BaseModeltmp.GetJson());
                }
                else
                {
                    json.Append(ConvertValue(data));
                }
                jsonresult = json.ToString();
            }

            return jsonresult;
        }

        /// <summary>
        /// Converts the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        string ConvertValue(object value)
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

        /// <summary>
        /// Class Types.
        /// </summary>
        class Types
        {
            /// <summary>
            /// The _int16
            /// </summary>
            public const string _int16 = "Int16";
            /// <summary>
            /// The _int32
            /// </summary>
            public const string _int32 = "Int32";
            /// <summary>
            /// The _int64
            /// </summary>
            public const string _int64 = "Int64";
            /// <summary>
            /// The _float
            /// </summary>
            public const string _float = "Single";
            /// <summary>
            /// The _double
            /// </summary>
            public const string _double = "Double";
            /// <summary>
            /// The _decimal
            /// </summary>
            public const string _decimal = "Decimal";
            /// <summary>
            /// The _bool
            /// </summary>
            public const string _bool = "Boolean";

        }
    }
}
