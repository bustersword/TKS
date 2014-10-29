using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace TKS.Core
{

	public class AssemblyNavigationContentLoaderAsyncResult : IAsyncResult {

		public bool IsCompleted {
			get;
			internal set;
		}

		public WaitHandle AsyncWaitHandle {
			get {
				return null;
			}
		}

		public object AsyncState {
			get;
			internal set;
		}

		public bool CompletedSynchronously {
			get {
				return false;
			}
		}

		public Assembly Assembly {
			get;
			internal set;
		}

		public string TypeName {
			get;
			internal set;
		}
        public string ModuleName
        {
            get;
            internal set;
        }
		 

        public Exception Error
        {
            get;
            internal set;
        }

		public object GetResultInstance() {
			var assembly = this.Assembly;
			if (assembly == null) {
				 
			}
			object result = null;
			if (assembly != null) {
                try
                {
                    result = assembly.CreateInstance(this.TypeName);
                }
                catch (TargetInvocationException ex)
                {
                    string error = string.Empty;
                    if (ex.InnerException != null)
                        error = ex.InnerException.Message;
                    throw new Exception("目标页内部错误：" + ex.Message+error);
                }
                catch (Exception ex)
                {
                   
                    throw new Exception("创建页面异常："+ex.Message);
                }
			}
			return result;
		}
	}
}