using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.Service.Helper
{
    public  class APPException:ApplicationException
    {
        public APPException(string message)
            : base(message)
        {

        }
    }
}
