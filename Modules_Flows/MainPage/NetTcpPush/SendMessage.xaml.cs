﻿using System;
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

namespace MainPage
{
    public partial class SendMessage : ChildWindow
    {
        public SendMessage()
        {
            InitializeComponent();
        }

        public string Message
        {
            get;
            private set;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
            Message = this.Content.Text;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

