namespace Trooper.Interface.Thorny.Configuration
{
    using Autofac;
    using System.ServiceModel;
    using Trooper.Interface.DynamicServiceHost;
    using Trooper.Interface.Thorny.Business.Operation.Core;

    public interface IBusinessOperationService : IStartable
    {
        IHostInfo HostInfo { get; set; }

        ServiceHost ServiceHost { get; }
    }
}
