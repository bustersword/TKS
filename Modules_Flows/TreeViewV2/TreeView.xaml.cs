using Microsoft.Practices.Prism.Modularity;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TKS.Common;
using TKS.Core;

namespace TreeViewV2
{
    public class MenuLink
    {
        public MenuLink()
        {
            Children = new ObservableCollection<MenuLink>();
        }

        public string Name { get; set; }
        public string Link { get; set; }
        public ObservableCollection<MenuLink> Children { get; set; }
    }

    [Export]
    public partial class TreeView : UserControl
    {

        Frame _mainFrame;


        AssemblyDownloader downloader = new AssemblyDownloader();

        [ImportingConstructor]
        public TreeView([Import("mainFrame")]Frame mainFrame, [Import]IModuleManager moduleManager)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            AssemblyNavigationContentLoader contentLoader = new AssemblyNavigationContentLoader();
            contentLoader.ModuleManager = moduleManager;
            contentLoader.OnDownloading += contentLoader_OnDownloading;
            contentLoader.OnDownloadFailed += contentLoader_OnDownloadFailed;
            contentLoader.IsCanload = (uri) =>
            {
                return true;
            };
            _mainFrame.ContentLoader = contentLoader;

           
            _mainFrame.Navigated += MainRegion_Navigated;
            ThreadPool.QueueUserWorkItem(new WaitCallback(target));
        }

        void contentLoader_OnDownloadFailed(object sender, DownloadFaileddEventArgs e)
        {
            CommonBusy.IsNotBusy();
            throw e.Error;
        }

        void contentLoader_OnDownloading(object sender, DownloadingEventArgs e)
        {
            CommonBusy.IsBusy("正在加载模块"+e.ModuleInfo.ModuleName +"["+e.ProgressPercentage+"%]");
        }

        void MainRegion_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            CommonBusy.IsNotBusy();
        }


        private void target(object state)
        {

            CommonBusy.IsBusy("初始化导航...");
            Thread.Sleep(3000);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Bind();
            });
        }

        void Bind()
        {
            CommonBusy.IsNotBusy();
            tvTree.ItemsSource = new ObservableCollection<MenuLink> { 
            new MenuLink { 
                Name = "菜单一", 
                Link =string.Empty ,
                Children = { new MenuLink 
                  { 
                    Name="菜单A",
                    Link =string.Empty ,
                    Children = { 
                        new MenuLink { Name = "Welcome",Link ="WelcomeManager,Welcome?UserId=Peter&Code=880" },
                        new MenuLink { Name= "欢迎，欢迎" ,Link ="WelcomeManager,Page1"}
                    }
                  },
                new MenuLink 
                  { 
                    Name="菜单B", 
                    Children = { 
                        new MenuLink { Name = "datagrid数据控件" ,Link ="DataGridManager,DataGrid_Sample.DataGridView"},
                        new MenuLink { Name= "MVVMdatagrid",Link ="DataGridMVVMManager,DView_Sample" }
                    }
                  }
               }
            },
            new MenuLink { 
                Name = "菜单二", 
                 Link =string.Empty ,
                Children = { new MenuLink 
                  { 
                    Name="菜单C",
                     Link =string.Empty ,
                    Children = { 
                        new MenuLink { Name = "控件样式",Link ="ControlsManager,ControlsSample.Controls" },
                        new MenuLink { Name= "别着急" }
                    }
                  },
                new MenuLink 
                  { 
                    Name="菜单D", 
                     Link =string.Empty ,
                    Children = { 
                        new MenuLink { Name = "再容我想想" },
                        new MenuLink { Name= "说了，别着急" }
                    }
                  }
               }
            }
        };
        }

        private void tvTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            MenuLink menu = e.NewValue as MenuLink;
            if (!string.IsNullOrEmpty(menu.Link))
                _mainFrame.Navigate(new Uri(menu.Link, UriKind.Relative));
        }
    }
}
