using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace TKS.Common.MVVMLite.Messaging
{
    /// <summary>
    /// 全局通讯类
    /// </summary>
    public class Messaging:IMessaging
    {
        static IMessaging _defaultInstance;
        Dictionary<Type, List<WeakObjectAndToken>> _recipientsStrictObject;
        readonly object _sendLock = new object();
        static readonly object CreationLock = new object();
        [StructLayout(LayoutKind.Sequential)]
        private struct WeakObjectAndToken
        {
            public WeakReference Reference;
            public object Token;
        }

        /// <summary>
        /// 发送自定义类型的消息
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="message">消息</param>
        public void Send<TMessage, TTarget>(TMessage message)
        {
            Send<TMessage, TTarget>(message, null);
        }

        /// <summary>
        /// 接收自定义类型的消息（接收完后即销毁该消息）
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <typeparam name="TReceive">接收者类型（和目标类型相同）</typeparam>
        /// <returns>消息</returns>
        public TMessage Receive<TMessage, TReceive>()
        {
            return Receive<TMessage, TReceive>(null);
        }

        /// <summary>
        /// 发送自定义类型的消息
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="message">消息</param>
        /// <param name="token">Token</param>
        public void Send<TMessage, TTarget>(TMessage message, object token)
        {
            lock (_sendLock)
            {
                Dictionary<Type, List<WeakObjectAndToken>> _dic;
                Type targetT = typeof(TTarget);
                WeakReference reference = new WeakReference(message);
                if (_recipientsStrictObject == null)
                {
                    this._recipientsStrictObject = new Dictionary<Type, List<WeakObjectAndToken>>();
                }
                _dic = this._recipientsStrictObject;

                lock (_dic)
                {
                    List<WeakObjectAndToken> list;
                    if (_dic.ContainsKey(targetT))
                    {
                        list = _dic[targetT];
                    }
                    else
                    {
                        list = new List<WeakObjectAndToken>();
                        _dic.Add(targetT, list);
                    }

                    List<WeakObjectAndToken> _hasList = list.Where(p => p.Token == token).ToList();
                    if (_hasList.Count > 0)
                    {
                        //存在移除
                        list.Remove(_hasList[0]);
                    }

                    WeakObjectAndToken item = new WeakObjectAndToken
                    {
                        Reference = reference,
                        Token = token
                    };
                    list.Add(item);
                }
            }
        }

        /// <summary>
        /// 接收自定义类型的消息（接收完后即销毁该消息）
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <typeparam name="TReceive">接收者类型（和目标类型相同）</typeparam>
        /// <param name="token">Token</param>
        /// <returns>消息</returns>
        public TMessage Receive<TMessage, TReceive>(object token)
        {
            if (_recipientsStrictObject != null)
            {
                lock (_recipientsStrictObject)
                {
                    Type receiveT = typeof(TReceive);
                    if(_recipientsStrictObject.ContainsKey(receiveT ))
                    {
                        List<WeakObjectAndToken> list = _recipientsStrictObject[receiveT].
                            Where(p => ((p.Reference!=null &&p.Reference.IsAlive)&& 
                                ((p.Token ==null &&token ==null ) 
                                ||( p.Token !=null &&p.Token.Equals(token)))
                                )).ToList ();
                        if (list.Count > 0)
                        {
                            TMessage res = default(TMessage);
                            res =(TMessage)( list[0].Reference.Target) ;
                            //即用即销
                            _recipientsStrictObject[receiveT].Remove(list[0]);
                            return  res;
                        }
                    }
                }
            }
            return default(TMessage);
        }

        public static IMessaging Default
        {
            get
            {
                if (_defaultInstance == null)
                {
                    lock (CreationLock)
                    {
                        if (_defaultInstance == null)
                        {
                            _defaultInstance = new Messaging();
                        }
                    }
                }
                return _defaultInstance;
            }
        }
    }
}
