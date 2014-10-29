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
using System.ComponentModel.DataAnnotations;
namespace TKS.Common
{
    public sealed class CustomizeValidation : ValidationAttribute
    {
        #region Private memebers
        private string message;
        #endregion

        #region Public Property
        public bool ShowErrorMessage
        {
            get;
            set;
        }

        public object ValidationError
        {
            get
            {
                return null;
            }
            set
            {
                if (ShowErrorMessage)
                {
                    throw new ValidationException(message);
                }
            }
        }
        #endregion

        #region Constructor
        public CustomizeValidation()
        {
        }

        public CustomizeValidation(string message)
        {
            this.message = message;
        }
        #endregion

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                String checkName = value.ToString();

                return checkName.Length < 12 ? ValidationResult.Success : new ValidationResult("用户名长度不能超过12");
                //return checkName == "jv9" ? ValidationResult.Success : new ValidationResult("请使用指定用户名");
            }
            else
            {
                return new ValidationResult("用户名不能为空");
            }
        }
    }
}
