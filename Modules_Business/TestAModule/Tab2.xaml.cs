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
using System.Windows.Navigation;
using System.Windows.Browser;
using TKS.Controls;
namespace TestAModule
{
    public partial class Tab2 : Page,ITab 
    {
        public Tab2()
        {
            InitializeComponent();
            this.up.UploadUri = new Uri(HtmlPage.Document.DocumentUri, "FileUpload.ashx");
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        public bool Verify()
        {
            return true;
        }
    }
}
