// ***********************************************************************
// Assembly         : Model.Net4
// Author           : Peter.Zhao
// Created          : 11-29-2013
//
// Last Modified By : Peter.Zhao
// Last Modified On : 01-06-2014
// ***********************************************************************
// <copyright file="RequestContext.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// The Net4 Model
/// </summary>
namespace TKS.DTO.Core
{
    /// <summary>
    /// Enum InvokeType
    /// </summary>
    public enum InvokeType
    {
        /// <summary>
        /// 普通方式
        /// </summary>
        normal,
        /// <summary>
        /// 调用方式依赖类
        /// </summary>
        dependOnClass
    }

    /// <summary>
    /// 请求的消息体
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// Gets or sets the type of the function.
        /// </summary>
        /// <value>The type of the function.</value>
        public string FuncType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public string MethodName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the attribute invoke.
        /// </summary>
        /// <value>The type of the attribute invoke.</value>
        public InvokeType TInvokeType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the authentication key.
        /// </summary>
        /// <value>The authentication key.</value>
        public string AuthKey
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 请求的消息体(只针对查询)
    /// </summary>
    public class RequestContextForQuery
    {
        /// <summary>
        /// Gets or sets the SQL string.
        /// </summary>
        /// <value>The SQL string.</value>
        public string SqlString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the prameter data.
        /// </summary>
        /// <value>The prameter data.</value>
        public string PrameterData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the authentication key.
        /// </summary>
        /// <value>The authentication key.</value>
        public string AuthKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>The page number.</value>
        public int PageNum
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 返回的消息体
    /// </summary>
    public class ResponseContext
    {
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public string Error
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the detail error.
        /// </summary>
        /// <value>The detail error.</value>
        public string DetailError
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the total number.
        /// </summary>
        /// <value>The total number.</value>
        public int TotalNum
        {
            get;
            set;
        }
    }
}
