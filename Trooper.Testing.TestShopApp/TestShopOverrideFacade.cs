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
    public class TestShopOverrideFacade : TestFacadeBase<IShopBusinessCore, Shop, IShop>
    {
		private const string ToBeImplemented = "Shop Facade Test to be implemented";

        #region Tests

        [Test]
        [Ignore(ToBeImplemented)]
	    public override void TestGetByKey()
	    {
			using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
			{
				var shop1 = new Shop {Name = "Kmart", Address = "Queensland"};
				var shop2 = new Shop {Name = "Coles", Address = "NSW"};

				bp.Facade.AddSome(new List<Shop> {shop1, shop2});
				bp.Uow.Save();
			}

		    using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
		    {
				
		    }
	    }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void DeleteAll()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void DeleteSome()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestAdd()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestAddSome()
        {
            base.TestAddSome();
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestAny()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestAreEqual()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestDelete()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestExists()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestGetAll()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestGetSome()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestLimit()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestUpdate()
        {
            Assert.That(true);
        }

        #endregion

        #region Setup

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

        #endregion
    }
}
