using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Trooper.DynamicServiceHost.HostFactoryBuilder
{
    public class DynamicServiceHostFactory : ServiceHostFactory
    {
        private readonly HostInfo hostInfo;

        public DynamicServiceHostFactory()
        {
            this.hostInfo = new HostInfo();
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new DynamicServiceHost(this.hostInfo, serviceType, baseAddresses);
        }
    }
}
