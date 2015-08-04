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

    //public delegate void HostBuiltHandler(IHostInfo hostInfo);

    public class BusinessOperationService : IBusinessOperationService
    {
        public BusinessOperationService(Func<object> supporter, IHostInfo hostInfo, Action<IHostInfo> hostInfoBuilt)
        {
            this.Supporter = supporter;
            this.HostInfo = hostInfo;
            this.HostInfoBuilt = hostInfoBuilt;
        }

        public Func<object> Supporter { get; set; }

        public IHostInfo HostInfo { get; set; }

        public ServiceHost ServiceHost { get; private set; }

        public Action<IHostInfo> HostInfoBuilt;

        public void Start()
        {
            HostInfoHelper.BuildHostInfo(this.Supporter, this.HostInfo);

            if (this.HostInfoBuilt != null)
            {
                this.HostInfoBuilt(this.HostInfo);
            }

            this.ServiceHost = HostBuilder.BuildHost(this.HostInfo, this.Supporter);

            this.ServiceHost.Open();
        }
    }
}
