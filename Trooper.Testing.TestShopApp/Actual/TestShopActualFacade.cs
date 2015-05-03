using System.Collections.Generic;

namespace Trooper.NUnitTesting.TestShopApp.Actual
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
    using Trooper.Testing.CoreShop.Business.Support;
    using Trooper.BusinessOperation2.Business.Operation.Core;

    [TestFixture]
    [Category("Facade")]
    public class TestShopActualFacade : TestFacadeBase<IShopBusinessCore, Shop, IShop>
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
        public override void TestAdd()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
                bp.Facade.Add(shop1);
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.That(result.Count(), Is.EqualTo(1));
                Assert.That(result.First().ShopId == shop1.ShopId);
                Assert.That(result.First().Name == "Kmart");
                Assert.That(result.First().Address == "Queensland");
            }
        }

        [Test]
        public override void TestAddSome()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
                var shop2 = new Shop { Name = "Coles", Address = "NSW" };
                var shop3 = new Shop { Name = "BigW", Address = "Vic" };

                bp.Facade.AddSome(new[] { shop1, shop2, shop3 });
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.That(result.Count(), Is.EqualTo(3));
                Assert.That(result.Any(s => s.ShopId == shop1.ShopId && s.Name == "Kmart" && s.Address == "Queensland"));
                Assert.That(result.Any(s => s.ShopId == shop2.ShopId && s.Name == "Coles" && s.Address == "NSW"));
                Assert.That(result.Any(s => s.ShopId == shop3.ShopId && s.Name == "BigW" && s.Address == "Vic"));
            }
        }

        [Test]
        public override void TestAny()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                Assert.IsFalse(bp.Facade.Any());

                var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
                bp.Facade.Add(shop1);
                bp.Uow.Save();

                Assert.IsTrue(bp.Facade.Any());
            }
        }

        [Test]
        public override void TestAreEqual()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new Shop { ShopId = 10, Name = "Kmart", Address = "Queensland" };
                var shop2 = new Shop { ShopId = 10, Name = "Coles", Address = "NSW" };
                var shop3 = new Shop { ShopId = 20, Name = "BigW", Address = "Vic" };

                Assert.IsTrue(bp.Facade.AreEqual(shop1, shop2));
                Assert.IsFalse(bp.Facade.AreEqual(shop1, shop3));
                Assert.IsTrue(bp.Facade.AreEqual(new { ShopId = 10 }, shop2));
                Assert.IsFalse(bp.Facade.AreEqual(new { ShopId = 10 }, shop3));
                Assert.IsFalse(bp.Facade.AreEqual(new { BadKey = "Nope!" }, shop1));
            }
        }

        [Test]
        public override void TestDelete()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
                var shop2 = new Shop { Name = "Coles", Address = "NSW" };

                bp.Facade.AddSome(new [] { shop1, shop2 });
                bp.Uow.Save();
                var result = bp.Facade.GetAll();

                Assert.AreEqual(result.Count(), 2);

                bp.Facade.Delete(shop1);
                bp.Uow.Save();
                result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First().ShopId, shop2.ShopId);

                bp.Facade.Delete(shop2);
                bp.Uow.Save();

                result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.AreEqual(result.Count(), 0);
            }
        }

        [Test]
        public override void TestExists()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
                var shop2 = new Shop { Name = "Coles", Address = "NSW" };

                bp.Facade.AddSome(new[] { shop1, shop2 });
                bp.Uow.Save();

                var all = bp.Facade.GetAll().ToList();
                Assert.NotNull(all);
                Assert.That(all.Count(), Is.EqualTo(2));

                bp.Facade.Delete(shop2);
                bp.Uow.Save();

                all = bp.Facade.GetAll().ToList();
                Assert.NotNull(all);
                Assert.That(all.Count(), Is.EqualTo(1));

                Assert.IsTrue(bp.Facade.Exists(new { shop1.ShopId }));
                Assert.IsTrue(bp.Facade.Exists(shop1));
                Assert.IsFalse(bp.Facade.Exists(new { shop2.ShopId }));
                Assert.IsFalse(bp.Facade.Exists(shop2));
                Assert.IsFalse(bp.Facade.Exists(new { BadKey = "Nope!" }));
            }
        }

        [Test]
        public override void TestGetAll()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new Shop { ShopId = 10, Name = "Kmart", Address = "Queensland" };
                var shop2 = new Shop { ShopId = 10, Name = "Coles", Address = "NSW" };
                bp.Facade.AddSome(new [] { shop1, shop2 });
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.That(result.Count(), Is.EqualTo(2));
            }
        }

        [Test]
        public override void TestGetSome()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
                var shop2 = new Shop { Name = "Kmart", Address = "NSW" };
                var shop3 = new Shop { Name = "BigW", Address = "Vic" };
                var shop4 = new Shop { Name = "Aldi", Address = "NSW" };

                bp.Facade.AddSome(new[] { shop1, shop2, shop3, shop4 });
                bp.Uow.Save();

                var names = bp.Facade.GetSome(new ShopNameSearch { Name = "Kmart" });
                var addresses = bp.Facade.GetSome(new ShopAddressSearch { Address = "NSW" });

                Assert.AreEqual(names.Count(), 2);
                Assert.IsTrue(names.Any(s => s.ShopId == shop1.ShopId));
                Assert.IsTrue(names.Any(s => s.ShopId == shop2.ShopId));

                Assert.AreEqual(addresses.Count(), 2);
                Assert.IsTrue(addresses.Any(s => s.ShopId == shop2.ShopId));
                Assert.IsTrue(addresses.Any(s => s.ShopId == shop4.ShopId));
            }
        }

        [Test]
        public override void TestLimit()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
                var shop2 = new Shop { Name = "Coles", Address = "NSW" };
                var shop3 = new Shop { Name = "BigW", Address = "Vic" };
                var shop4 = new Shop { Name = "Kmart", Address = "SA" };
                var shop5 = new Shop { Name = "Coles", Address = "NT" };
                var shop6 = new Shop { Name = "BigW", Address = "WA" };

                bp.Facade.AddSome(new [] { shop1, shop2, shop3, shop4, shop5, shop6 });
                bp.Uow.Save();

                var all = bp.Facade.GetAll().AsEnumerable();

                var some = bp.Facade.Limit(all, new Search { SkipItems = 3, TakeItems = 2 });

                Assert.IsNotNull(some);
                Assert.That(some.Count(), Is.EqualTo(2));
                Assert.IsTrue(some.Any(s => s.ShopId == shop4.ShopId));
                Assert.IsTrue(some.Any(s => s.ShopId == shop5.ShopId));
            }
        }

        [Test]
        public override void TestUpdate()
        {
            Shop shop1;

            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                shop1 = new Shop { Name = "Kmart", Address = "Queensland" };

                bp.Facade.Add(shop1);
                bp.Uow.Save();
            
                var shop2 = bp.Facade.GetByKey(new { shop1.ShopId });

                Assert.IsNotNull(shop2);
                shop2.Address = "NSW";
                bp.Facade.Update(shop2);
                bp.Uow.Save();

                var shop3 = bp.Facade.GetByKey(new { shop1.ShopId });
                Assert.IsNotNull(shop3);
                Assert.AreEqual(shop3.Address, "NSW");
            }

            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop4 = new Shop { ShopId = shop1.ShopId, Name = "Coles", Address = "Vic" };
                bp.Facade.Update(shop4);
                bp.Uow.Save();
            }

            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop5 = bp.Facade.GetByKey(new { shop1.ShopId });
                Assert.IsNotNull(shop5);
                Assert.AreEqual(shop5.Name, "Coles");
                Assert.AreEqual(shop5.Address, "Vic");
            }


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
