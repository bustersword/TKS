﻿#pragma checksum "D:\Works\PDA-Customs\COP\V3\Framework\ECI.PDA.COP.Framework\Modules_Business\MVVM_DataGrid_Sample\EView_Sample.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CC35CA88BABEC6CE25CDCCBBBAFDED7F"
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


namespace DataGrid_Sample_MVVM {
    
    
    public partial class EView_Sample : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.ChildWindow childWindow;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button SaveButton;
        
        internal System.Windows.Controls.TextBox TKS_USE_REASONS;
        
        internal System.Windows.Controls.TextBox TKS_DOC_NO;
        
        internal System.Windows.Controls.TextBox TKS_PROC_OPINION;
        
        internal System.Windows.Controls.DatePicker TKS_PLAN_START_TIME;
        
        internal System.Windows.Controls.DatePicker TKS_PLAN_END_TIME;
        
        internal System.Windows.Controls.TextBox TKS_CAR_NO;
        
        internal System.Windows.Controls.TextBox TKS_MAN_NO;
        
        internal System.Windows.Controls.TextBox TKS_CAR_TYPE;
        
        internal System.Windows.Controls.TextBox TKS_START_ADD;
        
        internal System.Windows.Controls.TextBox TKS_DEST_ADD;
        
        internal System.Windows.Controls.ComboBox TKS_STATUS_CODE;
        
        internal System.Windows.Controls.TextBox TKS_DECL_MAN_NAME;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/MVVM_DataGrid_Sample;component/EView_Sample.xaml", System.UriKind.Relative));
            this.childWindow = ((System.Windows.Controls.ChildWindow)(this.FindName("childWindow")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.SaveButton = ((System.Windows.Controls.Button)(this.FindName("SaveButton")));
            this.TKS_USE_REASONS = ((System.Windows.Controls.TextBox)(this.FindName("TKS_USE_REASONS")));
            this.TKS_DOC_NO = ((System.Windows.Controls.TextBox)(this.FindName("TKS_DOC_NO")));
            this.TKS_PROC_OPINION = ((System.Windows.Controls.TextBox)(this.FindName("TKS_PROC_OPINION")));
            this.TKS_PLAN_START_TIME = ((System.Windows.Controls.DatePicker)(this.FindName("TKS_PLAN_START_TIME")));
            this.TKS_PLAN_END_TIME = ((System.Windows.Controls.DatePicker)(this.FindName("TKS_PLAN_END_TIME")));
            this.TKS_CAR_NO = ((System.Windows.Controls.TextBox)(this.FindName("TKS_CAR_NO")));
            this.TKS_MAN_NO = ((System.Windows.Controls.TextBox)(this.FindName("TKS_MAN_NO")));
            this.TKS_CAR_TYPE = ((System.Windows.Controls.TextBox)(this.FindName("TKS_CAR_TYPE")));
            this.TKS_START_ADD = ((System.Windows.Controls.TextBox)(this.FindName("TKS_START_ADD")));
            this.TKS_DEST_ADD = ((System.Windows.Controls.TextBox)(this.FindName("TKS_DEST_ADD")));
            this.TKS_STATUS_CODE = ((System.Windows.Controls.ComboBox)(this.FindName("TKS_STATUS_CODE")));
            this.TKS_DECL_MAN_NAME = ((System.Windows.Controls.TextBox)(this.FindName("TKS_DECL_MAN_NAME")));
        }
    }
}
