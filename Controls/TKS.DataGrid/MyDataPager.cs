// ***********************************************************************
// Assembly         : TKS.DataGrid
// Author           : Peter.Zhao
// Created          : 01-13-2014
//
// Last Modified By : Peter.Zhao
// Last Modified On : 01-13-2014
// ***********************************************************************
// <copyright file="MyDataPager.cs" company="">
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
using System.Windows.Data;
/// <summary>
/// The MyDataGrid namespace.
/// </summary>
namespace TKS.Controls
{
    /// <summary>
    /// Class MyDataPager.
    /// </summary>
    public class MyDataPager : System.Windows.Controls.DataPager
    {
        #region 定义总条数
        /// <summary>
        /// 定义总条数
        /// </summary>
        /// <value>The page count.</value>
        /// <returns>源位于 <see cref="T:System.ComponentModel.IPagedCollectionView" /> 中时已知页面的当前数目；否则为 1。</returns>
        public new int PageCount
        {
            get { return (int)this.GetValue(PageCountProperty); }
            set
            {
                this.SetValue(PageCountProperty, value);
               
            }
        }
        /// <summary>
        /// The page count property
        /// </summary>
        public static new DependencyProperty PageCountProperty = DependencyProperty.Register("PagerPageCount", typeof(int), typeof(DataPager), new PropertyMetadata(0, PageCountPropertyChangedCallback));
        /// <summary>
        /// Pages the count property changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="arg">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PageCountPropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            MyDataPager dataPager = obj as MyDataPager;
            //  dataPager.PageCount = (int)arg.NewValue;
            dataPager.Bind();
        }
        #endregion

        /// <summary>
        /// 指定页数后重新绑定总页数
        /// </summary>
        public void Bind()
        {
            int totalpagers = PageCount;
            List<int> itemCount = new List<int>();//用于DataPager的数据提供

            for (int i = 1; i <= totalpagers; i++)
            {
                itemCount.Add(i);
            }
            PagedCollectionView pagedCollectionView = new PagedCollectionView(itemCount);
            pagedCollectionView.PageSize = PageSize;
            this.Source = pagedCollectionView;
            //this.PageCount = PageCount;



        }
    }
}
