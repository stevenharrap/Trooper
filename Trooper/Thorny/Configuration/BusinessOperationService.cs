namespace Trooper.Thorny.Configuration
{
    using System.ServiceModel;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Configuration;

    public class BusinessOperationService<TiBusinessOperation> : IBusinessOperationService<TiBusinessOperation>
        where TiBusinessOperation : IBusinessOperation
    {
        public BusinessOperationService(ServiceHost service, string address)
        {
            this.Service = service;
            this.Address = address;
        }

        public ServiceHost Service { get; set; }

        public string Address { get; set; }

        public void Start()
        {
            //this.Service.Open();
        }
    }
}
