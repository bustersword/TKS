using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MainPageV1
{
    [ModuleExport(typeof(MainViewV1), DependsOnModuleNames = new string[] { "BaseManager" })]
    public class MainViewV1 : IModule
    {
        [Import]
        public IRegionManager TheRegionManager { private get; set; }

        [Import]
        public object Token;

        public void Initialize()
        {

            if (Token.ToString() == "")
            {

                TheRegionManager.RegisterViewWithRegion("MyRegion", typeof(MainPage));
                TheRegionManager.RegisterViewWithRegion("MyRegion", typeof(LoginPage));
            }
            else
            {
                TheRegionManager.RegisterViewWithRegion("MyRegion", typeof(LoginPage));
                TheRegionManager.RegisterViewWithRegion("MyRegion", typeof(MainPage));

            }
        }
    }
}
