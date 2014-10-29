// ***********************************************************************
// Assembly         : RestHelper
// Author           : Peter.Zhao
// Created          : 01-06-2014
//
// Last Modified By : Peter.Zhao
// Last Modified On : 01-06-2014
// ***********************************************************************
// <copyright file="RestHelperForQuery.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using TKS.DTO.Core;
using TKS.Service.Helper;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

/// <summary>
/// The RestHelper namespace.
/// </summary>
namespace TKS.Service
{
    /// <summary>
    /// only for Query
    /// </summary>
    public  class RestHelperForQuery
    {
        /// <summary>
        /// Executes the data.
        /// </summary>
        /// <param name="_request">The _request.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>ResponseContext.</returns>
        public ResponseContext ExecuteData(RequestContextForQuery _request, string assemblyName)
        {
            //错误
            string _error = string.Empty;
            //详细错误
            string _detailError = string.Empty;
            //返回的数据
            string data = string.Empty;
            //是否本地执行
            bool isCurrentHandler = true;
            //记录总数
            int totalNum = 0;
            try
            {
                ////路由
                //Dictionary<string, RouteItem> routeList = ConfigHelper.GetRouteItems();

                ////路由列表中存在对应的通道，判断通道的地址有无，无则本地处理，有则将消息发送
                ////到对应的地址进行处理
                //string address = string.Empty;
                //if (routeList.Keys.Contains(_request.FuncType))
                //{
                //    address = routeList[_request.FuncType].RouteAddress;
                //    if (string.IsNullOrEmpty(address))
                //        isCurrentHandler = true;
                //}
                //else
                //{
                //    isCurrentHandler = true;
                //}

                if (isCurrentHandler)
                {

                    QueryCore query = new QueryCore(_request.SqlString, _request.PrameterData,_request.PageNum,_request.PageSize);
                   
                    data= query.GetData(out totalNum);
                    
                }
                else
                {
                    //路由到对应地址
                    //return RestfulClient.SubmitRequest(_request, address);

                }
            }
            catch (Exception ex)
            {
                _error = "查询异常，详细信息请点击详细按钮查看.";
                _detailError = ex.Message;
                if (ex.InnerException != null)
                    _detailError+=Environment.NewLine + ex.InnerException.Message + ex.InnerException.StackTrace;
            }
            return new ResponseContext { Data = data, TotalNum=totalNum, Error = _error, DetailError = _detailError };
        }


       
    }
}
