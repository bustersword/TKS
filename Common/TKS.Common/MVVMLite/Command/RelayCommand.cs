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
    public class RelayCommand : System.Windows.Input.ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;
        //private EventHandler canExecuteChanged;

        public event EventHandler CanExecuteChanged;


        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
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
           
            return this._canExecute();
        }

        public virtual void Execute(object parameter)
        {
            if ((this.CanExecute(parameter) && (this._execute != null)) )
            {
                this._execute();
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
