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
using TKS.Controls;
using System.ComponentModel.DataAnnotations;
 
using TKS.Common;
namespace TestAModule
{
    public partial class Tab3 : Page, ITab
    {
        public Tab3()
        {
            InitializeComponent();
            Init();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        public class TestSource
        {

            public string put1
            {
                get;
                set;
            }

            public int put2
            {
                get;
                set;
            }

            public string put3
            {
                get;
                set;
            }

            public List<string> com1
            {
                get;
                set;
            }

            public bool rad1
            {
                get;
                set;
            }

            public bool chk1
            {
                get;
                set;
            }

            public List<item> com2
            {
                get;
                set;
            }

            public List<item> checks
            {
                get;
                set;
            }

            public List<string> list
            {
                get;
                set;
            }

            public List<string> listSingle
            {
                get;
                set;
            }

        }
        public class TestResult
        {
            [Range(2, 5000, ErrorMessage = "范围大小2-5000")]
            [Required(ErrorMessage = "必填项")]
            [StringLength(4, ErrorMessage = "长度超过4个字符")]
            public string put1
            {
                get;
                set;
            }
            [Required(ErrorMessage = "必填项")]
            public int put2
            {
                get;
                set;
            }
            [Required(ErrorMessage = "必填项")]
            [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "请输入正确的Email")]
            public string put3
            {
                get;
                set;
            }

            [Required(ErrorMessage = "u must select one")]
            public string com1
            {
                get;
                set;
            }

            public bool rad1
            {
                get;
                set;
            }


            public bool chk1
            {
                get;
                set;
            }

            public item com2
            {
                get;
                set;
            }

            public List<item> checks
            {
                get;
                set;
            }
             [Required(ErrorMessage = "u must select one")]
            public List<string> list
            {
                get;
                set;
            }
            [Required(ErrorMessage = "u must select one")]
             public string listSingle
             {
                 get;
                 set;
             }

        }

        public class item
        {
            public string txt { get; set; }
            public bool chk { get; set; }
        }
        void Init()
        {
            TestSource test = new TestSource
            {
                put1 = "ceshi 1",
                put2 = 2,
                put3 = "test 3",
                chk1 = true,
                rad1 = true,
                com1 = new List<string> { "qq", "1", "23" },
                com2 = new List<item> { new item { chk=true , txt="aa"},
                new item { chk=false ,txt ="bb"}},
                checks = new List<item> { 
                    new item{ txt="A", chk=false }
                    ,new item { chk=true ,txt ="B"},
                    new item { chk =true ,txt ="C"},
                    new item { chk =false ,txt ="u can check me!"}
                },
                list = new List<string> { "1", "2", "3" }
                ,
                listSingle = new List<string> { "4","5","6"}
            };
            GridEx.SetValue(this.LayoutRoot, test,GridExSourceType.DataSource);



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TestResult res = GridEx.GetValue<TestResult>(this.LayoutRoot);
            if (GridEx.ResultOK)
                MessageBox.Show("Pass");
            else
                MessageBox.Show("Wrong!!!");

            // MessageBox.Show(res.put1 + res.put2 + res.put3 + "radiobutton"+res.rad1.ToString());
        }

        public bool Verify()
        {
            return true;
        }
    }


}
