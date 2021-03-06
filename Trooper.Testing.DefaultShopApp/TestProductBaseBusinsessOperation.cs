﻿namespace Trooper.Testing.DefaultShopApp
{
    using NUnit.Framework;
    using Trooper.Thorny.Configuration;
    using Trooper.Thorny.UnitTestBase;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Testing.DefaultShopApi;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Interface.Thorny.Business.Security;

    [TestFixture]
    [Category("BusinessOperation")]
    public class TestProductBaseBusinsessOperation : TestDefaultBaseBusinsessOperation<ProductEnt, Product, ShopAppModule>
    {
    }
}
