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

namespace TreeViewV2
{
    [ModuleExport(typeof(TRV1))]
    public class TRV1 : IModule
    {

        IRegionManager _regionManager {   get; set; }

        [Import("left")]
        public string LeftRegion;


        [ImportingConstructor]
        public TRV1([Import]IRegionManager regionManager)
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            _regionManager = regionManager;

        }
        public void Initialize()
        {
           _regionManager.RegisterViewWithRegion("Left", typeof(TreeView));
        }
    }
}
