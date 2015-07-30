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
    //public class BusinessHostBuilder
    //{
    //    public static IHostInfo AddService<TEnt, TPoco>(BusinessComponent<TEnt, TPoco> component, string baseAddress, bool useDefaultTypes = true, bool allowDefaultBusinessOperation = false)
    //        where TEnt : class, TPoco, new()
    //        where TPoco : class
    //    {
    //        var boType = component.BusinessOperationType;

    //        if (boType == null && !allowDefaultBusinessOperation)
    //        {
    //            throw new Exception("The Component must have BusinessOperation registered when allowDefaultBusinessOperation is false")
    //        }
    //        else if (boType == null)
    //        {
    //            boType = typeof(BusinessAll<TEnt, TPoco>);
    //        }

    //        var hostInfo = new HostInfo
    //        {
    //            Address = new Uri(string.Format("{0}/{1}", baseAddress, boType.FullName)),
    //            CodeNamespace = boType.Namespace
    //        };

    //        component.Builder.Register(c => new BusinessOperationService(boType, hostInfo, c.Resolve<IComponentContext>()))
    //            .As<IBusinessOperationService>()
    //            .As<IStartable>().SingleInstance();

    //        if (useDefaultTypes)
    //        {
    //            //var x = typeof(TEntBusinessOperation);
    //            //var y = x.GetInterfaces().FirstOrDefault(i => i.GetGenericArguments().)


    //            component.Builder.RegisterType<Identity>().As<IIdentity>();



    //        }

    //        return hostInfo;
    //    }



        //public static IHostInfo AddHost<TEnt, TPoco, TEntBusinessOperation, TPocoBusinessOperation>(ContainerBuilder builder, string baseAddress, bool useDefaultTypes)
        //    where TEntBusinessOperation : TPocoBusinessOperation, IBusinessOperation, new()
        //    where TPocoBusinessOperation : IBusinessOperation
        //    where TEnt : class, TPoco, new()
        //    where TPoco : class
        //{
        //    var boType = typeof(TEntBusinessOperation);
        //    var hostInfo = new HostInfo
        //    {
        //        Address = new Uri(string.Format("{0}/{1}", baseAddress, boType.FullName)),
        //        CodeNamespace = boType.Namespace
        //    };

        //    builder.Register(c => new BusinessOperationService(boType, hostInfo, c.Resolve<IComponentContext>()))
        //        .As<IBusinessOperationService>()
        //        .As<IStartable>().SingleInstance();

        //    if (useDefaultTypes)
        //    {
        //        var x = typeof(TEntBusinessOperation);
        //        //var y = x.GetInterfaces().FirstOrDefault(i => i.GetGenericArguments().)


        //        builder.RegisterType<Identity>().As<IIdentity>();
                


        //    }

        //    return hostInfo;
        //}

        //public static IEnumerable<IBusinessOperationService> GetAllServices(IComponentContext container)
        //{
        //    return container.Resolve<IEnumerable<IBusinessOperationService>>();
        //}

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
