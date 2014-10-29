using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TKS.Common;
using TKS.Controls;
using Microsoft.Practices.Prism.MefExtensions;
using System.ComponentModel.Composition.Hosting;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Logging;

namespace BootShell
{
    public class QuickBootstrap : MefBootstrapper
    {
        readonly APPLogger log = new APPLogger();
        protected override DependencyObject CreateShell()
        {
            return this.Container.GetExportedValue<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            CommonBusy.InitBusy((UIElement)this.Shell);
            Application.Current.RootVisual = CommonBusy.BusyContainer;

            if (!AppStorage.IsSpaceEnough(50))
            {
                Message.ActionMessage("请点击申请空间扩容", () =>
                {
                    AppStorage.ApplyStorageSpace(50);
                });
            }
        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();


            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(QuickBootstrap).Assembly));

        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

        }
        protected override ILoggerFacade CreateLogger()
        {
            return log;
        }
        protected override IModuleCatalog CreateModuleCatalog()
        {
            ModuleCatalog moduleCatalog = new ModuleCatalog();

            // this is the code responsible 

            //moduleCatalog.AddModule
            //(
            //    new ModuleInfo
            //    {
            //        InitializationMode = InitializationMode.WhenAvailable,
            //        Ref = "MainPage.xap",
            //        ModuleName = "ViewManager",
            //        ModuleType = "MainPage.ViewManager, MainPage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
            //    }
            //);

            ///*V1版本主页加载业务模块模式：依赖框架的初始化方式,业务模块需要实现相应的初始化类*/
            //moduleCatalog.AddModule
            //(
            //    new ModuleInfo
            //    {
            //        InitializationMode = InitializationMode.WhenAvailable,
            //        Ref = "MainPageV1.xap",
            //        ModuleName = "MainViewV1",
            //        ModuleType = "MainPage.MainViewV1,MainPage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
            //    }
            //);

            ///*V1版树目录：适配V1版的加载模式*/
            //moduleCatalog.AddModule
            //(
            //    new ModuleInfo
            //    {
            //        InitializationMode = InitializationMode.OnDemand,
            //        Ref = "TreeViewV1.xap",
            //        ModuleName = "TRV1",
            //        ModuleType = "TreeViewV1.TRV1,TreeViewV1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
            //    }
            //);

            /*V2版本主页加载业务模块模式：非依赖框架的初始化方式，重写导航接口，直接反射加载业务模块*/
            moduleCatalog.AddModule
            (
                new ModuleInfo
                {
                    InitializationMode = InitializationMode.WhenAvailable,
                    Ref = "MainPageV2.xap",
                    ModuleName = "MainViewV2",
                    ModuleType = "MainPageV2.MainViewV2,MainPageV2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                }
            );
            /*V2版树目录：适配V2版的加载模式*/
            moduleCatalog.AddModule
            (
                new ModuleInfo
                {
                    InitializationMode = InitializationMode.OnDemand,
                    Ref = "TreeViewV2.xap",
                    ModuleName = "TRV1",
                    ModuleType = "TreeViewV2.TRV1,TreeViewV2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                }
            );
            moduleCatalog.AddModule(
                new ModuleInfo
                {
                    InitializationMode = InitializationMode.OnDemand,
                    Ref = "DataGrid_Sample.xap",
                    ModuleName = "DataGridManager",
                    ModuleType = "DataGrid_Sample.DataGridManager,DataGrid_Sample,Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                }
                );

            moduleCatalog.AddModule(
              new ModuleInfo
              {
                  InitializationMode = InitializationMode.OnDemand,
                  Ref = "DataGrid_Sample_MVVM.xap",
                  ModuleName = "DataGridMVVMManager",
                  ModuleType = "DataGrid_Sample_MVVM.DataGridMVVMManager,DataGrid_Sample_MVVM,Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
              }
              );
            moduleCatalog.AddModule(
             new ModuleInfo
             {
                 InitializationMode = InitializationMode.OnDemand,
                 Ref = "WelcomePage.xap",
                 ModuleName = "WelcomeManager",
                 ModuleType = "WelcomePage.WelcomeManager,WelcomePage,Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
             }
             );

            moduleCatalog.AddModule(
             new ModuleInfo
             {
                 InitializationMode = InitializationMode.WhenAvailable,
                 Ref = "BaseConfigModule.xap",
                 ModuleName = "BaseManager",
                 ModuleType = "BaseConfigModule.BaseManager,BaseConfigModule,Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
             }
             );

            moduleCatalog.AddModule(
           new ModuleInfo
           {
               InitializationMode = InitializationMode.WhenAvailable,
               Ref = "ControlsSample.xap",
               ModuleName = "ControlsManager",
               ModuleType = "ControlsSample.ControlsManager,ControlsSample,Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
           }
           );

            return moduleCatalog;
        }
    }
}
