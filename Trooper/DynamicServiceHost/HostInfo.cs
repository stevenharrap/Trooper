using System;
using System.Collections.Generic;

namespace Trooper.DynamicServiceHost
{
    public class HostInfo
    {
        public Uri BaseAddress { get; set; }

        public Uri ServiceNampespace { get; set; }

        public string CodeNamespace { get; set; }

        public string ServiceName { get; set; }

        public string InterfaceName { get; set; }

        public List<Method> Methods { get; set; }

        public Type SupportType { get; set; }
    }
}
