using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TKS.Common.MVVMLite.Messaging
{
    public interface IMessaging
    {
        void Send<TMessage, TTarget>(TMessage message);
        TMessage Receive<TMessage, TReceive>();

        void Send<TMessage, TTarget>(TMessage message,object token);
        TMessage Receive<TMessage, TReceive>(object token);
    }
}
