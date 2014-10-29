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
using TKS.Common;

namespace MainPage
{
    public partial class DetailMenu : ChildWindow
    {
        public DetailMenu()
        {
            InitializeComponent();
        }

        public class AllMenus
        {
            private bool _isChecked = false;
            public bool IsChecked
            {
                get { return _isChecked; }
                set { _isChecked = value; }
            }

            public MenuEntity menu
            {
                get;
                set;
            }

        }

       public  List<MenuEntity> _SelectedMenus = new List<MenuEntity>();
        public DetailMenu(List<MenuEntity> SelectedMenus)
        {
            InitializeComponent();
            List<MenuEntity> menus = AppStorage.GetSessionValue("Menu") as List<MenuEntity>;
            _SelectedMenus = SelectedMenus;
            if (_SelectedMenus == null)
                _SelectedMenus = new List<MenuEntity>();
            foreach (MenuEntity menu in menus)
            {
                TreeViewItem _node1 = new TreeViewItem();
                _node1.Header = menu.MenuName;
                this.MenuTree.Items.Add(_node1);
               
                for (int i = 0; i < menu.ChildMenu.Count; i++)
                {
                    
                    AllMenus _allMenu2 = new AllMenus();
                    _allMenu2.menu = menu.ChildMenu[i];
                    _allMenu2.IsChecked = _SelectedMenus.Any(
                        (p) => p.MenuUrl.Trim() == _allMenu2.menu.MenuUrl.Trim());


                    TreeViewItem _node2 = new TreeViewItem();
                    StackPanel sp2 = new StackPanel();
                    sp2.Orientation = Orientation.Horizontal;
                    CheckBox ck2 = new CheckBox();
                    ck2.Click += ck2_Click;
                    ck2.DataContext = _allMenu2;
                    ck2.IsChecked = _allMenu2.IsChecked;
                    TextBlock tb2 = new TextBlock();
                    tb2.Text = _allMenu2.menu.MenuName;
                    sp2.Children.Add(ck2);
                    sp2.Children.Add(tb2);
                    _node2.Header = sp2;

                    _node1.Items.Add(_node2);
                }
            }
        }

        void ck2_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            AllMenus menu = chk.DataContext as AllMenus;
            if (chk.IsChecked == true)
            {
                menu.IsChecked = true;
                _SelectedMenus.Add(menu.menu);
            }
            else
            {
                menu.IsChecked = false;
                if (_SelectedMenus.Contains(menu.menu))
                    _SelectedMenus.Remove(menu.menu);
            }
        }



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

