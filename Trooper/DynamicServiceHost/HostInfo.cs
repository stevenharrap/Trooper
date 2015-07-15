using System;
using System.Collections.Generic;
using Trooper.Interface.DynamicServiceHost;

namespace Trooper.DynamicServiceHost
{
    public class HostInfo : IHostInfo
    {
        public Uri Address { get; set; }

        public Uri ServiceNampespace { get; set; }

        public string CodeNamespace { get; set; }

        public string ServiceName { get; set; }

        public string InterfaceName { get; set; }

        public List<Method> Methods { get; set; }

        public Type SupportType { get; set; }
    }
}
