using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TKS.Common;
using TKS.Core;

namespace MainPageV1
{
    [Export]
    public partial class MainPage : UserControl, INavigationAware
    {
        [Export("left")]
        public string LeftRegion = "Left";

        [Import]
        public IRegionManager TheRegionManager { private get; set; }
        [Import]
        public  IModuleManager ModuleManager;

        [Export("main")]
        public string MainRegion = "Main";

       

        [Export]
        public Action<object> DoNavigate;


        private void target(object obj)
        {
            var s = TheRegionManager.Regions[MainRegion].Context as string[];
            TheRegionManager.Regions[MainRegion].RequestNavigate(new Uri(s[1], UriKind.Relative));
        }


        [ImportingConstructor]
        public MainPage([Import]IModuleManager ModuleManager, IRegionManager regionManager)
        {
            InitializeComponent();
            DoNavigate = target;
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
            ModuleManager.LoadModule("TRV1");
        }
    }
}
