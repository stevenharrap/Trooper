namespace Trooper.Thorny.Configuration
{
    using Autofac;
    using System;
    using System.Linq;
    using System.ServiceModel;
    using Trooper.DynamicServiceHost;
    using Trooper.Interface.DynamicServiceHost;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Configuration;

    public class BusinessOperationService : IBusinessOperationService
    {
        public BusinessOperationService(Func<object> supporter, IBusinessHostInfo hostInfo)
        {
            this.Supporter = supporter;
            this.HostInfo = hostInfo;
            this.AutoStart = true;
        }

        public bool AutoStart { get; set; }

        public Func<object> Supporter { get; set; }

        public IBusinessHostInfo HostInfo { get; set; }

        public ServiceHost ServiceHost { get; private set; }

        public void Start()
        {
            HostInfoHelper.BuildHostInfo(this.Supporter, this.HostInfo);

            if (this.HostInfo.HostInfoBuilt != null)
            {
                this.HostInfo.HostInfoBuilt(this.HostInfo);
            }

            this.ServiceHost = HostBuilder.BuildHost(this.HostInfo, this.Supporter);

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
