using System;
using System.Windows;
using System.Windows.Controls;

namespace TKS.Controls
{
     partial class DetailMessage : ChildWindow
    {
        public DetailMessage()
        {
            InitializeComponent();
        }

        private void OKButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }


        public DetailMessage(Exception e)
        {
            InitializeComponent();
            if (e != null)
            {
                ErrorTextBox.Text =  e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace;
            }
        }
        
    }
}

