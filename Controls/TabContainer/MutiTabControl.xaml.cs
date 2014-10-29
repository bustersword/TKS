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

namespace TKS.Controls
{
    public partial class MutiTabControl : UserControl
    {
        public MutiTabControl()
        {
            
            InitializeComponent();
            
        }

        private void ResourceStartUp(string uri)
        {
            ResourceDictionary resourceDictionary = new ResourceDictionary();
            Application.LoadComponent(resourceDictionary, new Uri(uri, UriKind.Relative));
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }
      
        public class ItemSource
        {
            public string Header
            {
                get;set;

            }

            public UserControl Content
            {
                get;
                set;
            }
        }

        IEnumerable<ItemSource> _bindSource;
        public IEnumerable<ItemSource> BindSource
        {
            get { return _bindSource; }
            set {
                if (value  != null)
                {
                    _bindSource = value;
                    if (_bindSource != null)
                    {
                        this.tab.SetValue(TKS.Controls.TabControlExtensions.ItemsSourceProperty, _bindSource);
                    }
                }
            }
        }

        public void SubmitALL()
        {
            bool flag = true ;
            foreach (UserControl item in _bindSource.Select(p=>p.Content))
            {
                if (!((ITab)item).Verify())
                {
                    flag = false;
                    break;
                }
            }

            MessageBox.Show(BindSource.Count().ToString()+"verification  is  "+flag .ToString ());
        }
    }

    public interface ITab
    {
        bool Verify();
    }
}
