
namespace MainPage
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "PushInfoEntity", Namespace = "http://schemas.datacontract.org/2004/07/NotifyService")]
    public partial class PushInfoEntity : object, System.ComponentModel.INotifyPropertyChanged
    {
        private string NameField;

        private string UserFlagField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                if (object.ReferenceEquals(this.NameField, value) != true)
                {
                    this.NameField = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserFlag
        {
            get
            {
                return this.UserFlagField;
            }
            set
            {
                if (object.ReferenceEquals(this.UserFlag, value) != true)
                {
                    this.UserFlagField = value;
                    RaisePropertyChanged("UserFlag");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "INotifyBoard", CallbackContract = typeof(INotifyBoardCallback))]
    public interface INotifyBoard
    {


        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, AsyncPattern = true, Action = "http://tempuri.org/INotifyBoard/Say")]
        System.IAsyncResult BeginSay(string msg, System.AsyncCallback callback, object asyncState);

        void EndSay(System.IAsyncResult result);


        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, AsyncPattern = true, Action = "http://tempuri.org/INotifyBoard/Whisper")]
        System.IAsyncResult BeginWhisper(string to, string msg, System.AsyncCallback callback, object asyncState);

        void EndWhisper(System.IAsyncResult result);


        [System.ServiceModel.OperationContractAttribute(AsyncPattern = true, Action = "http://tempuri.org/INotifyBoard/Join", ReplyAction = "http://tempuri.org/INotifyBoard/JoinResponse")]
        System.IAsyncResult BeginJoin(PushInfoEntity name, System.AsyncCallback callback, object asyncState);

        PushInfoEntity[] EndJoin(System.IAsyncResult result);


        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, AsyncPattern = true, Action = "http://tempuri.org/INotifyBoard/Leave")]
        System.IAsyncResult BeginLeave(System.AsyncCallback callback, object asyncState);

        void EndLeave(System.IAsyncResult result);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface INotifyBoardCallback
    {

        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://tempuri.org/INotifyBoard/Receive")]
        void Receive(PushInfoEntity sender, string message);


        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://tempuri.org/INotifyBoard/ReceiveWhisper")]
        void ReceiveWhisper(PushInfoEntity sender, string message);


        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://tempuri.org/INotifyBoard/UserEnter")]
        void UserEnter(PushInfoEntity PushInfo);


        [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://tempuri.org/INotifyBoard/UserLeave")]
        void UserLeave(PushInfoEntity PushInfo);

    }

}
