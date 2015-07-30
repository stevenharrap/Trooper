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
    using Trooper.DynamicServiceHost;
    using Trooper.Interface.DynamicServiceHost;
    using Trooper.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Response;

    public class BusinessModule    {
        public static void AddContext<TContext>(ContainerBuilder builder)
            where TContext : IDbContext, new()
        {
            builder.Register(c => new UnitOfWork<TContext>()).As<IUnitOfWork>();
        }        

        public static void AddComponent<TEnt, TPoco>(BusinessComponent<TEnt, TPoco> component)
            where TEnt : class, TPoco, new()
            where TPoco : class
        {
            component.EnsureRegistrations();
        }

        //public static void AddStandardRegistrations(ContainerBuilder builder)
        //{
        //    builder.RegisterType<Identity>().As<IIdentity>();
        //}

        public static IContainer Start<TAppModule>()
            where TAppModule : Module, new()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TAppModule>();

            var container = builder.Build();

            return container;
        }
    }

    public class BusinessComponent<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {        
        private bool facadeRegistered;
        private bool authorizationRegistered;
        private bool validationRegistered;
        private bool businessCoreRegistered;
        private bool businessOperaitonRegistered;

        private ContainerBuilder builder;

        private Type businessOperationType;

        public BusinessComponent(ContainerBuilder builder)
        {
            this.builder = builder;
        }
        
        public void RegisterFacade<TcFacade, TiFacade>() 
            where TcFacade : TiFacade, IFacade<TEnt, TPoco>, new()
            where TiFacade : IFacade<TEnt, TPoco>
        {
            this.builder.Register(c => new TcFacade()).As<TiFacade>().As<IFacade<TEnt, TPoco>>();
            this.facadeRegistered = true;
        }

        public void RegisterAuthorization<TcAuthorization, TiAuthorization>()
            where TcAuthorization : TiAuthorization, IAuthorization<TEnt>, new()
            where TiAuthorization : IAuthorization<TEnt>
        {
            builder.Register(c => new TcAuthorization())
                .As<TiAuthorization>()
                .As<IAuthorization<TEnt>>();
            this.authorizationRegistered = true;
        }

        public void RegisterValidation<TcValidation, TiValidation>()
            where TcValidation : TiValidation, IValidation<TEnt>, new()
            where TiValidation : IValidation<TEnt>
        {
            builder.Register(c => new TcValidation())
                .As<TiValidation>()
                .As<IValidation<TEnt>>();
            this.validationRegistered = true;
        }

        public void RegisterBusinessCore<TcBusinessCore, TiBusinessCore>()
            where TcBusinessCore : TiBusinessCore, IBusinessCore<TEnt, TPoco>, new()
            where TiBusinessCore : IBusinessCore<TEnt, TPoco>
        {
            builder.Register(c =>
            {
                var core = new TcBusinessCore();
                var ctx = c.Resolve<IComponentContext>();
                core.OnRequestBusinessPack += new BusinessPackHandler<TEnt, TPoco>((uow) => NewBusinessPack(ctx, core, uow));
                return core;
            })
            .As<TiBusinessCore>()
            .As<IBusinessCore<TEnt, TPoco>>();

            this.businessCoreRegistered = true;
        }

        public void RegisterBusinessOperation<TcBusinessOperation, TiBusinessOperation>()
            where TcBusinessOperation : TiBusinessOperation, IBusinessOperation<TEnt, TPoco>, new()
            where TiBusinessOperation : IBusinessOperation<TEnt, TPoco>
        {
            builder.Register(c => new TcBusinessOperation
            {
                BusinessCore = c.Resolve<IBusinessCore<TEnt, TPoco>>()
            })
            .As<TiBusinessOperation>()
            .As<IBusinessOperation<TEnt, TPoco>>();

            this.businessOperaitonRegistered = true;
            this.businessOperationType = typeof(TcBusinessOperation);
        }

        public IHostInfo RegisterServiceHost(string baseAddress, bool useDefaultTypes = true, Action<IHostInfo> hostInfoBuilt = null, bool allowDefaultBusinessOperation = false)
        {
            var boType = this.businessOperationType;

            if (boType == null && !allowDefaultBusinessOperation)
            {
                throw new Exception("The Component must have BusinessOperation registered when allowDefaultBusinessOperation is false");
            }
            else if (boType == null)
            {
                boType = typeof(BusinessAll<TEnt, TPoco>);
            }

            var hostInfo = new HostInfo
            {
                Address = new Uri(string.Format("{0}/{1}", baseAddress, boType.FullName)),
                CodeNamespace = boType.Namespace.Replace(".", string.Empty),
                ServiceName = HostInfoHelper.MakeClassName(boType),
                InterfaceName = HostInfoHelper.MakeClassName(boType, true),
                ServiceNampespace = new Uri(string.Format("{0}/{1}Ns", baseAddress, boType.FullName)),
                Mappings = new List<ClassMapping>()
            };

            /*
             builder.Register(c =>
            {
                var core = new TcBusinessCore();
                var ctx = c.Resolve<IComponentContext>();
                core.OnRequestBusinessPack += new BusinessPackHandler<TEnt, TPoco>((uow) => NewBusinessPack(ctx, core, uow));
                return core;
            })
             */

            this.builder.Register(c =>                 
                {
                    var container = c.Resolve<IComponentContext>();
                    return new BusinessOperationService(() => container.Resolve<IBusinessOperation<TEnt, TPoco>>(), hostInfo, hostInfoBuilt);
                })
                .As<IBusinessOperationService>()
                .As<IStartable>()
                .SingleInstance();
           

            //this.builder.Register(c => new BusinessOperationService(() => 
            //{
            //    var container = c.Resolve<IComponentContext>();
            //    var c2 = container.Resolve<IComponentContext>();
            //    return c2.Resolve<IBusinessOperation<TEnt, TPoco>>(); 
            //}, hostInfo))
            //    .As<IBusinessOperationService>()
            //    .As<IStartable>().SingleInstance();

            if (useDefaultTypes)
            {
                hostInfo.Mappings.Add(ClassMapping.Make<ISingleResponse<TPoco>, SingleResponse<TPoco>>("CApoco"));
                hostInfo.Mappings.Add(ClassMapping.Make<IAddResponse<TPoco>, AddResponse<TPoco>>("CBpoco"));
                hostInfo.Mappings.Add(ClassMapping.Make<IAddSomeResponse<TPoco>, AddSomeResponse<TPoco>>("CCpoco"));
                hostInfo.Mappings.Add(ClassMapping.Make<IRequestArg<TPoco>, RequestArg<TPoco>>("CDpoco"));
                hostInfo.Mappings.Add(ClassMapping.Make<ISingleResponse<bool>, SingleResponse<bool>>("CEpoco"));
                hostInfo.Mappings.Add(ClassMapping.Make<IManyResponse<TPoco>, ManyResponse<TPoco>>("CFpoco"));
                hostInfo.Mappings.Add(ClassMapping.Make<ISearch, Search>("CGpoco"));
                hostInfo.Mappings.Add(ClassMapping.Make<IIdentity, Identity>("CHpoco"));

                //hostInfo.Mappings.Add(ClassMapping.Make<ISingleResponse<TPoco>>("CApoco"));
                //hostInfo.Mappings.Add(ClassMapping.Make<IAddResponse<TPoco>>("CBpoco"));
                //hostInfo.Mappings.Add(ClassMapping.Make<IAddSomeResponse<TPoco>>("CCpoco"));
                //hostInfo.Mappings.Add(ClassMapping.Make<IRequestArg<TPoco>>("CDpoco"));
                //hostInfo.Mappings.Add(ClassMapping.Make<ISingleResponse<bool>>("CEpoco"));
                //hostInfo.Mappings.Add(ClassMapping.Make<IManyResponse<TPoco>>("CFpoco"));
                //hostInfo.Mappings.Add(ClassMapping.Make<ISearch>("CGpoco"));
                //hostInfo.Mappings.Add(ClassMapping.Make<IIdentity>("CHpoco"));


                //this.builder.Register(c => new SingleResponse<TPoco>()).As<ISingleResponse<TPoco>>();
                //this.builder.Register(c => new AddResponse<TPoco>()).As<IAddResponse<TPoco>>();
                //this.builder.Register(c => new AddSomeResponse<TPoco>()).As<IAddSomeResponse<TPoco>>();
                //this.builder.Register(c => new RequestArg<TPoco>()).As<IRequestArg<TPoco>>();
                //this.builder.Register(c => new SingleResponse<bool>()).As<ISingleResponse<bool>>();
                //this.builder.Register(c => new ManyResponse<TPoco>()).As<IManyResponse<TPoco>>();
                //this.builder.Register(c => new Search()).As<ISearch>();
            }

            return hostInfo;        
        }

        public void EnsureRegistrations()
        {
            if (!this.facadeRegistered)
            {
                this.RegisterFacade<Facade<TEnt, TPoco>, IFacade<TEnt, TPoco>>();
            }

            if (!this.authorizationRegistered)
            {
                this.RegisterAuthorization<Authorization<TEnt>, IAuthorization<TEnt>>();
            }

            if (!this.validationRegistered)
            {
                this.RegisterValidation<Validation<TEnt>, IValidation<TEnt>>();
            }

            if (!this.businessCoreRegistered)
            {
                this.RegisterBusinessCore<BusinessCore<TEnt, TPoco>, IBusinessCore<TEnt, TPoco>>();
            }

            if (!this.businessOperaitonRegistered)
            {
                this.RegisterBusinessOperation<BusinessAll<TEnt, TPoco>, IBusinessAll<TEnt, TPoco>>();
            }
        }

        private IBusinessPack<TEnt, TPoco> NewBusinessPack(IComponentContext container, IBusinessCore<TEnt, TPoco> businessCore, IUnitOfWork uow = null)
        {
            uow = uow ?? container.Resolve<IUnitOfWork>();
            var facade = container.Resolve<IFacade<TEnt, TPoco>>();
            var authorization = container.Resolve<IAuthorization<TEnt>>();
            var validation = container.Resolve<IValidation<TEnt>>();
            //var businessCore = container.Resolve<IBusinessCore<TEnt, TPoco>>();

            facade.Uow = uow;
            authorization.Uow = uow;
            validation.Uow = uow;

            return new BusinessPack<TEnt, TPoco>
            {
                BusinessCore = businessCore,
                Authorization = authorization,
                Facade = facade,
                Uow = uow,
                Validation = validation,
                Container = container
            };
        }
    }







    //public class BusinessModuleBuilder
    //{
    //    #region public
    //    public static IContainer StartBusinessApp<TAppModule>()
    //        where TAppModule : Module, new()
    //    {
    //        var builder = new ContainerBuilder();
    //        builder.RegisterModule<TAppModule>();
            
    //        var container = builder.Build();

    //        //StartAllServices(container);

    //        return container;
    //    }

    //    public static void Initiate<TContext>(ContainerBuilder builder)
    //        where TContext : IDbContext, new()
    //    {
    //        builder.Register(c => new UnitOfWork<TContext>()).As<IUnitOfWork>();            
    //    }

    //    public static void AddBusinessCore<
    //        TcFacade, TiFacade,
    //        TcAuthorization, TiAuthorization,
    //        TcValidation, TiValidation,
    //        TcBusinessCore, TiBusinessCore, 
    //        TcBusinessOperation, TiBusinessOperation,
    //        TEnt, TPoco>(ContainerBuilder builder)
    //        where TcFacade : TiFacade, IFacade<TEnt, TPoco>, new()
    //        where TiFacade : IFacade<TEnt, TPoco>
    //        where TcAuthorization : TiAuthorization, IAuthorization<TEnt>, new()
    //        where TiAuthorization : IAuthorization<TEnt>
    //        where TcValidation : TiValidation, IValidation<TEnt>, new()
    //        where TiValidation : IValidation<TEnt>
    //        where TcBusinessCore : TiBusinessCore, IBusinessCore<TEnt, TPoco>, new()
    //        where TiBusinessCore : IBusinessCore<TEnt, TPoco>
    //        where TcBusinessOperation : TiBusinessOperation, IBusinessOperation<TEnt, TPoco>, new()
    //        where TiBusinessOperation : IBusinessOperation<TEnt, TPoco>
    //        where TEnt : class, TPoco, new()
    //        where TPoco : class
    //    {
    //        builder.Register(c => new TcFacade()).As<TiFacade>();
    //        builder.Register(c => new TcAuthorization()).As<TiAuthorization>();
    //        builder.Register(c => new TcValidation()).As<TiValidation>();

    //        builder.Register(c =>
    //        {
    //            var core = new TcBusinessCore();
    //            var ctx = c.Resolve<IComponentContext>();
    //            core.OnRequestBusinessPack += new BusinessPackHandler<TEnt, TPoco>((uow) =>
    //                NewBusinessPack<TiBusinessCore, TiFacade, TiAuthorization, TiValidation, TEnt, TPoco>(ctx, core, uow));
    //            return core;
    //        }).As<TiBusinessCore>();

    //        builder.Register(c => new TcBusinessOperation
    //        {
    //            BusinessCore = c.Resolve<TiBusinessCore>()
    //        }).As<TiBusinessOperation>();
    //    }

    //    public static void AddBusinessCore<TEnt, TPoco>(ContainerBuilder builder)
    //        where TEnt : class, TPoco, new()
    //        where TPoco : class
    //    {
    //        AddBusinessCore<
    //            Facade<TEnt, TPoco>, IFacade<TEnt, TPoco>,
    //            Authorization<TEnt>, IAuthorization<TEnt>,
    //            Validation<TEnt>, IValidation<TEnt>,
    //            BusinessCore<TEnt, TPoco>, IBusinessCore<TEnt, TPoco>,
    //            BusinessAll<TEnt, TPoco>, IBusinessAll<TEnt, TPoco>,
    //            TEnt, TPoco>(builder);
    //    }

        
        
    //    #endregion

    //    #region private

    //    private static IBusinessPack<Tc, Ti> NewBusinessPack<
    //        TiBusinessCore,
    //        TiFacade,
    //        TiAuthorization,
    //        TiValidation,
    //        Tc, Ti>(IComponentContext container, TiBusinessCore businessCore, IUnitOfWork uow = null)
    //        where TiBusinessCore : IBusinessCore<Tc, Ti>
    //        where TiFacade : IFacade<Tc, Ti>
    //        where TiAuthorization : IAuthorization<Tc>
    //        where TiValidation : IValidation<Tc>
    //        where Tc : class, Ti, new()
    //        where Ti : class
    //    {
    //        uow = uow ?? container.Resolve<IUnitOfWork>();            
    //        var facade = container.Resolve<TiFacade>();
    //        var authorization = container.Resolve<TiAuthorization>();
    //        var validation = container.Resolve<TiValidation>();

    //        facade.Uow = uow;
    //        authorization.Uow = uow;
    //        validation.Uow = uow;

    //        return new BusinessPack<Tc, Ti>
    //        {
    //            BusinessCore = businessCore,
    //            Authorization = authorization,
    //            Facade = facade,
    //            Uow = uow,
    //            Validation = validation,
    //            Container = container
    //        };
    //    }
        
    //    #endregion
    //}
}
