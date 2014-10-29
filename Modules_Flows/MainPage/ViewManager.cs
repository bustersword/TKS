using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;

namespace MainPage
{
    [ModuleExport(typeof(ViewManager), DependsOnModuleNames = new string[] { "BaseManager" })]
    public class ViewManager : IModule
    {
        [Import]
        public IRegionManager TheRegionManager { private get; set; }

        [Import]
        public object Token;

        public void Initialize()
        {

            if (Token.ToString() == "ABC")
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
