// ***********************************************************************
// Assembly         : AppCommon
// Author           : Peter.Zhao
// Created          : 01-21-2014
//
// Last Modified By : Peter.Zhao
// Last Modified On : 01-22-2014
// ***********************************************************************
// <copyright file="MyDataGridEx.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections;
using TKS.Controls;
using TKS.DTO.Core;
using TKS.Common;
/// <summary>
/// The Extension namespace.
/// </summary>
namespace TKS.Common
{
    /// <summary>
    /// TKS.Controls.MyDataGrid控件辅助类
    /// </summary>
    public static  class MyDataGridEx
    {
        /// <summary>
        /// The _my data grid
        /// </summary>
        static MyDataGrid _myDataGrid;
        /// <summary>
        /// The _select SQL
        /// </summary>
        static string _selectSql;
        /// <summary>
        /// The _grid
        /// </summary>
        static Grid _grid;
        /// <summary>
        /// The _param
        /// </summary>
        static BaseModel _param;
        /// <summary>
        /// The _page size
        /// </summary>
        static int _pageSize;
        /// <summary>
        /// Initializes my data grid.
        /// </summary>
        /// <param name="myDataGrid">My data grid.</param>
        /// <param name="layout">The layout.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="selectSql">The select SQL.</param>
        public static void InitMyDataGrid(MyDataGrid myDataGrid, Grid layout, int pageSize, string selectSql)
        {
            _myDataGrid = myDataGrid;
            _grid = layout;
            _pageSize = pageSize;
            _selectSql = selectSql;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T1">结果集类型.</typeparam>
        /// <typeparam name="T2">参数类型.</typeparam>
        /// <param name="myDataGrid">My data grid.</param>
        /// <exception cref="System.Exception">请初始化MyDataGrid,不能为null</exception>
        public static void Query<T1,T2>(this MyDataGrid myDataGrid)where T2:BaseModel ,new ()  
        {
            if (_myDataGrid == null)
            {
                throw new Exception("请初始化MyDataGrid,不能为null");
            }
            _param = GridEx.GetValue<T2>(_grid);
            if (GridEx.ResultOK)
            {
                CommonBusy.IsBusy("请稍等...");
                RESTFulClient.SubmitRequestOnlyForQuery<T1>(_selectSql, 1, _pageSize, (P, total) =>
                {
                    _myDataGrid.ItemsSource = P as IEnumerable;
                    _myDataGrid.PageCount = total;
                    _myDataGrid.PageSize = _pageSize;
                    CommonBusy.IsNotBusy();
                }, _param);
            }
        }

        /// <summary>
        /// 指定索引页查询
        /// </summary>
        /// <typeparam name="T1">结果集类型.</typeparam>
        /// <typeparam name="T2">参数类型.</typeparam>
        /// <param name="myDataGrid">My data grid.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <exception cref="System.Exception">请初始化MyDataGrid,不能为null</exception>
        public static void Query<T1, T2>(this MyDataGrid myDataGrid, int pageIndex) where T2 : BaseModel, new()   
        {
            if (_myDataGrid == null)
            {
                throw new Exception("请初始化MyDataGrid,不能为null");
            }
          
            _param = GridEx.GetValue<T2>(_grid);
            if (GridEx.ResultOK)
            {
                CommonBusy.IsBusy("正在加载第" + pageIndex + "页");
                RESTFulClient.SubmitRequestOnlyForQuery<T1>(_selectSql, pageIndex, _pageSize, (P, total) =>
                {
                    _myDataGrid.ItemsSource = P as IEnumerable;
                    _myDataGrid.PageCount = total;
                    _myDataGrid.PageSize = _pageSize;
                    CommonBusy.IsNotBusy();
                }, _param);
            }
        }

        /// <summary>
        /// 指定索引页查询
        /// </summary>
        /// <typeparam name="T1">结果集类型</typeparam>
        /// <typeparam name="T2">参数类型.</typeparam>
        /// <param name="myDataGrid">My data grid.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="Call">The call.</param>
        /// <exception cref="System.Exception">请初始化MyDataGrid,不能为null</exception>
        public static void Query<T1, T2>(this MyDataGrid myDataGrid, int pageIndex, Action Call) where T2 : BaseModel, new()     
        {
            if (_myDataGrid == null)
            {
                throw new Exception("请初始化MyDataGrid,不能为null");
            }
           
            _param = GridEx.GetValue<T2>(_grid);
            if (GridEx.ResultOK)
            {
                CommonBusy.IsBusy("正在加载第" + pageIndex + "页");
                RESTFulClient.SubmitRequestOnlyForQuery<T1>(_selectSql, pageIndex, _pageSize, (P, total) =>
                {
                    _myDataGrid.ItemsSource = P as IEnumerable;
                    _myDataGrid.PageCount = total;
                    _myDataGrid.PageSize = _pageSize;
                    CommonBusy.IsNotBusy();
                    if (Call != null)
                    {
                        Call();
                    }

                }, _param);
            }
        }
    }
}
