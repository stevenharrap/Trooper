namespace Trooper.Testing.DefaultShopApi.Business.Support
{
    using Autofac;
    using Trooper.Thorny.Injection;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;

    public class ProductInjection
    {
        public static void AddProduct(ContainerBuilder builder)
        {
            BusinessOperationInjection.AddBusinessCore<Product, IProduct>(builder);
        }
    }
}
