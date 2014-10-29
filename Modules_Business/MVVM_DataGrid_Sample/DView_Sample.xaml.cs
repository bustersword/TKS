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

namespace DataGrid_Sample_MVVM
{
    public partial class DView_Sample : UserControl
    {
        public DView_Sample()
        {
            InitializeComponent();
            //this.LayoutRoot.DataContext = new DViewVM();
        }
    }
}
