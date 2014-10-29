using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateFlows
{
    class Flow
    {
        public string FlowName
        {
            get;
            set;
        }

        public List<Step> Steps
        {
            get;
            set;
        }
    }

    class Step
    {
        public string AssemblyName
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public bool IsIgnore
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }
    }
}
