using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TKS.Service
{
    #region Public enums/event args
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType { Receive, UserEnter, UserLeave, ReceiveWhisper };

    /// <summary>
    /// 执行客户端回调的参数
    /// Receive, ReceiveWhisper, UserEnter, UserLeave <see cref="INotifyBoardCallback">
    /// INotifyBoardCallback</see> 
    /// </summary>
    public class ChatEventArgs : EventArgs
    {
        public MessageType msgType;
        public PushInfoEntity person;
        public string message;
    }

    #endregion
    #region NotifyService
    /// <summary>
    ///  通知服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class NotifyService : INotifyBoard
    {
        class DepedentInfo
        {
            public NotifyEventHandler NotifyHandler;
            public INotifyBoardCallback CallBackContext;
            public string SessionId;
        }
        #region Instance fields
        //thread sync lock object
        private static Object syncObj = new Object();

        INotifyBoardCallback callback;
        //delegate used for BroadcastEvent
        public delegate void NotifyEventHandler(object sender, ChatEventArgs e);
        public static event NotifyEventHandler ChatEvent;
        private NotifyEventHandler myEventHandler = null;
        //用户集合
        static Dictionary<PushInfoEntity, DepedentInfo> chatters = new Dictionary<PushInfoEntity, DepedentInfo>();
        //当前用户 
        private PushInfoEntity person;
        #endregion

        #region Helpers
        /// <summary>
        /// 判断是否存在指定用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool checkIfPersonExists(string name)
        {
            foreach (PushInfoEntity p in chatters.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///  从用户集合中找出指定用户，返回该用户的NotifyEventHandler委托，用以调用
        /// </summary>
        private NotifyEventHandler getPersonHandler(string name)
        {
            foreach (PushInfoEntity p in chatters.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    DepedentInfo chatTo = null;
                    chatters.TryGetValue(p, out chatTo);
                    return chatTo.NotifyHandler;
                }
            }
            return null;
        }
        private INotifyBoardCallback getPersonContextHandler(string name)
        {
            foreach (PushInfoEntity p in chatters.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    DepedentInfo context = null;
                    chatters.TryGetValue(p, out context);
                    return context.CallBackContext;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取指定用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private PushInfoEntity getPerson(string name)
        {
            foreach (PushInfoEntity p in chatters.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return p;
                }
            }
            return null;
        }
        #endregion
        #region INotifyBoard implementation

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public PushInfoEntity[] Join(PushInfoEntity person)
        {

            // 创建一个新的委托
            myEventHandler = new NotifyEventHandler(MyEventHandler);

            lock (syncObj)
            {
                if (person != null)
                {
                    if (!checkIfPersonExists(person.Name))
                    {
                        this.person = person;
                        INotifyBoardCallback _callback = OperationContext.Current.GetCallbackChannel<INotifyBoardCallback>();
                        callback = _callback;
                        string sessionid = OperationContext.Current.SessionId;
                        chatters.Add(person, new DepedentInfo
                        {
                            NotifyHandler = myEventHandler,
                            CallBackContext = _callback,
                             SessionId=sessionid
                        });

                        ChatEvent += myEventHandler;
                    }
                    else
                    {
                        this.person = getPerson(person.Name);
                    }
                }
            }
            ChatEventArgs e = new ChatEventArgs();
            e.msgType = MessageType.UserEnter;
           
            BroadcastMessage(e);

            PushInfoEntity[] list = new PushInfoEntity[chatters.Count];
            lock (syncObj)
            {
                chatters.Keys.CopyTo(list, 0);
            }

            return list;
        }

        /// <summary>
        ///  向所有用户发送信息
        /// </summary>
        public void Say(string msg)
        {
            ChatEventArgs e = new ChatEventArgs();
            e.msgType = MessageType.Receive;
          
            e.message = msg;
            try
            {
                BroadcastMessage(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 向指定的用户发送消息
        /// </summary>
        /// <param name="to"> </param>
        /// <param name="msg"></param>
        public void Whisper(string to, string msg)
        {
            ChatEventArgs e = new ChatEventArgs();
            e.msgType = MessageType.ReceiveWhisper;
            e.person = this.person;
            e.message = msg;
            try
            {
                NotifyEventHandler chatterTo;

                lock (syncObj)
                {
                    chatterTo = getPersonHandler(to);
                    if (chatterTo == null)
                    {
                        throw new KeyNotFoundException("用户 " + to +
                                                        " 找不到");
                    }
                }

                chatterTo.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
            }
            catch (KeyNotFoundException)
            {
            }
        }

        /// <summary>
        /// 用户登出，去除相关委托
        /// </summary>
        public void Leave()
        {
            if (this.person == null)
                return;


            NotifyEventHandler chatterToRemove = getPersonHandler(this.person.Name);


            lock (syncObj)
            {
                chatters.Remove(this.person);
            }

            ChatEvent -= chatterToRemove;
            ChatEventArgs e = new ChatEventArgs();
            e.msgType = MessageType.UserLeave;
          
            this.person = null;

            BroadcastMessage(e);
        }
        #endregion
        #region private methods

        /// <summary>
        /// NotifyEventHandler 委托调用的时候，调用的方法 
        /// </summary>
        private void MyEventHandler(object sender, ChatEventArgs e)
        {
            try
            {
                //INotifyBoardCallback callback = getPersonContextHandler(e.person.Name);
                switch (e.msgType)
                {
                    case MessageType.Receive:
                        callback.Receive(e.person, e.message);
                        break;
                    case MessageType.ReceiveWhisper:
                        callback.ReceiveWhisper(e.person, e.message);
                        break;
                    case MessageType.UserEnter:
                        callback.UserEnter(e.person);
                        break;
                    case MessageType.UserLeave:
                        callback.UserLeave(e.person);
                        break;
                }
            }
            catch (Exception ex)
            {
                Leave();
            }
        }

        /// <summary>
        ///遍历所有连接的用户，调用NotifyEventHandler委托
        /// </summary>
        /// <param name="e">The ChatEventArgs to use to send to all connected chatters</param>
        private void BroadcastMessage(ChatEventArgs e)
        {

            //NotifyEventHandler temp = ChatEvent;


            foreach (PushInfoEntity person in chatters.Keys)
            {
                NotifyEventHandler handler = getPersonHandler(person.Name);
                e.person = person;
                handler.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
            }

        }


        /// <summary>
        /// NotifyEventHandler 委托成功调用后，返回的调用
        /// </summary>
        /// <param name="ar">The asnch result</param>
        private void EndAsync(IAsyncResult ar)
        {
            NotifyEventHandler d = null;

            try
            {

                System.Runtime.Remoting.Messaging.AsyncResult asres = (System.Runtime.Remoting.Messaging.AsyncResult)ar;
                d = ((NotifyEventHandler)asres.AsyncDelegate);
                d.EndInvoke(ar);
            }
            catch
            {
                ChatEvent -= d;
            }
        }
        #endregion
    }
    #endregion
}
