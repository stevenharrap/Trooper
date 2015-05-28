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

	/// <summary>
	/// Shop Testing. Tests need to be broken into specific components rather than giant catch-alls.
	/// </summary>
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

        #region Add

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
            Assert.AreEqual(add.Messages.Count(), 1);
            Assert.That(add.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(invalidShop, invalidIdentity);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.AreEqual(add.Messages.Count(), 1);
            Assert.That(add.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(null, null);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.That(add.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(shop1, null);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.That(add.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(invalidShop, null);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.That(add.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(null, validIdentity);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.That(add.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            add = bc.Add(null, invalidIdentity);
            Assert.IsFalse(add.Ok);
            Assert.IsNull(add.Item);
            Assert.IsNotNull(add.Messages);
            Assert.That(add.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);
        }

        #endregion

        #region AddSome

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
            Assert.IsTrue(addSome.Messages.Any(m => m.Code == Validation.InvalidPropertyCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { shop1, invalidShop }, validIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.That(addSome.Messages.Any(m => m.Code == Validation.InvalidPropertyCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { shop1 }, invalidIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.AreEqual(addSome.Messages.Count(), 1);
            Assert.That(addSome.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { invalidShop }, invalidIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.AreEqual(addSome.Messages.Count(), 1);
            Assert.That(addSome.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { shop1, invalidShop }, invalidIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.AreEqual(addSome.Messages.Count(), 1);
            Assert.That(addSome.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(null, null);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.That(addSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { shop1 }, null);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.That(addSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(new[] { invalidShop }, null);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.That(addSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(null, validIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.That(addSome.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            addSome = bc.AddSome(null, invalidIdentity);
            Assert.IsFalse(addSome.Ok);
            Assert.IsNull(addSome.Items);
            Assert.IsNotNull(addSome.Messages);
            Assert.That(addSome.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);
        }

        #endregion

        #region DeleteByKey

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
            Assert.That(deleteByKey.Messages.Any(m => m.Code == BusinessCore.NoRecordCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);            

            deleteByKey = bc.DeleteByKey(shop1, invalidIdentity);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.AreEqual(deleteByKey.Messages.Count(), 1);
            Assert.That(deleteByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(invalidShop, invalidIdentity);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.AreEqual(deleteByKey.Messages.Count(), 1);
            Assert.That(deleteByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(null, null);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.That(deleteByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(shop1, null);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.That(deleteByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(invalidShop, null);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.That(deleteByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(null, validIdentity);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.That(deleteByKey.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            deleteByKey = bc.DeleteByKey(null, invalidIdentity);
            Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.That(deleteByKey.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);
        }

        #endregion

        #region DeleteSomeByKey

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

            var deleteSomeByKey = bc.DeleteSomeByKey(shops, validIdentity);
            Assert.IsNotNull(deleteSomeByKey);
            Assert.IsTrue(deleteSomeByKey.Ok);

            all = bc.GetAll(validIdentity);            
            Assert.AreEqual(all.Items.Count, 1);
            Assert.IsTrue(all.Items.Any(i => i.Name == "Coles"));

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { invalidShop }, validIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.That(deleteSomeByKey.Messages.Any(m => m.Code == BusinessCore.NoRecordCode));

            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { added.Items.First(), invalidShop }, validIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.That(deleteSomeByKey.Messages.Any(m => m.Code == BusinessCore.NoRecordCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { added.Items.First() }, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.AreEqual(deleteSomeByKey.Messages.Count(), 1);
            Assert.That(deleteSomeByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { added.Items.First(), invalidShop }, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.AreEqual(deleteSomeByKey.Messages.Count(), 1);
            Assert.That(deleteSomeByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { shop1 }, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.AreEqual(deleteSomeByKey.Messages.Count(), 1);
            Assert.That(deleteSomeByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { invalidShop }, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.AreEqual(deleteSomeByKey.Messages.Count(), 1);
            Assert.That(deleteSomeByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { shop1, invalidShop }, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.AreEqual(deleteSomeByKey.Messages.Count(), 1);
            Assert.That(deleteSomeByKey.Messages.First().Code == Authorization.UserDeniedCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(null, null);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.IsTrue(deleteSomeByKey.Messages.Any());
            Assert.That(deleteSomeByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode)); 
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { shop1 }, null);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.That(deleteSomeByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(new[] { invalidShop }, null);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.That(deleteSomeByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(null, validIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.That(deleteSomeByKey.Messages.Any(m => m.Code == BusinessCore.NullItemsCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            deleteSomeByKey = bc.DeleteSomeByKey(null, invalidIdentity);
            Assert.IsFalse(deleteSomeByKey.Ok);
            Assert.IsNotNull(deleteSomeByKey.Messages);
            Assert.That(deleteSomeByKey.Messages.Any(m => m.Code == BusinessCore.NullItemsCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);
        }

        #endregion

        #region ExistsByKey

        [Test]
        public override void Test_Base_ExistsByKey()
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

            var existsByKey = bc.ExistsByKey(invalidShop, validIdentity);
            Assert.IsTrue(existsByKey.Ok);

            existsByKey = bc.ExistsByKey(shop2, validIdentity);
            Assert.IsNotNull(existsByKey);
            Assert.IsTrue(existsByKey.Ok);
            Assert.AreEqual(shop2.Name, "Coles");

            existsByKey = bc.ExistsByKey(invalidShop, invalidIdentity);
            Assert.IsFalse(existsByKey.Ok);
            Assert.IsFalse(existsByKey.Item);
            Assert.IsNotNull(existsByKey.Messages);
            Assert.IsTrue(existsByKey.Messages.Any());
            Assert.That(existsByKey.Messages.First().Code == Authorization.UserDeniedCode);

            existsByKey = bc.ExistsByKey(shop2, invalidIdentity);
            Assert.IsFalse(existsByKey.Ok);
            Assert.IsFalse(existsByKey.Item);
            Assert.IsNotNull(existsByKey.Messages);
            Assert.IsTrue(existsByKey.Messages.Any());
            Assert.That(existsByKey.Messages.First().Code == Authorization.UserDeniedCode);

            existsByKey = bc.ExistsByKey(null, null);
            Assert.IsFalse(existsByKey.Ok);
            Assert.IsFalse(existsByKey.Item);
            Assert.IsNotNull(existsByKey.Messages);
            Assert.That(existsByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            existsByKey = bc.ExistsByKey(shop2, null);
            Assert.IsFalse(existsByKey.Ok);
            Assert.IsFalse(existsByKey.Item);
            Assert.IsNotNull(existsByKey.Messages);
			Assert.That(existsByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            existsByKey = bc.ExistsByKey(invalidShop, null);
            Assert.IsFalse(existsByKey.Ok);
            Assert.IsFalse(existsByKey.Item);
            Assert.IsNotNull(existsByKey.Messages);
			Assert.That(existsByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            existsByKey = bc.ExistsByKey(null, validIdentity);
            Assert.IsFalse(existsByKey.Ok);
            Assert.IsFalse(existsByKey.Item);
            Assert.IsNotNull(existsByKey.Messages);
			Assert.That(existsByKey.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            existsByKey = bc.ExistsByKey(null, invalidIdentity);
            Assert.IsFalse(existsByKey.Ok);
            Assert.IsFalse(existsByKey.Item);
            Assert.IsNotNull(existsByKey.Messages);
			Assert.That(existsByKey.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);
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
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var addSome = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, validIdentity);
            Assert.IsTrue(addSome.Ok);

            var getAll = bc.GetAll(validIdentity);
            Assert.That(getAll.Ok);
            Assert.AreEqual(getAll.Items.Count, 3);
            Assert.IsTrue(getAll.Items.Any(i => i.Name == "Kmart"));
            Assert.IsTrue(getAll.Items.Any(i => i.Name == "Coles"));
            Assert.IsTrue(getAll.Items.Any(i => i.Name == "BigW"));

            getAll = bc.GetAll(invalidIdentity);
            Assert.IsFalse(getAll.Ok);
            Assert.IsNull(getAll.Items);
            Assert.IsNotNull(getAll.Messages);
            Assert.That(getAll.Messages.Any(m => m.Code == Authorization.UserDeniedCode));

            getAll = bc.GetAll(null);
            Assert.IsFalse(getAll.Ok);
            Assert.IsNull(getAll.Items);
            Assert.IsNotNull(getAll.Messages);
            Assert.That(getAll.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            
            var all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);
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
            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var added = bc.AddSome(new List<IShop> { shop1, shop2, shop3 }, validIdentity);
            Assert.IsTrue(added.Ok);

            var all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);
            shop2 = all.Items.FirstOrDefault(i => i.Name == "Coles") as Shop;
            Assert.IsNotNull(shop2);

            var getByKey = bc.GetByKey(shop2, validIdentity);
            Assert.IsNotNull(getByKey);
            Assert.IsTrue(getByKey.Ok);
            Assert.IsNotNull(getByKey.Item);
            Assert.AreEqual(getByKey.Item.Name, "Coles");

            getByKey = bc.GetByKey(invalidShop, validIdentity);
            Assert.IsFalse(getByKey.Ok);
            Assert.IsNull(getByKey.Item);
            Assert.IsNotNull(getByKey.Messages);
            Assert.IsTrue(getByKey.Messages.Any());
            Assert.That(getByKey.Messages.First().Code == BusinessCore.NoRecordCode);

            getByKey = bc.GetByKey(added.Items.First(), invalidIdentity);
            Assert.IsFalse(getByKey.Ok);
            Assert.IsNull(getByKey.Item);
            Assert.IsNotNull(getByKey.Messages);
            Assert.That(getByKey.Messages.Any(m => m.Code == Authorization.UserDeniedCode));

            getByKey = bc.GetByKey(null, null);
            Assert.IsFalse(getByKey.Ok);
            Assert.IsNull(getByKey.Item);
            Assert.IsNotNull(getByKey.Messages);
            Assert.That(getByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            getByKey = bc.GetByKey(shop2, null);
            Assert.IsFalse(getByKey.Ok);
            Assert.IsNull(getByKey.Item);
            Assert.IsNotNull(getByKey.Messages);
			Assert.That(getByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            getByKey = bc.GetByKey(invalidShop, null);
            Assert.IsFalse(getByKey.Ok);
            Assert.IsNull(getByKey.Item);
            Assert.IsNotNull(getByKey.Messages);
			Assert.That(getByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            getByKey = bc.GetByKey(null, validIdentity);
            Assert.IsFalse(getByKey.Ok);
            Assert.IsNull(getByKey.Item);
            Assert.IsNotNull(getByKey.Messages);
			Assert.That(getByKey.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            getByKey = bc.GetByKey(null, invalidIdentity);
            Assert.IsFalse(getByKey.Ok);
            Assert.IsNull(getByKey.Item);
            Assert.IsNotNull(getByKey.Messages);
			Assert.That(getByKey.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);
        }

        #endregion

        #region GetSomeByKey

        public override void Test_Base_GetSomeByKey()
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

            shop1 = all.Items.FirstOrDefault(i => i.Name == "Kmart") as Shop;
            Assert.IsNotNull(shop1);
            shop2 = all.Items.FirstOrDefault(i => i.Name == "Coles") as Shop;
            Assert.IsNotNull(shop2);

            var getSomeByKey = bc.GetSomeByKey(new [] { shop1, shop2 }, validIdentity);
            Assert.IsNotNull(getSomeByKey);
            Assert.IsTrue(getSomeByKey.Ok);
            Assert.IsNotNull(getSomeByKey.Items);
            Assert.AreEqual(getSomeByKey.Items.Count(), 2);
            Assert.That(getSomeByKey.Items.Any(i => i.Name == "Kmart"));
            Assert.That(getSomeByKey.Items.Any(i => i.Name == "Coles"));

            getSomeByKey = bc.GetSomeByKey(new[] { invalidShop }, validIdentity);
            Assert.IsFalse(getSomeByKey.Ok);
            Assert.IsNull(getSomeByKey.Items);
            Assert.IsNotNull(getSomeByKey.Messages);
            Assert.IsTrue(getSomeByKey.Messages.Any());
            Assert.That(getSomeByKey.Messages.First().Code == BusinessCore.NoRecordCode);

            getSomeByKey = bc.GetSomeByKey(new [] {added.Items.First()}, invalidIdentity);
            Assert.IsFalse(getSomeByKey.Ok);
            Assert.IsNull(getSomeByKey.Items);
            Assert.IsNotNull(getSomeByKey.Messages);
            Assert.That(getSomeByKey.Messages.Any(m => m.Code == Authorization.UserDeniedCode));

            getSomeByKey = bc.GetSomeByKey(null, null);
            Assert.IsFalse(getSomeByKey.Ok);
            Assert.IsNull(getSomeByKey.Items);
            Assert.IsNotNull(getSomeByKey.Messages);
            Assert.That(getSomeByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            getSomeByKey = bc.GetSomeByKey(new [] {shop2}, null);
            Assert.IsFalse(getSomeByKey.Ok);
            Assert.IsNull(getSomeByKey.Items);
            Assert.IsNotNull(getSomeByKey.Messages);
            Assert.That(getSomeByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            getSomeByKey = bc.GetSomeByKey(new[] { invalidShop }, null);
            Assert.IsFalse(getSomeByKey.Ok);
            Assert.IsNull(getSomeByKey.Items);
            Assert.IsNotNull(getSomeByKey.Messages);
            Assert.That(getSomeByKey.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            getSomeByKey = bc.GetSomeByKey(null, validIdentity);
            Assert.IsFalse(getSomeByKey.Ok);
            Assert.IsNull(getSomeByKey.Items);
            Assert.IsNotNull(getSomeByKey.Messages);
            Assert.That(getSomeByKey.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);

            getSomeByKey = bc.GetSomeByKey(null, invalidIdentity);
            Assert.IsFalse(getSomeByKey.Ok);
            Assert.IsNull(getSomeByKey.Items);
            Assert.IsNotNull(getSomeByKey.Messages);
            Assert.That(getSomeByKey.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 3);
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
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var result = bc.AddSome(new List<IShop> { shop1, shop2, shop3, shop4 }, validIdentity);
            Assert.IsTrue(result.Ok);

            var names = bc.GetSome(new ShopNameSearch { Name = "Kmart" }, validIdentity);
            var addresses = bc.GetSome(new ShopAddressSearch { Address = "NSW" }, validIdentity);

            Assert.IsTrue(names.Ok);
            Assert.AreEqual(names.Items.Count(), 2);
            Assert.IsTrue(names.Items.Any(s => s.Name == "Kmart" && s.Address == "Queensland"));
            Assert.IsTrue(names.Items.Any(s => s.Name == "Kmart" && s.Address == "NSW"));

            Assert.IsTrue(addresses.Ok);
            Assert.AreEqual(addresses.Items.Count(), 2);
            Assert.IsTrue(addresses.Items.Any(s => s.Name == "Kmart" && s.Address == "NSW"));
            Assert.IsTrue(addresses.Items.Any(s => s.Name == "Aldi" && s.Address == "NSW"));

            var getSome = bc.GetSome(new ShopNameSearch(), invalidIdentity);
            Assert.IsFalse(getSome.Ok);
            Assert.IsNull(getSome.Items);
            Assert.IsNotNull(getSome.Messages);
            Assert.That(getSome.Messages.Any(m => m.Code == Authorization.UserDeniedCode));

            getSome = bc.GetSome(null, null);
            Assert.IsFalse(getSome.Ok);
            Assert.IsNull(getSome.Items);
            Assert.IsNotNull(getSome.Messages);
			Assert.That(getSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));

            getSome = bc.GetSome(new ShopNameSearch(), null);
            Assert.IsFalse(getSome.Ok);
            Assert.IsNull(getSome.Items);
            Assert.IsNotNull(getSome.Messages);
			Assert.That(getSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));

            getSome = bc.GetSome(null, validIdentity);
            Assert.IsFalse(getSome.Ok);
            Assert.IsNull(getSome.Items);
            Assert.IsNotNull(getSome.Messages);
			Assert.That(getSome.Messages.Any(m => m.Code == BusinessCore.NullSearchCode));

            getSome = bc.GetSome(null, invalidIdentity);
            Assert.IsFalse(getSome.Ok);
            Assert.IsNull(getSome.Items);
            Assert.IsNotNull(getSome.Messages);
			Assert.That(getSome.Messages.Any(m => m.Code == BusinessCore.NullSearchCode));
        }

        #endregion

        #region IsAllowed

        [Test]
        public override void Test_Base_IsAllowed()
        {
            var bc = this.NewBusinessCoreInstance();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

			var isAllowed = bc.IsAllowed(new RequestArg<IShop> { Action = Action.AddAction }, new Identity { Username = "NoAdderUser" });
			Assert.IsTrue(isAllowed.Ok);
			Assert.IsFalse(isAllowed.Item);

            isAllowed = bc.IsAllowed(new RequestArg<IShop> { Action = Action.GetAllAction }, validIdentity);
            Assert.IsTrue(isAllowed.Ok);
            Assert.IsTrue(isAllowed.Item);

			isAllowed = bc.IsAllowed(new RequestArg<IShop> { Action = Action.GetAllAction }, invalidIdentity);
			Assert.IsFalse(isAllowed.Ok);
			Assert.IsFalse(isAllowed.Item);
			Assert.IsNotNull(isAllowed.Messages);
			Assert.AreEqual(isAllowed.Messages.Count(), 1);
			Assert.IsTrue(isAllowed.Messages.First().Code == Authorization.UserDeniedCode);

			isAllowed = bc.IsAllowed(null, null);
			Assert.IsFalse(isAllowed.Ok);
			Assert.IsFalse(isAllowed.Item);
			Assert.IsNotNull(isAllowed.Messages);
			Assert.That(isAllowed.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
			Assert.That(isAllowed.Messages.Any(m => m.Code == BusinessCore.NullArgumentCode));

			isAllowed = bc.IsAllowed(new RequestArg<IShop> { Action = Action.GetAllAction }, null);
			Assert.IsFalse(isAllowed.Ok);
			Assert.IsFalse(isAllowed.Item);
			Assert.IsNotNull(isAllowed.Messages);
			Assert.That(isAllowed.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));

			isAllowed = bc.IsAllowed(null, validIdentity);
			Assert.IsFalse(isAllowed.Ok);
			Assert.IsFalse(isAllowed.Item);
			Assert.IsNotNull(isAllowed.Messages);
			Assert.That(isAllowed.Messages.Any(m => m.Code == BusinessCore.NullArgumentCode));
        }

        #endregion

        #region Update

        [Test]
        public override void Test_Base_Update()
        {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Kmart", Address = "NSW" };
            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var add = bc.Add(shop1, validIdentity);
            Assert.IsTrue(add.Ok);

            add.Item.Address = "NT";
            var update = bc.Update(add.Item, validIdentity);
            Assert.IsTrue(update.Ok);

            var getByKey = bc.GetByKey(add.Item, validIdentity);
            Assert.IsTrue(getByKey.Ok);
            Assert.AreEqual(getByKey.Item.Address, "NT");
            
            update = bc.Update(shop2, validIdentity);
            Assert.IsFalse(update.Ok);
            Assert.IsNotNull(update.Messages);
			Assert.That(update.Messages.Any(m => m.Code == BusinessCore.NoRecordCode));

            update = bc.Update(shop2, invalidIdentity);
            Assert.IsFalse(update.Ok);
            Assert.IsNotNull(update.Messages);
			Assert.That(update.Messages.Any(m => m.Code == BusinessCore.NoRecordCode));

            update = bc.Update(add.Item, invalidIdentity);
            Assert.IsFalse(update.Ok);
            Assert.IsNotNull(update.Messages);
			Assert.That(update.Messages.Any(m => m.Code == Authorization.UserDeniedCode));
      
            add.Item.Name = invalidShop.Name;
            add.Item.Address = invalidShop.Address;
            update = bc.Update(add.Item, validIdentity);
            Assert.IsFalse(update.Ok);
            Assert.IsNotNull(update.Messages);
			Assert.That(update.Messages.Any(m => m.Code == Validation.InvalidPropertyCode));

            update = bc.Update(null, null);
            Assert.IsFalse(update.Ok);
            Assert.IsNotNull(update.Messages);
			Assert.That(update.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            var all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            update = bc.Update(shop2, null);
            Assert.IsFalse(update.Ok);
            Assert.IsNotNull(update.Messages);
			Assert.That(update.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            update = bc.Update(add.Item, null);
            Assert.IsFalse(update.Ok);
            Assert.IsNotNull(update.Messages);
			Assert.That(update.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            update = bc.Update(null, validIdentity);
            Assert.IsFalse(update.Ok);
            Assert.IsNotNull(update.Messages);
			Assert.That(update.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);

            update = bc.Update(null, invalidIdentity);
            Assert.IsFalse(update.Ok);
            Assert.IsNotNull(update.Messages);
			Assert.That(update.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 1);            
        }

        #endregion

        #region UpdateSome

        [Test]
		public override void Test_Base_UpdateSome()
		{
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Coles", Address = "NSW" };
            var shop3 = new Shop { Name = "Aldi", Address = "Queensland" };
            var shop4 = new Shop { Name = "IGA", Address = "NSW" };

            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var add = bc.AddSome(new [] {shop1, shop2, shop3, shop4}, validIdentity);
            Assert.IsTrue(add.Ok);

            shop1 = add.Items.FirstOrDefault(i => i.Name == "Kmart") as Shop;
            Assert.IsNotNull(shop1);
            shop3 = add.Items.FirstOrDefault(i => i.Name == "Aldi") as Shop;
            Assert.IsNotNull(shop3);

            shop1.Address = "NT";
            shop3.Address = "ACT";
            var updateSome = bc.UpdateSome(new [] {shop1, shop3}, validIdentity);
            Assert.IsTrue(updateSome.Ok);
            Assert.IsNotNull(updateSome.Items);
            Assert.AreEqual(updateSome.Items.Count, 2);
            Assert.That(updateSome.Items.Any(i => i.Name == "Kmart" && i.Address == "NT"));
            Assert.That(updateSome.Items.Any(i => i.Name == "Aldi" && i.Address == "ACT"));
            
            var all = bc.GetAll(validIdentity);
            Assert.IsTrue(all.Ok);
            Assert.IsNotNull(all.Items);
            Assert.That(all.Items.Any(i => i.Name == "Kmart" && i.Address == "NT"));
            Assert.That(all.Items.Any(i => i.Name == "Coles" && i.Address == "NSW"));
            Assert.That(all.Items.Any(i => i.Name == "Aldi" && i.Address == "ACT"));
            Assert.That(all.Items.Any(i => i.Name == "IGA" && i.Address == "NSW"));
            
            updateSome = bc.UpdateSome(new [] {shop2}, validIdentity);
            Assert.IsFalse(updateSome.Ok);
            Assert.IsNull(updateSome.Items);
            Assert.IsNotNull(updateSome.Messages);
            Assert.That(updateSome.Messages.Any(m => m.Code == BusinessCore.NoRecordCode));

            updateSome = bc.UpdateSome(new[] { shop2 }, invalidIdentity);
            Assert.IsFalse(updateSome.Ok);
            Assert.IsNull(updateSome.Items);
            Assert.IsNotNull(updateSome.Messages);
            Assert.AreEqual(updateSome.Messages.Count, 1);
            Assert.That(updateSome.Messages.Any(m => m.Code == Authorization.UserDeniedCode));

            shop1.Name = invalidShop.Name;
            shop1.Address = invalidShop.Address;
            updateSome = bc.UpdateSome(new [] {shop1}, validIdentity);
            Assert.IsFalse(updateSome.Ok);
            Assert.IsNotNull(updateSome.Messages);
            Assert.That(updateSome.Messages.Any(m => m.Code == Validation.InvalidPropertyCode));

            shop1.Name = invalidShop.Name;
            shop1.Address = invalidShop.Address;
            updateSome = bc.UpdateSome(new[] { shop1, shop3 }, validIdentity);
            Assert.IsFalse(updateSome.Ok);
            Assert.IsNotNull(updateSome.Messages);
            Assert.That(updateSome.Messages.Any(m => m.Code == Validation.InvalidPropertyCode));

            updateSome = bc.UpdateSome(null, null);
            Assert.IsFalse(updateSome.Ok);
            Assert.IsNotNull(updateSome.Messages);
            Assert.That(updateSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            updateSome = bc.UpdateSome(new []  {shop2}, null);
            Assert.IsFalse(updateSome.Ok);
            Assert.IsNotNull(updateSome.Messages);
            Assert.That(updateSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            updateSome = bc.UpdateSome(new[] { shop3 }, null);
            Assert.IsFalse(updateSome.Ok);
            Assert.IsNotNull(updateSome.Messages);
            Assert.That(updateSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            updateSome = bc.UpdateSome(null, validIdentity);
            Assert.IsFalse(updateSome.Ok);
            Assert.IsNotNull(updateSome.Messages);
            Assert.That(updateSome.Messages.Any(m => m.Code == BusinessCore.NullItemsCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            updateSome = bc.UpdateSome(null, invalidIdentity);
            Assert.IsFalse(updateSome.Ok);
            Assert.IsNotNull(updateSome.Messages);
            Assert.That(updateSome.Messages.Any(m => m.Code == BusinessCore.NullItemsCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);
		}

        #endregion

        #region Save

        [Test]
        public override void Test_Base_Save()
        {
            var bc = this.NewBusinessCoreInstance();
            var shop1 = new Shop { Name = "Kmart", Address = "Queensland" };
            var shop2 = new Shop { Name = "Kmart", Address = "NSW" };
            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var add = bc.Add(shop1, validIdentity);
            Assert.IsTrue(add.Ok);

            add.Item.Address = "NT";

            var save = bc.Save(add.Item, validIdentity);
            Assert.IsTrue(save.Ok);
            Assert.IsNotNull(save.Item);
            Assert.AreEqual(save.Change, SaveChangeType.Update);
            Assert.AreEqual(save.Item.Address, "NT");

            var getByKey = bc.GetByKey(save.Item, validIdentity);
            Assert.IsTrue(getByKey.Ok);
            Assert.AreEqual(getByKey.Item.Address, "NT");
            shop1 = getByKey.Item as Shop;

            save = bc.Save(shop2, validIdentity);
            Assert.IsTrue(save.Ok);
            Assert.IsNotNull(save.Item);
            Assert.AreEqual(save.Change, SaveChangeType.Add);
            Assert.AreEqual(save.Item.Address, "NSW");

            getByKey = bc.GetByKey(save.Item, validIdentity);
            Assert.IsTrue(getByKey.Ok);
            Assert.AreEqual(getByKey.Item.Address, "NSW");
            shop2 = getByKey.Item as Shop;

            save = bc.Save(shop2, invalidIdentity);            
            Assert.IsFalse(save.Ok);
            Assert.IsNotNull(save.Messages);
            Assert.IsTrue(save.Messages.Any());
            Assert.That(save.Messages.First().Code == Authorization.UserDeniedCode);

            shop2.Name = invalidShop.Name;
            shop2.Address = invalidShop.Address;
            save = bc.Save(shop2, validIdentity);
            Assert.IsFalse(save.Ok);
            Assert.IsNotNull(save.Messages);
            Assert.That(save.Messages.Any(m => m.Code == Validation.InvalidPropertyCode));

            save = bc.Save(null, null);
            Assert.IsFalse(save.Ok);
            Assert.IsNull(save.Item);
            Assert.IsNotNull(save.Messages);
			Assert.That(save.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            var all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            save = bc.Save(shop2, null);
            Assert.IsFalse(save.Ok);
            Assert.IsNull(save.Item);
            Assert.IsNotNull(save.Messages);
			Assert.That(save.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            save = bc.Save(shop1, null);
            Assert.IsFalse(save.Ok);
            Assert.IsNull(save.Item);
            Assert.IsNotNull(save.Messages);
            Assert.IsTrue(save.Messages.Any());
            Assert.That(save.Messages.First().Code == BusinessCore.NullIdentityCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            save = bc.Save(null, validIdentity);
            Assert.IsFalse(save.Ok);
            Assert.IsNull(save.Item);
            Assert.IsNotNull(save.Messages);
			Assert.That(save.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);

            save = bc.Save(null, invalidIdentity);
            Assert.IsFalse(save.Ok);
            Assert.IsNull(save.Item);
            Assert.IsNotNull(save.Messages);
			Assert.That(save.Messages.Any(m => m.Code == BusinessCore.NullItemCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 2);
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
            var invalidShop = this.GetInvalidItem();
            var validIdentity = this.GetValidIdentity();
            var invalidIdentity = this.GetInvalidIdentity();

            var addSome = bc.AddSome(new[] { shop1, shop2 }, validIdentity);
            Assert.IsTrue(addSome.Ok);

            var first = addSome.Items.FirstOrDefault(i => i.Name == "Kmart");
            Assert.IsNotNull(shop1);
            var second = addSome.Items.FirstOrDefault(i => i.Name == "Coles");
            Assert.IsNotNull(shop2);

            first.Address = "SA";
            second.Address = "WA";

            var saveSome = bc.SaveSome(new[] { first, second, shop3, shop4 }, validIdentity);
            Assert.IsTrue(saveSome.Ok);
			Assert.IsNotNull(saveSome.Items);
            Assert.AreEqual(saveSome.Items.Count(), 4);

            var s1 = saveSome.Items.FirstOrDefault(i => i.Item.Name == "Kmart");
            Assert.IsNotNull(s1);
            Assert.AreEqual(s1.Change, SaveChangeType.Update);
            var getShop1 = bc.GetByKey(s1.Item, validIdentity);
            Assert.IsTrue(getShop1.Ok);
            Assert.AreEqual(getShop1.Item.Address, "SA");
            Assert.IsTrue(saveSome.Items.Any(i => i.Item.ShopId == getShop1.Item.ShopId));

            var s2 = saveSome.Items.FirstOrDefault(i => i.Item.Name == "Coles");
            Assert.IsNotNull(s2);
            Assert.AreEqual(s2.Change, SaveChangeType.Update);
            var getShop2 = bc.GetByKey(s2.Item, validIdentity);
            Assert.IsTrue(getShop2.Ok);
            Assert.AreEqual(getShop2.Item.Address, "WA");
            Assert.IsTrue(saveSome.Items.Any(i => i.Item.ShopId == getShop2.Item.ShopId));

            var s3 = saveSome.Items.FirstOrDefault(i => i.Item.Name == "BigW");
            Assert.IsNotNull(s3);
            Assert.AreEqual(s3.Change, SaveChangeType.Add);
            var getShop3 = bc.GetByKey(s3.Item, validIdentity);
            Assert.IsTrue(getShop3.Ok);
            Assert.AreEqual(getShop3.Item.Address, "Vic");
            Assert.IsTrue(saveSome.Items.Any(i => i.Item.ShopId == getShop3.Item.ShopId));

            var s4 = saveSome.Items.FirstOrDefault(i => i.Item.Name == "Aldi");
            Assert.IsNotNull(s4);
            Assert.AreEqual(s4.Change, SaveChangeType.Add);
            var getShop4 = bc.GetByKey(s4.Item, validIdentity);
            Assert.IsTrue(getShop4.Ok);
            Assert.AreEqual(getShop4.Item.Address, "NSW");
            Assert.IsTrue(saveSome.Items.Any(i => i.Item.ShopId == getShop4.Item.ShopId));

            saveSome = bc.SaveSome(new[] { shop4 }, invalidIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
            Assert.That(saveSome.Messages.Any(m => m.Code == Authorization.UserDeniedCode));

            saveSome = bc.SaveSome(new[] { shop4, invalidShop }, invalidIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == Authorization.UserDeniedCode));

            saveSome = bc.SaveSome(new[] { invalidShop }, validIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == Validation.InvalidPropertyCode));

            saveSome = bc.SaveSome(new[] { shop4, invalidShop }, validIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == Validation.InvalidPropertyCode));

            saveSome = bc.SaveSome(new[] { invalidShop }, validIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == Validation.InvalidPropertyCode));
            var all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            saveSome = bc.SaveSome(new[] { shop1, invalidShop }, validIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
            Assert.IsTrue(saveSome.Messages.Any());
            Assert.That(saveSome.Messages.First().Code == Validation.InvalidPropertyCode);
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            saveSome = bc.SaveSome(new[] { shop1 }, invalidIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == Authorization.UserDeniedCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            saveSome = bc.SaveSome(new[] { invalidShop }, invalidIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == Authorization.UserDeniedCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            saveSome = bc.SaveSome(new[] { shop1, invalidShop }, invalidIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == Authorization.UserDeniedCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            saveSome = bc.SaveSome(null, null);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
			Assert.That(saveSome.Messages.Any(m => m.Code == BusinessCore.NullItemsCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            saveSome = bc.SaveSome(new[] { shop1 }, null);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            saveSome = bc.SaveSome(new[] { invalidShop }, null);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == BusinessCore.NullIdentityCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            saveSome = bc.SaveSome(null, validIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == BusinessCore.NullItemsCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);

            saveSome = bc.SaveSome(null, invalidIdentity);
            Assert.IsFalse(saveSome.Ok);
            Assert.IsNull(saveSome.Items);
            Assert.IsNotNull(saveSome.Messages);
			Assert.That(saveSome.Messages.Any(m => m.Code == BusinessCore.NullItemsCode));
            all = bc.GetAll(validIdentity);
            Assert.AreEqual(all.Items.Count, 4);
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
