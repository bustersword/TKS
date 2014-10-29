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

namespace TKS.Common.MVVMLite.Command
{
    public class RelayCommand<T> : System.Windows.Input.ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;
        //private EventHandler CanExecuteChanged;

        public event EventHandler CanExecuteChanged;


        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this._execute = execute;
            if (canExecute != null)
            {
                this._canExecute = canExecute;
            }
        }

        public bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }
          
            if ((parameter == null) && typeof(T).IsValueType)
            {
                return this._canExecute(default(T));
            }
            return this._canExecute((T)parameter);
        }

        public virtual void Execute(object parameter)
        {
            object obj2 = parameter;
            if (((parameter != null) && (parameter.GetType() != typeof(T))) && (parameter is IConvertible))
            {
                obj2 = Convert.ChangeType(parameter, typeof(T), null);
            }
            if ((this.CanExecute(obj2) && (this._execute != null)))
            {
                if (obj2 == null)
                {
                    if (typeof(T).IsValueType)
                    {
                        this._execute(default(T));
                    }
                    else
                    {
                        this._execute((T)obj2);
                    }
                }
                else
                {
                    this._execute((T)obj2);
                }
            }
        }

        public void RaiseCanExecuteChanged()
        {
            EventHandler canExecuteChanged = this.CanExecuteChanged;
            if (canExecuteChanged != null)
            {
                canExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
