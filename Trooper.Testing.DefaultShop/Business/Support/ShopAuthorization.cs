﻿namespace Trooper.Testing.DefaultShopApi.Business.Support
{
    using Trooper.BusinessOperation2.Business.Security;
    using Trooper.Testing.DefaultShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    
    public class ShopAuthorization : Authorization<Shop>, IShopAuthorization
    {
    }
}
