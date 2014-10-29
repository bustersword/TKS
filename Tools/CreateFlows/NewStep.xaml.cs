using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CreateFlows
{
    /// <summary>
    /// NewStep.xaml 的交互逻辑
    /// </summary>
    public partial class NewStep : Window
    {
        string _title;
        public NewStep(string title)
        {
            InitializeComponent();
            _title = title;
            lblTitle.Content = _title;
        }

        public string Result
        {
            get;
            set;
        }

        private void btnOK_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtAssemblyName.Text))
            {
                MessageBox.Show("请输入"+_title+"名");
            }
            else
            {
                Result = this.txtAssemblyName.Text;
                this.DialogResult = true;
                this.Close();
            }

        }

        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false ;
            this.Close();
        }

    }
}
