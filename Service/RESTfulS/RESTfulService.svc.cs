// ***********************************************************************
// Assembly         : RESTfulS
// Author           : Peter.Zhao
// Created          : 11-29-2013
//
// Last Modified By : Peter.Zhao
// Last Modified On : 12-20-2013
// ***********************************************************************
// <copyright file="RESTfulService.svc.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
/// <summary>
/// The RESTfulS namespace.
/// </summary>
namespace RESTfulS
{
    /// <summary>
    /// Class RESTfulService.
    /// </summary>
    public class RESTfulService : TKS.Service.Helper.IRESTfulService 
    {
        /*
        *数据json格式
        * List<string> datas
        * {"datas":["aaa"]}
        * 
        * public class ExampleData
        * {
        *      public string Name
        *      {
        *          get;
        *          set;
        *      }

        *      public string Age
        *      {
        *          get;
        *          set;
        *      }
        * }
        * 
        * List<ExampleData> datas
        * {"datas":[{Name:"xiaohua",Age:"13"}]}
        * 
        * List<ExampleData> datas1, List<ExampleData> datas2
        * {"datas1":[{Name:"xiaohua",Age:"13"}],"datas2":[{Name:"li",Age:"13"}]}
        *
        */

        /// <summary>
        /// Executes the data.
        /// </summary>
        /// <param name="_request">The _request.</param>
        /// <returns>ResponseContext.</returns>
        public ResponseContext ExecuteData(RequestContext _request)
        {

            TKS.Service.RestHelper helper = new TKS.Service.RestHelper();
            var r = helper.ExecuteData(_request, "RESTfulS");
            return r;
        }




        public ResponseContext ExecuteDataForQuery(RequestContextForQuery _request)
        {
            TKS.Service.RestHelperForQuery helper = new TKS.Service.RestHelperForQuery();
            var r = helper.ExecuteData(_request, "RESTfulS");
            return r;
        }

     
    }
}
