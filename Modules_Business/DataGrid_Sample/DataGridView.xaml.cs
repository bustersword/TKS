using TKS.DTO.Core;
using TKS.DTO.Models;
using TKS.Common;
using TKS.Controls;
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
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.MefExtensions.Modularity;

namespace DataGrid_Sample
{
   
    public partial class DataGridView : UserControl
    {
        public DataGridView()
        {
            InitializeComponent();
            TKS_PLAN_START_TIME.Text = "2013-1-1";
            TKS_STATUS_CODE.Text = "20";
            string sql= @"select  *
            from IC_CAR_APPLY   where  USE_REASONS like '%'||@USE_REASONS||'%' and PROC_OPINION like '%'||@PROC_OPINION||'%' and
            status_code=@STATUS_CODE  and plan_start_time>to_date(@PLAN_START_TIME,'yyyy/mm/dd hh24:mi:ss')";

            string sql2= @"select  *
            from IC_CAR_APPLY   where 
            status_code=@STATUS_CODE  ";
            //初始化DataGrid
            MyDataGridEx.InitMyDataGrid(MyDataGrid, this.LayoutSubNode, 10,
              sql );

            //添加双击事件
            MyDataGrid.RowItemDoubleClick += MyDataGrid_RowItemDoubleClick;
        }


        //双击行事件
        private void MyDataGrid_RowItemDoubleClick(object sender, EventArgs e)
        {
            IC_CAR_APPLY_E o = sender as IC_CAR_APPLY_E;
            EView_Sample frm = new EView_Sample(o);
            frm.Show();
        }

        //翻页事件
        private void MyDataGrid_PageIndexChanged_1(object sender, PageIndexArgs e)
        {
            MyDataGrid.Query<List<IC_CAR_APPLY_E>, IC_CAR_APPLY_D>(e.ViewIndex);
        }

        //查询按钮
        private void btn_Query_Click_1(object sender, RoutedEventArgs e)
        {
            MyDataGrid.Query<List<IC_CAR_APPLY_E>, IC_CAR_APPLY_D>();
        }

        //行内按钮点击事件
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Button o = e.OriginalSource as Button;
            IC_CAR_APPLY_E m = o.DataContext as IC_CAR_APPLY_E;
            Message.InfoMessage(m.PLAN_START_TIME.ToString("yyyy-MM-dd hh:mm:ss"));
        }
        //新增
        private void btn_Add_Click_1(object sender, RoutedEventArgs e)
        {
            EView_Sample frm = new EView_Sample();
            frm.Show();
        }

        //删除
        private void btn_Delete_Click_1(object sender, RoutedEventArgs e)
        {
            List<IC_CAR_APPLY_E> source = MyDataGrid.ItemsSource as List<IC_CAR_APPLY_E>;
            if (source != null)
            {
                List<IC_CAR_APPLY_E> ckd = source.Where(p => p.IsChecked == true).ToList();
                if (ckd.Count >= 2)
                {
                    Message.WarnMessage("只能选择一项");
                }
                else
                {
                    RESTFulClient.SubmitRequest<int>("RESTfulS.BAL.IC_CAR_APPLY_D001", "Execute", (res) =>
                    {
                        Message.InfoMessage("成功删除了" + res.ToString() + "行数据");
                    },  ckd[0] );
                }
            }
        }

       
    }
}
