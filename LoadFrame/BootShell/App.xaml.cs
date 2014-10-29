using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TKS.Common;
using TKS.Controls;
using System.Windows.Browser;
using System.Threading;
using System.Globalization;

namespace BootShell
{

    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
            //Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
            this.Exit += App_Exit;
        }

        void App_Exit(object sender, EventArgs e)
        {
            AppStorage.ClearSession();
        }



        private void Application_Startup(object sender, StartupEventArgs e)
        {

            string config = "";
            e.InitParams.TryGetValue("config", out config);
            HtmlDocument doc = HtmlPage.Document;
            string token = "ABC";
            if (doc.QueryString.ContainsKey("token"))
            {
                token = doc.QueryString["token"];
            }
            AppStorage.SetSessionValue("token", token);
            AppStorage.SetConfig(config);
            var dic = AppStorage.GetAppsettings("appSettings");
            RESTFulClient.RestFulAddress = dic["RestfulAddress"];
            (new QuickBootstrap()).Run();



        }

        void ApplySpace(Action call)
        {
            try
            {

                if (call != null)
                {
                    call();
                }
            }
            catch (Exception ex)
            {
                Message.ErrorMessage("分配空间错误", ex);
            }
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
                CommonBusy.IsNotBusy();
                e.Handled = true;
                Message.ErrorMessage("意外异常，详细信息请联系管理员", e.ExceptionObject);
            }
            else
            {
                CommonBusy.IsNotBusy();
                e.Handled = true;
                Message.ErrorMessage("意外异常，详细信息请联系管理员", e.ExceptionObject);
            }

        }
    }
}