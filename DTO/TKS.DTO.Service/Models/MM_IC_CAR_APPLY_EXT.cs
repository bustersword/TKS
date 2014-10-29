using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace TKS.DTO.Models
{
    public class IC_CAR_APPLY_E : IC_CAR_APPLY
    {

        public IC_CAR_APPLY_E()
        {
            //IsChecked = true;
        }

        bool isChecked = false ;
        /// <summary>
        /// 记录编号
        /// </summary>
        /// <value>The document c_ no.</value>
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value;
            RaisePropertyChanged(() => this.IsChecked);
            }
        }

        [Required(ErrorMessage = "必填项")]
        public override string DOC_NO
        {
            get
            {
                return base.DOC_NO;
            }
            set
            {
                
                base.DOC_NO = value;
            }
        }
      
        public override string USE_REASONS
        {
            get
            {
                return base.USE_REASONS;
            }
            set
            {
                base.USE_REASONS = value;
              
               
            }
        }

        [RegularExpression(@"^-?\d+$", ErrorMessage = "只能输入数字")]
        [Required(ErrorMessage = "必填项")]
        public override string STATUS_CODE
        {
            get
            {
                return base.STATUS_CODE;
            }
            set
            {
                base.STATUS_CODE = value;
                
            }
        }
        [Required(ErrorMessage = "必填项")]
        public override DateTime PLAN_START_TIME
        {
            get
            {
                return base.PLAN_START_TIME;
            }
            set
            {
               
                base.PLAN_START_TIME = value;
            }
        }
        [Required(ErrorMessage = "必填项")]
        public override DateTime PLAN_END_TIME
        {
            get
            {
                return base.PLAN_END_TIME;
            }
            set
            {
                base.PLAN_END_TIME = value;
            }
        }

        [Required(ErrorMessage = "必填项")]
        public override string PROC_OPINION
        {
            get
            {
                return base.PROC_OPINION;
            }
            set
            {
                base.PROC_OPINION = value;
            }
        }
        [Range(2, 50, ErrorMessage = "范围大小2-50")]
        [RegularExpression(@"^-?\d+$", ErrorMessage = "只能输入数字")]
        [Required(ErrorMessage = "必填项")]
        public override decimal MAN_NO
        {
            get
            {
                return base.MAN_NO;
            }
            set
            {
               
                base.MAN_NO = value;
            }
        }

        [Required(ErrorMessage = "必填项")]
        public override DateTime DECL_DATE
        {
            get
            {
                return base.DECL_DATE;
            }
            set
            {
              
                base.DECL_DATE = value;
            }
        }

    }

    public class IC_CAR_APPLY_D : IC_CAR_APPLY
    {
       

        [RegularExpression(@"^-?\d+$", ErrorMessage = "只能输入数字")]
        [Required(ErrorMessage = "必填项")]
        public override string STATUS_CODE
        {
            get
            {
                return base.STATUS_CODE;
            }
            set
            {
                base.STATUS_CODE = value;
            }
        }

        [Required(ErrorMessage = "必填项")]
        public override DateTime PLAN_START_TIME
        {
            get
            {
                return base.PLAN_START_TIME;
            }
            set
            {
                base.PLAN_START_TIME = value;
            }
        }
      
    }
}
