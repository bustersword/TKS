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

namespace TKS.Common
{
    public class TabMenuItem
    {
        /// <summary>
        /// 程序集名
        /// </summary>
        public string AssemblyName
        {
            get;
            set;
        }

        /// <summary>
        /// 描述标题
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool Ignore
        {
            get;
            set;
        }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Order
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get;
            set;
        }
    }
}
