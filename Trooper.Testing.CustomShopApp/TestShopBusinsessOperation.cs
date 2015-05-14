namespace Trooper.Testing.CustomShopApp
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using Trooper.Thorny.Injection;
    using Trooper.Thorny.UnitTestBase;
    using Trooper.Testing.CustomShopApi;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using System.Linq;
    using Trooper.Testing.CustomShopApi.Business.Support;
    using Trooper.Thorny.Business.Security;
    using Trooper.Thorny;
    using Trooper.Testing.ShopModel.Model;

    [TestFixture]
    [Category("BusinessOperation")]
    public class TestShopBusinsessOperation : TestBusinessOperationBase<IShopBusinessCore, Shop, IShop>
    {
	    private const string ToBeImplemented = "Shop BO Test to be implemented";

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

        #region Tests

        [Test]
        public override void Test_Base_Add()
        {
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var identity = this.GetValidIdentity();
            var bc = this.NewBusinessCoreInstance();

            var add = bc.Add(shop1, identity);

            Assert.IsNotNull(add);
            Assert.IsTrue(add.Ok);

            var all = bc.GetAll(identity);

            Assert.IsNotNull(all);
            Assert.IsTrue(all.Ok);
            Assert.That(all.Items.Count, Is.EqualTo(1));

            Assert.IsFalse(bc.Add(null, null).Ok);
            Assert.IsFalse(bc.Add(shop1, null).Ok);
            Assert.IsFalse(bc.Add(null, identity).Ok);
        }

        [Test]
        public override void Test_Base_AddSome()
        {
            var bc = this.NewBusinessCoreInstance();

            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var identity = this.GetValidIdentity();

            var result = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, identity);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.That(result.Items.Count(), Is.EqualTo(3));
            Assert.That(result.Items.Any(s => s.Name == "Kmart" && s.Address == "Queensland"));
            Assert.That(result.Items.Any(s => s.Name == "Coles" && s.Address == "NSW"));
            Assert.That(result.Items.Any(s => s.Name == "BigW" && s.Address == "Vic"));

            Assert.IsFalse(bc.AddSome(null, null).Ok);
            Assert.IsFalse(bc.AddSome(new [] {shop1}, null).Ok);
            Assert.IsFalse(bc.AddSome(null, identity).Ok);
        }

        [Test]
        public override void Test_Base_DeleteAll()
        {
            var bc = this.NewBusinessCoreInstance();
            var identity = this.GetValidIdentity();
            
            var allResult = bc.GetAll(identity);
            Assert.That(allResult.Ok);

            var deleteResult = bc.DeleteSomeByKey(allResult.Items, identity);
            Assert.That(deleteResult.Ok);

            allResult = bc.GetAll(identity);
            Assert.That(allResult.Ok);
            Assert.IsFalse(allResult.Items.Any());
        }

        [Test]
        public override void Test_Base_DeleteByKey()
        {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var identity = this.GetValidIdentity();

            var result = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, identity);
            Assert.IsTrue(result.Ok);

            var getAllResult = bc.GetAll(identity);
            Assert.That(getAllResult.Ok);
            Assert.AreEqual(getAllResult.Items.Count, 3);
            shop2 = getAllResult.Items.FirstOrDefault(i => i.Name == "Coles") as Shop;
            Assert.IsNotNull(shop2);

            var deleteByKey = bc.DeleteByKey(shop2, identity);
            Assert.IsNotNull(deleteByKey);
            Assert.IsTrue(deleteByKey.Ok);

            getAllResult = bc.GetAll(identity);
            Assert.IsTrue(getAllResult.Ok);
            Assert.AreEqual(getAllResult.Items.Count, 2);
            Assert.IsTrue(getAllResult.Items.Any(i => i.Name == "Kmart"));
            Assert.IsTrue(getAllResult.Items.Any(i => i.Name == "BigW"));

            Assert.IsFalse(bc.DeleteByKey(null, null).Ok);
            Assert.IsFalse(bc.DeleteByKey(shop1, null).Ok);
            Assert.IsFalse(bc.DeleteByKey(null, identity).Ok);
        }

        [Test]
        public override void Test_Base_DeleteSomeByKey()
        {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var identity = this.GetValidIdentity();

            var result = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, identity);
            Assert.IsTrue(result.Ok);

            var getAllResult = bc.GetAll(identity);
            Assert.That(getAllResult.Ok);
            Assert.AreEqual(getAllResult.Items.Count, 3);
            var shops = getAllResult.Items.Where(i => i.Name == "Kmart" || i.Name == "BigW");
            Assert.IsNotNull(shop2);            

            var deleteSomeByKey = bc.DeleteSomeByKey(shops, identity);
            Assert.IsNotNull(deleteSomeByKey);
            Assert.IsTrue(deleteSomeByKey.Ok);

            getAllResult = bc.GetAll(identity);
            Assert.IsTrue(getAllResult.Ok);
            Assert.AreEqual(getAllResult.Items.Count, 1);
            Assert.IsTrue(getAllResult.Items.Any(i => i.Name == "Coles"));

            Assert.IsFalse(bc.DeleteSomeByKey(null, null).Ok);
            Assert.IsFalse(bc.DeleteSomeByKey(new [] {shop1}, null).Ok);
            Assert.IsFalse(bc.DeleteSomeByKey(null, identity).Ok);
        }

        [Test]
        public override void Test_Base_ExistsByKey()
        {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var identity = this.GetValidIdentity();

            var result = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, identity);
            Assert.IsTrue(result.Ok);

            var getAllResult = bc.GetAll(identity);
            Assert.That(getAllResult.Ok);
            Assert.AreEqual(getAllResult.Items.Count, 3);
            shop2 = getAllResult.Items.FirstOrDefault(i => i.Name == "Coles") as Shop;
            Assert.IsNotNull(shop2);

            var existsByKey = bc.ExistsByKey(shop2, identity);
            Assert.IsNotNull(existsByKey);
            Assert.IsTrue(existsByKey.Ok);
            Assert.AreEqual(shop2.Name, "Coles");

            Assert.IsFalse(bc.ExistsByKey(null, null).Ok);
            Assert.IsFalse(bc.ExistsByKey(shop1, null).Ok);
            Assert.IsFalse(bc.ExistsByKey(null, identity).Ok);
        }

        [Test]
        public override void Test_Base_GetAll()
        {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var identity = this.GetValidIdentity();

            var result = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, identity);
            Assert.IsTrue(result.Ok);

            var getAllResult = bc.GetAll(identity);
            Assert.That(getAllResult.Ok);
            Assert.AreEqual(getAllResult.Items.Count, 3);
            Assert.IsTrue(getAllResult.Items.Any(i => i.Name == "Kmart"));
            Assert.IsTrue(getAllResult.Items.Any(i => i.Name == "Coles"));
            Assert.IsTrue(getAllResult.Items.Any(i => i.Name == "BigW"));

            Assert.IsFalse(bc.GetAll(null).Ok);
        }

        [Test]
        public override void Test_Base_GetByKey()
        {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var identity = this.GetValidIdentity();

            var result = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, identity);
            Assert.IsTrue(result.Ok);

            var getAllResult = bc.GetAll(identity);
            Assert.That(getAllResult.Ok);
            Assert.AreEqual(getAllResult.Items.Count, 3);
            shop2 = getAllResult.Items.FirstOrDefault(i => i.Name == "Coles") as Shop;
            Assert.IsNotNull(shop2);

            var getByKey = bc.GetByKey(shop2, identity);
            Assert.IsNotNull(getByKey);
            Assert.IsTrue(getByKey.Ok);
            Assert.AreEqual(shop2.Name, "Coles");

            Assert.IsFalse(bc.GetByKey(null, null).Ok);
            Assert.IsFalse(bc.GetByKey(shop1, null).Ok);
            Assert.IsFalse(bc.GetByKey(null, identity).Ok);
        }

        [Test]
        public override void Test_Base_GetSome()
        {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Kmart", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var shop4 = new Shop { Name = "Aldi", Address = "NSW" };
            var identity = this.GetValidIdentity();

            var result = bc.AddSome(new List<IShop> { shop1, shop2, shop3, shop4 }, identity);
            Assert.IsTrue(result.Ok);

            var names = bc.GetSome(new ShopNameSearch { Name = "Kmart" }, identity);
            var addresses = bc.GetSome(new ShopAddressSearch { Address = "NSW" }, identity);
            
            Assert.IsTrue(names.Ok);
            Assert.AreEqual(names.Items.Count(), 2);
            Assert.IsTrue(names.Items.Any(s => s.Name == "Kmart" && s.Address == "Queensland"));
            Assert.IsTrue(names.Items.Any(s => s.Name == "Kmart" && s.Address == "NSW"));

            Assert.IsTrue(addresses.Ok);
            Assert.AreEqual(addresses.Items.Count(), 2);
            Assert.IsTrue(addresses.Items.Any(s => s.Name == "Kmart" && s.Address == "NSW"));
            Assert.IsTrue(addresses.Items.Any(s => s.Name == "Aldi" && s.Address == "NSW"));

            Assert.IsFalse(bc.GetSome(null, null).Ok);
            Assert.IsFalse(bc.GetSome(new ShopNameSearch(), null).Ok);
            Assert.IsFalse(bc.GetSome(null, identity).Ok);
        }

		[Test]
	    public override void TestIsAllowed()
	    {
            var bc = this.NewBusinessCoreInstance();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var allowedResult = bc.IsAllowed(new RequestArg<IShop> { Action = Action.GetAllAction }, validIdentity);
            Assert.IsTrue(allowedResult.Ok);
            Assert.IsTrue(allowedResult.Item);

            var deniedResult = bc.IsAllowed(new RequestArg<IShop> { Action = Action.GetAllAction }, invalidIdentity);
            Assert.IsTrue(deniedResult.Ok);
            Assert.IsFalse(deniedResult.Item);

            Assert.IsFalse(bc.IsAllowed(null, null).Ok);
            Assert.IsFalse(bc.IsAllowed(new RequestArg<IShop> { Action = Action.GetAllAction }, null).Ok);
            Assert.IsFalse(bc.IsAllowed(null, validIdentity).Ok);
	    }

		[Test]
	    public override void TestUpdate()
	    {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Kmart", Address = "NSW" };

            var identity = this.GetValidIdentity();

            var add = bc.Add(shop1, identity);
            Assert.IsTrue(add.Ok);

            add.Item.Address = "NT";
            var update = bc.Update(add.Item, identity);
            Assert.IsTrue(update.Ok);            

            var getByKey = bc.GetByKey(add.Item, identity);
            Assert.IsTrue(getByKey.Ok);
            Assert.AreEqual(getByKey.Item.Address, "NT");

            update = bc.Update(shop2, identity);
            Assert.IsFalse(update.Ok);
	    }

		[Test]
	    public override void TestSave()
	    {
			var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Kmart", Address = "NSW" };
            var identity = this.GetValidIdentity();

            var add = bc.Add(shop1, identity);
            Assert.IsTrue(add.Ok);

            add.Item.Address = "NT";

            var save1 = bc.Save(add.Item, identity);
            Assert.IsTrue(save1.Ok);
            Assert.IsNotNull(save1.Item);
            Assert.AreEqual(save1.Change, SaveChangeType.Update);
            Assert.AreEqual(save1.Item.Address, "NT");

            var getShop1 = bc.GetByKey(save1.Item, identity);
            Assert.IsTrue(getShop1.Ok);
            Assert.AreEqual(getShop1.Item.Address, "NT");

            var save2 = bc.Save(shop2, identity);
            Assert.IsTrue(save2.Ok);
            Assert.IsNotNull(save2.Item);
            Assert.AreEqual(save2.Change, SaveChangeType.Add);
            Assert.AreEqual(save2.Item.Address, "NSW");

            var getShop2 = bc.GetByKey(save2.Item, identity);
            Assert.IsTrue(getShop2.Ok);
            Assert.AreEqual(getShop2.Item.Address, "NSW");

            Assert.IsFalse(bc.Save(null, null).Ok);
            Assert.IsFalse(bc.Save(shop1, null).Ok);
            Assert.IsFalse(bc.Save(null, identity).Ok);
	    }

		[Test]
	    public override void TestSaveSome()
	    {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var shop4 = new Shop { Name = "Aldi", Address = "NSW" };
            var identity = this.GetValidIdentity();

            var addSome = bc.AddSome(new[] { shop1, shop2 }, identity);
            Assert.IsTrue(addSome.Ok);

            var first = addSome.Items.FirstOrDefault(i => i.Name == "Kmart");
            Assert.IsNotNull(shop1);
            var second = addSome.Items.FirstOrDefault(i => i.Name == "Coles");
            Assert.IsNotNull(shop2);

            first.Address = "SA";
            second.Address = "WA";

            var saveSome = bc.SaveSome(new[] { first, second, shop3, shop4 }, identity);
            Assert.IsTrue(saveSome.Ok);
            Assert.AreEqual(saveSome.Items.Count(), 4);

            var s1 = saveSome.Items.FirstOrDefault(i => i.Item.Name == "Kmart");
            Assert.IsNotNull(s1);
            Assert.AreEqual(s1.Change, SaveChangeType.Update);
            var getShop1 = bc.GetByKey(s1.Item, identity);
            Assert.IsTrue(getShop1.Ok);
            Assert.AreEqual(getShop1.Item.Address, "SA");
            Assert.IsTrue(saveSome.Items.Any(i => i.Item.ShopId == getShop1.Item.ShopId));

            var s2 = saveSome.Items.FirstOrDefault(i => i.Item.Name == "Coles");
            Assert.IsNotNull(s2);
            Assert.AreEqual(s2.Change, SaveChangeType.Update);
            var getShop2 = bc.GetByKey(s2.Item, identity);
            Assert.IsTrue(getShop2.Ok);
            Assert.AreEqual(getShop2.Item.Address, "WA");
            Assert.IsTrue(saveSome.Items.Any(i => i.Item.ShopId == getShop2.Item.ShopId));

            var s3 = saveSome.Items.FirstOrDefault(i => i.Item.Name == "BigW");
            Assert.IsNotNull(s3);
            Assert.AreEqual(s3.Change, SaveChangeType.Add);
            var getShop3 = bc.GetByKey(s3.Item, identity);
            Assert.IsTrue(getShop3.Ok);
            Assert.AreEqual(getShop3.Item.Address, "Vic");
            Assert.IsTrue(saveSome.Items.Any(i => i.Item.ShopId == getShop3.Item.ShopId));

            var s4 = saveSome.Items.FirstOrDefault(i => i.Item.Name == "Aldi");
            Assert.IsNotNull(s4);
            Assert.AreEqual(s4.Change, SaveChangeType.Add);
            var getShop4 = bc.GetByKey(s4.Item, identity);
            Assert.IsTrue(getShop4.Ok);
            Assert.AreEqual(getShop4.Item.Address, "NSW");
            Assert.IsTrue(saveSome.Items.Any(i => i.Item.ShopId == getShop4.Item.ShopId));

            Assert.IsFalse(bc.SaveSome(null, null).Ok);
            Assert.IsFalse(bc.SaveSome(new [] { shop1 }, null).Ok);
            Assert.IsFalse(bc.SaveSome(null, identity).Ok);
	    }

		[Test]
	    public override void TestValidate()
	    {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "KmartKmartKmartKmartKmartKmartKmartKmartKmartKmartKmartKmart", Address = "NSW" };
            var identity = this.GetValidIdentity();

            var validate = bc.Validate(shop1, identity);
            Assert.IsTrue(validate.Ok);
            Assert.IsTrue(validate.Item);

            validate = bc.Validate(shop2, identity);
            Assert.IsTrue(validate.Ok);
            Assert.IsTrue(validate.Item);

            Assert.IsFalse(bc.Validate(null, null).Ok);
            Assert.IsFalse(bc.Validate(shop1, null).Ok);
            Assert.IsFalse(bc.Validate(null, identity).Ok);
        }

        #endregion
    }
}
