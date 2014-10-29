using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TKS.Common;
using TKS.Core;
namespace TreeViewV1
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
        [Import]
        public IRegionManager TheRegionManager;

        [Import("main")]
        public string MainRegion;

        AssemblyDownloader downloader = new AssemblyDownloader();

        [ImportingConstructor]
        public TreeView([Import]IRegionManager regionManager, [Import]IModuleManager ModuleManager)
        {
            InitializeComponent();
            TheRegionManager = regionManager;
            downloader.ModuleManager = ModuleManager;
            downloader.OnDownloading += Navigating;
            downloader.OnDownloadCompleted += MainPage_OnDownloadCompleted;
            downloader.ReadyNavigate += MainPage_ReadyNavigate;
            ThreadPool.QueueUserWorkItem(new WaitCallback(target));
        }

        private void MainPage_ReadyNavigate(object sender, DownloadCommpletedEventArgs e)
        {
           

            TheRegionManager.Regions[MainRegion].RequestNavigate(new Uri(e.Module.Uri, UriKind.Relative));
        }

        private void MainPage_OnDownloadCompleted(object sender, DownloadCommpletedEventArgs e)
        {
            CommonBusy.IsNotBusy();
        }

        private void Navigating(object sender, DownloadingEventArgs e)
        {

            TheRegionManager.Regions[MainRegion].Context = new string[2] { e.Module.ViewName, e.Module.Uri };
            CommonBusy.IsBusy("正在加载模块" + e.ModuleInfo.ModuleName + "中，" + e.BytesReceived + "/" + e.TotalBytesToReceive);

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
                        new MenuLink { Name = "容我想想" },
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
        //兼容旧框架导航地址，转换旧地址DataGrid_Sample.DataGridView==>DataGridView
        string ConvertUri(string targetUri)
        {
            string uri = string.Empty;
            if (!string.IsNullOrEmpty(targetUri))
            {
                string[] s = targetUri.Split(new char[] { ',' });
                if (s.Length != 2)
                {
                    throw new Exception("菜单配置异常");
                }
                string moduleName = s[0];

                if (targetUri.IndexOf('.') != -1)
                {

                    uri = targetUri.Substring(targetUri.LastIndexOf('.') + 1, targetUri.Length - targetUri.LastIndexOf('.') - 1);
                    uri = moduleName + "," + uri;
                }
                else
                {
                    uri = targetUri;
                }

               
            }
            return uri;

        }
        private void tvTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            MenuLink menu = e.NewValue as MenuLink;
            if (!string.IsNullOrEmpty(menu.Link))
            {
                menu.Link = ConvertUri(menu.Link);

                downloader.Navigate(new Uri(menu.Link, UriKind.Relative));
            }
        }


    }
}
