﻿#pragma checksum "D:\GitHub\TKS\LoadFrame\BootShell\Shell.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "987B64AA585577C7215813FC2D699677"
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


namespace BootShell {
    
    
    public partial class Shell : System.Windows.Controls.UserControl {
        
        internal System.Windows.VisualStateGroup GeneralVisualStateGroup;
        
        internal System.Windows.VisualState IdleState;
        
        internal System.Windows.VisualState InfoPrintState;
        
        internal System.Windows.Controls.Grid InfoGrid;
        
        internal System.Windows.Shapes.Rectangle rectangle;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/BootShell;component/Shell.xaml", System.UriKind.Relative));
            this.GeneralVisualStateGroup = ((System.Windows.VisualStateGroup)(this.FindName("GeneralVisualStateGroup")));
            this.IdleState = ((System.Windows.VisualState)(this.FindName("IdleState")));
            this.InfoPrintState = ((System.Windows.VisualState)(this.FindName("InfoPrintState")));
            this.InfoGrid = ((System.Windows.Controls.Grid)(this.FindName("InfoGrid")));
            this.rectangle = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle")));
        }
    }
}

