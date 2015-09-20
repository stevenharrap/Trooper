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
    public interface IBusinessDynamicHostInfo : IDynamicHostInfo
    {
        string BaseAddress { get; set; }

        bool UseDefaultTypes { get; set; }

        Action<IBusinessDynamicHostInfo> HostInfoBuilt { get; set; }
    }
}
