using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
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
using TKS.Controls;
namespace BaseConfigModule
{
    [ModuleExport(typeof(BaseManager))]
    public class BaseManager : IModule
    {
        ILoggerFacade _log;
        [ImportingConstructor]
        public BaseManager(ILoggerFacade logger)
        {
            if (logger == null)
            {
                throw new  ArgumentNullException("日志插件为null");
            }
            _log = logger;
        }

        public void Initialize()
        {
            _log.Log("", Category.Info, Priority.None);
            var f = new MainPage();
            f.Load();
        }
    }
}
