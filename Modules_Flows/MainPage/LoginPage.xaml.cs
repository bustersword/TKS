using System;
using System.Windows.Controls;
using System.Windows.Input;
using TKS.Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;

namespace MainPage
{
  
    [Export]
    public partial class LoginPage : UserControl ,INavigationAware
    {
        [Import]
        public IRegionManager TheRegionManager { private get; set; }
        public LoginPage()
        {
            InitializeComponent();
           
        }

        //会员登录
        private void LoginImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AppStorage.SetSessionValue("LoginId", "12345");
            AppStorage.SetSessionValue("LoginName", "测试用户");
            AppStorage.SetSessionValue("LoginTrueName", "测试用户");
            TheRegionManager.RequestNavigate
           (
               "MyRegion",
               new Uri("MainPage", UriKind.Relative),
               a => { }
           );
        }

        private void root_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               
                  LoginImage_MouseLeftButtonUp(null, null);
                
            }
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
            this.userName.Text = string.Empty;
            this.password.Password = string.Empty;
        }
    }
}
