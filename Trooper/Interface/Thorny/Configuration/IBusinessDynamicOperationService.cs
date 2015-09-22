namespace Trooper.Interface.Thorny.Configuration
{
    using Autofac;
    using System;
    using System.ServiceModel;
    using Trooper.Interface.DynamicServiceHost;
    using Trooper.Interface.Thorny.Business.Operation.Core;

    public interface IBusinessDynamicOperationService : IBusinessOperationService
    {
        Func<object> Supporter { get; set; }

        IBusinessDynamicHostInfo HostInfo { get; set; }
    }
}
