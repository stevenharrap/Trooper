using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Trooper.Interface.DynamicServiceHost;

namespace Trooper.DynamicServiceHost.HostFactoryBuilder
{
    public class DynamicServiceHostFactory : ServiceHostFactory
    {
        private readonly IDynamicHostInfo hostInfo;

        private readonly Func<object> supporter;

        public DynamicServiceHostFactory()
        {
            this.hostInfo = null;
            this.supporter = null;
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new DynamicServiceHost(this.hostInfo, this.supporter, serviceType, baseAddresses);
        }
    }
}
