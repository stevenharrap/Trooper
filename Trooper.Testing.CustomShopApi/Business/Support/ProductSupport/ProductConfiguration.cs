namespace Trooper.Testing.CustomShop.Api.Business.Support.ProductSupport
{
    using Autofac;
    using ShopModel.Model;
    using ShopPoco;
    using Thorny.Configuration;
    using Interface.Business.Support.ProductSupport;
    using Interface.Business.Support.OutletSupport;
    using Operation;
    using Interface.Business.Operation;

    public class ProductConfiguration
    {
        public static void AddProduct(ContainerBuilder builder)
        {
            var component = new BusinessComponent<ProductEnt, Product>(builder);
            component.RegisterAuthorization<ProductAuthorization, IProductAuthorization>();
            component.RegisterValidation<ProductValidation, IProductValidation>();
            component.RegisterBusinessCore<ProductBusinessCore, IProductBusinessCore>();
            component.RegisterBusinessOperation<ProductBo, IProductBo>();

            component.RegisterDynamicServiceHost(new BusinessHostInfo { BaseAddress = "http://localhost:8000" });

            BusinessModule.AddComponent(component);
        }
    }
}
