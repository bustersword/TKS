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
using TKS.Controls;
namespace MainPage
{


    public partial class QuickStart : UserControl
    {
        private Frame contentFrame;
        private BubbleContainer _bubble;
        private NetTcpProxy _proxy;
        public QuickStart()
        {
            InitializeComponent();
            Init();
        }



        public QuickStart(Frame contentFrame, BubbleContainer bubble, NetTcpProxy proxy)
        {

            this.contentFrame = contentFrame;
            InitializeComponent();
            Init();

            this.list.MouseLeftButtonDown += list_MouseLeftButtonDown;
            this.contentFrame = contentFrame;
            _bubble = bubble;
            _proxy = proxy;
            _bubble.ButtonClick += _bubble_ButtonClick;
        }

        void _bubble_ButtonClick(object sender, MouseEventArgs e)
        {

            List<MenuEntity> selectedMenu = this.list.ItemsSource as List<MenuEntity>;

            DetailMenu add = new DetailMenu(selectedMenu);

            add.Show();
            add.Closed += (p, q) =>
            {
                if (add.DialogResult == true)
                {
                    if (add._SelectedMenus.Count > 0)
                    {
                        this.list.ItemsSource = null;
                        this.list.ItemsSource = add._SelectedMenus;
                        Save(add._SelectedMenus);
                    }
                }
            };


        }


        void list_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock _menu = sender as TextBlock;
            contentFrame.Navigate(new Uri(_menu.Tag.ToString(), UriKind.Relative));
        }
        void Save(object value)
        {
            var loginUserName = AppStorage.GetSessionValue("LoginName");
            if (loginUserName == null)
            {
                return;
            }
            AppStorage.SaveAppsetting(loginUserName + "quickmenu", value);
        }
        void Init()
        {
            var loginUserName = AppStorage.GetSessionValue("LoginName");
            if (loginUserName ==null )
            {
                return;
            }
            object menu=  AppStorage.GetAppStettingObject(loginUserName +"quickmenu");
            if (menu != null)
            {
                this.list.ItemsSource = (List<MenuEntity>)menu;
            }
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            TextBlock item = sender as TextBlock;

            MenuEntity menuety = item.Tag as MenuEntity;
            List<MenuEntity> _selectedMenu = this.list.ItemsSource as List<MenuEntity>;
            if (_selectedMenu.Contains(menuety))
            {
                _selectedMenu.Remove(menuety);
                this.list.ItemsSource = null;
                this.list.ItemsSource = _selectedMenu;
                Save(_selectedMenu);
            }
        }

        private void btnSend_Click_1(object sender, RoutedEventArgs e)
        {
            SendMessage frmSend = new SendMessage();
            frmSend.Show();
            frmSend.Closed += (p,q) => {
                if (frmSend.DialogResult == true)
                {
                    if (frmSend.Message != "")
                    {
                        _proxy.Say(frmSend.Message);
                    }
                }
            };
        }

         

    }
}
