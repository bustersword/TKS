using System;
using System.Collections;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TKS.Controls
{
    public class TabControlExtensions
    {
       

        public static IEnumerable GetItemsSource(DependencyObject d)
        {
            return (IEnumerable)d.GetValue(ItemsSourceProperty);
        }

        public static void SetItemsSource(DependencyObject d, IEnumerable value)
        {
            d.SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.RegisterAttached("ItemsSource", typeof(IEnumerable), typeof(TabControl),
        new PropertyMetadata(OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as TabControl;
            var items = e.NewValue as IEnumerable;

            source.Items.Clear();

            if (items != null)
            {
                var headerTemplate = GetHeaderTemplate(source);
                var contentTemplate = GetContentTempalte(source);

                foreach (var item in items)
                {
                    var tabItem = new TabItem
                    {
                        DataContext = item,
                        Header = item,
                        HeaderTemplate = headerTemplate,
                        Content = item,
                        ContentTemplate = contentTemplate
                     
                    };
                
                    source.Items.Add(tabItem);
                }
            }
        }



        public static DataTemplate GetHeaderTemplate(DependencyObject d)
        {
            return (DataTemplate)d.GetValue(HeaderTemplateProperty);
        }

        public static void SetHeaderTemplate(DependencyObject d, DataTemplate value)
        {
            d.SetValue(HeaderTemplateProperty, value);
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
        DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(TabControl),
        new PropertyMetadata(OnHeaderTemplateChanged));

        private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as TabControl;
            var template = e.NewValue as DataTemplate;

            foreach (TabItem item in source.Items)
            {
                item.HeaderTemplate = template;
            }
        }


        public static DataTemplate GetContentTempalte(DependencyObject d)
        {
            return (DataTemplate)d.GetValue(ContentTempalteProperty);
        }

        public static void SetContentTempalte(DependencyObject d, DataTemplate value)
        {
            d.SetValue(ContentTempalteProperty, value);
        }

        public static readonly DependencyProperty ContentTempalteProperty =
        DependencyProperty.Register("ContentTempalte", typeof(DataTemplate), typeof(TabControl),
        new PropertyMetadata(OnContentTempalteChanged));

        private static void OnContentTempalteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as TabControl;
            var template = e.NewValue as DataTemplate;

            foreach (TabItem item in source.Items)
            {
                item.ContentTemplate = template;
            }
        }
    }
}
