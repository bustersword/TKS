using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using TKS.Controls;
using Microsoft.Practices.Prism.Regions;

namespace TKS.Core
{

    public class AssemblyDownloader
    {
        #region field

        public IModuleManager ModuleManager;

        private bool _isbusy;

        #endregion
        public event EventHandler<DownloadCommpletedEventArgs> OnDownloadCompleted;
        public event EventHandler<DownloadingEventArgs> OnDownloading;
        public event EventHandler<DownloadFaileddEventArgs> OnDownloadFailed;
        public event EventHandler<DownloadCommpletedEventArgs> ReadyNavigate;
        private string _loadingModuleName;
        private string TargetUri;

        string ViewName
        {
            get
            {
                string viewName = string.Empty;
                if (!string.IsNullOrEmpty(TargetUri))
                    if (TargetUri.IndexOf('?') != -1)
                    {
                        string[] s = TargetUri.Split(new char[] { '?' });
                        
                        viewName = s[0];
                        
                    }
                    else
                    {
                        viewName = TargetUri;
                    }
                return viewName;
            }
        }

       
        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="targetUri"></param>
        public void Navigate(Uri targetUri)
        {
            string uri = targetUri.ToString();
            string[] u = uri.Split(new char[] { ',' });
            var moduleName = uri;

            DownloadAsync(new ModuleDes { MoudleName = u[0], Uri = u[1] });

        }

        /// <summary>
        /// 导航入口点
        /// </summary>
        /// <param name="moduleName"></param>
        public void DownloadAsync(ModuleDes module)
        {
            if (this._isbusy)
            {
                //throw new InvalidOperationException(string.Format("当前正在加载模块{0}, 请等待 ...", this._loadingModuleName));
            }
            this._loadingModuleName = module.MoudleName;
            TargetUri = module.Uri;
            MefModuleManager manager = ModuleManager as MefModuleManager;

            Assembly assembly = GetReadyAssembly(manager, module.MoudleName);
            if (assembly != null)
            {

                this.ReadyNavigate(this, new DownloadCommpletedEventArgs(new ModuleDes
                {
                    MoudleName = module.MoudleName,
                    Uri = TargetUri,
                    CurrentAssembly = assembly,
                    ViewName = ViewName
                },
                   null, true, null));
            }
            else
            {

                ModuleManager.ModuleDownloadProgressChanged -= ModuleManager_ModuleDownloadProgressChanged;
                ModuleManager.ModuleDownloadProgressChanged += ModuleManager_ModuleDownloadProgressChanged;
                ModuleManager.LoadModuleCompleted -= ModuleManager_LoadModuleCompleted;
                ModuleManager.LoadModuleCompleted += ModuleManager_LoadModuleCompleted;

                ModuleManager.LoadModule(module.MoudleName);
            }
        }

        void ModuleManager_LoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            this._isbusy = false;
            if (e.Error != null)
            {
                if (OnDownloadFailed != null)
                    OnDownloadFailed(sender,
                        new DownloadFaileddEventArgs(new ModuleDes
                        {
                            MoudleName = e.ModuleInfo.ModuleName,
                            Uri =  TargetUri
                        },new Exception( "加载包异常",e.Error), false, null));
            }
            else
            {
                MefModuleManager manager = sender as MefModuleManager;
                Assembly asm = GetReadyAssembly(manager, e.ModuleInfo.ModuleName);


                if (OnDownloadCompleted != null)
                    this.OnDownloadCompleted(this, new DownloadCommpletedEventArgs(new ModuleDes
                    {
                        MoudleName = e.ModuleInfo.ModuleName,
                        Uri =  TargetUri,
                        ViewName = ViewName,
                        CurrentAssembly = asm
                    },
                        null, true, null));
            }
        }

        private Assembly GetReadyAssembly(MefModuleManager manager, string moduleName)
        {

            List<Lazy<IModule, IModuleExport>> modules = manager.ImportedModules.Where(p => p.Metadata.ModuleName == moduleName).ToList();
            Assembly assembly = null;
            if (modules.Count > 0)
            {
                assembly = modules[0].Metadata.ModuleType.Assembly;
            }

            return assembly;
        }

        void ModuleManager_ModuleDownloadProgressChanged(object sender, ModuleDownloadProgressChangedEventArgs e)
        {

            if (OnDownloading != null)
                OnDownloading(sender, new DownloadingEventArgs(new ModuleDes
                {
                    MoudleName = e.ModuleInfo.ModuleName,
                    Uri =  TargetUri,
                    ViewName = ViewName
                }, e.ModuleInfo, e.BytesReceived, e.TotalBytesToReceive));
        }
    }
}