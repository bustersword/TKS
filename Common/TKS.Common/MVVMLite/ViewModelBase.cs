namespace TKS.Common.MVVMLite
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;

    public class ViewModelBase : INotifyPropertyChanged 
    {
      

        public event PropertyChangedEventHandler PropertyChanged;


       


        protected string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }
            MemberExpression body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }
            PropertyInfo member = body.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }
            return member.Name;
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                string propertyName = this.GetPropertyName<T>(propertyExpression);
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        

        protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
           
            field = newValue;
            this.RaisePropertyChanged<T>(propertyExpression);
            return true;
        }

        protected bool Set<T>(string propertyName, ref T field, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
           
            field = newValue;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        [Conditional("DEBUG"), DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            Type type = base.GetType();
            if (!string.IsNullOrEmpty(propertyName) && (type.GetProperty(propertyName) == null))
            {
                throw new ArgumentException("Property not found", propertyName);
            }
        }

        protected PropertyChangedEventHandler PropertyChangedHandler
        {
            get
            {
                return this.PropertyChanged;
            }
        }

       
    }
}

