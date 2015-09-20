using Exp018DynamicTake6.DynamicServiceHost;
using System;
using System.Collections.Generic;
using Trooper.Interface.DynamicServiceHost;
using Trooper.Utility;

namespace Trooper.DynamicServiceHost
{
    public class HostInfo : IDynamicHostInfo
    {
        public Uri Address { get; set; }

        public Uri ServiceNampespace { get; set; }

        public string CodeNamespace { get; set; }

        public string ServiceName { get; set; }

        public string InterfaceName { get; set; }

        public List<Method> Methods { get; set; }

        public List<ClassMapping> Mappings { get; set; }


    }
}
