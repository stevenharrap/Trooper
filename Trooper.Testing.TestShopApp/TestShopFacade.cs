namespace Trooper.NUnitTesting.TestShopApp
{
    using Autofac;
    using NUnit.Framework;
    using Trooper.BusinessOperation2.Injection;
    using Trooper.BusinessOperation2.Interface;
    using Trooper.BusinessOperation2.UnitTestBase;
    using Trooper.Testing.CoreShop;
    using Trooper.Testing.CoreShop.Facade;
    using Trooper.Testing.CoreShop.Interface.Business.Support;
    using Trooper.Testing.CoreShop.Interface.Model;
    using Trooper.Testing.CoreShop.Model;

    [TestFixture]
    [Category("Facade")]
    public class TestShopFacade : TestFacadeBase<IShopBusinessCore, Shop, IShop>
    {
		private const string ToBeImplemented = "Shop Facade Test to be implemented";

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var container = BusinessOperationInjection.BuildBusinessApp<ShopAppModule>();

            base.TestFixtureSetup(container);
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

		[Test]
		[Ignore(ToBeImplemented)]
        public override void TestUpdate()
        {
            Assert.That(true);
        }
    }
}
