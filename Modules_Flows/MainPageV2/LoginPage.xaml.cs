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
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;

namespace MainPageV2
{
    [Export]
    public partial class LoginPage : Page
    {
        [Import]
        public IRegionManager TheRegionManager { private get; set; }
        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            TheRegionManager.RequestNavigate
            (
                "MyRegion",
                new Uri("MainPage", UriKind.Relative),
                a => { }
            );
        }

    }
}
