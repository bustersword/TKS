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
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;
using System.Threading;
using TKS.Common;

namespace WelcomePage
{
    [ModuleExport(typeof(WelcomeManager))]
    public class WelcomeManager : IModule
    {
        [Import("main")]
        public string MainRegion;

        [Import]
        public Action<object> DoNavigate;

        public IRegionManager _regionManager { get; set; }


        [ImportingConstructor]
        public WelcomeManager([Import]IRegionManager regionManager)
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            _regionManager = regionManager;

        }

        public void Initialize()
        {
            //模块的初始化，应用程序仅会执行一次
            ThreadPool.QueueUserWorkItem(new WaitCallback(target));
        }

        private void target(object state)
        {
            CommonBusy.IsBusy("加载基础数据");
            //模拟加载基础数据
            BaseData.Default.GetData(() =>
            {

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                   
                    CommonBusy.IsNotBusy();

                    //获取框架主容器
                    IRegion main = _regionManager.Regions[MainRegion];

                    //tag[0]: view 名称，与页类名同名
                    //tag[1]: uri 导航地址，可带参数
                    string[] tag = main.Context as string[];


                    var frm = new Welcome();
                    var frm2 = new Page1();

                    //页面注册到框架主内容容器中
                    //这里注册了多个页面，考虑到老的模块没有分开，
                    //所以要在一次初始化的时候一次注册所需的所有页面，后面导航不会再重新实例化
                    main.Add(frm);
                    main.Add(frm2);

                    //可以按照不同的进入菜单，做一些自定义操作
                    switch (tag[0])
                    { 
                        case "Welcome":
                            //do something
                            break;
                        case "Page1":
                            //do something
                            break;
                    }
                    //根据导航的URI导航到指定页面
                   // main.RequestNavigate(new Uri(tag[1], UriKind.Relative));
                    //模块初始化完毕，控制权交回主框架
                    DoNavigate(null);
                });
            });

        }

       
    }
}
