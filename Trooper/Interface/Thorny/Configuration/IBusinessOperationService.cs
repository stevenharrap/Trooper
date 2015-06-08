namespace Trooper.Interface.Thorny.Configuration
{
    using Autofac;
    using System.ServiceModel;
    using Trooper.Interface.Thorny.Business.Operation.Core;

    public interface IBusinessOperationService<TiBusinessOperation> : IBusinessOperationService
        where TiBusinessOperation : IBusinessOperation
    {
    }

    public interface IBusinessOperationService : IStartable
    {
        ServiceHost Service { get; set; }

        string Address { get; set; }
    }
}
