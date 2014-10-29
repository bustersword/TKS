using Microsoft.Practices.Prism.Modularity;
using System;
using System.ComponentModel;
using System.Reflection;

namespace TKS.Core
{
    public class DownloadingEventArgs : ModuleDownloadProgressChangedEventArgs
    {


        public DownloadingEventArgs(ModuleDes module, ModuleInfo moduleInfo, long bytesReceived, long totalBytesToReceive)
            : base(moduleInfo,bytesReceived,totalBytesToReceive)
        {
            Module = module;
        }

        public ModuleDes Module
        {
            get;
            set;
        }

    }

    public class DownloadCommpletedEventArgs : AsyncCompletedEventArgs
    {


        public DownloadCommpletedEventArgs(ModuleDes module,Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
            Module = module;
        }

        public ModuleDes Module
        {
            get;
            set;
        }
       
    }

    public class DownloadFaileddEventArgs : AsyncCompletedEventArgs
    {
        public DownloadFaileddEventArgs(ModuleDes module, Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
            Module = module;
        }
        public ModuleDes Module
        {
            get;
            set;
        }
    }

    public class ModuleDes
    {
        public string MoudleName
        {
            get;
            set;
        }

        /// <summary>
        /// 当前加载的程序集，实现INavigationContentLoader，加载页面
        /// </summary>
        public Assembly CurrentAssembly
        {
            get;
            set;
        }

        public string Uri { get; set; }

        public string ViewName
        {
            get;
            set;
        }
    }
}