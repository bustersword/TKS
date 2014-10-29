using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TKS.Service
{
    #region INotifyBoard interface
    /// <summary>
    ///  
    ///  接口提供四个方法，以供客户端调用
    ///  客户端需实现
    ///  <see cref="INotifyBoardCallback">IChatCallback</see>
    /// 回调方法
    ///  
    /// Say :  发送消息给所有人
    /// Whisper :  对指定用户发送消息
    /// Join :  用户登录
    /// Leave : 用户离开
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(INotifyBoardCallback))]
    interface INotifyBoard
    {
        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void Say(string msg);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void Whisper(string to, string msg);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        PushInfoEntity[] Join(PushInfoEntity name);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
        void Leave();
    }
    #endregion
    #region INotifyBoardCallback interface
    /// <summary>
    ///  客户端回调方法
    ///  
    /// Receive : 接收全局信息
    /// ReceiveWhisper :  接收指定接受者消息
    /// UserEnter : 接收通知，用户登录
    /// UserLeave : 接收通知，用户离开
    /// </summary>
    interface INotifyBoardCallback
    {
        [OperationContract(IsOneWay = true)]
        void Receive(PushInfoEntity sender, string message);

        [OperationContract(IsOneWay = true)]
        void ReceiveWhisper(PushInfoEntity sender, string message);

        [OperationContract(IsOneWay = true)]
        void UserEnter(PushInfoEntity PushInfo);

        [OperationContract(IsOneWay = true)]
        void UserLeave(PushInfoEntity PushInfo);
    }
    #endregion

    [DataContract]
    public class PushInfoEntity : INotifyPropertyChanged
    {


        #region Instance Fields
        private string userFlag;
        private string name;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

 
        public PushInfoEntity()
        {
        }


        public PushInfoEntity(string imageURL, string name)
        {
            this.userFlag = imageURL;
            this.name = name;
        }


        [DataMember]
        public string UserFlag
        {
            get { return userFlag; }
            set
            {
                userFlag = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("UserFlag");
            }
        }


        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Name");
            }
        }


        /// <summary>
        /// Notifies the parent bindings (if any) that a property
        /// value changed and that the binding needs updating
        /// </summary>
        /// <param name="propValue">The property which changed</param>
        protected void OnPropertyChanged(string propValue)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propValue));
            }
        }

    }
}
