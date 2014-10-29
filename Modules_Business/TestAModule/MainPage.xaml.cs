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
using System.Windows.Navigation;
using TKS.Common;
using TKS.Core;
using TKS.Controls;
namespace TestAModule
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Init();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void Download(List<TabMenuItem> needDownloadItems, Action sucessCall)
        {
            CommonBusy.IsBusy("加载tab");
            if (needDownloadItems.Count > 0)
                DeploymentExtensions.DownloadAssemblyAsync(needDownloadItems[0].AssemblyName, (asm) =>
                {
                    bool flag = true;
                    object o=null ;
                    try
                    {
                         o = asm.CreateInstance(needDownloadItems[0].Type);
                    }
                    catch(Exception ex)
                    {
                        flag = false;
                        CommonBusy.IsNotBusy();
                        Message.ErrorMessage("创建" + needDownloadItems[0].Description+"标签页异常", ex);
                    }
                    UserControl item = o as UserControl;

                    if (item == null)
                    {
                        flag = false;
                    }

                    if (flag)
                    {
                        MutiTabControl.ItemSource tabitem = new MutiTabControl.ItemSource();

                        tabitem.Header = needDownloadItems[0].Description;
                        item.Margin = new Thickness(0);
                        item.HorizontalAlignment = HorizontalAlignment.Stretch;
                        tabitem.Content = item;

                        this.sources.Add(tabitem);
                    }
                    needDownloadItems.RemoveAt(0);
                    if (needDownloadItems.Count > 0)
                        Download(needDownloadItems, sucessCall);
                    else
                    {
                        if (sucessCall != null)
                            sucessCall();
                    }

                });
            else
            {
                if (sucessCall != null)
                    sucessCall();
            }
        }
        List<MutiTabControl.ItemSource> sources = new List<MutiTabControl.ItemSource>();

        void Init()
        {
            List<TabMenuItem> tabsConfig = AppStorage.GetTabMenuItems("sb");

            Download(tabsConfig, () =>
            {

                this.tabContainer.BindSource = sources;
                CommonBusy.IsNotBusy();
            });

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            tabContainer.SubmitALL();
        }

    }
}
