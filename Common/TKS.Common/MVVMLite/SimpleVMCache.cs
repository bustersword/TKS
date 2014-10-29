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

namespace TKS.Common.MVVMLite
{
    public class SimpleVMCache
    {
        private static SimpleVMCache _default;
        private readonly Dictionary<Type, object> _instancesRegistry = new Dictionary<Type,object>();
        private readonly object _syncLock = new object();
        public void Register<T>()where T:ViewModelBase,new()
        {
            Type tclass=typeof(T);
            lock (_syncLock)
            {
                if (_instancesRegistry.ContainsKey(tclass))
                {
                    return;
                }
                else
                {
                    _instancesRegistry.Add(tclass, new T());
                }
            }
        }

        public TClass GetInstance<TClass>()where TClass:ViewModelBase ,new()
        {
            Type tClass = typeof(TClass);
            lock (_instancesRegistry)
            {
                if (_instancesRegistry.ContainsKey(tClass))
                {
                    return _instancesRegistry[tClass] as TClass;
                }
                else
                {
                    throw new Exception("缓存内未注册的类型");
                }
            }
        }

        public static SimpleVMCache Default
        {
            get
            {
                return (_default ?? (_default = new SimpleVMCache()));
            }
        }
    }
}
