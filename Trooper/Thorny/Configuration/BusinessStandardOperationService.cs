namespace Trooper.Thorny.Configuration
{
    using Autofac;
    using Autofac.Integration.Wcf;
    using System;
    using System.ServiceModel;
    using Trooper.Interface.Thorny.Configuration;

    public class BusinessStandardOperationService : IBusinessStandardOperationService
    {
        public BusinessStandardOperationService(IBusinessStandardHostInfo hostInfo, ILifetimeScope container)
        {
            this.Container = container;
            this.HostInfo = hostInfo;
            this.AutoStart = true;
        }

        public bool AutoStart { get; set; }

        public IBusinessStandardHostInfo HostInfo { get; set; }        

        public ILifetimeScope Container { get; set; }

        public ServiceHost ServiceHost { get; private set; }

        public void Start()
        {
            this.ServiceHost = new ServiceHost(this.HostInfo.ServiceClass, this.HostInfo.Address);

            this.ServiceHost.AddDependencyInjectionBehavior(this.HostInfo.ServiceInterface, this.Container);

            BusinessOperationService.ConfigureHost(
                this.ServiceHost, 
                this.HostInfo.Address, 
                this.HostInfo.ServiceInterface, 
                this.HostInfo.ServiceNampespace);
            
            if (this.AutoStart)
            {
                this.ServiceHost.Open();
            }
        }

        public void Stop()
        {
            this.ServiceHost.Close();
        }
    }
}
