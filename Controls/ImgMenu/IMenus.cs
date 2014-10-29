// ***********************************************************************
// Assembly         : ImgMenu
// Author           : Peter.Zhao
// Created          : 08-23-2013
//
// Last Modified By : Peter.Zhao
// Last Modified On : 10-18-2013
// ***********************************************************************
// <copyright file="IMenus.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

/// <summary>
/// 菜单控件
/// </summary>
namespace TKS.Controls
{
    /// <summary>
    /// Class IMenus.
    /// </summary>
    public class IMenus : Control
    {
        /// <summary>
        /// The _float menu translate
        /// </summary>
        private TranslateTransform _floatMenuTranslate;
        /// <summary>
        /// The _menu list
        /// </summary>
        private Dictionary<string, object> _menuList;
        /// <summary>
        /// The menu information
        /// </summary>
        Dictionary<string, string> menuInfo = new Dictionary<string, string>();
        /// <summary>
        /// The _root
        /// </summary>
        private StackPanel _root;
        /// <summary>
        /// The _scroll
        /// </summary>
        private ScrollViewer _scroll;
        /// <summary>
        /// The _left panel
        /// </summary>
        private Border _leftPanel;
        /// <summary>
        /// The _menu
        /// </summary>
        private Border _menu;

        /// <summary>
        /// The _storyboard hide
        /// </summary>
        private Storyboard _storyboardHide;
        /// <summary>
        /// The _storyboard show
        /// </summary>
        private Storyboard _storyboardShow;
        /// <summary>
        /// The _auto hide button
        /// </summary>
        private Image _autoHideButton;

        /// <summary>
        /// The root height
        /// </summary>
        private double rootHeight;
        /// <summary>
        /// The _BTN shrink
        /// </summary>
        private Image _btnShrink;
        /// <summary>
        /// The _BTN shrink2
        /// </summary>
        private Image _btnShrink2;
        /// <summary>
        /// The _can hide
        /// </summary>
        private bool _canHide;
        /// <summary>
        /// The menus property
        /// </summary>
        public static readonly DependencyProperty MenusProperty = DependencyProperty.Register("Menus", typeof(Dictionary<string, object>), typeof(IMenus), new PropertyMetadata(new PropertyChangedCallback(OnMenusPropertyChanged)));
        /// <summary>
        /// The viewless offset property
        /// </summary>
        public static readonly DependencyProperty ViewlessOffsetProperty = DependencyProperty.Register("ViewlessOffset", typeof(double), typeof(IMenus), new PropertyMetadata(-38.0, OnViewlessOffsetPropertyChanged));
        /// <summary>
        /// The viewport offset property
        /// </summary>
        public static readonly DependencyProperty ViewportOffsetProperty = DependencyProperty.Register("ViewportOffset", typeof(double), typeof(IMenus), new PropertyMetadata(0.0, OnViewportOffsetPropertyChanged));
        /// <summary>
        /// The head height
        /// </summary>
        public static readonly DependencyProperty headHeight = DependencyProperty.Register("HeadHeight", typeof(double), typeof(TKS.Controls.IMenus), new PropertyMetadata(OnHeadHeightChanged));
        /// <summary>
        /// The head information
        /// </summary>
        public static readonly DependencyProperty headInfo = DependencyProperty.Register("HeadInfo", typeof(UIElement), typeof(IMenus), new PropertyMetadata(OnHeadInfoChanged));

        /// <summary>
        /// do when menu is clicked
        /// </summary>
        public event MouseEventHandler ButtonClick;

        /// <summary>
        /// do when  the menu panel is fixed
        /// </summary>
        public event EventHandler FixedClick;

        /// <summary>
        /// do when  the menu panel is not  fixed
        /// </summary>
        public event EventHandler NoFixedClick;
        /// <summary>
        /// The _menuItem width
        /// </summary>
        private static readonly double _menuWidth = 100;
        /// <summary>
        /// The _menuItem height
        /// </summary>
        private static readonly double _menuHeight = 110;

        /// <summary>
        /// Initializes a new instance of the <see cref="IMenus"/> class.
        /// </summary>
        public IMenus()
        {
            this.DefaultStyleKey = typeof(IMenus);
        }

        /// <summary>
        /// Determines whether [is have children menu].
        /// </summary>
        /// <returns><c>true</c> if [is have children menu]; otherwise, <c>false</c>.</returns>
        bool IsHaveChildrenMenu()
        {
            foreach (Dictionary<string, object> item in _menuList.Values)
            {

                if (item != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Analyzes the menu.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="scroll">The scroll.</param>
        private void AnalyzeMenu(StackPanel root, ScrollViewer scroll)
        {
            menuInfo.Clear();

            if (_menuList != null&&root !=null &&scroll !=null )
            {

                if (IsHaveChildrenMenu())
                {
                    //创建树目录
                    CreateTreeMenu(root, scroll);
                }
                else
                {
                    //创建图形目录
                    CreateImgMenu(root, scroll);
                }
            }
        }
        /// <summary>
        /// Resources the start up.
        /// </summary>
        /// <param name="uri">The URI.</param>
        private static void ResourceStartUp(string uri)
        {
            ResourceDictionary resourceDictionary = new ResourceDictionary();
            Application.LoadComponent(resourceDictionary, new Uri(uri, UriKind.Relative));
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        #region 创建树形菜单
        /// <summary>
        /// Creates the tree menu.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="scroll">The scroll.</param>
        void CreateTreeMenu(StackPanel root, ScrollViewer scroll)
        {
           

            root.Children.Clear();
            TreeViewItem t = new TreeViewItem();
            RetriveChildMenu(_menuList, ref t);
            TreeView tr = new TreeView();

            //递归出来的树多了一层空目录，这里去掉
            List<TreeViewItem> menus = new List<TreeViewItem>();
            foreach (TreeViewItem item in t.Items)
            {
                menus.Add(item);
            }

            t.Items.Clear();
            for (int i = 0; i < menus.Count; i++)
            {

                tr.Items.Add(menus[i]);
            }
          
            tr.BorderThickness = new Thickness(0);


            double _tempHeight = MenuAreaHeight();
            scroll.Height = _tempHeight;
            tr.VerticalAlignment = VerticalAlignment.Top;
            root.VerticalAlignment = VerticalAlignment.Top;
            root.Children.Add(tr);
        }

        /// <summary>
        /// Creates the tree menu item.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>StackPanel.</returns>
        StackPanel CreateTreeMenuItem(string value)
        {
            if (value != "")
            {
                string name = "";
                string imgPath = "";
                string url = "";
                string[] s = value.Split(new char[] { '#' });
                if (s.Length == 1)
                {
                    name = s[0];
                    imgPath = "";
                    url = "";
                }
                if (s.Length == 2)
                {
                    name = s[0];
                    imgPath = s[1];
                    url = "";
                }
                else if (s.Length > 3)
                {
                    name = s[0];
                    imgPath = s[1];
                    url = s[2];
                }

                StackPanel p = new StackPanel();
                p.Orientation = Orientation.Horizontal;
                p.HorizontalAlignment = HorizontalAlignment.Stretch;
                Image img = new Image()
                {
                    Source = new BitmapImage(new Uri(imgPath, UriKind.RelativeOrAbsolute))
                    ,
                    Height = 20
                    ,
                    Width = 20
                    ,
                    Margin = new Thickness(5, 0, 5, 0)
                };
                TextBlock tx = new TextBlock();
                tx.Text = name;
                p.Children.Add(tx);
                p.Tag = url;

                return p;
            }
            else
            {
                string[] s = new string[1];
                s[0] = "初始化错误";
                string imgPath = "/TKS.Controls.ImgMenu;component/error.png";
                StackPanel p = new StackPanel();
                p.Orientation = Orientation.Horizontal;
                Image img = new Image()
                {
                    Source = new BitmapImage(new Uri(imgPath, UriKind.RelativeOrAbsolute))
                    ,
                    Height = 20
                    ,
                    Width = 20
                    ,
                    Margin = new Thickness(5, 0, 5, 0)
                };
                TextBlock tx = new TextBlock();
                tx.Text = s[0];
                p.Children.Add(tx);


                return p;
            }


        }



        /// <summary>
        /// Retrives the child menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="tree">The tree.</param>
        void RetriveChildMenu(Dictionary<string, object> menu, ref TreeViewItem tree)
        {
            foreach (KeyValuePair<string, object> pair in menu)
            {

                if (pair.Value != null)
                {

                    TreeViewItem treeitem = tree;
                   
                    TreeViewItem temp = new TreeViewItem();
                    StackPanel menuitem = CreateTreeMenuItem(pair.Key);
                    temp.Header = menuitem;
                    if (!string.IsNullOrEmpty(menuitem.Tag.ToString()))
                        temp.Selected += temp_Selected;
                    treeitem.Items.Add(temp);
                    RetriveChildMenu(pair.Value as Dictionary<string, object>, ref temp);


                }
                else
                {
                    StackPanel menuitem = CreateTreeMenuItem(pair.Key);
                    TreeViewItem temp = new TreeViewItem();
                    temp.Header = menuitem;
                    if (!string.IsNullOrEmpty(menuitem.Tag.ToString()))
                        temp.Selected += temp_Selected;

                    tree.Items.Add(temp);
                   
                }


            }

        }

        /// <summary>
        /// Handles the Selected event of the temp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void temp_Selected(object sender, RoutedEventArgs e)
        {

            TreeViewItem s = sender as TreeViewItem;
            StackPanel p = s.Header as StackPanel;

            if (ButtonClick != null)
            {
                ButtonClick(p.Tag.ToString(), null);
            }
        }


        #endregion
        /// <summary>
        /// Creates the img menu.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="scroll">The scroll.</param>
        private void CreateImgMenu(StackPanel root, ScrollViewer scroll)
        {
            int columns = 2;
            root.Children.Clear();
            List<Grid> btns = new List<Grid>();
            foreach (string str in _menuList.Keys)
            {
                if (str != "")
                {

                    string[] s = str.Split(new char[] { '#' });
                    if (s.Length < 4)
                    {
                        s = new string[4];
                        s[0] = "初始化错误";
                        s[1] = "/TKS.Controls.ImgMenu;component/error.png";
                        s[2] = "";
                        s[3] = "";
                    }
                    else if (s.Length >= 4)
                    {
                        if (s[1].Trim() == "")
                            s[1] = "/TKS.Controls.ImgMenu;component/default.png";
                    }
                    menuInfo.Add(s[0], s[3]);
                    btns.Add(MakeButton(s[0], s[1], s[2]));
                }
            }
            int j = 0;
            Grid gitem = MakeGrid(1, columns, _menuHeight, _menuWidth);
            for (int i = 0; i < btns.Count; i++)
            {
                if (j == columns)
                {
                    root.Children.Add(gitem);
                    gitem = MakeGrid(1, columns, _menuHeight, _menuWidth);
                    j = 0;
                }

                gitem.Children.Add(btns[i]);
                btns[i].SetValue(Grid.ColumnProperty, j);
                j++;
            }
            if (btns.Count / columns != 0)
            {
                root.Children.Add(gitem);
            }
            else if (btns.Count == 1)
            {
                root.Children.Add(gitem);
            }

            root.Width = _menuWidth * columns;

            double _tempHeight = MenuAreaHeight();
            rootHeight = _tempHeight;
            scroll.Height = _tempHeight;
        }

        /// <summary>
        /// Menus the height of the area.
        /// </summary>
        /// <returns>System.Double.</returns>
        private double MenuAreaHeight()
        {
            return LeftPanel.ActualHeight - Head.ActualHeight - 100;
        }



        /// <summary>
        /// Makes the grid.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="rowHeight">Height of the row.</param>
        /// <param name="columnWidth">Width of the column.</param>
        /// <returns>Grid.</returns>
        private static Grid MakeGrid(int rows, int columns, double rowHeight, double columnWidth)
        {
            Grid gitem = new Grid();
            gitem.Width = columnWidth * columns + 3;
            gitem.Height = rowHeight * rows;
            gitem.HorizontalAlignment = HorizontalAlignment.Center;
            gitem.Margin = new Thickness(1.0, 2.0, 1.0, 2.0);

            for (int i = 0; i < rows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(rowHeight);
                gitem.RowDefinitions.Add(row);
            }

            for (int i = 0; i < columns; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(columnWidth);
                gitem.ColumnDefinitions.Add(column);
            }

            return gitem;
        }
        /// <summary>
        /// Changes the state of the visual.
        /// </summary>
        /// <param name="stateBool">if set to <c>true</c> [state bool].</param>
        public void ChangeVisualState(bool stateBool)
        {
            if (stateBool)
            {
                RightArrow.Visibility = Visibility.Collapsed;
            }
            else
            {
                RightArrow.Visibility = Visibility.Visible;
            }
            VisualStateManager.GoToState(this, stateBool ? "ShowFloatMenu" : "HideFloatMenu", true);
        }
        /// <summary>
        /// Makes the button.
        /// </summary>
        /// <param name="buttonContent">Content of the button.</param>
        /// <param name="imgPath">The img path.</param>
        /// <param name="url">The URL.</param>
        /// <returns>Grid.</returns>
        private Grid MakeButton(string buttonContent, string imgPath, string url)
        {
            Image img = new Image();
            img.Width = 48;
            img.Height = 48;
            img.Source = new BitmapImage(new Uri(imgPath, UriKind.RelativeOrAbsolute));
            img.Visibility = Visibility.Visible;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.HorizontalAlignment = HorizontalAlignment.Center;
            var block = new TextBlock
            {
                Height = 40,
                Width = _menuWidth,
                FontFamily = new FontFamily("Microsoft YaHei"),
                FontSize = 11.0,
                Foreground = new SolidColorBrush(Colors.Black),
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Text = buttonContent
            };
            Grid gitem = MakeGrid(2, 1, _menuHeight / 2, _menuWidth);
            // gitem.Background = new SolidColorBrush(Colors.White);
            gitem.Tag = url;

            gitem.Children.Add(img);
            img.SetValue(Grid.RowProperty, 0);

            gitem.Children.Add(block);
            block.SetValue(Grid.RowProperty, 1);

            Grid panel = gitem;
            panel.Cursor = Cursors.Hand;
            panel.MouseLeftButtonDown += panel_MouseLeftButtonDown;
            panel.MouseEnter += panel_MouseEnter;
            panel.MouseLeave += panel_MouseLeave;

            return panel;
        }

        /// <summary>
        /// Handles the MouseLeave event of the panel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        void panel_MouseLeave(object sender, MouseEventArgs e)
        {
            ClearBg();
        }


        /// <summary>
        /// Handles the MouseEnter event of the panel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        void panel_MouseEnter(object sender, MouseEventArgs e)
        {
            ClearBg();

            Grid btn = sender as Grid;
            ImageBrush img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/bg.png", UriKind.Relative));
            btn.Background = img;
            string key = (btn.Children[1] as TextBlock).Text;
            if (menuInfo.ContainsKey(key))
            {
                Info.Text = menuInfo[key];
            }

        }

        /// <summary>
        /// Clears the debug.
        /// </summary>
        private void ClearBg()
        {
            for (int i = 0; i < Root.Children.Count; i++)
            {
                if (Root.Children[i] is Grid)
                {
                    Grid item = Root.Children[i] as Grid;
                    for (int j = 0; j < item.Children.Count; j++)
                    {
                        if (item.Children[j] is Grid)
                        {
                            Grid button = item.Children[j] as Grid;
                            SolidColorBrush b = new SolidColorBrush(Colors.White);
                            button.Background = b;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Handles the MouseLeftButtonDown event of the panel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void panel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;
            MouseEventHandler buttonClick = ButtonClick;
            if (buttonClick != null)
            {
                buttonClick(g.Tag.ToString(), e);
            }
        }

        /// <summary>
        /// 在派生类中重写后，每当应用程序代码或内部进程（如重新生成布局处理过程）调用 <see cref="M:System.Windows.Controls.Control.ApplyTemplate" />，都将调用此方法。简而言之，这意味着就在 UI 元素在应用程序中显示前调用该方法。有关更多信息，请参见“备注”。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Root = GetTemplateChild("Root") as StackPanel;
            Scroll = GetTemplateChild("scrolls") as ScrollViewer;
            Head = GetTemplateChild("Head") as StackPanel;
            LeftPanel = GetTemplateChild("leftPanel") as Border;
            _autoHideButton = GetTemplateChild("autoHideButton") as Image;
            _btnShrink = GetTemplateChild("btnShrink") as Image;
            _btnShrink2 = GetTemplateChild("btnShrink2") as Image;
            Info = GetTemplateChild("Info") as TextBlock;
            Menu = GetTemplateChild("menu") as Border;
            var txtUser = GetTemplateChild("txtUser") as TextBlock;
            var txtMenu = GetTemplateChild("txtMenuName") as TextBlock;
          
            txtMenu.Text = "功能选项";
            txtMenu.Foreground = new SolidColorBrush(Colors.White);
            txtUser.Text = "用户信息";
            txtUser.Foreground = new SolidColorBrush(Colors.White);
            if (LeftPanel != null)
            {
                IList visualStateGroups = VisualStateManager.GetVisualStateGroups(this.LeftPanel);
                {
                    IList list2 = null;
                    foreach (VisualStateGroup group in visualStateGroups)
                    {
                        if (group.Name == "CommonStates")
                        {
                            list2 = group.States;
                            break;
                        }
                    }
                    if (list2 != null)
                    {
                        foreach (VisualState state in list2)
                        {
                            string str = state.Name;
                            if (str != null)
                            {
                                if (str != "ShowFloatMenu")
                                {
                                    if (str == "HideFloatMenu")
                                    {
                                        _storyboardHide = state.Storyboard;
                                    }
                                }
                                else
                                {
                                    _storyboardShow = state.Storyboard;
                                }
                            }
                        }
                    }
                }
            }
            _floatMenuTranslate = GetTemplateChild("floatMenuTranslate") as TranslateTransform;
            if (_floatMenuTranslate != null)
            {
                _floatMenuTranslate.X = ViewlessOffset;
            }
            //auto hidden button
            InitAutoButton(_autoHideButton);
            //shrink button
            InitBtnShrink(_btnShrink);
            InitBtnShrink2(_btnShrink2);
            //left panel arrow button
            InitRightArrowImgButton();
            SetStoryboardValue();
           // AnalyzeMenu(Root, Scroll);
        }

        /// <summary>
        /// Initializes the right arrow img button.
        /// </summary>
        private void InitRightArrowImgButton()
        {
            RightArrow = GetTemplateChild("rightArrow") as Image;
            RightArrow.Source = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/menuon.png", UriKind.Relative));
            ToolTipService.SetToolTip(RightArrow, "点击弹出菜单");
            RightArrow.Cursor = Cursors.Hand;
            //click the right arrow button then pop up the menu panel
            RightArrow.MouseLeftButtonDown += RightArrow_MouseLeftButtonDown;
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the RightArrow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void RightArrow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChangeVisualState(true);
        }


        /// <summary>
        /// Initializes the automatic button.
        /// </summary>
        /// <param name="img">The img.</param>
        void InitAutoButton(Image img)
        {
            img.Source = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/autohide.png", UriKind.Relative));
            img.MouseLeftButtonDown += img_MouseLeftButtonDown;
            ToolTipService.SetToolTip(img, "自动隐藏");
            _canHide = true;
        }
        /// <summary>
        /// The is shrink
        /// </summary>
        private bool isShrink;
        /// <summary>
        /// The is shrink2
        /// </summary>
        private bool isShrink2;

        /// <summary>
        /// Initializes the BTN shrink.
        /// </summary>
        /// <param name="shrink">The shrink.</param>
        void InitBtnShrink(Image shrink)
        {
            shrink.Source = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/ss01.png", UriKind.Relative));
            shrink.MouseLeftButtonDown += shrink_MouseLeftButtonDown;
            isShrink = true;
        }

        /// <summary>
        /// Initializes the BTN shrink2.
        /// </summary>
        /// <param name="shrink2">The shrink2.</param>
        void InitBtnShrink2(Image shrink2)
        {
            shrink2.Source = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/ss01.png", UriKind.Relative));
            shrink2.MouseLeftButtonDown += shrink2_MouseLeftButtonDown;
            isShrink2 = true;
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the shrink2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void shrink2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (isShrink2)
            {
                isShrink2 = false;
                img.Source = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/ss02.png", UriKind.Relative));
                if (Root != null)
                    Root.Height = 0;
            }
            else
            {
                isShrink2 = true;
                img.Source = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/ss01.png", UriKind.Relative));
                if (Root != null)
                {
                    Root.Height = rootHeight;
                }
            }
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the shrink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void shrink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (isShrink)
            {
                isShrink = false;
                img.Source = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/ss02.png", UriKind.Relative));
                if (Head != null)
                    Head.Height = 0;
            }
            else
            {
                isShrink = true;
                img.Source = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/ss01.png", UriKind.Relative));
                if (Head != null)
                {
                    Head.Height = HeadHeight;
                }
            }
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the img control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (_canHide)
            {
                _canHide = false;
                img.Source = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/autohideleft.png", UriKind.Relative));
                if (FixedClick != null)
                {
                    FixedClick(sender, e);
                }
            }
            else
            {
                _canHide = true;
                img.Source = new BitmapImage(new Uri("/TKS.Controls.ImgMenu;component/autohide.png", UriKind.Relative));
                if (NoFixedClick != null)
                {
                    NoFixedClick(sender, e);
                }
            }
        }

        /// <summary>
        /// Called when [menus property changed].
        /// </summary>
        /// <param name="d">The command.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnMenusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = d as IMenus;
            if (menu != null)
            {
                menu._menuList = e.NewValue as Dictionary<string, object>;
                if (menu.Root != null && menu.Scroll != null)
                    menu.AnalyzeMenu(menu.Root, menu.Scroll);

            }
        }

        /// <summary>
        /// Called when [viewless offset property changed].
        /// </summary>
        /// <param name="d">The command.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnViewlessOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = d as IMenus;
            if (menu != null)
            {
                if (menu._storyboardShow != null)
                {
                    ((menu._storyboardShow.Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[0] as SplineDoubleKeyFrame).Value = (double)e.NewValue;
                }
                if (menu._storyboardHide != null)
                {
                    ((menu._storyboardHide.Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[0] as SplineDoubleKeyFrame).Value = (double)e.NewValue;
                }
                if (menu._floatMenuTranslate != null)
                {
                    menu._floatMenuTranslate.X = (double)e.NewValue;
                }
            }
        }
        /// <summary>
        /// Called when [viewport offset property changed].
        /// </summary>
        /// <param name="d">The command.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnViewportOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = d as IMenus;
            if (menu != null)
            {
                if (menu._storyboardShow != null)
                {
                    (((DoubleAnimationUsingKeyFrames)menu._storyboardShow.Children[0]).KeyFrames[1] as SplineDoubleKeyFrame).Value = (double)e.NewValue;
                }
                if (menu._storyboardHide != null)
                {
                    (((DoubleAnimationUsingKeyFrames)menu._storyboardHide.Children[0]).KeyFrames[0] as SplineDoubleKeyFrame).Value = ((double)e.NewValue);
                }
            }
        }
        /// <summary>
        /// Called when [head information changed].
        /// </summary>
        /// <param name="d">The command.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnHeadInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IMenus menu = d as IMenus;
            if (menu != null)
            {
                if (menu.Head != null)
                {
                    (menu.Head.Children[0] as UserControl).Content = e.NewValue as UIElement;
                }

            }
        }

        /// <summary>
        /// Called when [head height changed].
        /// </summary>
        /// <param name="d">The command.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnHeadHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IMenus menu = d as IMenus;
            if (menu != null)
                if (menu.Head != null)
                    menu.Head.Height = double.Parse(e.NewValue.ToString());
        }

        /// <summary>
        /// Roots the mouse enter.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void RootMouseEnter(object sender, MouseEventArgs e)
        {
            // if(_canHide)
            ChangeVisualState(true);
        }
        /// <summary>
        /// Roots the mouse leave.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void RootMouseLeave(object sender, MouseEventArgs e)
        {
            if (_canHide)
            {
                ChangeVisualState(false);
            }
        }
        /// <summary>
        /// Sets the storyboard value.
        /// </summary>
        private void SetStoryboardValue()
        {
            if (_storyboardShow != null && _storyboardHide != null)
            {
                ((_storyboardShow.Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[1] as SplineDoubleKeyFrame).Value = this.ViewportOffset;
                ((_storyboardHide.Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[0] as SplineDoubleKeyFrame).Value = this.ViewportOffset;
                ((_storyboardShow.Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[0] as SplineDoubleKeyFrame).Value = this.ViewlessOffset;
                ((_storyboardHide.Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[1] as SplineDoubleKeyFrame).Value = this.ViewlessOffset;
            }
        }
        /// <summary>
        /// Gets or sets the menus.
        /// </summary>
        /// <value>The menus.</value>
        public Dictionary<string, object> Menus
        {
            get
            {
                return (Dictionary<string, object>)GetValue(MenusProperty);
            }
            set
            {
                SetValue(MenusProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets the root.
        /// </summary>
        /// <value>The root.</value>
        private StackPanel Root
        {
            get
            {
                return _root;
            }
            set
            {
                _root = value;
            }
        }

        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        private TextBlock Info
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>The menu.</value>
        private Border Menu
        {
            get { return _menu; }
            set
            {
                if (_menu != null)
                {
                    _menu.MouseEnter -= RootMouseEnter;
                    _menu.MouseLeave -= RootMouseLeave;

                }
                _menu = value;
                if (_menu != null)
                {
                    _menu.MouseEnter += RootMouseEnter;
                    _menu.MouseLeave += RootMouseLeave;
                }
            }
        }

        /// <summary>
        /// Gets or sets the left panel.
        /// </summary>
        /// <value>The left panel.</value>
        private Border LeftPanel
        {
            get
            { return _leftPanel; }
            set
            {
                _leftPanel = value;
            }
        }
        /// <summary>
        /// Gets or sets the head.
        /// </summary>
        /// <value>The head.</value>
        private StackPanel Head
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scroll.
        /// </summary>
        /// <value>The scroll.</value>
        private ScrollViewer Scroll
        {
            get
            {
                return _scroll;
            }
            set
            {
                _scroll = value;
            }
        }

        /// <summary>
        /// Gets or sets the viewless offset.
        /// </summary>
        /// <value>The viewless offset.</value>
        public double ViewlessOffset
        {
            get
            {
                return (double)GetValue(ViewlessOffsetProperty);
            }
            set
            {
                SetValue(ViewlessOffsetProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets the viewport offset.
        /// </summary>
        /// <value>The viewport offset.</value>
        public double ViewportOffset
        {
            get
            {
                return (double)GetValue(ViewportOffsetProperty);
            }
            set
            {
                SetValue(ViewportOffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the height of the head.
        /// </summary>
        /// <value>The height of the head.</value>
        public double HeadHeight
        {
            get
            {
                return (double)GetValue(headHeight);
            }
            set
            {
                SetValue(headHeight, value);
            }
        }
        /// <summary>
        /// Gets or sets the head information.
        /// </summary>
        /// <value>The head information.</value>
        public UIElement HeadInfo
        {
            get
            {
                return GetValue(headInfo) as UIElement;
            }
            set
            {
                SetValue(headInfo, value);
            }
        }



        /// <summary>
        /// Gets or sets the right arrow.
        /// </summary>
        /// <value>The right arrow.</value>
        public Image RightArrow { get; set; }
    }
}
