namespace Trooper.Testing.CustomShopApi.Business.Support.ShopSupport
{
    using Autofac;
    using Trooper.Thorny.Configuration;
    using Trooper.Testing.CustomShopApi.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;
    using Trooper.Thorny.Interface;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Testing.CustomShopApi.Business.Support.ProductSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ProductSupport;
    using Trooper.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Operation.Core;

    public class ProductConfiguration
    {
        public static void AddProduct(ContainerBuilder builder)
        {
            BusinessModuleBuilder.AddBusinessCore<
                Facade<Product, IProduct>, IFacade<Product, IProduct>, 
                ProductAuthorization, IProductAuthorization, 
                ProductValidation, IProductValidation,
                ProductBusinessCore, IProductBusinessCore, 
                ProductBo, IProductBo, 
                Product, IProduct>(builder);

            BusinessModuleBuilder.AddServiceHost<ProductBo, IProductBo>(builder);

            //BusinessOperationInjection.AddBusinessOperation<IShopBusinessCore, ShopBo, IShopBo, Shop, IShop>(builder);
        }
    }
}
