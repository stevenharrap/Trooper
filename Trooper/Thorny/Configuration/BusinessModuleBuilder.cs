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
    using System.Linq;
    using System.ServiceModel;
    using Trooper.Interface.Thorny.Configuration;
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Security;
    using Trooper.DynamicServiceHost;
    using Trooper.Interface.DynamicServiceHost;
    using Trooper.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Utility;

    public class BusinessModule    
    {
        public static void AddContext<TContext>(ContainerBuilder builder)
            where TContext : IDbContext, new()
        {
            builder.Register(c => new UnitOfWork<TContext>()).As<IUnitOfWork>();
        }        

        public static void AddComponent<TEnt, TPoco>(BusinessComponent<TEnt, TPoco> component)
            where TEnt : class, TPoco, new()
            where TPoco : class, new()
        {
            component.EnsureRegistrations();
        }

        public static IContainer Start<TAppModule>()
            where TAppModule : Module, new()
        {
            return Start<TAppModule>(new BusinessModuleStartParameters());
        }

        public static IContainer Start<TAppModule>(IBusinessModuleStartParameters startParameters)
            where TAppModule : Module, new()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TAppModule>();
            builder.Register(c => startParameters).As<IBusinessModuleStartParameters>();

            var container = builder.Build();

            return container;
        }

        public static void Stop(IContainer container)
        {
            var dynamicServices = container.Resolve<IEnumerable<IBusinessDynamicOperationService>>();
            foreach (var service in dynamicServices)
            {
                service.Stop();
            }
        }
    }

    public class BusinessComponent<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class, new()
    {        
        private bool facadeRegistered;
        private bool authorizationRegistered;
        private bool validationRegistered;
        private bool businessCoreRegistered;
        private bool businessOperaitonRegistered;

        private ContainerBuilder builder;

        private Type businessOperationClassType;
        private Type businessOperationInterfaceType;

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
            where TcAuthorization : TiAuthorization, IAuthorization<TPoco>, new()
            where TiAuthorization : IAuthorization<TPoco>
        {
            builder.Register(c => new TcAuthorization())
                .As<TiAuthorization>()
                .As<IAuthorization<TPoco>>();
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
        {
            builder.Register(c => new TcBusinessOperation
            {
                BusinessCore = c.Resolve<IBusinessCore<TEnt, TPoco>>()
            })
            .As<TiBusinessOperation>()
            .As<IBusinessOperation<TEnt, TPoco>>();

            this.businessOperaitonRegistered = true;
            this.businessOperationClassType = typeof(TcBusinessOperation);
            this.businessOperationInterfaceType = typeof(TiBusinessOperation);
        }

        public void RegisterDynamicServiceHost(IBusinessDynamicHostInfo businessHostInfo)
        {
            var boType = this.businessOperationClassType;

            if (boType == null) 
            {
                throw new Exception("To Register as a service host please register a business operation using RegisterBusinessOperation<TcBusinessOperation, TiBusinessOperation>");
            }                       

            if (businessHostInfo.Address == null)        
            {
                businessHostInfo.Address = new Uri(string.Format("{0}/{1}", businessHostInfo.BaseAddress, boType.FullName));
            }

            businessHostInfo.CodeNamespace = boType.Name.Replace(".", string.Empty);
            businessHostInfo.ServiceName = HostInfoHelper.MakeClassName(boType);
            businessHostInfo.InterfaceName = HostInfoHelper.MakeClassName(boType, true);
            businessHostInfo.ServiceNampespace = new Uri(string.Format("{0}/{1}Ns", businessHostInfo.BaseAddress, boType.FullName));

            if (businessHostInfo.UseDefaultTypes)
            {
                if (businessHostInfo.Mappings == null)
                {
                    businessHostInfo.Mappings = new List<ClassMapping>();
                }

                businessHostInfo.Mappings.Add(ClassMapping.Make<ISingleResponse<TPoco>, SingleResponse<TPoco>>());
                businessHostInfo.Mappings.Add(ClassMapping.Make<IAddResponse<TPoco>, AddResponse<TPoco>>());
                businessHostInfo.Mappings.Add(ClassMapping.Make<IAddSomeResponse<TPoco>, AddSomeResponse<TPoco>>());
                businessHostInfo.Mappings.Add(ClassMapping.Make<ISaveResponse<TPoco>, SaveResponse<TPoco>>());
                businessHostInfo.Mappings.Add(ClassMapping.Make<ISaveSomeResponse<TPoco>, SaveSomeResponse<TPoco>>());
                businessHostInfo.Mappings.Add(ClassMapping.Make<IRequestArg<TPoco>, RequestArg<TPoco>>());
                businessHostInfo.Mappings.Add(ClassMapping.Make<ISingleResponse<bool>, SingleResponse<bool>>());
                businessHostInfo.Mappings.Add(ClassMapping.Make<IManyResponse<TPoco>, ManyResponse<TPoco>>());
                businessHostInfo.Mappings.Add(ClassMapping.Make<ISearch, Search>(commonType: true));
                businessHostInfo.Mappings.Add(ClassMapping.Make<IIdentity, Identity>(commonType: true));
            }
            
            this.builder.Register(c =>
                {
                    var container = c.Resolve<IComponentContext>();
                    var operation = c.Resolve<IBusinessOperation<TEnt, TPoco>>();
                    var startParameters = c.Resolve<IBusinessModuleStartParameters>();
                    businessHostInfo.HostInfoBuilt = (IBusinessDynamicHostInfo bhi) => { this.AddSearchMethods(operation, bhi); };
                    return new BusinessDynamicOperationService(() => container.Resolve<IBusinessOperation<TEnt, TPoco>>(), businessHostInfo)
                    {
                        AutoStart = startParameters.AutoStartServices
                    };
                })
                .As<IBusinessDynamicOperationService>()
                .As<IStartable>()
                .SingleInstance();
        }

        public void EnsureRegistrations()
        {
            if (!this.facadeRegistered)
            {
                this.RegisterFacade<Facade<TEnt, TPoco>, IFacade<TEnt, TPoco>>();
            }

            if (!this.authorizationRegistered)
            {
                this.RegisterAuthorization<Authorization<TPoco>, IAuthorization<TPoco>>();
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
                this.RegisterBusinessOperation<BusinessAll<TEnt, TPoco>, IBusinessAll<TPoco>>();
            }
        }

        private IBusinessPack<TEnt, TPoco> NewBusinessPack(IComponentContext container, IBusinessCore<TEnt, TPoco> businessCore, IUnitOfWork uow = null)
        {
            uow = uow ?? container.Resolve<IUnitOfWork>();
            var facade = container.Resolve<IFacade<TEnt, TPoco>>();
            var authorization = container.Resolve<IAuthorization<TPoco>>();
            var validation = container.Resolve<IValidation<TEnt>>();

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

        private void AddSearchMethods(IBusinessOperation<TEnt, TPoco> operation, IBusinessDynamicHostInfo businessHostInfo)
        {
            var bp = operation.BusinessCore.GetBusinessPack();
            var searches = operation.BusinessCore.GetSearches(bp);
            var hasGetSomeMethod = operation.GetType().GetMethod(OperationAction.GetSomeAction) != null;

            businessHostInfo.Methods.RemoveAll(i => i.Name == OperationAction.GetSomeAction);

            if (searches == null || !searches.Any() || !hasGetSomeMethod)
            {
                return;
            }            

            foreach (var mapping in searches)
            {
                if (businessHostInfo.Methods == null)
                {
                    businessHostInfo.Methods = new List<Method>();
                }

                businessHostInfo.Methods.Add(new Method
                {
                    Name = string.Format("GetSomeBy{0}", mapping.ResolveTo.Name),
                    Returns = typeof(IManyResponse<TPoco>),
                    Parameters = new List<Paramater> {
                        new Paramater(mapping.ResolveTo, "search"),
                        new Paramater(typeof(IIdentity), "identity")
                    },
                    Body = (MethodInput p) => {
                        var getSomeMethod = p.Supporter.GetType().GetMethod(OperationAction.GetSomeAction);
                        return getSomeMethod.Invoke(p.Supporter, p.Inputs);
                    } 
                });
            }
        }
    }
}
