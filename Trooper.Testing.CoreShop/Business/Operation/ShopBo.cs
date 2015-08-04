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
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;

    public class ShopBo : BusinessCr<ShopEnt, Shop>, IShopBo
    {
        public ISaveResponse<ProductInShop> SaveProduct(ProductInShop productInShop, IIdentity identity)
        {
            var bc = this.BusinessCore as IShopBusinessCore;

            return bc.SaveProduct(productInShop, identity);
        }

        public IAddResponse<Shop> SimpleLittleThing(IIdentity identity)
        {
            var bc = this.BusinessCore as IShopBusinessCore;

            return bc.SimpleLittleThing(identity);
        }
    }
}
