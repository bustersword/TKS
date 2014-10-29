using System;
using System.Collections.Generic;
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
using TKS.Controls;

namespace BaseConfigModule
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
           

        }

        public void Load()
        {
            CommonBusy.IsBusy("加载基础资料中。。。");
            ThreadPool.QueueUserWorkItem(new WaitCallback(target));
            
        }

        private void target(object state)
        {
            System.Threading.Thread.Sleep(3000);
            CommonBusy.IsNotBusy();
        }
    }
}