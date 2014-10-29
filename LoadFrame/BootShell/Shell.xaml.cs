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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TKS.Common;

namespace BootShell
{
    [Export]
    public partial class Shell : UserControl, IPartImportsSatisfiedNotification
    {
        public enum ApplicationState
        {
            IdleState,
            InfoPrintState,
        }

        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;

        [Export]
        public object Token;
    

        public Shell()
        {
            InitializeComponent();
            Token = AppStorage.GetSessionValue("token");
        }

        

        public void OnImportsSatisfied()
        {
          
        }
    }
}