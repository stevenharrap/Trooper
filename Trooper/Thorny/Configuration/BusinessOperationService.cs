namespace Trooper.Thorny.Configuration
{
    using Autofac;
    using System.ServiceModel;
    using Trooper.DynamicServiceHost;
    using Trooper.Interface.DynamicServiceHost;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Configuration;

    public class BusinessOperationService : IBusinessOperationService
    {
        private IComponentContext container;

        public BusinessOperationService(IHostInfo hostInfo, IComponentContext container)
        {
            this.HostInfo = hostInfo;
            this.container = container;
        }

        public IHostInfo HostInfo { get; set; }

        public ServiceHost ServiceHost { get; private set; }

        public void Start()
        {
            HostInfoHelper.BuildHostInfo(this.HostInfo, container);

            this.ServiceHost = HostBuilder.BuildHost(this.HostInfo);

            this.ServiceHost.Open();
        }
    }
}
