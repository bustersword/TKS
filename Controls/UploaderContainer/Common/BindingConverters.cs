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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TKS.Controls.Common
{

    public class HostUrlConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = value == null ? string.Empty : value.ToString();

            return UrlTransfer.ConvertHostUrl(url);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = value == null ? string.Empty : value.ToString();

            return UrlTransfer.ConvertBackHostUrl(url);
        }

        #endregion
    }

    public class ImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = value.ToString();
            ImageSource imageSource = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage imageSource = value as BitmapImage;
            return imageSource.UriSource.AbsolutePath;
        }

        #endregion
    }

    public class MImageUrlConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = string.Empty;

            if (value == null || value.ToString().Trim() == string.Empty)
                url = Url.GetHtmlPageUrl("Files/Images/m_nopic.png");
            else
                url = value.ToString();

            if (url.StartsWith("Files", StringComparison.OrdinalIgnoreCase))
            {
                int index = url.LastIndexOf('/') + 1;
                url = url.Insert(index, "Thumb/m_");
                url = Url.GetHtmlPageUrl(url);
            }

            ImageSource imageSource = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = value == null ? string.Empty : value.ToString();

            return UrlTransfer.ConvertBackHostUrl(url);
        }
        #endregion
    }

    public class SImageUrlConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = string.Empty;

            if (value == null || value.ToString().Trim() == string.Empty)
                url = Url.GetHtmlPageUrl("Files/Images/s_nopic.png");
            else
                url = value.ToString();

            if (url.StartsWith("Files", StringComparison.OrdinalIgnoreCase))
            {
                int index = url.LastIndexOf('/') + 1;
                url = url.Insert(index, "Thumb/s_");
                url = Url.GetHtmlPageUrl(url);
            }

            ImageSource imageSource = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = value == null ? string.Empty : value.ToString();

            return UrlTransfer.ConvertBackHostUrl(url);
        }
        #endregion
    }

    public class NumberImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = string.Format("../images/Number/num{0}.png", value);

            ImageSource imageSource = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    public class NumbersImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;

            foreach (char s in value.ToString())
            {
                string imageUrl = string.Format("../images/Number/number{0}.png", s);

                Image image = new Image();
                image.Source = new BitmapImage(new Uri(imageUrl, UriKind.RelativeOrAbsolute));
                image.Stretch = Stretch.None;

                panel.Children.Add(image);
            }

            return panel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    public class UrlTransfer
    {
        public static string ConvertHostUrl(string url)
        {
            if (url.StartsWith("Files", StringComparison.OrdinalIgnoreCase))
                url = Url.GetHtmlPageUrl(url);

            return url;
        }

        public static string ConvertBackHostUrl(string url)
        {
            string host = Url.GetHtmlPageHost();
            if (url.StartsWith(host))
                url = url.Replace(host, string.Empty);

            return url;
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return System.Convert.ToBoolean(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
        #endregion
    }

    public class VisibilityReverseConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return System.Convert.ToBoolean(value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Collapsed;
        }
        #endregion
    }
}

