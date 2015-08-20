﻿namespace Trooper.Testing.CustomShopApi.Business.Operation
{
    using Trooper.Thorny.Business.Operation.Composite;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Thorny.Business.Operation.Single;

    public class InventoryBo : BusinessRead<InventoryEnt, Inventory>, IInventoryBo
    {
    }
}