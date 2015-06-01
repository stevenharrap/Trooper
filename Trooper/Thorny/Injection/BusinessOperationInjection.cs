using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Injection
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

    public class BusinessOperationInjection
    {
        #region public
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
                core.OnRequestBusinessPack += new BusinessPackHandler<Tc, Ti>((uow) => 
                    NewBusinessPack<TiBusinessCore, TiFacade, TiAuthorization, TiValidation, Tc, Ti>(ctx, core, uow));
                return core; }).As<TiBusinessCore>();

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

        //public static void AddBusinessCoreComponent1<TcFacade, TiFacade, Tc, Ti>(ContainerBuilder builder)
        //    where TcFacade : TiFacade, IFacade<Tc, Ti>, new()
        //    where TiFacade : IFacade<Tc, Ti>            
        //    where Tc : class, Ti, new()
        //    where Ti : class
        //{
        //    builder.Register(c => new TcFacade()).As<TiFacade>();            
        //}

        //public static void AddBusinessCoreComponent2<TcAuthorization, TiAuthorization, Tc, Ti>(ContainerBuilder builder)            
        //    where TcAuthorization : TiAuthorization, IAuthorization<Tc>, new()
        //    where TiAuthorization : IAuthorization<Tc>            
        //    where Tc : class, Ti, new()
        //    where Ti : class
        //{            
        //    builder.Register(c => new TcAuthorization()).As<TiAuthorization>();            
        //}

        //public static void AddBusinessCoreComponent3<TcValidation, TiValidation, Tc, Ti>(ContainerBuilder builder)           
        //    where TcValidation : TiValidation, IValidation<Tc>, new()
        //    where TiValidation : IValidation<Tc>            
        //    where Tc : class, Ti, new()
        //    where Ti : class
        //{            
        //    builder.Register(c => new TcValidation()).As<TiValidation>();
        //}

        //public static void AddBusinessCoreComponent4<          
        //    TcBusinessCore, TiBusinessCore,
        //    TcBusinessOperation, TiBusinessOperation,
        //    Tc, Ti>(ContainerBuilder builder)           
        //    where TcBusinessCore : TiBusinessCore, IBusinessCore<Tc, Ti>, new()
        //    where TiBusinessCore : IBusinessCore<Tc, Ti>
        //    where TcBusinessOperation : TiBusinessOperation, IBusinessOperation<Tc, Ti>, new()
        //    where TiBusinessOperation : IBusinessOperation<Tc, Ti>
        //    where Tc : class, Ti, new()
        //    where Ti : class
        //{
        //    builder.Register(c =>
        //    {
        //        var core = new TcBusinessCore();
        //        var ctx = c.Resolve<IComponentContext>();
        //        core.OnRequestBusinessPack += new BusinessPackHandler<Tc, Ti>(() =>
        //            NewBusinessPack<TiFacade, TiAuthorization, TiValidation, Tc, Ti>(ctx));
        //        return core;
        //    }).As<TiBusinessCore>();

        //    builder.Register(c => new TcBusinessOperation
        //    {
        //        BusinessCore = c.Resolve<TiBusinessCore>()
        //    }).As<TiBusinessOperation>();
        //}

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

        #endregion
    }
}
