﻿using Trooper.Interface.Thorny.Business.Operation.Composite;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;
using Trooper.Testing.CustomShopApi.Interface.Business.Model;
using Trooper.Testing.ShopModel;
using Trooper.Testing.ShopModel.Interface;
using Trooper.Testing.ShopModel.Model;

namespace Trooper.Testing.CustomShopApi.Interface.Business.Operation
{
    public interface IShopBo : IBusinessCr<Shop, IShop>
    {
        ISingleResponse<IProduct> SaveProduct(IProductInShop productInShop, IIdentity identity);
    }
}
