using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.Controls
{
    public  class PageIndexArgs :EventArgs
    {
        /// <summary>
        /// 用于显示的页面索引从1开始计数，默认datapager控件索引从0开始计数，这里为了更好理解开放这个属性
        /// </summary>
        public int ViewIndex
        {
            get;
            private set;
        }

        public PageIndexArgs(int pageIndex)
        {
            ViewIndex = pageIndex+1;
        }
    }
}
