using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.DynamicServiceHost.HostFactoryBuilder
{
    public class DynamicServiceHost : ServiceHost
    {
        public DynamicServiceHost(HostInfo hostInfo, Type serviceType, params Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
            if (hostInfo == null)
            {
                throw new ArgumentNullException("hostInfo");
            }

            foreach (var cd in this.ImplementedContracts.Values)
            {
                cd.Behaviors.Add(new DynamicInstanceProvider(hostInfo, serviceType));
            }
        }
    }

}
