using Autofac;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Trooper.Interface.DynamicServiceHost;

namespace Trooper.DynamicServiceHost.HostFactoryBuilder
{
    public class DynamicInstanceProvider : IInstanceProvider, IContractBehavior
    {
        private readonly IHostInfo hostInfo;
        private readonly Type serviceType;
        private readonly Func<object> supporter;

        public DynamicInstanceProvider(IHostInfo hostInfo, Func<object> supporter, Type serviceType)
        {
            if (hostInfo == null)
            {
                throw new ArgumentNullException("hostInfo");
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            this.hostInfo = hostInfo;
            this.serviceType = serviceType;
            this.supporter = supporter;
        }

        #region IInstanceProvider Members

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            //Console.WriteLine("Instance of {0} created.", this.serviceType.Name);
            
            var supporterObj = this.supporter == null ? null : this.supporter();
            var service = Activator.CreateInstance(this.serviceType, hostInfo, supporterObj);

            //return new MyService(this.hostInfo);
            return service;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        #endregion

        #region IContractBehavior Members

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = this;
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
