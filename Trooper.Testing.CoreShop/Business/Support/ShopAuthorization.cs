namespace Trooper.Testing.CoreShop.Business.Support
{
    using Trooper.BusinessOperation2.Business.Security;
    using Trooper.Testing.CoreShop.Interface.Business.Support;
    using Trooper.Testing.CoreShop.Model;

    public class ShopAuthorization : Authorization<Shop>, IShopAuthorization
    {
    }
}
