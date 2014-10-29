using System;
using System.Collections.Generic;

namespace TKS.Controls 
{
    /// <summary>
    /// 自定义用户对话框
    /// </summary>
    public static class Message
    {

        /// <summary>
        /// 错误信息提示对话框
        /// </summary>
        /// <param name="message">显示的信息</param>
        /// <param name="ex">抛出的异常</param>
        public static void ErrorMessage(string message, Exception ex)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    new CustomMessage(message, CustomMessage.MessageType.Error, ex).Show();
                });
            }
            else
            {
                new CustomMessage(message, CustomMessage.MessageType.Error, ex).Show();
            }
        }

        /// <summary>
        /// 一般提示对话框
        /// </summary>
        /// <param name="message">显示的信息</param>
        public static void InfoMessage(string message)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    new CustomMessage(message, CustomMessage.MessageType.Info).Show();
                });
            }
            else
            {
                new CustomMessage(message, CustomMessage.MessageType.Info).Show();
            }
        }

        /// <summary>
        /// 警告对话框
        /// </summary>
        /// <param name="message">显示的信息</param>
        public static void WarnMessage(string message)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    Warn(message, null, null);
                });
            }
            else
            {
                Warn(message, null, null);
            }
        }

        /// <summary>
        /// 警告对话框
        /// </summary>
        /// <param name="message">显示的信息</param>
        /// <param name="okCall">确定后回调方法</param>
        /// <param name="cancelCall">取消后回调方法</param>
        public static void WarnMessage(string message, Action okCall, Action cancelCall)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    Warn(message, okCall, cancelCall);
                });
            }
            else
            {
                Warn(message, okCall, cancelCall);
            }
        }

        private static void Warn(string message, Action okCall, Action cancelCall)
        {
            CustomMessage cus = new CustomMessage(message, CustomMessage.MessageType.Warn);
            cus.OKButton.Click += (sender, e) =>
            {
                if (okCall != null)
                {
                    okCall();
                }
            };
            cus.CancelButton.Click += (sender, e) =>
            {
                if (cancelCall != null)
                {
                    cancelCall();
                }
            };

            cus.Show();
        }

        /// <summary>
        /// 确认对话框
        /// </summary>
        /// <param name="message">显示信息</param>
        /// <param name="okCall">确定后回调方法</param>
        /// <param name="cancelCall">取消后回调方法</param>
        public static void ConfirmMessage(string message, Action okCall, Action cancelCall)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    Confirm(message, okCall, cancelCall);
                });
            }
            else
            {
                Confirm(message, okCall, cancelCall);
            }
        }
        /// <summary>
        /// 确认对话框
        /// </summary>
        /// <param name="message">显示信息</param>
        /// <param name="okCall">确定后回调方法</param>
        /// <param name="cancelCall">取消后回调方法</param>
        public static void ActionMessage(string message, Action okCall)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    Action(message, okCall);
                });
            }
            else
            {
                Action(message, okCall);
            }
        }
        private static void Confirm(string message, Action okCall, Action cancelCall)
        {
            CustomMessage cus = new CustomMessage(message, CustomMessage.MessageType.Confirm);
            cus.OKButton.Click += (sender, e) =>
            {
                if (okCall != null)
                {
                    okCall();
                }
            };
            cus.CancelButton.Click += (sender, e) =>
            {
                if (cancelCall != null)
                {
                    cancelCall();
                }
            };

            cus.Show();
        }


        private static void Action(string message, Action okCall)
        {
            CustomMessage cus = new CustomMessage(message, CustomMessage.MessageType.Action);
            cus.OKButton.Click += (sender, e) =>
            {
                if (okCall != null)
                {
                    okCall();
                }
            };

            cus.Show();
        }
        /// <summary>
        /// 文本对话框
        /// </summary>
        /// <param name="message">显示的信息</param>
        /// <param name="call">确认后回调方法</param>
        public static void TextMessage(string message, Action<string> call)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    Text(message, call);
                });
            }
            else
            {
                Text(message, call);
            }
        }

        private static void Text(string message, Action<string> call)
        {
            CustomMessage cus = new CustomMessage(message, CustomMessage.MessageType.TextInput);
            cus.OKButton.Click += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(cus.Input))
                {
                    call(cus.Input);
                }
                else
                {
                    Message.WarnMessage("没有输入");
                }
            };

            cus.Show();
        }

        /// <summary>
        /// 选择项对话框
        /// </summary>
        /// <param name="message">显示的信息</param>
        /// <param name="inputOptions">供选择的项</param>
        /// <param name="Call">确定后回调方法</param>
        public static void ComboMessage(string message, List<string> inputOptions, Action<string> call)
        {
            if (!System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    Combo(message, inputOptions, call);
                });
            }
            else
            {
                Combo(message, inputOptions, call);
            }
        }

        private static void Combo(string message, List<string> inputOptions, Action<string> call)
        {
            CustomMessage cus = new CustomMessage(message, CustomMessage.MessageType.ComboInput, null, inputOptions);

            cus.OKButton.Click += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(cus.Input))
                {
                    call(cus.Input);
                }
                else
                {
                    Message.WarnMessage("没有选择项");
                }
            };

            cus.Show();
        }


    }
}
