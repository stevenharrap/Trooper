namespace Trooper.Testing.CustomShopApi.Business.Operation
{
    using Trooper.Thorny.Business.Operation.Composite;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Trooper.Testing.CustomShopApi.Business.Model;

    public class ShopBo : BusinessCr<ShopEnt, Shop>, IShopBo
    {
        public ISingleResponse<Product> SaveProduct(ProductInShop productInShop, IIdentity identity)
        {
            return this.SaveProduct(productInShop, identity);
        }
    }
}
