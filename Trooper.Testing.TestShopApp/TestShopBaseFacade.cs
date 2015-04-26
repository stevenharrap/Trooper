using System.Collections.Generic;

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
    public class TestShopBaseFacade : TestFacadeBase<IShopBusinessCore, Shop, IShop>
    {
		private const string ToBeImplemented = "Ignored in base testing.";

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

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestGetSome()
        {
            Assert.That(true);
        }
    }
}
