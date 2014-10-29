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
using TKS.Core;

namespace TestAModule
{
    public partial class Tab5 : Page
    {
        public Tab5()
        {
            InitializeComponent();
          
            contentFrame.ContentLoader = AssemblyNavigationContentLoader.Default;
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(new Uri("WelcomeModule.WelcomePage,WelcomeModule", UriKind.Relative));
        }

    }
}
