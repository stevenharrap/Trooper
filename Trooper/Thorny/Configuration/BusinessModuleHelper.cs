using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.DynamicServiceHost;
using Trooper.Interface.DynamicServiceHost;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Security;
using Trooper.Interface.Thorny.Configuration;
using Trooper.Thorny.Business.Operation.Composite;
using Trooper.Thorny.Business.Operation.Core;
using Trooper.Thorny.Business.Security;

namespace Trooper.Thorny.Configuration
{
    public class BusinessModuleHelper
    {
        public static IEnumerable<IBusinessOperationService> GetAllServices(IComponentContext container)
        {
            return container.Resolve<IEnumerable<IBusinessOperationService>>();
        }

        //public static void StartService<TiBusinessOperation>(IComponentContext container)
        //    where TiBusinessOperation : IBusinessOperation
        //{
        //    var businessService = container.Resolve<IBusinessOperationService<TiBusinessOperation>>();

        //    businessService.Service.Open();
        //}

        //public static void StartAllServices(IComponentContext container)
        //{
        //    var allServices = container.Resolve<IEnumerable<IBusinessOperationService>>();

        //    foreach (var bos in allServices)
        //    {
        //        bos.Service.Open();
        //    }
        //}

        //public static void StopService<TiBusinessOperation>(IComponentContext container)
        //    where TiBusinessOperation : IBusinessOperation
        //{
        //    var businessService = container.Resolve<IBusinessOperationService<TiBusinessOperation>>();

        //    businessService.Service.Close();
        //}

        //public static void StopAllServices(IComponentContext container)
        //{
        //    var allServices = container.Resolve<IEnumerable<IBusinessOperationService>>();

        //    foreach (var bos in allServices)
        //    {
        //        bos.Service.Close();
        //    }
        //}        
    }
}