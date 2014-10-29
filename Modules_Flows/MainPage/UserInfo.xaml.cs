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

namespace MainPage
{
    public partial class UserInfo : Page
    {
        public UserInfo()
        {
            InitializeComponent();
            this.userTxt.Text = AppStorage.GetSessionValue<string>("LoginName");
            this.entNo.Text = AppStorage.GetSessionValue<string>("LoginCompCode");
            this.entName.Text = AppStorage.GetSessionValue<string>("LoginCompName");
            this.relType.Text = AppStorage.GetSessionValue<string>("RelType") == "1" ? "区内企业" : "区外企业";
            this.loginDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }

        public UserInfo(string info)
        {
            InitializeComponent();
            
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
