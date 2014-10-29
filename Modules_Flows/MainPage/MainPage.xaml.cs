using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using TKS.Controls;
using TKS.Core;
using TKS.Common;
using TKS.Common.Navigator;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Modularity;

namespace MainPage
{

    [Export]
    public partial class MainPage : UserControl, INavigationAware
    {
        [Import]
        public IRegionManager TheRegionManager { private get; set; }

        [Export]
        public string MainRegion = "Main";

        [Export]
        public Action<object> DoNavigate;

        private void target(object obj)
        {
            var s = TheRegionManager.Regions[MainRegion].Context as string[];
            TheRegionManager.Regions[MainRegion].RequestNavigate(new Uri(s[1], UriKind.Relative));
        }
        AssemblyDownloader downloader = new AssemblyDownloader();
        [ImportingConstructor]
        public MainPage([Import]IModuleManager ModuleManager, IRegionManager regionManager)
        {

            InitializeComponent();
            InitMenus();
            TheRegionManager = regionManager;
            // InitProxy();

            UserInfo info = new UserInfo();
            leftmenu.HeadInfo = info;

          
            DoNavigate = target;

            downloader.ModuleManager = ModuleManager;
            downloader.OnDownloading += Navigating;
            downloader.OnDownloadCompleted += MainPage_OnDownloadCompleted;
            downloader.ReadyNavigate += MainPage_ReadyNavigate;

            leftmenu.ButtonClick += leftmenu_ButtonClick;
            // QuickStart quickStart = new QuickStart(contentFrame, this.bubbleControl, proxy);
            //this.bubbleControl.HeadInfo = quickStart;
            this.mainmenu.LayoutUpdated += mainmenu_LayoutUpdated;
            this.SizeChanged += MainPage_SizeChanged;

            Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);

            //HiddenElements();
        }

        void MainPage_ReadyNavigate(object sender, DownloadCommpletedEventArgs e)
        {
           
            TheRegionManager.Regions[MainRegion].RequestNavigate(new Uri(e.Module.Uri, UriKind.Relative));
        }

        void MainPage_OnDownloadCompleted(object sender, DownloadCommpletedEventArgs e)
        {
            CommonBusy.IsNotBusy();
        }

        private void Navigating(object sender, DownloadingEventArgs e)
        {
            TheRegionManager.Regions[MainRegion].Context = new string[2] { e.Module.ViewName, e.Module.Uri };
            CommonBusy.IsBusy("正在加载模块" + e.ModuleInfo.ModuleName + "中，" + e.BytesReceived + "/" + e.TotalBytesToReceive);
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            foreach (Button item in mainmenu.Children)
            {
                MenuEntity dic = item.Tag as MenuEntity;
                string path = "";
                string totalpath = "";

                GetPath(dic, navigationContext.Uri.ToString(), ref path, ref totalpath);

                if (!string.IsNullOrEmpty(totalpath))
                {
                    item.Foreground = new SolidColorBrush(Color.FromArgb(255, 45, 178, 251));
                    this.contentPath.Text = "主页" + totalpath;

                    if (leftmenu.Menus == null)
                    {
                        Dictionary<string, object> child = new Dictionary<string, object>();
                        RetriveChildMenu(dic, ref child);
                        var childmenu = child[dic.MenuName] as Dictionary<string, object>;
                        leftmenu.Menus = childmenu;

                    }
                }
                else
                {
                    item.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                }
            }
        }
        private void Content_FullScreenChanged(object sender, EventArgs e)
        {
            if (Application.Current.Host.Content.IsFullScreen)
            {
                this.FullScreen.Content = "退出全屏";
            }
            else
            {
                this.FullScreen.Content = "全屏";
            }

        }

        void leftmenu_ButtonClick(object sender, MouseEventArgs e)
        {
            downloader.Navigate(new Uri(sender.ToString(), UriKind.Relative));
        }
        void HiddenElements()
        {
            this.headrow.Height = new GridLength(0);
            this.leftmenu.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// 推送信息代理
        /// </summary>
        NetTcpProxy proxy;
        EventHandler<ProxyEventArgs> hand = new EventHandler<ProxyEventArgs>(ProxyEventCall);
        EventHandler<ProxyCallBackEventArgs> handCallback = new EventHandler<ProxyCallBackEventArgs>(ProxyEventCallBack);

        private static void ProxyEventCallBack(object sender, ProxyCallBackEventArgs e)
        {
            switch (e.callbackType)
            {
                case CallBackType.Receive:
                    Receive(e.person, e.message);
                    break;
                case CallBackType.UserLeave:
                    UserLeave(e.person);
                    break;
            }
        }
        private static void ProxyEventCall(object sender, ProxyEventArgs e)
        {
            //Message.InfoMessage(e.list .Count().ToString() );
        }

        void InitProxy()
        {
            proxy = NetTcpProxy.GetInstance();
            PushInfoEntity person = new PushInfoEntity();
            person.Name = AppStorage.GetSessionValue("LoginName") == null ? "" :
                AppStorage.GetSessionValue("LoginName").ToString() + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            person.UserFlag = "001";

            proxy.ProxyEvent -= hand;
            proxy.ProxyEvent += hand;
            proxy.ProxyCallBackEvent -= handCallback;
            proxy.ProxyCallBackEvent += handCallback;
            proxy.Connect(person);

        }

        static void UserLeave(PushInfoEntity pushInfoEntity)
        {

        }

        static void Receive(PushInfoEntity person, string msg)
        {
            Message.InfoMessage(msg);
        }



        void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetPopup();
        }

        #region 导航事件
        private void ContentFrame_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            CommonBusy.IsNotBusy();
            e.Handled = true;

            Message.ErrorMessage("加载页面" + e.Uri + "错误", e.Exception);
        }

        void GetPath(MenuEntity oldmenu, string findpath, ref string path, ref string pointpath)
        {

            if (oldmenu.ChildMenu != null)
            {
                List<MenuEntity> childmenu = oldmenu.ChildMenu;

                path = path + ">>" + oldmenu.MenuName;


                for (int i = 0; i < childmenu.Count; i++)
                {


                    GetPath(childmenu[i], findpath, ref path, ref pointpath);
                    if (i == childmenu.Count - 1 && string.IsNullOrEmpty(pointpath) && childmenu.Count >= 2)
                    {
                        path = path.Substring(0, path.LastIndexOf(">>"));
                    }
                }
            }
            else
            {
                if (oldmenu.MenuUrl == findpath)
                {
                    path += ">>" + oldmenu.MenuName;
                    pointpath = path;
                    path = "";
                }


            }
        }






        #endregion

        #region 导航栏菜单
        private static readonly object _lock = new object();
        void mainmenu_LayoutUpdated(object sender, EventArgs e)
        {

            double width;

            if (LayoutRoot.ActualWidth > 600)
            {
                width = LayoutRoot.ActualWidth - 600;
            }
            else
            {
                width = 0;
            }

            if (mainmenu.ActualWidth < 300)
            {
                width = mainmenu.ActualWidth;
            }

            this.OutterBorder.Rect = new Rect(0, 0, width, this.mainmenu.Height);
            this.outterframe.Width = width;

            if (menus.Count == 0)
            {
                Init();
            }
        }
        private int totalButtons = 0;
        private int currentButton = 0;
        private Dictionary<int, double> menus = new Dictionary<int, double>();

        void Init()
        {
            if (mainmenu.ActualWidth > outterframe.Width)
            {
                int i = 0;
                double pos = 0.0;
                foreach (UIElement item in mainmenu.Children)
                {
                    Button btn = item as Button;
                    pos += btn.ActualWidth;
                    menus.Add(i++, pos);
                }
                currentButton = 0;
                totalButtons = menus.Count;
            }
        }

        private void leftArrow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if (currentButton >= totalButtons)
            {
                currentButton = totalButtons - 1;
            }

            if (mainmenu.ActualWidth > outterframe.Width)
            {
                double left = double.Parse(this.mainmenu.GetValue(Canvas.LeftProperty).ToString());
                if (left < 0)
                {
                    this.rightArrow.IsEnabled = false;
                    double to = 0.0;
                    if (currentButton > 1)
                    {
                        to = menus[currentButton - 2];
                    }

                    MenuMove(left, -to, () =>
                    {

                        lock (_lock)
                        {
                            currentButton--;
                        }
                        if (currentButton < 0) currentButton = 0;
                        rightArrow.IsEnabled = true;
                    });

                }
            }

        }

        private void MenuMove(double from, double to, Action call)
        {
            Storyboard story = new Storyboard();
            DoubleAnimation da = new DoubleAnimation();
            Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(da, mainmenu);
            da.From = from;
            da.To = to;
            da.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            story.Children.Clear();
            story.Children.Add(da);
            var handler = new EventHandler[1];

            handler[0] = (o, s) =>
            {
                story.Completed -= handler[0];
                this.mainmenu.SetValue(Canvas.LeftProperty, to);
                story.Stop();
                if (call != null)
                {
                    call();
                }
            };
            story.Completed += handler[0];
            story.Begin();

        }

        private void rightArrow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (mainmenu.ActualWidth > outterframe.Width)
            {

                double chazhi = -(this.mainmenu.ActualWidth - this.outterframe.Width);
                double left = double.Parse(this.mainmenu.GetValue(Canvas.LeftProperty).ToString());
                if (Math.Abs(left) < Math.Abs(chazhi))
                {

                    if (currentButton >= totalButtons)
                        return;
                    this.leftArrow.IsEnabled = false;
                    double to = menus[currentButton];
                    MenuMove(left, -to, () =>
                    {
                        lock (_lock)
                        { currentButton++; }
                        this.leftArrow.IsEnabled = true;
                    });

                }
            }

        }

        Button MakeButton(string content, MenuEntity dicChild, Action call)
        {
            Button btn = new Button();
            btn.Content = content;
            btn.Tag = dicChild;
            btn.Style = this.Resources["menubutton"] as Style;
            var handler = new RoutedEventHandler[1];
            handler[0] = (o, e) =>
            {

                foreach (Button item in mainmenu.Children)
                {
                    item.Foreground = new SolidColorBrush(Colors.Black);
                }

                if (call != null)
                    call();
            };
            btn.Click += handler[0];

            return btn;
        }
        #endregion

        /// <summary>
        /// 初始化菜单
        /// </summary>
        void InitMenus()
        {
            // contentFrame.Navigate(new Uri("WelcomeModule.WelcomePage,WelcomeModule", UriKind.Relative));

            var appName = AppStorage.GetSessionValue("AppName");

            if (appName != null)
                txtappName.Text = appName.ToString();
            var loginUserName = AppStorage.GetSessionValue("LoginName");
            if (loginUserName != null)
            {
                txtUser.Text = loginUserName.ToString();
            }

            List<MenuEntity> menus = AppStorage.GetSessionValue("Menu") as List<MenuEntity>;
            if (menus == null)
            {
                menus = new List<MenuEntity>();
                menus.Add(new MenuEntity { MenuName = "这是树形菜单" });
                menus.Add(new MenuEntity { MenuName = "这是图形菜单" });

                menus[0].ChildMenu = new List<MenuEntity>(){new MenuEntity { MenuName ="test1", ChildMenu=
                new List<MenuEntity>(){ 
                new MenuEntity { MenuName="test2",
                    ChildMenu=new List<MenuEntity>(){new MenuEntity { MenuName="测试A",MenuUrl="TestAModule.MainPage,TestAModule"} 
                    ,new MenuEntity { MenuName="测试B",MenuUrl="hello hello"}
                     ,new MenuEntity { MenuName="测试C",MenuUrl="这个地址是故意输入错误滴"}}
                },
                new MenuEntity {MenuName="test22"
                ,ChildMenu=new List<MenuEntity>(){
                new MenuEntity { MenuName="测试a",MenuUrl="aa"}
                     ,new MenuEntity { MenuName="测试b",MenuUrl="bb"}}
                }

                ,
                new MenuEntity {MenuName="test33"}
                ,
                new MenuEntity {MenuName="test44"}
                ,
                new MenuEntity {MenuName="test55"}
                ,
                new MenuEntity {MenuName="test66"}
                 
                }
                    },
                new MenuEntity { MenuName="test11",MenuUrl="testqq"}};

                menus[1].ChildMenu = new List<MenuEntity>(){
                new MenuEntity { MenuName ="datagrid数据控件", MenuUrl="DataGridManager,DataGridView"}
                ,
                new MenuEntity {MenuName="再欢迎",MenuUrl="WelcomeManager,Page1"}
                ,
                new MenuEntity {MenuName="MVVMdatagrid",MenuUrl="DataGridMVVMManager,DView_Sample"}
                ,
                new MenuEntity {MenuName="欢迎",MenuUrl="WelcomeManager,Welcome?UserId=Peter&Code=880"}
                ,
                new MenuEntity {MenuName="test66",MenuUrl="hahahah"} };
            }
            if (menus != null)
            {
                foreach (MenuEntity menu in menus)
                {
                    Dictionary<string, object> child = new Dictionary<string, object>();

                    RetriveChildMenu(menu, ref child);
                    var childmenu = child[menu.MenuName] as Dictionary<string, object>;
                    Button btntemp = MakeButton(menu.MenuName, menu
                       , () =>
                    {
                        leftmenu.Menus = childmenu;
                        leftmenu.ChangeVisualState(true);
                    });
                    mainmenu.Children.Add(btntemp);

                }
            }



        }

        void RetriveChildMenu(MenuEntity oldmenu, ref Dictionary<string, object> menu)
        {
            if (oldmenu.ChildMenu != null)
            {
                List<MenuEntity> childmenu = oldmenu.ChildMenu;
                Dictionary<string, object> father = menu;

                father.Add(oldmenu.MenuName, new Dictionary<string, object>());
                father = father[oldmenu.MenuName] as Dictionary<string, object>;

                for (int i = 0; i < childmenu.Count; i++)
                {
                    RetriveChildMenu(childmenu[i], ref father);
                }
            }
            else
            {
                menu.Add(oldmenu.MenuName + "#" + oldmenu.ImagePath + "#" +
                            oldmenu.MenuUrl + "#" + oldmenu.Description, null);
            }
        }




        private void loginOut_Click_1(object sender, RoutedEventArgs e)
        {
            TheRegionManager.RequestNavigate
           (
               "MyRegion",
               new Uri("LoginPage", UriKind.Relative),
               a => { }
           );

        }



        private void leftmenu_FixedClick(object sender, EventArgs e)
        {
            contentPanel.SetValue(Frame.MarginProperty, new Thickness { Left = 230, Bottom = 5, Right = 5, Top = 5 });
        }

        private void leftmenu_NoFixedClick(object sender, EventArgs e)
        {
            contentPanel.SetValue(Frame.MarginProperty, new Thickness { Left = 25, Bottom = 5, Right = 5, Top = 5 });
        }




        private void showPopup_Click(object sender, RoutedEventArgs e)
        {
            if (this.bubbleControl.Visibility == Visibility.Collapsed)
            {

                // 设置位置
                SetPopup();
                bubbleControl.Visibility = Visibility.Visible;
            }
            else
            {
                bubbleControl.Visibility = Visibility.Collapsed;
            }
        }

        private void SetPopup()
        {
            GeneralTransform gt = this.showPopup.TransformToVisual(null);
            Point pop = gt.Transform(new Point(-225, -65));
            bubbleControl.Margin = new Thickness(pop.X, pop.Y, 0, 0);
        }

        private void FrameBorder_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.bubbleControl.Visibility = Visibility.Collapsed;
        }


        //全屏
        private void FullScreen_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Interop.Content contentObject = Application.Current.Host.Content;
            contentObject.IsFullScreen = !contentObject.IsFullScreen;
        }


    }
}