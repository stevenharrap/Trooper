namespace Trooper.Testing.CustomShopApi.Business.Operation
{
    using Trooper.BusinessOperation2.Business.Operation.Composite;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;

    public class ShopBo : BusinessCr<Shop, IShop>, IShopBo
    {
    }
}
