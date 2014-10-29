// ***********************************************************************
// Assembly         : TKS.DataGrid
// Author           : Peter.Zhao
// Created          : 01-13-2014
//
// Last Modified By : Peter.Zhao
// Last Modified On : 01-13-2014
// ***********************************************************************
// <copyright file="MyDataGrid.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Data;




namespace TKS.Controls
{
    /// <summary>
    /// silverlight custom datagrid(has datapager func) 
    /// </summary>
    [TemplatePart(Name = MyDataGrid.ElementFood, Type = typeof(MyDataPager))]
    [TemplatePart(Name = MyDataGrid.ElementDataGrid, Type = typeof(MyDataGrid))]
    public class MyDataGrid : Control
    {


        #region
        /// <summary>
        /// The element food
        /// </summary>
        private const string ElementFood = "DataPagerFood";
        /// <summary>
        /// The element data grid
        /// </summary>
        private const string ElementDataGrid = "DataGrid";
        #endregion

        #region
        /// <summary>
        /// The _ data pager food
        /// </summary>
        internal MyDataPager _DataPagerFood;
        /// <summary>
        /// The _ data grid
        /// </summary>
        private System.Windows.Controls.DataGrid _DataGrid;



        #endregion
        /// <summary>
        /// DataPager索引改变事件
        /// </summary>
        public event EventHandler<PageIndexArgs> PageIndexChanged;

        /// <summary>
        /// 行双击事件
        /// </summary>
        public event EventHandler<EventArgs> RowItemDoubleClick;

        private void _DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TimeSpan t = DateTime.Now.TimeOfDay;
            if (_DataGrid.Tag != null)
            {
                TimeSpan oldT = (TimeSpan)_DataGrid.Tag;
                if (t != oldT && (t - oldT) < TimeSpan.FromMilliseconds(180))
                { DataGrid g = sender as DataGrid;
                    if (RowItemDoubleClick != null)
                    {
                        RowItemDoubleClick(g.SelectedItem, e);
                    }
                    if (RowItemDoubleClickCommand != null)
                    {
                        if (RowItemDoubleClickCommand.CanExecute(g.SelectedItem))
                        {
                            RowItemDoubleClickCommand.Execute(g.SelectedItem);
                        }
                    }
                }
            }
            _DataGrid.Tag = t;
        }

        ContextMenu rightMenu = new ContextMenu();

        void CreateRightClickMenu(DependencyObject dependent)
        {
            ContextMenuService.SetContextMenu(dependent, rightMenu);
        }

        private void _DataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            DataGrid g = sender as DataGrid;
            if (g.CurrentColumn == null)
            {
                rightMenu.Items.Clear();
                MenuItem noitem = new MenuItem();
                noitem.Header = "没有项";
                rightMenu.Items.Add(noitem);
                rightMenu.IsOpen = true;
                return;
            }
            TextBlock textBlock = g.CurrentColumn.GetCellContent(g.SelectedItem) as TextBlock;
            string value = "";
            if (textBlock != null)
            {
                value = textBlock.Text;
            }
            rightMenu.Items.Clear();
            MenuItem item = new MenuItem();
            item.Header = "复制当前项";
            item.Click += (_o, _e) =>
            {
                Clipboard.SetText(value);
                rightMenu.IsOpen = false;
            };
            rightMenu.Items.Add(item);
            rightMenu.IsOpen = true;
        }


        /// <summary>
        /// The PageIndexChangedCommandtProperty
        /// </summary>
        public static readonly DependencyProperty RowItemDoubleClickCommandProperty = DependencyProperty.Register("RowItemDoubleClickCommand",
            typeof(ICommand), typeof(MyDataGrid), new PropertyMetadata(null));


        public ICommand RowItemDoubleClickCommand
        {
            get { return (ICommand)this.GetValue(RowItemDoubleClickCommandProperty); }
            set
            {
                this.SetValue(RowItemDoubleClickCommandProperty, value);
            }
        }

        /// <summary>
        /// The PageIndexChangedCommandtProperty
        /// </summary>
        public static readonly DependencyProperty PageIndexChangedCommandProperty = DependencyProperty.Register("PageIndexChangedCommand",
            typeof(ICommand), typeof(MyDataGrid), new PropertyMetadata(null));


        public ICommand PageIndexChangedCommand
        {
            get { return (ICommand)this.GetValue(PageIndexChangedCommandProperty); }
            set
            {
                this.SetValue(PageIndexChangedCommandProperty, value);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ValueChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnValueChanged(PageIndexArgs e)
        {
            EventHandler<PageIndexArgs> handler = PageIndexChanged;

            if (handler != null)
            {
                handler(this, e);
            }
            if (PageIndexChangedCommand != null)
            {
                if (PageIndexChangedCommand.CanExecute(e))
                    PageIndexChangedCommand.Execute(e);
            }

        }
        /// <summary>
        /// 定义总页数
        /// </summary>
        /// <value>The page count.</value>
        public int PageCount
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
        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register("PageCount",
            typeof(int), typeof(MyDataGrid), new PropertyMetadata(0, new PropertyChangedCallback(PageCountChangedCallback)));
        /// <summary>
        /// Pages the count changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PageCountChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            MyDataGrid dataGrid = (MyDataGrid)obj;
            int _PageCount = (int)args.NewValue;
            if (dataGrid != null)
            {
                if (dataGrid._DataPagerFood != null)
                {
                    dataGrid.SetPageCount(_PageCount);
                }
            }

        }
        /// <summary>
        /// Sets the page count.
        /// </summary>
        /// <param name="value">The value.</param>
        private void SetPageCount(int value)
        {
            if (_DataPagerFood != null)
            {

                _DataPagerFood.PageCount = value;
            }
        }

        #region 每页多少条
        /// <summary>
        /// 定义每页多少条
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return (int)this.GetValue(PagerPageSizeProperty);
            }
            set
            {
                this.SetValue(PagerPageSizeProperty, value);
            }
        }
        /// <summary>
        /// The pager page size property
        /// </summary>
        public static readonly DependencyProperty PagerPageSizeProperty = DependencyProperty.Register("PageSize",
            typeof(int), typeof(MyDataGrid), new PropertyMetadata(0, new PropertyChangedCallback(PageSizeChangedCallback)));
        /// <summary>
        /// Pages the size changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PageSizeChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            MyDataGrid dataGrid = (MyDataGrid)obj;
            int _PageSize = (int)args.NewValue;
            dataGrid.SetPageSize(_PageSize);
        }
        /// <summary>
        /// Sets the size of the page.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        private void SetPageSize(int pageSize)
        {

            if (_DataPagerFood != null)
            {
                _DataPagerFood.PageSize = pageSize;

            }

        }

        #endregion


        /// <summary>
        /// 索引页
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex
        {
            get
            {
                return (int)this.GetValue(PageIndexProperty);
            }
            set
            {

                this.SetValue(PageIndexProperty, value);

            }
        }
        /// <summary>
        /// The page index property
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register("PageIndex",
            typeof(int), typeof(MyDataGrid), new PropertyMetadata(new PropertyChangedCallback(PageIndexChangedCallback)));
        /// <summary>
        /// Pages the index changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        static void PageIndexChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            MyDataGrid dataGrid = (MyDataGrid)obj;
            int _PageIndex = (int)args.NewValue;
            if (dataGrid != null)
            {
                DependencyPropertyChangedEventArgs e = args;
                dataGrid.OnValueChanged(new PageIndexArgs(int.Parse(e.NewValue.ToString())));

            }

        }

        /// <summary>
        /// 锁定列数
        /// </summary>
        /// <value>The frozen column count.</value>
        public int FrozenColumnCount
        {
            get { return (int)GetValue(FrozenColumnCountProperty); }
            set { SetValue(FrozenColumnCountProperty, value); }
        }

        /// <summary>
        /// The frozen column count property
        /// </summary>
        public static readonly DependencyProperty FrozenColumnCountProperty = DependencyProperty.Register("FrozenColumnCount",
        typeof(int), typeof(MyDataGrid), new PropertyMetadata(FrozenColumnCountChangedCallback));

        /// <summary>
        /// Frozens the column count changed callback.
        /// </summary>
        /// <param name="d">The command.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void FrozenColumnCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyDataGrid dataGrid = (MyDataGrid)d;
            int frozenColumnCount = (int)e.NewValue;
            if (dataGrid._DataGrid != null)
            {
                dataGrid._DataGrid.FrozenColumnCount = frozenColumnCount;
            }
        }

        /// <summary>
        /// 自定义列
        /// </summary>
        /// <value>The grid colums.</value>
        public ObservableCollection<DataGridColumn> GridColums
        {
            get
            {

                return this.GetValue(GridColumsProperty) as ObservableCollection<DataGridColumn>;
            }
            set
            {
                this.SetValue(GridColumsProperty, value);
            }
        }

        /// <summary>
        /// The grid colums property
        /// </summary>
        public static readonly DependencyProperty GridColumsProperty = DependencyProperty.Register("GridColums",
          typeof(ObservableCollection<DataGridColumn>), typeof(MyDataGrid), new PropertyMetadata(GridColumsChangedCallback));

        /// <summary>
        /// Grids the colums changed callback.
        /// </summary>
        /// <param name="d">The command.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void GridColumsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyDataGrid dataGrid = (MyDataGrid)d;
            ObservableCollection<DataGridColumn> _GridColumns = (ObservableCollection<DataGridColumn>)e.NewValue;
            if (dataGrid._DataGrid != null)
            {
                dataGrid._DataGrid.Columns.Clear();
                foreach (var item in _GridColumns)
                    dataGrid._DataGrid.Columns.Add(item);

            }
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource
        {
            get
            {
                return this.GetValue(ItemsSourceProperty) as IEnumerable;
            }
            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }




        /// <summary>
        /// The items source property
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IEnumerable), typeof(MyDataGrid), new PropertyMetadata(ItemsSourceChangedCallback));
        /// <summary>
        /// Itemses the source changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        static void ItemsSourceChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MyDataGrid dataGrid = (MyDataGrid)obj;
            IEnumerable _ItemsSource = (IEnumerable)args.NewValue;
            if (dataGrid._DataGrid != null)
            {
                SetAllSource(dataGrid, _ItemsSource, dataGrid.PageCount, dataGrid.PageSize);

            }
        }




        /// <summary>
        /// Sets all source.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <param name="_ItemsSource">The _ items source.</param>
        /// <param name="pageCount">The page count.</param>
        /// <param name="pageSize">Size of the page.</param>
        private static void SetAllSource(MyDataGrid dataGrid, IEnumerable _ItemsSource, int pageCount, int pageSize)
        {
            if (dataGrid.GridColums.Count > dataGrid._DataGrid.Columns.Count)
            {
                dataGrid._DataGrid.Columns.Clear();
                foreach (var item in dataGrid.GridColums)
                    dataGrid._DataGrid.Columns.Add(item);
            }

            dataGrid._DataGrid.FrozenColumnCount = dataGrid.FrozenColumnCount;
            if (_ItemsSource == null)
            {
                return;
            }
            PagedCollectionView pagedCollectionView = new PagedCollectionView(_ItemsSource);
            if (pageSize > 0)
            {
                //控件初始化的时候，依赖属性会优先初始化
                pagedCollectionView.PageSize = pageSize;
            }
            else
            {
                pagedCollectionView.PageSize = dataGrid.PageSize;
            }


            if (pageCount > 0)
            {
                dataGrid._DataPagerFood.PageCount = pageCount;
            }
            else
            {
                dataGrid._DataPagerFood.Source = pagedCollectionView;
            }
            dataGrid._DataGrid.ItemsSource = pagedCollectionView;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public MyDataGrid()
        {
            this.DefaultStyleKey = typeof(MyDataGrid);
            GridColums = new ObservableCollection<DataGridColumn>();
        }
        /// <summary>
        /// 重写OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _DataPagerFood = this.GetTemplateChild(ElementFood) as MyDataPager;
            _DataGrid = this.GetTemplateChild(ElementDataGrid) as System.Windows.Controls.DataGrid;
            _DataGrid.MouseLeftButtonUp += _DataGrid_MouseLeftButtonUp;
            _DataGrid.MouseRightButtonDown += _DataGrid_MouseRightButtonDown;
            _DataPagerFood.PageIndexChanged += new EventHandler<EventArgs>(_DataPager_PageIndexChanged);
            _DataGrid.AutoGenerateColumns = false;
            _DataGrid.ColumnWidth = DataGridLength.Auto;
            SetPageSize(PageSize);
            SetAllSource(this, ItemsSource, PageCount, PageSize);
            PageIndex = 0;
            CreateRightClickMenu(_DataGrid);
        }






        /// <summary>
        /// Handles the PageIndexChanged event of the _DataPager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void _DataPager_PageIndexChanged(object sender, EventArgs e)
        {
            DataPager dataPager = sender as DataPager;
            _DataPagerFood.PageIndex = dataPager.PageIndex;
            PageIndex = _DataPagerFood.PageIndex;
        }

    }
}
