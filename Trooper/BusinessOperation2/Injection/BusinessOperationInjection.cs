﻿using Trooper.Interface.BusinessOperation2.Business.Operation.Core;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.BusinessOperation2.Injection
{
    using Autofac;
    using Trooper.BusinessOperation2.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface;
    using Trooper.BusinessOperation2.Interface.DataManager;

    public class BusinessOperationInjection
    {
        public static IContainer BuildBusinessApp<TAppModule>()
            where TAppModule : Module, new()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TAppModule>();
            return builder.Build();
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

            builder.Register(c => { 
                var core = new TcBusinessCore();
                var ctx = c.Resolve<IComponentContext>();
                core.OnRequestBusinessPack += new BusinessPackHandler<Tc, Ti>(() => 
                    NewBusinessPack<TiFacade, TiAuthorization, TiValidation, Tc, Ti>(ctx));
                return core; }).As<TiBusinessCore>();
            builder.Register(c => new TcBusinessOperation
            {
                BusinessCore = c.Resolve<TiBusinessCore>()
            }).As<TiBusinessOperation>();
        }        

        private static IBusinessPack<Tc, Ti> NewBusinessPack<
            TiFacade,
            TiAuthorization,
            TiValidation,
            Tc, Ti>(IComponentContext container)
            where TiFacade : IFacade<Tc, Ti>
            where TiAuthorization : IAuthorization<Tc>
            where TiValidation : IValidation<Tc>
            where Tc : class, Ti, new()
            where Ti : class
        {
            var uow = container.Resolve<IUnitOfWork>();
            var facade = container.Resolve<TiFacade>();
            var authorization = container.Resolve<TiAuthorization>();
            var validation = container.Resolve<TiValidation>();

            facade.Uow = uow;
            authorization.Uow = uow;
            validation.Uow = uow;

            return new BusinessPack<Tc, Ti>
            {
                Authorization = authorization,
                Facade = facade,
                Uow = uow,
                Validation = validation
            };
        }

        /*public static void AddBusinessOperation<TiBusinessCore, TcBusinessOperation, TiBusinessOperation, Tc, Ti>(ContainerBuilder builder)
            where TiBusinessCore : IBusinessCore<Tc, Ti>
            where TcBusinessOperation : TiBusinessOperation, IBusinessOperation<Tc, Ti>, new()
            where TiBusinessOperation : IBusinessOperation<Tc, Ti>
            where Tc : class, Ti, new()
            where Ti : class
        {
            builder.Register(c => new TcBusinessOperation
            {
                BusinessCore = c.Resolve<TiBusinessCore>()
            }).As<TiBusinessOperation>();
        }*/

        public static TiBusinessOperation ResoveBusinessOperation<TiBusinessOperation, Tc, Ti>(IContainer container)
            where TiBusinessOperation : IBusinessOperation<Tc, Ti>
            where Tc : class, Ti, new()
            where Ti : class
        {
            return container.Resolve<TiBusinessOperation>();
        }

        public static TiBusinessCore ResoveBusinessCore<TiBusinessCore, Tc, Ti>(IContainer container)
            where TiBusinessCore : IBusinessCore<Tc, Ti>
            where Tc : class, Ti, new()
            where Ti : class
        {
            return container.Resolve<TiBusinessCore>();
        }
    }
}
