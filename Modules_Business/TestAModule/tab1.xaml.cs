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
using System.Windows.Navigation;
using TKS.DTO.Models;
using TKS.Common;
using TKS.Controls;
namespace TestAModule
{
    public partial class tab1 : Page,ITab
    {
        public tab1()
        {
            InitializeComponent();

        
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            List<ExampleData> ls = new List<ExampleData>();
            ls.Add(new ExampleData { Name = "test", Age = 10 });

            for (int i = 0; i < 100000; i++)
            {
                ls.Add(new ExampleData { Name = "test", Age = i });
            }

            Dictionary<string, ParamDataItem> param = new Dictionary<string, ParamDataItem>();
            //参数的顺序非常重要，是后台方法的参数顺序
            param.Add("_paras", new ParamDataItem { Data = ls });

            param.Add("_a", new ParamDataItem { Data = new ExampleData { Name = "king", Age = 30 } });

            param.Add("_b", new ParamDataItem { Data = "我是测试字符串的" });

            RESTFulClient.SubmitRequest<List<ExampleData>>(new SubmitContext
            {
                FuncType = "RESTfulS.MYLPA_APPLY",
                MethodName = "Test",
                Parameters = param
            }, (p) =>
            {
                Message.InfoMessage(p[0].Name);
            });

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {


            Dictionary<string, ParamDataItem> param = new Dictionary<string, ParamDataItem>();

            param.Add("_a", new ParamDataItem { Data = new ExampleData { Name = "king", Age = 30 } });

            RESTFulClient.SubmitRequest<ExampleData>(new SubmitContext
            {
                FuncType = "RESTfulS.MYLPA_APPLY",
                MethodName = "Test",
                Parameters = param
            }, (p) =>
            {

                Message.InfoMessage(p.Name);
            });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {


            RESTFulClient.SubmitRequest<string>(new SubmitContext
            {
                FuncType = "RESTfulS.MYLPA_APPLY",
                MethodName = "Test",
                Parameters = null
            }, (p) =>
            {

                Message.InfoMessage(p);
            });
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            RESTFulClient.SubmitRequest<string>(new SubmitContext
            {
                FuncType = "RESTfuls2.TEST2Handler",
                MethodName = "Test",
                Parameters = null
            }, (p) =>
            {

                Message.InfoMessage(p);
            });
        }
        public bool Verify()
        {
            return true;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            RESTFulClient.SubmitRequest<ExampleData>("RESTfulS.MYLPA_APPLY", "Test", (P) =>
            {
                Message.InfoMessage(P.Name);
            },
            new ExampleData { Name = "123" });
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            RESTFulClient.SubmitRequest<string>("RESTfulS.MYLPA_APPLY", "FuncA",
             (P) =>
           {
               Message.InfoMessage(P);
           },"a","b","c");
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            RESTFulClient.SubmitRequest<string>("RESTfulS.MYLPA_APPLY", "FuncB",
            (P) =>
            {
                Message.InfoMessage(P);
            });
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            RESTFulClient.SubmitRequestOnlyForQuery<List<IC_CAR_APPLY>>(@"select  *
          
            from IC_CAR_APPLY   where  
            status_code=@STATUS_CODE  and plan_start_time>to_date(@PLAN_START_TIME,'yyyy/mm/dd hh24:mi:ss')",
           2, 10, (P, total) => {
               Message.InfoMessage(P.Count.ToString()+"#"+total.ToString());
           }, new IC_CAR_APPLY { STATUS_CODE="20", PLAN_START_TIME =DateTime.Parse("2013-1-1") });
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            RESTFulClient.SubmitRequest<List<IC_CAR_APPLY>>("RESTfulS.MYLPA_APPLY", "FuncC",
          (P) =>
          {
              Message.InfoMessage(P.Count .ToString());
          });
        }

      

       
    }
}
