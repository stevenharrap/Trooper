﻿using Trooper.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.UnitTestBase
{
    using Autofac;
using NUnit.Framework;
using Trooper.Thorny.Business.Security;
    using Trooper.Thorny.Interface.UnitTestBase;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Trooper.Thorny.Interface.DataManager;

    public abstract class TestBusinessOperationBase<TiBusinessCore, TEnt, TPoco> : TestBase<TiBusinessCore, TEnt, TPoco>
        where TiBusinessCore : IBusinessCore<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        #region Setup

        public override void TestFixtureSetup(IContainer container, IItemGenerator<TEnt, TPoco> itemGenerator) 
        {
            base.TestFixtureSetup(container, itemGenerator); 
        }

        public override void TestFixtureSetup(IContainer container)
        {
            base.TestFixtureSetup(container);
        }

        public override void SetUp()
        {
            base.SetUp();

            this.DeleteAll();
        }

        #endregion

        #region Support

        public virtual IIdentity GetValidIdentity()
        {
            return new Identity
            {
                Username = ValidUsername
            };
        }

        public abstract IIdentity GetInvalidIdentity();

        public virtual TEnt GetValidItem(IFacade<TEnt, TPoco> facade)
        {
            return this.ItemGenerator.NewItem(facade);
        }

        public abstract TEnt GetInvalidItem();

		public virtual void DeleteAll()
		{
			var bc = this.NewBusinessCoreInstance();
			var identity = this.GetValidIdentity();
			var all = bc.GetAll(identity);

			Assert.IsNotNull(all);
			Assert.IsTrue(all.Ok);

			var delete = bc.DeleteSomeByKey(all.Items, identity);

			Assert.IsNotNull(delete);
			Assert.IsTrue(delete.Ok);

			all = bc.GetAll(identity);

			Assert.IsNotNull(all);
			Assert.IsFalse(all.Items.Any());
		}

        #endregion

        #region Tests

        [Test]
        public virtual void TestGetBusinessPack() 
        {
            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();

            Assert.IsNotNull(bp.Authorization);
            Assert.IsNotNull(bp.Authorization.Uow);

            Assert.IsNotNull(bp.Facade);
            Assert.IsNotNull(bp.Facade.Uow);

            Assert.IsNotNull(bp.Validation);
            Assert.IsNotNull(bp.Validation.Uow);
        }

        #region Add

        [Test]
        public virtual void Test_Base_Add() 
        {
            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();
            var item = this.GetValidItem(bp.Facade);
            var identity = this.GetValidIdentity();

            var add = bc.Add(item, identity);

            Assert.IsNotNull(add);
            Assert.IsTrue(add.Ok);

            var all = bc.GetAll(identity);

            Assert.IsNotNull(all);
            Assert.IsTrue(all.Ok);
            Assert.That(all.Items.Count, Is.EqualTo(1));

            Assert.IsFalse(bc.Add(null, null).Ok);
            Assert.IsFalse(bc.Add(item, null).Ok);
            Assert.IsFalse(bc.Add(null, identity).Ok);
        }

        #endregion

        #region AddSome

        [Test]
        public virtual void Test_Base_AddSome() 
        {
            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();
            var item1 = this.GetValidItem(bp.Facade);
            var item2 = this.GetValidItem(bp.Facade);
            var item3 = this.GetValidItem(bp.Facade);

			this.TestAddSome(new List<TPoco> { item1, item2, item3 });
        }

        #endregion

        #region DeleteByKey

        [Test]
        public virtual void Test_Base_DeleteByKey() 
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var item1 = this.GetValidItem(bp.Facade);
			var item2 = this.GetValidItem(bp.Facade);
			var item3 = this.GetValidItem(bp.Facade);
			var identity = this.GetValidIdentity();

			var addSome = this.TestAddSome(new List<TPoco> { item1, item2 }).ToList();

			var deleteByKey = bc.DeleteByKey(addSome[1], identity);
			Assert.IsNotNull(deleteByKey);
			Assert.IsTrue(deleteByKey.Ok);

			this.TestGetAll(new List<TPoco> { addSome[0] });

			deleteByKey = bc.DeleteByKey(item3, identity);
			Assert.IsNotNull(deleteByKey);
			Assert.IsFalse(deleteByKey.Ok);
            Assert.IsNotNull(deleteByKey.Messages);
            Assert.That(deleteByKey.Messages.Any(m => m.Code == BusinessCore.NoRecordCode));

            Assert.IsFalse(bc.AddSome(null, null).Ok);
            Assert.IsFalse(bc.AddSome(new[] { item1 }, null).Ok);
            Assert.IsFalse(bc.AddSome(null, identity).Ok);
        }

        #endregion

        #region DeleteSomeByKey

        [Test]
        public virtual void Test_Base_DeleteSomeByKey()
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var item1 = this.GetValidItem(bp.Facade);
			var item2 = this.GetValidItem(bp.Facade);
			var item3 = this.GetValidItem(bp.Facade);
			var item4 = this.GetValidItem(bp.Facade);
			var identity = this.GetValidIdentity();

			var addSome = this.TestAddSome(new List<TPoco> { item1, item2, item3, item4 }).ToList();

			var deleteSomeByKey = bc.DeleteSomeByKey(new List<TPoco> { addSome[1], addSome[2] }, identity);
			Assert.NotNull(deleteSomeByKey);
			Assert.IsTrue(deleteSomeByKey.Ok);

			this.TestGetAll(new List<TPoco> { addSome[0], addSome[3] });

            Assert.IsFalse(bc.DeleteSomeByKey(null, null).Ok);
            Assert.IsFalse(bc.DeleteSomeByKey(new[] { item1 }, null).Ok);
            Assert.IsFalse(bc.DeleteSomeByKey(null, identity).Ok);
        }

        #endregion

        #region GetAll

        [Test]
        public virtual void Test_Base_GetAll()
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var item1 = this.GetValidItem(bp.Facade);
			var item2 = this.GetValidItem(bp.Facade);

	        var addSome = this.TestAddSome(new List<TPoco> {item1, item2});

			this.TestGetAll(addSome.ToList());
        }

        #endregion

        #region GetSome

        [Test]
        public virtual void Test_Base_GetSome()
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var identity = this.GetValidIdentity();

			var item1 = this.GetValidItem(bp.Facade);
			var item2 = this.GetValidItem(bp.Facade);
			var item3 = this.GetValidItem(bp.Facade);
			var item4 = this.GetValidItem(bp.Facade);
			var item5 = this.GetValidItem(bp.Facade);
			var item6 = this.GetValidItem(bp.Facade);

			var addSome = this.TestAddSome(new List<TPoco>{item1, item2, item3, item4, item5, item6}).ToList();
	        var getSome = bc.GetSome(new Search {SkipItems = 3, TakeItems = 2}, identity);

			Assert.IsNotNull(getSome);
			Assert.IsTrue(getSome.Ok);
			Assert.IsNotNull(getSome.Items);
			Assert.That(getSome.Items.Count(), Is.EqualTo(2));
			Assert.IsTrue(bp.Facade.AreEqual(bp.Facade.Map(getSome.Items.First()), bp.Facade.Map(addSome[3])));
			Assert.IsTrue(bp.Facade.AreEqual(bp.Facade.Map(getSome.Items.Last()), bp.Facade.Map(addSome[4])));

            Assert.IsFalse(bc.GetSome(null, null).Ok);
            Assert.IsFalse(bc.GetSome(new Search(), null).Ok);
            Assert.IsFalse(bc.GetSome(null, identity).Ok);
        }

        #endregion

        #region GetByKey

        [Test]
        public virtual void Test_Base_GetByKey()
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var identity = this.GetValidIdentity();

			var item1 = this.GetValidItem(bp.Facade);
			var item2 = this.GetValidItem(bp.Facade);

			this.TestAddSome(new List<TPoco> { item1, item2 });
	        var all = this.TestGetAll(2);

			var first = this.ItemGenerator.CopyItem(bp.Facade.Map(all[0]));
			var second = this.ItemGenerator.CopyItem(bp.Facade.Map(all[1]));	     

	        var getByKey = bc.GetByKey(first, identity);
			Assert.IsNotNull(getByKey);
			Assert.IsTrue(getByKey.Ok);
			Assert.IsNotNull(getByKey.Item);
			Assert.IsTrue(bp.Facade.AreEqual(
                bp.Facade.Map(getByKey.Item), 
                bp.Facade.Map(first)));

            Assert.IsFalse(bc.GetByKey(null, null).Ok);
            Assert.IsFalse(bc.GetByKey(item1, null).Ok);
            Assert.IsFalse(bc.GetByKey(null, identity).Ok);
        }

        #endregion

        #region GetSomeByKey

        [Test]
	    public virtual void Test_Base_GetSomeByKey()
	    {
            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();
            var identity = this.GetValidIdentity();

            var item1 = this.GetValidItem(bp.Facade);
            var item2 = this.GetValidItem(bp.Facade);
            var item3 = this.GetValidItem(bp.Facade);

            this.TestAddSome(new List<TPoco> { item1, item2, item3 });
            var all = this.TestGetAll(3);

            var first = this.ItemGenerator.CopyItem(bp.Facade.Map(all[0]));
            var second = this.ItemGenerator.CopyItem(bp.Facade.Map(all[1]));
            var third = this.ItemGenerator.CopyItem(bp.Facade.Map(all[2]));            

            var getSomeByKey = bc.GetSomeByKey(new [] { first, second }, identity);
            Assert.IsNotNull(getSomeByKey);
            Assert.IsTrue(getSomeByKey.Ok);
            Assert.IsNotNull(getSomeByKey.Items);
            Assert.AreEqual(getSomeByKey.Items.Count(), 2);
            Assert.That(getSomeByKey.Items.Any(i => bp.Facade.AreEqual(bp.Facade.Map(i), first)));
            Assert.That(getSomeByKey.Items.Any(i => bp.Facade.AreEqual(bp.Facade.Map(i), second)));

            Assert.IsFalse(bc.GetSomeByKey(null, null).Ok);
            Assert.IsFalse(bc.GetSomeByKey(new [] { first }, null).Ok);
            Assert.IsFalse(bc.GetSomeByKey(null, identity).Ok);
	    }

        #endregion

        #region ExistsByKey

        [Test]
        public virtual void Test_Base_ExistsByKey()
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var identity = this.GetValidIdentity();

			var item1 = this.GetValidItem(bp.Facade);
			var item2 = this.GetValidItem(bp.Facade);

			this.TestAddSome(new List<TPoco> { item1, item2 });
			var all = this.TestGetAll(2);

			var first = this.ItemGenerator.CopyItem(bp.Facade.Map(all[0]));
			var second = this.ItemGenerator.CopyItem(bp.Facade.Map(all[1]));

			bc.DeleteByKey(second, identity);

			this.TestGetAll(1);

			var existsByKey = bc.ExistsByKey(first, identity);
			Assert.IsNotNull(existsByKey);
			Assert.IsTrue(existsByKey.Ok);
			Assert.IsTrue(existsByKey.Item);

			existsByKey = bc.ExistsByKey(second, identity);
			Assert.IsNotNull(existsByKey);
			Assert.IsTrue(existsByKey.Ok);
			Assert.IsFalse(existsByKey.Item);

            Assert.IsFalse(bc.ExistsByKey(null, null).Ok);
            Assert.IsFalse(bc.ExistsByKey(item1, null).Ok);
            Assert.IsFalse(bc.ExistsByKey(null, identity).Ok);
        }

        #endregion

        #region IsAllowed

        public abstract void Test_Base_IsAllowed();

        #endregion

        #region Update

        public abstract void Test_Base_Update();

        #endregion

        #region UpdateSome

        public abstract void Test_Base_UpdateSome();

        #endregion

        #region Save

        public abstract void Test_Base_Save();

        #endregion

        #region SaveSome

        public abstract void Test_Base_SaveSome();

        #endregion

        #endregion

        #region private methods

        private IEnumerable<TPoco> TestAddSome(ICollection<TPoco> expected)
        {
            Assert.IsNotNull(expected);

            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();
            var identity = this.GetValidIdentity();

            var addSome = bc.AddSome(expected, identity);
            Assert.IsNotNull(addSome);
            Assert.IsTrue(addSome.Ok);
            Assert.IsNotNull(addSome.Items);
            Assert.That(addSome.Items.Count(), Is.EqualTo(expected.Count));

            return addSome.Items;
        }

        private IList<TPoco> TestGetAll(int expected)
        {
            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();
            var identity = this.GetValidIdentity();

            Assert.IsNotNull(bc);
            Assert.IsNotNull(identity);

            var getAll = bc.GetAll(identity);

            Assert.IsNotNull(getAll);
            Assert.IsNotNull(getAll.Items);
            Assert.IsNotNull(getAll.Ok);
            Assert.That(getAll.Items.Count(), Is.EqualTo(expected));

            Assert.IsFalse(bc.GetAll(null).Ok);

            return getAll.Items;
        }

        private void TestGetAll(List<TPoco> expected)
        {
            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();

            Assert.IsNotNull(expected);

            var all = this.TestGetAll(expected.Count());

            foreach (var expectedItem in expected)
            {
                var expectedItemTc = bp.Facade.Map(expectedItem);

                Assert.IsTrue(all.Any(
                    foundItem =>
                    {
                        var foundItemTc = bp.Facade.Map(foundItem);
                        return bp.Facade.AreEqual(foundItemTc, expectedItemTc);
                    }));
            }
        }

        #endregion
    }
}
