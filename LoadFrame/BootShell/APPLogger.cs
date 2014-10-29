using Microsoft.Practices.Prism.Logging;
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

namespace BootShell
{
    public class APPLogger : ILoggerFacade
    {
        private readonly Queue<Tuple<string, Category, Priority>> savedLogs = new Queue<Tuple<string, Category, Priority>>();
        private Action<string, Category, Priority> callback;

        public Action<string, Category, Priority> Callback
        {
            get { return this.callback; }
            set { this.callback = value; }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message">日志主体</param>
        /// <param name="category">日志类别</param>
        /// <param name="priority">优先级</param>
        public void Log(string message, Category category, Priority priority)
        {
            if (this.Callback != null)
            {
                this.Callback(message, category, priority);
            }
            else
            {
                this.savedLogs.Enqueue(new Tuple<string, Category, Priority>(message, category, priority));
            }
        }

        /// <summary>
        /// 重放日志
        /// </summary>
        public void ReplaySavedLogs()
        {
            if (this.Callback != null)
            {
                while (this.savedLogs.Count > 0)
                {
                    var log = this.savedLogs.Dequeue();
                    this.Callback(log.Item1, log.Item2, log.Item3);
                }
            }
        }
    }
}
