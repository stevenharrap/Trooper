namespace Trooper.NUnitTesting.TestShopApp
{
    using NUnit.Framework;
    using Trooper.BusinessOperation2.Injection;
    using Trooper.BusinessOperation2.UnitTestBase;
    using Trooper.Testing.CoreShop;
    using Trooper.Testing.CoreShop.Interface.Business.Support;
    using Trooper.Testing.CoreShop.Interface.Model;
    using Trooper.Testing.CoreShop.Model;

    [TestFixture]
    public class TestShopBusinsessOperation : TestBusinessOperationBase<IShopBusinessCore, Shop, IShop>
    {
        [SetUp]
        public void Setup()
        {
            var container = BusinessOperationInjection.BuildBusinessApp<ShopAppModule>();

            base.Setup(container);
        }

    }
}
