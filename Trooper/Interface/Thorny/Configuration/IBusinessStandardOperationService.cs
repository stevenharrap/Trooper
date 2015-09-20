namespace Trooper.Interface.Thorny.Configuration
{
    using Autofac;
    using System;
    using System.ServiceModel;
    using Trooper.Interface.DynamicServiceHost;
    using Trooper.Interface.Thorny.Business.Operation.Core;

    public interface IBusinessStandardOperationService : IBusinessOperationService
    {
        IBusinessStandardHostInfo HostInfo { get; set; }

        ILifetimeScope Container { get; set; }
    }
}
