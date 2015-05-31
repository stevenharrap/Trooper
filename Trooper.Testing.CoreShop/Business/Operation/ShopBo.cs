namespace Trooper.Testing.CustomShopApi.Business.Operation
{
    using Trooper.Thorny.Business.Operation.Composite;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Trooper.Testing.CustomShopApi.Interface.Business.Model;

    public class ShopBo : BusinessCr<Shop, IShop>, IShopBo
    {
        public ISingleResponse<IProduct> SaveProduct(IProductInShop productInShop, IIdentity identity)
        {
            return this.SaveProduct(productInShop, identity);
        }
    }
}
