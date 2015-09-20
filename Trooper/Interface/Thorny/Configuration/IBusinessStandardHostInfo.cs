using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.DynamicServiceHost;
using Trooper.Interface.DynamicServiceHost;
using Trooper.Thorny.Interface.DataManager;

namespace Trooper.Interface.Thorny.Configuration
{
    public interface IBusinessStandardHostInfo
    {
        Uri Address { get; set; }

        Uri ServiceNampespace { get; set; }

        Type ServiceClass { get; set; }

        Type ServiceInterface { get; set; }

        string BaseAddress { get; set; }
    }
}
