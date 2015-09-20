using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.DynamicServiceHost;
using Trooper.Interface.Thorny.Configuration;
using Trooper.Thorny.Interface.DataManager;

namespace Trooper.Thorny.Configuration
{
    public class BusinessHostInfo : HostInfo, IBusinessDynamicHostInfo
    {
        public BusinessHostInfo()
        {
            this.UseDefaultTypes = true;
        }

        public string BaseAddress { get; set; }

        public bool UseDefaultTypes { get; set; }

        public Action<IBusinessDynamicHostInfo> HostInfoBuilt { get; set; }
    }

    
}
