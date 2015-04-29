using System.Collections.Generic;

namespace Trooper.NUnitTesting.TestShopApp
{
    using System.Linq;
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
	    public override void TestGetByKey()
	    {
            Shop shop1;
            Shop shop2;

			using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
			{
				shop1 = new Shop {Name = "Kmart", Address = "Queensland"};
				shop2 = new Shop {Name = "Coles", Address = "NSW"};

				bp.Facade.AddSome(new List<Shop> {shop1, shop2});
				bp.Uow.Save();
			}

		    using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
		    {
                var found1 = bp.Facade.GetByKey(new Shop { ShopId = shop1.ShopId });
                var found2 = bp.Facade.GetByKey(new { shop2.ShopId });
                var notFound = bp.Facade.GetByKey(new Shop { ShopId = shop2.ShopId + 10 });
                var falseKey = bp.Facade.GetByKey(new { NotAKey = "nope!" });

                Assert.IsNotNull(found1);
                Assert.IsNotNull(found2);
                Assert.IsNull(notFound);
                Assert.IsNull(falseKey);
		    }
	    }

        [Test]
        public override void DeleteSome()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
                var shop2 = new Shop { Name = "Coles", Address = "NSW" };
                var shop3 = new Shop { Name = "BigW", Address = "Vic" };
                var shop4 = new Shop { Name = "Aldi", Address = "NSW" };

                bp.Facade.AddSome(new[] { shop1, shop2, shop3, shop4});
                bp.Uow.Save();

                bp.Facade.DeleteSome(new[] { shop2, shop3 });
                bp.Uow.Save();

                var all = bp.Facade.GetAll().ToList();
                Assert.IsNotNull(all);
                Assert.That(all.Count(), Is.EqualTo(2));
                Assert.IsTrue(all[0].ShopId == shop1.ShopId);
                Assert.IsTrue(all[1].ShopId == shop4.ShopId);
            }
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
