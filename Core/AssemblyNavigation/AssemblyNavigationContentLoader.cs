/*  ʵ��INavigationContentLoader ������Assembly��ʹ��creatInstance��ʽ�����ݲ˵�URI��ֱ�ӵ�����ҳ��
 *  �������ַ���е�URI��ַ��ʾ���֣����Է�����ϵͳҵ��ģ��ԭ�еĻ�ȡURI������ʽ
 *  ���ݳ���ʹ��Frame 
 *  ����Ҫҵ��ģ���������
 * 
 * 
 * */

using Microsoft.Practices.Prism.Modularity;
using System;
using System.Windows.Navigation;

namespace TKS.Core
{

    public class AssemblyNavigationContentLoader : INavigationContentLoader
    {
        public Func<Uri, bool> IsCanload
        {
            get;
            set;
        }

        public IModuleManager ModuleManager
        {
            get;
            set;
        }
        public event EventHandler<DownloadingEventArgs> OnDownloading;
        public event EventHandler<DownloadFaileddEventArgs> OnDownloadFailed;

        public IAsyncResult BeginLoad(Uri targetUri, Uri currentUri, AsyncCallback userCallback, object asyncState)
        {
            AssemblyDownloader _downloader = new AssemblyDownloader();
            _downloader.ModuleManager = ModuleManager;
            string uri = targetUri.ToString();
            string[] u = uri.Split(new char[] { ',' });
            if(u.Length !=2)throw  new Exception ("�벻Ҫ�����޸ĵ�ַ����ַ");
            var moduleName = uri;
            AssemblyNavigationContentLoaderAsyncResult asyncResult = new AssemblyNavigationContentLoaderAsyncResult() {  
                AsyncState=asyncState,
                IsCompleted =false  
                 
            };
            _downloader.ReadyNavigate += (_o, _e) => {
                asyncResult.Assembly = _e.Module.CurrentAssembly;
                asyncResult.IsCompleted = false ;
                asyncResult.ModuleName = _e.Module.MoudleName;
                asyncResult.Error = null;
                asyncResult.TypeName = _e.Module.ViewName;
                //�����ص�
                userCallback(asyncResult);
            
            };
            _downloader.OnDownloadCompleted += (o, e) =>
            {
                asyncResult.Assembly = e.Module.CurrentAssembly;
                asyncResult.IsCompleted = false;
                asyncResult.ModuleName = e.Module.MoudleName;
                asyncResult.Error = null;
                asyncResult.TypeName = e.Module.ViewName;
                //��һ������
                userCallback(asyncResult);

            };

            _downloader.OnDownloading += OnDownloading;
            _downloader.OnDownloadFailed += OnDownloadFailed;
            _downloader.DownloadAsync(new ModuleDes { MoudleName = u[0], Uri = u[1] });
            return asyncResult;
        }


        public bool CanLoad(Uri targetUri, Uri currentUri)
        {
            return IsCanload(targetUri);
        }

        public void CancelLoad(IAsyncResult asyncResult)
        {

        }

        public LoadResult EndLoad(IAsyncResult asyncResult)
        {
            AssemblyNavigationContentLoaderAsyncResult res = asyncResult as AssemblyNavigationContentLoaderAsyncResult;
            if (res.Assembly != null)
            {
                var o = res.GetResultInstance();
                return new LoadResult(o);
            }
            else
            {
                throw new Exception(res.ModuleName +"�����쳣��"+res.Error.Message);
            }
        }
    }
}