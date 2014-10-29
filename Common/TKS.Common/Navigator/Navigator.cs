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
using System.Windows.Controls;

namespace TKS.Common.Navigator
{
    public class Navigator
    {
        private static Navigator _default;
        Frame _frame; 
        public static Navigator Default
        {
            get { return _default ?? (_default=new Navigator()); }
        }

        public  void Initialize(Frame frame)
        {
            if (_frame == null)
                _frame = frame;
        }

        public void NavigateTo<T>(T control) where T : UIElement, new() 
        {
            _frame.Content = control;
        }

       



    }
}
