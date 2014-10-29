using System;
using System.Windows;

using TKS.Common;
using TKS.Controls;
namespace MainPage
{


    public enum CallBackType { Receive, ReceiveWhisper, UserEnter, UserLeave };

    /// <summary>
    /// Proxy event args
    /// </summary>
    public class ProxyEventArgs : EventArgs
    {

        public PushInfoEntity[] list;
    }

    /// <summary>
    /// Proxy callback event args
    /// </summary>
    public class ProxyCallBackEventArgs : EventArgs
    {

        public CallBackType callbackType;

        public string message = "";

        public PushInfoEntity person = null;

    }

    public class NetTcpProxy : INotifyBoardCallback
    {
        public int UserCount
        {
            get;
            set;
        }

        static INotifyBoard proxy;

        private static NetTcpProxy _tcpProxy;

        public static NetTcpProxy GetInstance()
        {
            if (_tcpProxy == null)
            {
                _tcpProxy = new NetTcpProxy();
            }
            return _tcpProxy;
        }

        #region INotifyBoardCallback 成员


        public void Receive(PushInfoEntity sender, string message)
        {
            Receive(sender, message, CallBackType.Receive);
        }


        public void ReceiveWhisper(PushInfoEntity sender, string message)
        {
            Receive(sender, message, CallBackType.ReceiveWhisper);
        }

        private void Receive(PushInfoEntity sender, string message, CallBackType callbackType)
        {
            ProxyCallBackEventArgs e = new ProxyCallBackEventArgs();
            e.message = message;
            e.callbackType = callbackType;
            e.person = sender;
            OnProxyCallBackEvent(e);
        }


        public void UserEnter(PushInfoEntity person)
        {
            UserEnterLeave(person, CallBackType.UserEnter);
        }


        public void UserLeave(PushInfoEntity person)
        {
            UserEnterLeave(person, CallBackType.UserLeave);
        }


        private void UserEnterLeave(PushInfoEntity person, CallBackType callbackType)
        {
            ProxyCallBackEventArgs e = new ProxyCallBackEventArgs();
            e.person = person;
            e.callbackType = callbackType;
            OnProxyCallBackEvent(e);
        }

        /// <summary>
        /// Raises the event for connected subscribers
        /// </summary>
        /// <param name="e"><see cref="ProxyCallBackEventArgs">ProxyCallBackEventArgs</see> event args</param>
        protected void OnProxyCallBackEvent(ProxyCallBackEventArgs e)
        {
            if (ProxyCallBackEvent != null)
            {
                // Invokes the delegates. 
                ProxyCallBackEvent(this, e);
            }
        }

        public void Connect(PushInfoEntity p)
        {
            System.ServiceModel.InstanceContext site = new System.ServiceModel.InstanceContext(this);
            proxy = ClientService<INotifyBoard>.GetNetTcpService(site);

            proxy.BeginJoin(p, (res) =>
            {
                try
                {
                    PushInfoEntity[] r = proxy.EndJoin(res);

                    HandleEndJoin(r);
                }
                catch (Exception ex)
                {
                    Message.ErrorMessage("注册失败", ex);
                }
            }, null);


        }



        private void HandleEndJoin(PushInfoEntity[] list)
        {

            if (list == null)
            {
                // Message.ErrorMessage ("用户为空",null);
            }
            else
            {
                ProxyEventArgs e = new ProxyEventArgs();
                e.list = list;
                if (ProxyEvent != null)
                    ProxyEvent(this, e);
            }
        }

        public void Say(string msg)
        {

            proxy.BeginSay(msg, (res) =>
            {
                try
                {
                    proxy.EndSay(res);
                }
                catch (Exception ex)
                {
                    Message.ErrorMessage("发送信息错误", ex);
                }

            }, null);
        }

        public void Leave(Action call)
        {

            proxy.BeginLeave((res) =>
            {
                proxy.EndLeave(res);

                if (call != null)
                    call();
            }, null);
        }

        //main proxy event
       
        public EventHandler<ProxyEventArgs > ProxyEvent;
        //callback proxy event
     
        public EventHandler<ProxyCallBackEventArgs>   ProxyCallBackEvent;

        #endregion
    }
}
