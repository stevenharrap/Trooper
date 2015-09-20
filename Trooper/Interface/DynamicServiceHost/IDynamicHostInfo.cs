
using Exp018DynamicTake6.DynamicServiceHost;
using System;
using System.Collections.Generic;
using Trooper.DynamicServiceHost;
using Trooper.Utility;

namespace Trooper.Interface.DynamicServiceHost
{
    public interface IDynamicHostInfo
    {
        Uri Address { get; set; }

        Uri ServiceNampespace { get; set; }

        string CodeNamespace { get; set; }

        string ServiceName { get; set; }

        string InterfaceName { get; set; }

        List<Method> Methods { get; set; }

        List<ClassMapping> Mappings { get; set; }
    }
}
