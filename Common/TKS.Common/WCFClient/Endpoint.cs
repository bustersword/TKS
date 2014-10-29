using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TKS.Common
{
    public   class Endpoint
    {
        public EndpointAddress EndPointAddress
        {
            get;
            set;
        }

        public string Contract
        {
            get;
            set;
        }

        public string BindingConfiguration
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
