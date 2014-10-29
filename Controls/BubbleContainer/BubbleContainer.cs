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
    public class BubbleContainer : Control
    {
        public BubbleContainer()
        {
            this.DefaultStyleKey = typeof(BubbleContainer);
            
             
        }

        public static readonly DependencyProperty headInfo = DependencyProperty.Register("HeadInfo", typeof(UserControl), typeof(BubbleContainer), new PropertyMetadata(OnHeadInfoChanged));

        public static readonly DependencyProperty btnContent = DependencyProperty.Register("BtnContent", typeof(string), typeof(BubbleContainer), new PropertyMetadata(OnbtnContentChanged));

        private static void OnbtnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BubbleContainer _bubble = d as BubbleContainer;
            if (_bubble != null)
            {
                
                _bubble.BtnContent = e.NewValue.ToString ();
            }
        }
        
        /// <summary>
        /// do when menu is clicked
        /// </summary>
        public event MouseEventHandler ButtonClick;

        private static void OnHeadInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BubbleContainer _bubble = d as BubbleContainer;
            if (_bubble != null)
            {
                if (_bubble.Panel != null)
                    (_bubble.Panel.Children[0] as UserControl).Content = e.NewValue as UserControl;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //  HeadInfo = GetTemplateChild("content") as UserControl;
            Scroll = GetTemplateChild("bubbleScroll") as ScrollViewer;
            Panel = GetTemplateChild("panel") as StackPanel;
            BtnAdd = GetTemplateChild("btnAdd") as StackPanel;
            BtnAddContent = GetTemplateChild("btnAddContent") as TextBlock;

            BtnAdd.MouseLeftButtonDown += BtnAdd_MouseLeftButtonDown;
            BtnAdd.Cursor = Cursors.Hand;
            if (Scroll != null)
            {
                Scroll.Height = 350;
                
            }
            
           

        }

        void BtnAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseEventHandler btnAdd = ButtonClick;
            if (BtnAdd != null)
            {
                btnAdd.Invoke(sender, e);
            }
        }

        public StackPanel BtnAdd
        {
            get;
            set;
        }

        public TextBlock BtnAddContent
        {
            get;
            set;
        }




        public StackPanel Panel
        {
            get;
            set;
        }

        public ScrollViewer Scroll
        {
            get;
            set;
        }

        public UserControl HeadInfo
        {
            get
            {
                return GetValue(headInfo) as UserControl;
            }
            set
            {
                SetValue(headInfo, value);
            }
        }

        public string BtnContent
        {
            get
            {
                return GetValue(btnContent) as string ;
            }
            set
            {
                SetValue(btnContent, value);
            }
        }
       
    }
}
