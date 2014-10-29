using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace WelcomePage
{
    public class BaseData
    {
        static BaseData _default;

        public static  BaseData Default
        {
            get {

                return _default ?? (_default = new BaseData());
            }
        }

        Dictionary<string, string> data = new Dictionary<string, string>();

       public  void GetData(Action callBack)
        {
            System.Threading.Thread.Sleep(2000);
            data.Add("A", "just test,I am baseData~ friend");
            callBack();
        }

        public Dictionary<string, string> Data
        {
            get {
                return data;
            }
        }
    }
}
