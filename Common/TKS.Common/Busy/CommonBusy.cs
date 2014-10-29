using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
namespace TKS.Common
{
    /// <summary>
    ///  全局busy 2013-10-15 新增
    /// </summary>
    public static class CommonBusy
    {
        private static BusyIndicator _busy;
        static CommonBusy()
        {
            if (_busy == null)
            {
                _busy = new BusyIndicator();
               
            }

        }

        public static BusyIndicator BusyContainer
        {
            get { return _busy; }
        }

        public static void InitBusy(UIElement content)
        {
            _busy.Content = content;
        }

        public static void IsBusy(string msg)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _busy.BusyContent = msg;
                    _busy.IsBusy = true;
                });
            }
            else
            {
                _busy.BusyContent = msg;
                _busy.IsBusy = true;
            }
           
        }

        public static void IsNotBusy()
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    _busy.IsBusy = false;
                });
            }
            else
            {
                _busy.IsBusy = false;
            }
           
        }
    }
}
