﻿#pragma checksum "D:\GitHub\TKS\Modules_Flows\MainPage\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0A0F5260D871A3E41F16E4D00046F25D"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18063
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using TKS.Controls;


namespace MainPage {
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.RowDefinition headrow;
        
        internal System.Windows.Controls.Border controls_header;
        
        internal System.Windows.Controls.TextBlock txtappName;
        
        internal System.Windows.Shapes.Rectangle stripe;
        
        internal System.Windows.Shapes.Path stripe_left;
        
        internal System.Windows.Shapes.Rectangle shadow_A;
        
        internal System.Windows.Shapes.Path stripe_right;
        
        internal System.Windows.Controls.TextBlock txtUser;
        
        internal System.Windows.Controls.Canvas outterframe;
        
        internal System.Windows.Media.RectangleGeometry OutterBorder;
        
        internal System.Windows.Controls.StackPanel mainmenu;
        
        internal System.Windows.Controls.Button leftArrow;
        
        internal System.Windows.Controls.Button rightArrow;
        
        internal System.Windows.Controls.Button showPopup;
        
        internal System.Windows.Controls.Button FullScreen;
        
        internal System.Windows.Controls.Button loginOut;
        
        internal System.Windows.Controls.Border FrameBorder;
        
        internal System.Windows.Controls.Grid contentPanel;
        
        internal System.Windows.Controls.TextBlock contentPath;
        
        internal TKS.Controls.IMenus leftmenu;
        
        internal TKS.Controls.BubbleContainer bubbleControl;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/MainPage;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.headrow = ((System.Windows.Controls.RowDefinition)(this.FindName("headrow")));
            this.controls_header = ((System.Windows.Controls.Border)(this.FindName("controls_header")));
            this.txtappName = ((System.Windows.Controls.TextBlock)(this.FindName("txtappName")));
            this.stripe = ((System.Windows.Shapes.Rectangle)(this.FindName("stripe")));
            this.stripe_left = ((System.Windows.Shapes.Path)(this.FindName("stripe_left")));
            this.shadow_A = ((System.Windows.Shapes.Rectangle)(this.FindName("shadow_A")));
            this.stripe_right = ((System.Windows.Shapes.Path)(this.FindName("stripe_right")));
            this.txtUser = ((System.Windows.Controls.TextBlock)(this.FindName("txtUser")));
            this.outterframe = ((System.Windows.Controls.Canvas)(this.FindName("outterframe")));
            this.OutterBorder = ((System.Windows.Media.RectangleGeometry)(this.FindName("OutterBorder")));
            this.mainmenu = ((System.Windows.Controls.StackPanel)(this.FindName("mainmenu")));
            this.leftArrow = ((System.Windows.Controls.Button)(this.FindName("leftArrow")));
            this.rightArrow = ((System.Windows.Controls.Button)(this.FindName("rightArrow")));
            this.showPopup = ((System.Windows.Controls.Button)(this.FindName("showPopup")));
            this.FullScreen = ((System.Windows.Controls.Button)(this.FindName("FullScreen")));
            this.loginOut = ((System.Windows.Controls.Button)(this.FindName("loginOut")));
            this.FrameBorder = ((System.Windows.Controls.Border)(this.FindName("FrameBorder")));
            this.contentPanel = ((System.Windows.Controls.Grid)(this.FindName("contentPanel")));
            this.contentPath = ((System.Windows.Controls.TextBlock)(this.FindName("contentPath")));
            this.leftmenu = ((TKS.Controls.IMenus)(this.FindName("leftmenu")));
            this.bubbleControl = ((TKS.Controls.BubbleContainer)(this.FindName("bubbleControl")));
        }
    }
}
