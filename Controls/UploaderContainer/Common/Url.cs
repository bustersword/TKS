using System;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TKS.Controls.Common
{
    public class Url
    {
        public static string GetHtmlPageUrl(string path)
        {
            Uri uri = new Uri(HtmlPage.Document.DocumentUri, path);
            return uri.AbsoluteUri;
        }

        public static string GetHtmlPageHost()
        {
            return HtmlPage.Document.DocumentUri.Host;
        }
    }
}
