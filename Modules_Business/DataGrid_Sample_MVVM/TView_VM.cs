using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TKS.Common.MVVMLite;
using TKS.Common.MVVMLite.Command ;
using TKS.Common.Navigator;

namespace DataGrid_Sample_MVVM
{
    public class TView_VM : ViewModelBase
    {
        public TView_VM()
        {

            NavCommand = new RelayCommand(NavTo);
        }

        private void NavTo()
        {
            Navigator.Default.NavigateTo(new DataGrid_Sample_MVVM.DView_Sample());
        }

        public RelayCommand NavCommand
        {
            get;
            set;
        }
    }
}
