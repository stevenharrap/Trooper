namespace Trooper.Thorny.Configuration
{
    using Autofac;
    using Trooper.Thorny.Business.Operation.Composite;
    using Trooper.Thorny.Business.Operation.Core;
    using Trooper.Thorny.Business.Security;
    using Trooper.Thorny.DataManager;
    using Trooper.Thorny.Interface;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Interface.Thorny.Business.Operation.Composite;
    using System;
    using System.ServiceModel;
    using Trooper.Interface.Thorny.Configuration;
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Security;

    public class BusinessModuleBuilder
    {
        #region public
        public static IContainer StartBusinessApp<TAppModule>()
            where TAppModule : Module, new()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TAppModule>();
            
            var container = builder.Build();

            //StartAllServices(container);

            return container;
        }

        public static void AddUnitOfWork<TContext>(ContainerBuilder builder)
            where TContext : IDbContext, new() 
        {
            builder.Register(c => new UnitOfWork<TContext>()).As<IUnitOfWork>();
        }

        public static void AddBusinessCore<
            TcFacade, TiFacade,
            TcAuthorization, TiAuthorization,
            TcValidation, TiValidation,
            TcBusinessCore, TiBusinessCore, 
            TcBusinessOperation, TiBusinessOperation,
            Tc, Ti>(ContainerBuilder builder)
            where TcFacade : TiFacade, IFacade<Tc, Ti>, new()
            where TiFacade : IFacade<Tc, Ti>
            where TcAuthorization : TiAuthorization, IAuthorization<Tc>, new()
            where TiAuthorization : IAuthorization<Tc>
            where TcValidation : TiValidation, IValidation<Tc>, new()
            where TiValidation : IValidation<Tc>
            where TcBusinessCore : TiBusinessCore, IBusinessCore<Tc, Ti>, new()
            where TiBusinessCore : IBusinessCore<Tc, Ti>
            where TcBusinessOperation : TiBusinessOperation, IBusinessOperation<Tc, Ti>, new()
            where TiBusinessOperation : IBusinessOperation<Tc, Ti>
            where Tc : class, Ti, new()
            where Ti : class
        {
            builder.Register(c => new TcFacade()).As<TiFacade>();
            builder.Register(c => new TcAuthorization()).As<TiAuthorization>();
            builder.Register(c => new TcValidation()).As<TiValidation>();

            builder.Register(c =>
            {
                var core = new TcBusinessCore();
                var ctx = c.Resolve<IComponentContext>();
                core.OnRequestBusinessPack += new BusinessPackHandler<Tc, Ti>((uow) =>
                    NewBusinessPack<TiBusinessCore, TiFacade, TiAuthorization, TiValidation, Tc, Ti>(ctx, core, uow));
                return core;
            }).As<TiBusinessCore>();

            builder.Register(c => new TcBusinessOperation
            {
                BusinessCore = c.Resolve<TiBusinessCore>()
            }).As<TiBusinessOperation>();
        }

        public static void AddBusinessCore<Tc, Ti>(ContainerBuilder builder)
            where Tc : class, Ti, new()
            where Ti : class
        {
            AddBusinessCore<
                Facade<Tc, Ti>, IFacade<Tc, Ti>,
                Authorization<Tc>, IAuthorization<Tc>,
                Validation<Tc>, IValidation<Tc>,
                BusinessCore<Tc, Ti>, IBusinessCore<Tc, Ti>,
                BusinessAll<Tc, Ti>, IBusinessAll<Tc, Ti>,
                Tc, Ti>(builder);
        }

        public static void AddServiceHost<TcBusinessOperation, TiBusinessOperation>(ContainerBuilder builder)
            where TcBusinessOperation : TiBusinessOperation, IBusinessOperation, new()
            where TiBusinessOperation : IBusinessOperation
        {
            var boType = typeof(TiBusinessOperation);
            var service = new ServiceHost(typeof(TcBusinessOperation));
            var binding = new NetHttpBinding(BasicHttpSecurityMode.None) { HostNameComparisonMode = HostNameComparisonMode.Exact };
            var address = string.Format("http://localhost:8000/{0}", boType.FullName);

            service.AddServiceEndpoint(
                boType,
                binding,
                address);

            var businessService = new BusinessOperationService<TiBusinessOperation>(service, address);

            builder.Register(c => businessService)
                .As <IBusinessOperationService>()
                .As<IBusinessOperationService<TiBusinessOperation>>()
                .As<IStartable>().SingleInstance();
        }

        public static IEnumerable<IBusinessOperationService> GetAllServices(IComponentContext container)
        {
            return container.Resolve<IEnumerable<IBusinessOperationService>>();            
        }

        public static void StartService<TiBusinessOperation>(IComponentContext container)
            where TiBusinessOperation : IBusinessOperation
        {
            var businessService = container.Resolve<IBusinessOperationService<TiBusinessOperation>>();

            businessService.Service.Open();
        }

        public static void StartAllServices(IComponentContext container)
        {
            var allServices = container.Resolve<IEnumerable<IBusinessOperationService>>();

            foreach (var bos in allServices)
            {
                bos.Service.Open();
            }
        }

        public static void StopService<TiBusinessOperation>(IComponentContext container)
            where TiBusinessOperation : IBusinessOperation
        {
            var businessService = container.Resolve<IBusinessOperationService<TiBusinessOperation>>();

            businessService.Service.Close();
        }

        public static void StopAllServices(IComponentContext container)
        {
            var allServices = container.Resolve<IEnumerable<IBusinessOperationService>>();

            foreach (var bos in allServices)
            {
                bos.Service.Close();
            }
        }        
        
        #endregion

        #region private

        private static IBusinessPack<Tc, Ti> NewBusinessPack<
            TiBusinessCore,
            TiFacade,
            TiAuthorization,
            TiValidation,
            Tc, Ti>(IComponentContext container, TiBusinessCore businessCore, IUnitOfWork uow = null)
            where TiBusinessCore : IBusinessCore<Tc, Ti>
            where TiFacade : IFacade<Tc, Ti>
            where TiAuthorization : IAuthorization<Tc>
            where TiValidation : IValidation<Tc>
            where Tc : class, Ti, new()
            where Ti : class
        {
            uow = uow ?? container.Resolve<IUnitOfWork>();            
            var facade = container.Resolve<TiFacade>();
            var authorization = container.Resolve<TiAuthorization>();
            var validation = container.Resolve<TiValidation>();

            facade.Uow = uow;
            authorization.Uow = uow;
            validation.Uow = uow;

            return new BusinessPack<Tc, Ti>
            {
                BusinessCore = businessCore,
                Authorization = authorization,
                Facade = facade,
                Uow = uow,
                Validation = validation,
                Container = container
            };
        }
        
        #endregion
    }
}
