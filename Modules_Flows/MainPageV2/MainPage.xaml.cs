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
using Microsoft.Practices.Prism.Modularity;
using TKS.Core;

namespace MainPageV2
{
    [Export]
    public partial class MainPage : Page, INavigationAware
    {
        [Export("left")]
        public string LeftRegion = "Left";

        [Import]
        public IRegionManager TheRegionManager { private get; set; }
        [Import]
        public IModuleManager ModuleManager;

        [Export("mainFrame")]
        public Frame MainFrame
        {
            get { return this.Main; }
        }

        [ImportingConstructor]
        public MainPage([Import]IModuleManager ModuleManager, IRegionManager regionManager)
        {
            InitializeComponent();
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
            ModuleManager.LoadModule("TRV1");
        }
    }
}
