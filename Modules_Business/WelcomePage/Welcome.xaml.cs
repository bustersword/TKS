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
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;

namespace WelcomePage
{
    public partial class Welcome :basePage,INavigationAware
    {
        public Welcome()
        {
            InitializeComponent();
            txtTag.Text = BaseData.Default.Data["A"];
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }






        public bool IsNavigationTarget(Microsoft.Practices.Prism.Regions.NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(Microsoft.Practices.Prism.Regions.NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(Microsoft.Practices.Prism.Regions.NavigationContext navigationContext)
        {
            var u = navigationContext.Uri;
        }
    }
}
