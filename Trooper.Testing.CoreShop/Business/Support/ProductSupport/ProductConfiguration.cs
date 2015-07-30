namespace Trooper.Testing.CustomShopApi.Business.Support.ShopSupport
{
    using Autofac;
    using Trooper.Testing.CustomShopApi.Business.Operation;
    using Trooper.Testing.CustomShopApi.Business.Support.ProductSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ProductSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Thorny.Configuration;
    using Trooper.Thorny.Interface;
    using Trooper.Thorny.Interface.DataManager;

    public class ProductConfiguration
    {
        public static void AddProduct(ContainerBuilder builder)
        {
            var component = new BusinessComponent<ProductEnt, Product>(builder);
            component.RegisterAuthorization<ProductAuthorization, IProductAuthorization>();
            component.RegisterValidation<ProductValidation, IProductValidation>();
            component.RegisterBusinessCore<ProductBusinessCore, IProductBusinessCore>();
            component.RegisterBusinessOperation<ProductBo, IProductBo>();

            BusinessModule.AddComponent(component);

            //BusinessModuleBuilder.AddBusinessCore<
            //    Facade<ProductEnt, Product>, IFacade<ProductEnt, Product>, 
            //    ProductAuthorization, IProductAuthorization, 
            //    ProductValidation, IProductValidation,
            //    ProductBusinessCore, IProductBusinessCore, 
            //    ProductBo, IProductBo, 
            //    ProductEnt, Product>(builder);

            //BusinessModuleBuilder.AddServiceHost<ProductBo, IProductBo>(builder, "http://localhost:8000");

            //BusinessOperationInjection.AddBusinessOperation<IShopBusinessCore, ShopBo, IShopBo, Shop, IShop>(builder);
        }
    }
}
