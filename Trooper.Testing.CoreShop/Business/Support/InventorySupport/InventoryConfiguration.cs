namespace Trooper.Testing.CustomShopApi.Business.Support.InventorySupport
{
    using Autofac;
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Testing.CustomShopApi.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Thorny.Business.Response;
    using Trooper.Thorny.Configuration;

    public class InventoryConfiguration
    {
        public static void AddInventory(ContainerBuilder builder)
        {
            var component = new BusinessComponent<InventoryEnt, Inventory>(builder);
            component.RegisterFacade<InventoryFacade, IInventoryFacade>();
            component.RegisterAuthorization<InventoryAuthorization, IInventoryAuthorization>();
            component.RegisterValidation<InventoryValidation, IInventoryValidation>();
            component.RegisterBusinessCore<InventoryBusinessCore, IInventoryBusinessCore>();
            component.RegisterBusinessOperation<InventoryBo, IInventoryBo>();

            BusinessModule.AddComponent(component);
            
            //BusinessModuleBuilder.AddBusinessCore<
            //    InventoryFacade, IInventoryFacade,
            //    InventoryAuthorization, IInventoryAuthorization,
            //    InventoryValidation, IInventoryValidation,
            //    InventoryBusinessCore, IInventoryBusinessCore,
            //    InventoryBo, IInventoryBo,
            //    InventoryEnt, Inventory>(builder);

            //builder.RegisterType<ManyResponseOfInventory>().As<IManyResponse<Inventory>>();

            //BusinessModuleBuilder.AddServiceHost<InventoryBo, IInventoryBo>(builder, "http://localhost:8000");

            

            //BusinessOperationInjection.AddBusinessOperation<IShopBusinessCore, ShopBo, IShopBo, Shop, IShop>(builder);
        }
    }

    //public class ManyResponseOfInventory : ManyResponse<InventoryEnt>
    //{
    //    public ManyResponse<InventoryEnt> Getit(IManyResponse<Inventory> input)
    //    {
    //        return new ManyResponse<InventoryEnt>
    //        {
    //            Items = input.Items as List<InventoryEnt>,
    //            Messages = input.Messages
    //        };
    //    }
    //}
}
