using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Thorny.Configuration
{
    public class BusinessOperationService
    {
        public static void ConfigureHost(ServiceHost host, Uri address, Type serviceInterface, Uri serviceNameSpace)
        {
            ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
            host.Description.Behaviors.Add(mBehave);
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
            BasicHttpBinding httpb = new BasicHttpBinding();

            var endPoint = host.AddServiceEndpoint(serviceInterface, httpb, address);
            endPoint.Binding.Namespace = serviceNameSpace.ToString();

            var debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (debug == null)
            {
                host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
            }
            else
            {
                if (!debug.IncludeExceptionDetailInFaults)
                {
                    debug.IncludeExceptionDetailInFaults = true;
                }
            }
        }
    }
}
