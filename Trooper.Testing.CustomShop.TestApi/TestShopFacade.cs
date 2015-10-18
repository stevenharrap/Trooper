namespace Trooper.Testing.CustomShopTestApi
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.Testing.CustomShopApi;
    using Trooper.Testing.CustomShopApi.Business.Support.OutletSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.OutletSupport;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Thorny.Business.Operation.Core;
    using Trooper.Thorny.Configuration;
    using Trooper.Thorny.UnitTestBase;

    [TestFixture]
    [Category("Facade")]
    public class TestShopFacade : TestFacadeBase<IOutletBusinessCore, OutletEnt, Outlet>
    {
		private const string ToBeImplemented = "Shop Facade Test to be implemented";

        #region Tests

        [Test]
	    public override void TestGetByKey()
	    {
            OutletEnt shop1;
            OutletEnt shop2;

			using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
			{
				shop1 = new OutletEnt {Name = "Kmart", Address = "Queensland"};
				shop2 = new OutletEnt {Name = "Coles", Address = "NSW"};

				bp.Facade.AddSome(new List<OutletEnt> {shop1, shop2});
				bp.Uow.Save();
			}

		    using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
		    {
                var found1 = bp.Facade.GetByKey(new OutletEnt { OutletId = shop1.OutletId });
                //var found2 = bp.Facade.GetByKey(new { shop2.ShopId });
                var notFound = bp.Facade.GetByKey(new OutletEnt { OutletId = shop2.OutletId + 10 });
                //var falseKey = bp.Facade.GetByKey(new { NotAKey = "nope!" });

                Assert.IsNotNull(found1);
                //Assert.IsNotNull(found2);
                Assert.IsNull(notFound);
                //Assert.IsNull(falseKey);
		    }
	    }

        [Test]
        public override void DeleteSome()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new OutletEnt { Name = "Kmart", Address = "Queensland" };
                var shop2 = new OutletEnt { Name = "Coles", Address = "NSW" };
                var shop3 = new OutletEnt { Name = "BigW", Address = "Vic" };
                var shop4 = new OutletEnt { Name = "Aldi", Address = "NSW" };

                bp.Facade.AddSome(new[] { shop1, shop2, shop3, shop4});
                bp.Uow.Save();

                bp.Facade.DeleteSome(new[] { shop2, shop3 });
                bp.Uow.Save();

                var all = bp.Facade.GetAll().ToList();
                Assert.IsNotNull(all);
                Assert.That(all.Count(), Is.EqualTo(2));
                Assert.IsTrue(all[0].OutletId == shop1.OutletId);
                Assert.IsTrue(all[1].OutletId == shop4.OutletId);
            }
        }

        [Test]
        public override void TestAdd()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new OutletEnt { Name = "Kmart", Address = "Queensland" };
                bp.Facade.Add(shop1);
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.That(result.Count(), Is.EqualTo(1));
                Assert.That(result.First().OutletId == shop1.OutletId);
                Assert.That(result.First().Name == "Kmart");
                Assert.That(result.First().Address == "Queensland");
            }
        }

        [Test]
        public override void TestAddSome()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new OutletEnt { Name = "Kmart", Address = "Queensland" };
                var shop2 = new OutletEnt { Name = "Coles", Address = "NSW" };
                var shop3 = new OutletEnt { Name = "BigW", Address = "Vic" };

                bp.Facade.AddSome(new[] { shop1, shop2, shop3 });
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.That(result.Count(), Is.EqualTo(3));
                Assert.That(result.Any(s => s.OutletId == shop1.OutletId && s.Name == "Kmart" && s.Address == "Queensland"));
                Assert.That(result.Any(s => s.OutletId == shop2.OutletId && s.Name == "Coles" && s.Address == "NSW"));
                Assert.That(result.Any(s => s.OutletId == shop3.OutletId && s.Name == "BigW" && s.Address == "Vic"));
            }
        }

        [Test]
        public override void TestAny()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                Assert.IsFalse(bp.Facade.Any());

                var shop1 = new OutletEnt { Name = "Kmart", Address = "Queensland" };
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
                var shop1 = new OutletEnt { OutletId = 10, Name = "Kmart", Address = "Queensland" };
                var shop2 = new OutletEnt { OutletId = 10, Name = "Coles", Address = "NSW" };
                var shop3 = new OutletEnt { OutletId = 20, Name = "BigW", Address = "Vic" };

                Assert.IsTrue(bp.Facade.AreEqual(shop1, shop2));
                Assert.IsFalse(bp.Facade.AreEqual(shop1, shop3));
                //Assert.IsTrue(bp.Facade.AreEqual(new { ShopId = 10 }, shop2));
                //Assert.IsFalse(bp.Facade.AreEqual(new { ShopId = 10 }, shop3));
                //Assert.IsFalse(bp.Facade.AreEqual(new { BadKey = "Nope!" }, shop1));
            }
        }

        [Test]
        public override void TestDelete()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new OutletEnt { Name = "Kmart", Address = "Queensland" };
                var shop2 = new OutletEnt { Name = "Coles", Address = "NSW" };

                bp.Facade.AddSome(new [] { shop1, shop2 });
                bp.Uow.Save();
                var result = bp.Facade.GetAll();

                Assert.AreEqual(result.Count(), 2);

                bp.Facade.Delete(shop1);
                bp.Uow.Save();
                result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.AreEqual(result.Count(), 1);
                Assert.AreEqual(result.First().OutletId, shop2.OutletId);

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
                var shop1 = new OutletEnt { Name = "Kmart", Address = "Queensland" };
                var shop2 = new OutletEnt { Name = "Coles", Address = "NSW" };

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

                //Assert.IsTrue(bp.Facade.Exists(new { shop1.ShopId }));
                Assert.IsTrue(bp.Facade.Exists(shop1));
                //Assert.IsFalse(bp.Facade.Exists(new { shop2.ShopId }));
                Assert.IsFalse(bp.Facade.Exists(shop2));
                //Assert.IsFalse(bp.Facade.Exists(new { BadKey = "Nope!" }));
            }
        }

        [Test]
        public override void TestGetAll()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new OutletEnt { OutletId = 10, Name = "Kmart", Address = "Queensland" };
                var shop2 = new OutletEnt { OutletId = 10, Name = "Coles", Address = "NSW" };
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
                var shop1 = new OutletEnt { Name = "Kmart", Address = "Queensland" };
                var shop2 = new OutletEnt { Name = "Kmart", Address = "NSW" };
                var shop3 = new OutletEnt { Name = "BigW", Address = "Vic" };
                var shop4 = new OutletEnt { Name = "Aldi", Address = "NSW" };

                bp.Facade.AddSome(new[] { shop1, shop2, shop3, shop4 });
                bp.Uow.Save();

                var names = bp.Facade.GetSome(new OutletNameSearch { Name = "Kmart" });
                var addresses = bp.Facade.GetSome(new OutletAddressSearch { Address = "NSW" });

                Assert.AreEqual(names.Count(), 2);
                Assert.IsTrue(names.Any(s => s.OutletId == shop1.OutletId));
                Assert.IsTrue(names.Any(s => s.OutletId == shop2.OutletId));

                Assert.AreEqual(addresses.Count(), 2);
                Assert.IsTrue(addresses.Any(s => s.OutletId == shop2.OutletId));
                Assert.IsTrue(addresses.Any(s => s.OutletId == shop4.OutletId));
            }
        }

        [Test]
        public override void TestLimit()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop1 = new OutletEnt { Name = "Kmart", Address = "Queensland" };
                var shop2 = new OutletEnt { Name = "Coles", Address = "NSW" };
                var shop3 = new OutletEnt { Name = "BigW", Address = "Vic" };
                var shop4 = new OutletEnt { Name = "Kmart", Address = "SA" };
                var shop5 = new OutletEnt { Name = "Coles", Address = "NT" };
                var shop6 = new OutletEnt { Name = "BigW", Address = "WA" };

                bp.Facade.AddSome(new [] { shop1, shop2, shop3, shop4, shop5, shop6 });
                bp.Uow.Save();

                var all = bp.Facade.GetAll().AsEnumerable();

                var some = bp.Facade.Limit(all, new Search { SkipItems = 3, TakeItems = 2 });

                Assert.IsNotNull(some);
                Assert.That(some.Count(), Is.EqualTo(2));
                Assert.IsTrue(some.Any(s => s.OutletId == shop4.OutletId));
                Assert.IsTrue(some.Any(s => s.OutletId == shop5.OutletId));
            }
        }

        [Test]
        public override void TestUpdate()
        {
            OutletEnt shop1;

            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                shop1 = new OutletEnt { Name = "Kmart", Address = "Queensland" };

                bp.Facade.Add(shop1);
                bp.Uow.Save();
            
                var shop2 = bp.Facade.GetByKey(new OutletEnt{ OutletId = shop1.OutletId });

                Assert.IsNotNull(shop2);
                shop2.Address = "NSW";
                bp.Facade.Update(shop2);
                bp.Uow.Save();

                var shop3 = bp.Facade.GetByKey(new OutletEnt{ OutletId = shop1.OutletId });
                Assert.IsNotNull(shop3);
                Assert.AreEqual(shop3.Address, "NSW");
            }

            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop4 = new OutletEnt { OutletId = shop1.OutletId, Name = "Coles", Address = "Vic" };
                bp.Facade.Update(shop4);
                bp.Uow.Save();
            }

            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var shop5 = bp.Facade.GetByKey(new OutletEnt{ OutletId = shop1.OutletId });
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
            var container = BusinessModule.Start<ShopAppModule>();
                //BusinessModuleBuilder.StartBusinessApp<ShopAppModule>();

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
