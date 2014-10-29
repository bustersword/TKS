using System;
using System.Windows;
using System.Windows.Controls;
using TKS.Controls;
using System.Windows.Browser;

using TKS.Common;
 
using TKS.Core;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Net;
using System.Net.Browser;
namespace LoadFrame
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.Application_Startup;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();

        }


     
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            string config = "";
            e.InitParams.TryGetValue("NavigationWcf", out config);
            HtmlDocument doc = HtmlPage.Document;
            string token = "";
            if (doc.QueryString.ContainsKey("token"))
            {
                token = doc.QueryString["token"];
            }

            CommonBusy.InitBusy(new MainPage());
            this.RootVisual = CommonBusy.BusyContainer;

            AppStorage.WcfConfigAddress = config;
         
        }

      
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // 如果应用程序是在调试器外运行的，则使用浏览器的
            // 一个 ChildWindow 控件。
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // 注意: 这使应用程序可以在已引发异常但尚未处理该异常的情况下
                // 继续运行。 
                // 对于生产应用程序，此错误处理应替换为向网站报告错误
                // 并停止应用程序。
                e.Handled = true;
                Message.ErrorMessage("意外异常，详细信息请联系管理员", e.ExceptionObject);
            }
            else
            {
                e.Handled = true;
                Message.ErrorMessage("意外异常，详细信息请联系管理员", e.ExceptionObject);
            }
        }
    }
}