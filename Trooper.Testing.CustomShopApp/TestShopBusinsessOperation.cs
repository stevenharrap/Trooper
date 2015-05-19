namespace Trooper.Testing.CustomShopApp
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using Thorny.Injection;
    using Thorny.UnitTestBase;
    using CustomShopApi;
    using CustomShopApi.Interface.Business.Support;
    using ShopModel.Interface;
    using System.Linq;
    using CustomShopApi.Business.Support;
    using Thorny.Business.Security;
    using Thorny;
    using ShopModel.Model;
    using Interface.Thorny.Business.Security;
    using Thorny.Business.Operation.Core;
    using Thorny.DataManager;

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
            var shop2 = new Shop { Name = "Coles", Address = "Queensland" };
            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();
            var bc = this.NewBusinessCoreInstance();

            var add = bc.Add(shop1, validIdentity);

            Assert.IsNotNull(add);
            Assert.IsTrue(add.Ok);

            var all = bc.GetAll(validIdentity);

            Assert.That(all.Items.Count, Is.EqualTo(1));

            add = bc.Add(invalidShop, validIdentity);
            Assert.IsNotNull(add);
            Assert.IsFalse(add.Ok);

            all = bc.GetAll(validIdentity);
            Assert.That(all.Items.Count, Is.EqualTo(1));

            add = bc.Add(invalidShop, validIdentity);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.IsTrue(add.Messages.Any());
            Assert.That(add.Messages.First().Code == Validation.InvalidPropertyCode);

            add = bc.Add(shop2, invalidIdentity);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.IsTrue(add.Messages.Any());
            Assert.That(add.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(invalidShop, invalidIdentity);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.IsTrue(add.Messages.Any());
            Assert.That(add.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(null, null);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.IsTrue(add.Messages.Any());
            Assert.That(add.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(shop1, null);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.IsTrue(add.Messages.Any());
            Assert.That(add.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(invalidShop, null);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.IsTrue(add.Messages.Any());
            Assert.That(add.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(null, validIdentity);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.IsTrue(add.Messages.Any());
            Assert.That(add.Messages.First().Code == BusinessCore.NullItemCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(null, invalidIdentity);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.IsTrue(add.Messages.Any());
            Assert.That(add.Messages.First().Code == BusinessCore.NullItemCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);
        }

        [Test]
        public override void Test_Base_AddSome()
        {
            var bc = this.NewBusinessCoreInstance();

            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var addSome = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, validIdentity);

            Assert.IsNotNull(addSome);
            Assert.IsTrue(addSome.Ok);
            Assert.That(addSome.Items.Count(), Is.EqualTo(3));
            Assert.That(addSome.Items.Any(s => s.Name == "Kmart" && s.Address == "Queensland"));
            Assert.That(addSome.Items.Any(s => s.Name == "Coles" && s.Address == "NSW"));
            Assert.That(addSome.Items.Any(s => s.Name == "BigW" && s.Address == "Vic"));

            var all = bc.GetAll(validIdentity);
            Assert.That(all.Items.Count, Is.EqualTo(3));

            addSome = bc.AddSome(new List<IShop> { invalidShop }, validIdentity);
            Assert.IsFalse(addSome.Ok);

            all = bc.GetAll(validIdentity);
            Assert.That(all.Items.Count, Is.EqualTo(3));

            addSome = bc.AddSome(new List<IShop> { shop1, invalidShop }, validIdentity);
            Assert.IsFalse(addSome.Ok);

            all = bc.GetAll(validIdentity);
            Assert.That(all.Items.Count, Is.EqualTo(3));

            addSome = bc.AddSome(new[] { invalidShop }, validIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.IsTrue(addSome.Messages.Any());
            Assert.That(addSome.Messages.First().Code == Validation.InvalidPropertyCode);

            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 0);

            addSome = bc.AddSome(new[] { shop1, invalidShop }, validIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.IsTrue(addSome.Messages.Any());
            Assert.That(addSome.Messages.First().Code == Validation.InvalidPropertyCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { shop1 }, invalidIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.IsTrue(addSome.Messages.Any());
            Assert.That(addSome.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { invalidShop }, invalidIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.IsTrue(addSome.Messages.Any());
            Assert.That(addSome.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { shop1, invalidShop }, invalidIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.IsTrue(addSome.Messages.Any());
            Assert.That(addSome.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(null, null);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.IsTrue(addSome.Messages.Any());
            Assert.That(addSome.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { shop1 }, null);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.IsTrue(addSome.Messages.Any());
            Assert.That(addSome.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { invalidShop }, null);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.IsTrue(addSome.Messages.Any());
            Assert.That(addSome.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(null, validIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.IsTrue(addSome.Messages.Any());
            Assert.That(addSome.Messages.First().Code == BusinessCore.NullItemCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(null, invalidIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.IsTrue(addSome.Messages.Any());
            Assert.That(addSome.Messages.First().Code == BusinessCore.NullItemCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);
        }        

        [Test]
        public override void Test_Base_DeleteByKey()
        {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var result = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, validIdentity);
            Assert.IsTrue(result.Ok);

            var all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            shop2 = all.Items.FirstOrDefault(i => i.Name == "Coles") as Shop;
            Assert.IsNotNull(shop2);
            var deleteByKey = bc.DeleteByKey(shop2, validIdentity);
            Assert.IsNotNull(deleteByKey);
            Assert.IsTrue(deleteByKey.Ok);

            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);
            Assert.IsTrue(all.Items.Any(i => i.Name == "Kmart"));
            Assert.IsTrue(all.Items.Any(i => i.Name == "BigW"));

            deleteByKey = bc.DeleteByKey(shop2, validIdentity);
            Assert.IsNotNull(deleteByKey);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsTrue(deleteByKey.Messages.Any());
            Assert.That(deleteByKey.Messages.First().Code == BusinessCore.NoRecordCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);            

            deleteByKey = bc.DeleteByKey(shop1, invalidIdentity);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.IsTrue(deleteByKey.Messages.Any());
            Assert.That(deleteByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(invalidShop, invalidIdentity);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.IsTrue(deleteByKey.Messages.Any());
            Assert.That(deleteByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(null, null);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.IsTrue(deleteByKey.Messages.Any());
            Assert.That(deleteByKey.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(shop1, null);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.IsTrue(deleteByKey.Messages.Any());
            Assert.That(deleteByKey.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(invalidShop, null);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.IsTrue(deleteByKey.Messages.Any());
            Assert.That(deleteByKey.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(null, validIdentity);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.IsTrue(deleteByKey.Messages.Any());
            Assert.That(deleteByKey.Messages.First().Code == BusinessCore.NullItemCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(null, invalidIdentity);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.IsTrue(deleteByKey.Messages.Any());
            Assert.That(deleteByKey.Messages.First().Code == BusinessCore.NullItemCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);
        }

        [Test]
        public override void Test_Base_DeleteSomeByKey()
        {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "BigW", Address = "Vic" };
            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var added = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, validIdentity);
            Assert.IsTrue(added.Ok);

            var all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);
            var shops = all.Items.Where(i => i.Name == "Kmart" || i.Name == "BigW");
            Assert.IsNotNull(shop2);

            var deleteSomeByKey = bc.DeleteSomeByKey(shops, validIdentity);
            Assert.IsNotNull(deleteSomeByKey);
            Assert.IsTrue(deleteSomeByKey.Ok);

            all = bc.GetAll(validIdentity);            
            Assert.AreEqual(all.Items.Count, 1);
            Assert.IsTrue(all.Items.Any(i => i.Name == "Coles"));

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { invalidShop }, validIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == BusinessCore.NoRecordCode);

            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { added.Items.First(), invalidShop }, validIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == BusinessCore.NoRecordCode);

            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { added.Items.First() }, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == Authorization.UserDeniedCode);

            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { added.Items.First(), invalidShop }, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == Authorization.UserDeniedCode);

            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { shop1, invalidShop }, validIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == Validation.InvalidPropertyCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { shop1 }, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { invalidShop }, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { shop1, invalidShop }, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteSomeByKey = bc.DeleteSomeByKey(null, null);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { shop1 }, null);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { invalidShop }, null);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteSomeByKey = bc.DeleteSomeByKey(null, validIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == BusinessCore.NullItemCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteSomeByKey = bc.DeleteSomeByKey(null, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.First().Code == BusinessCore.NullItemCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);
        }

        #region ExistsByKey

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
        public override void Test_Access_ExistsByKey()
        {
            var validShop = this.GetValidItem();
            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();
            var bc = this.NewBusinessCoreInstance();

            var added = bc.Add(validShop, validIdentity);
            Assert.IsTrue(added.Ok);

            var all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            var allowed = bc.ExistsByKey(invalidShop, validIdentity);
            Assert.IsTrue(allowed.Ok);

            allowed = bc.ExistsByKey(added.Item, validIdentity);
            Assert.IsTrue(allowed.Ok);

            var denied = bc.ExistsByKey(invalidShop, invalidIdentity);
            Assert.IsFalse(denied.Ok);
            Assert.IsFalse(denied.Item);
            Assert.IsNotNull(denied.Messages);
            Assert.IsTrue(denied.Messages.Any());
            Assert.That(denied.Messages.First().Code == Authorization.UserDeniedCode);

            denied = bc.ExistsByKey(added.Item, invalidIdentity);
            Assert.IsFalse(denied.Ok);
            Assert.IsFalse(denied.Item);
            Assert.IsNotNull(denied.Messages);
            Assert.IsTrue(denied.Messages.Any());
            Assert.That(denied.Messages.First().Code == Authorization.UserDeniedCode);
        }

        #endregion

        #region GetAll

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
        public override void Test_Access_GetAll()
        {
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();
            var bc = this.NewBusinessCoreInstance();

            var allowed = bc.GetAll(validIdentity);
            Assert.IsTrue(allowed.Ok);

            var denied = bc.GetAll(invalidIdentity);
            Assert.IsFalse(denied.Ok);
            Assert.IsNull(denied.Items);
            Assert.IsNotNull(denied.Messages);
            Assert.IsTrue(denied.Messages.Any());
            Assert.That(denied.Messages.First().Code == Authorization.UserDeniedCode);
        }

        #endregion

        #region GetByKey

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
        public override void Test_Access_GetByKey()
        {
            var validShop = this.GetValidItem();
            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();
            var bc = this.NewBusinessCoreInstance();

            var added = bc.Add(validShop, validIdentity);
            Assert.IsTrue(added.Ok);

            var allowed = bc.GetByKey(added.Item, validIdentity);
            Assert.IsTrue(allowed.Ok);

            var denied = bc.GetByKey(invalidShop, validIdentity);
            Assert.IsFalse(denied.Ok);
            Assert.IsNull(denied.Item);
            Assert.IsNotNull(denied.Messages);
            Assert.IsTrue(denied.Messages.Any());
            Assert.That(denied.Messages.First().Code == BusinessCore.NoRecordCode);

            denied = bc.GetByKey(added.Item, invalidIdentity);
            Assert.IsFalse(allowed.Ok);
            Assert.IsNull(denied.Item);
            Assert.IsNotNull(denied.Messages);
            Assert.IsTrue(denied.Messages.Any());
            Assert.That(denied.Messages.First().Code == Authorization.UserDeniedCode);
        }

        #endregion

        #region GetSome

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
        public override void Test_Access_GetSome()
        {
            var validShop = this.GetValidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();
            var bc = this.NewBusinessCoreInstance();

            var added = bc.Add(validShop, validIdentity);
            Assert.IsTrue(added.Ok);

            var allowed = bc.GetSome(new ShopNameSearch(), validIdentity);
            Assert.IsTrue(allowed.Ok);

            var denied = bc.GetSome(new ShopNameSearch(), invalidIdentity);
            Assert.IsFalse(denied.Ok);
            Assert.IsNull(denied.Items);
            Assert.IsNotNull(denied.Messages);
            Assert.IsTrue(denied.Messages.Any());
            Assert.That(denied.Messages.First().Code == Authorization.UserDeniedCode);
        }

        #endregion

        #region IsAllowed

        [Test]
        public override void Test_Base_IsAllowed()
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

        #endregion

        #region Update

        [Test]
        public override void Test_Base_Update()
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
        public override void Test_Access_Update()
        {

        }

        #endregion

        #region Save

        [Test]
        public override void Test_Base_Save()
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
        public override void Test_Access_Save()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region SaveSome

        [Test]
        public override void Test_Base_SaveSome()
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
            Assert.IsFalse(bc.SaveSome(new[] { shop1 }, null).Ok);
            Assert.IsFalse(bc.SaveSome(null, identity).Ok);
        }

        [Test]
        public override void Test_Access_SaveSome()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #endregion

        #region Support

        public override IIdentity GetInvalidIdentity()
        {
            return new Identity
            {
                Username = InvalidUsername
            };
        }

        public Shop GetValidItem()
        {
            return new Shop { Name = "Kmart", Address = "Queensland" };
        }

        public override Shop GetInvalidItem()
        {
            return new Shop
            {
                Address = "To long street To long street To long street To long street To long street To long street To long street To long street To long street To long street To long street",
                Name = "To long name To long name To long name To long name To long name To long name To long name To long name To long name To long name To long name To long name To long name"
            };
        }

        #endregion
    }
}
