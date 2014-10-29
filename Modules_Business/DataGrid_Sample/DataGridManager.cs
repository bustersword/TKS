using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DataGrid_Sample
{
     [ModuleExport(typeof(DataGridManager))]
    public class DataGridManager : IModule
    {
        //[Import("main")]
        //public string MainRegion;
        //[Import]
        //public Action<object> DoNavigate;

        //[ImportingConstructor]
        //public DataGridManager([Import]IRegionManager regionManager)
        //{
        //    if (regionManager == null)
        //    {
        //        throw new ArgumentNullException("regionManager");
        //    }
        //    _regionManager = regionManager;

        //}
/*
        public void Initialize()
        {

            //获取框架主容器
            IRegion main = _regionManager.Regions[MainRegion];

            //tag[0]: view 名称，与页类名同名
            //tag[1]: uri 导航地址，可带参数
            string[] tag = main.Context as string[];


            var frm = new DataGridView();

            //页面注册到框架主内容容器中
            //后面导航不会再重新实例化
            main.Add(frm);

            //可以按照不同的进入菜单，做一些自定义操作
            switch (tag[0])
            {
                case "DataGridView":
                    //do something
                    break;
            }
            DoNavigate(null);
        }
         */
        public void Initialize()
        {
        }

        public IRegionManager _regionManager { get; set; }
    }
}
