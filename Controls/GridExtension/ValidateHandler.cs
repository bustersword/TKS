using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TKS.Common.GridExtension;

namespace PDA.COP.FW.GridExtension
{
    public static class ValidateHandler
    {
       

        public static void SetValidation(this FrameworkElement frameworkElement, string message)
        {
            CustomizeValidation customValidation = new CustomizeValidation(message);

            Binding binding = new Binding("ValidationError")
            {
                Mode = System.Windows.Data.BindingMode.TwoWay,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
                Source = customValidation
            };
            frameworkElement.SetBinding(Control.TagProperty, binding);
        }

        public static void RaiseValidationError(this FrameworkElement frameworkElement)
        {
            BindingExpression b = frameworkElement.GetBindingExpression(Control.TagProperty);

            if (b != null)
            {
                ((CustomizeValidation)b.DataItem).ShowErrorMessage = true;
                b.UpdateSource();
            }
        }

        public static void ClearValidationError(this FrameworkElement frameworkElement)
        {
            BindingExpression b = frameworkElement.GetBindingExpression(Control.TagProperty);

            if (b != null)
            {
                ((CustomizeValidation)b.DataItem).ShowErrorMessage = false;
                b.UpdateSource();
            }
        }
    }
}
