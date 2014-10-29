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
using TKS.Common;
using TKS.DTO.Models;
using TKS.Controls;

namespace DataGrid_Sample
{
    public partial class EView_Sample : ChildWindow
    {
        public EView_Sample()
        {
            InitializeComponent();
        }
        bool _edit = false;
        public EView_Sample(IC_CAR_APPLY_E model)
        {
            InitializeComponent();
            GridEx.SetValue(this.LayoutRoot, model,GridExSourceType.DTO);
            this.TKS_DOC_NO.IsReadOnly = true;
            _edit = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var val = GridEx.GetValue<IC_CAR_APPLY_E>(this.LayoutRoot);
          
            if (GridEx.ResultOK)
            {
                if (!_edit)
                {
                    val.DECL_DATE = DateTime.Now;
                    RESTFulClient.SubmitRequest<int>("RESTfulS.BAL.IC_CAR_APPLY_I001", "Execute", (res) =>
                    {
                        Message.InfoMessage("成功增加了" + res.ToString() + "行数据");
                    }, val);
                }
                else
                {
                    RESTFulClient.SubmitRequest<int>("RESTfulS.BAL.IC_CAR_APPLY_U001", "Execute", (res) =>
                    {
                        Message.InfoMessage("成功更新了" + res.ToString() + "行数据");
                    }, val);
                }
            }
            else
            {
                Message.InfoMessage("验证未通过");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

