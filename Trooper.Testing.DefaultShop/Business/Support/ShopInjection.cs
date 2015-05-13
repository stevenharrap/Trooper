﻿namespace Trooper.Testing.DefaultShopApi.Business.Support
{
    using Autofac;
    using Trooper.BusinessOperation2.Injection;
    //using Trooper.Testing.DefaultShopApi.Business.Operation;
    //using Trooper.Testing.DefaultShopApi.Facade;
    using Trooper.Testing.DefaultShopApi.Interface.Business.Operation;
    using Trooper.Testing.DefaultShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;

    public class ShopInjection
    {
        public static void AddShop(ContainerBuilder builder)
        {
            BusinessOperationInjection.AddBusinessCore<Shop, IShop>(builder);

            //BusinessOperationInjection.AddBusinessOperation<IShopBusinessCore, ShopBo, IShopBo, Shop, IShop>(builder);
        }
    }
}
