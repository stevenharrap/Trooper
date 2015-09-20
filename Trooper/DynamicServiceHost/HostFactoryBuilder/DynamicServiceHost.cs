using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.DynamicServiceHost;

namespace Trooper.DynamicServiceHost.HostFactoryBuilder
{
    public class DynamicServiceHost : ServiceHost
    {
        public DynamicServiceHost(IDynamicHostInfo hostInfo, Func<object> supporter, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            if (hostInfo == null)
            {
                throw new ArgumentNullException("hostInfo");
            }

            foreach (var cd in this.ImplementedContracts.Values)
            {
                cd.Behaviors.Add(new DynamicInstanceProvider(hostInfo, supporter, serviceType));
            }
        }
    }

}
