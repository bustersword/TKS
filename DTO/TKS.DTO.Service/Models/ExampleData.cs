using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using TKS.DTO.Core;


namespace TKS.DTO.Models
{
    public class ExampleData:BaseModel
    {
        [Required(ErrorMessage = "必填项")]
        public string Name
        {
            get { return GetDataField<string>("Name"); }
            set { SetDataField<string>("Name", value); }
        }
        [Required(ErrorMessage = "必填项")]
        [RegularExpression(@"^-?\d+$", ErrorMessage = "只能输入数字")]
        public int Age
        {
            get { return GetDataField<int>("Age"); }
            set { SetDataField<int>("Age", value); }
        }
    }
}
