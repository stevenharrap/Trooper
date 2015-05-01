using Trooper.BusinessOperation2.Business.Operation.Core;
using Trooper.Interface.BusinessOperation2.Business.Operation.Core;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.BusinessOperation2.UnitTestBase
{
    using Autofac;
using NUnit.Framework;
using Trooper.BusinessOperation2.Business.Security;
    using Trooper.BusinessOperation2.Interface.UnitTestBase;
    using System.Linq;
    using System;
    using System.Collections.Generic;
        
    public abstract class TestBusinessOperationBase<TiBusinessCore, Tc, Ti> : TestBase<TiBusinessCore, Tc, Ti>
        where TiBusinessCore : IBusinessCore<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        public override void TestFixtureSetup(IContainer container, IItemGenerator<Tc, Ti> itemGenerator) 
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

            this.TestDeleteAll();
        }

        public virtual ICredential GetValidCredential()
        {
            return new Credential
            {
                Username = "ValidTestUser"
            };
        }

        public virtual ICredential GetInvalidCredential()
        {
            throw new NotImplementedException();
        }

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

        [Test]
        public virtual void TestDeleteAll() 
        {
            var bc = this.NewBusinessCoreInstance();
            var credential = this.GetValidCredential();
            var all = bc.GetAll(credential);

            Assert.IsNotNull(all);
            Assert.IsTrue(all.Ok);

            var delete = bc.DeleteSomeByKey(all.Items, credential);

            Assert.IsNotNull(delete);
            Assert.IsTrue(delete.Ok);

            all = bc.GetAll(credential);

            Assert.IsNotNull(all);
            Assert.IsFalse(all.Items.Any());
        }

        [Test]
        public virtual void TestAdd() 
        {
            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();
            var item = this.ItemGenerator.NewItem(bp.Facade);
            var credential = this.GetValidCredential();

            var add = bc.Add(item, credential);

            Assert.IsNotNull(add);
            Assert.IsTrue(add.Ok);

            var all = bc.GetAll(credential);

            Assert.IsNotNull(all);
            Assert.IsTrue(all.Ok);
            Assert.That(all.Items.Count, Is.EqualTo(1));
        }

	    private IEnumerable<Ti> TestAddSome(ICollection<Ti> expected)
	    {
			Assert.IsNotNull(expected);

			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var credential = this.GetValidCredential();

		    var addSome = bc.AddSome(expected, credential);
			Assert.IsNotNull(addSome);
			Assert.IsTrue(addSome.Ok);
			Assert.IsNotNull(addSome.Items);
			Assert.That(addSome.Items.Count(), Is.EqualTo(expected.Count));

		    return addSome.Items;
	    }
		
        [Test]
        public virtual void TestAddSome() 
        {
            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();
            var item1 = this.ItemGenerator.NewItem(bp.Facade);
            var item2 = this.ItemGenerator.NewItem(bp.Facade);
            var item3 = this.ItemGenerator.NewItem(bp.Facade);

			this.TestAddSome(new List<Ti> { item1, item2, item3 });
        }

        [Test]
        public virtual void TestDeleteByKey() 
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var item1 = this.ItemGenerator.NewItem(bp.Facade);
			var item2 = this.ItemGenerator.NewItem(bp.Facade);
			var item3 = this.ItemGenerator.NewItem(bp.Facade);
			var credential = this.GetValidCredential();

			var addSome = this.TestAddSome(new List<Ti> { item1, item2 }).ToList();

			var deleteByKey = bc.DeleteByKey(addSome[1], credential);
			Assert.IsNotNull(deleteByKey);
			Assert.IsTrue(deleteByKey.Ok);

			this.TestGetAll(new List<Ti> { addSome[0] });

			deleteByKey = bc.DeleteByKey(item3, credential);
			Assert.IsNotNull(deleteByKey);
			Assert.IsTrue(deleteByKey.Ok);
        }

        [Test]
        public virtual void TestDeleteSomeByKey()
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var item1 = ItemGenerator.NewItem(bp.Facade);
			var item2 = ItemGenerator.NewItem(bp.Facade);
			var item3 = ItemGenerator.NewItem(bp.Facade);
			var item4 = ItemGenerator.NewItem(bp.Facade);
			var credential = this.GetValidCredential();

			var addSome = this.TestAddSome(new List<Ti> { item1, item2, item3, item4 }).ToList();

			var deleteSomeByKey = bc.DeleteSomeByKey(new List<Ti> { addSome[1], addSome[2] }, credential);
			Assert.NotNull(deleteSomeByKey);
			Assert.IsTrue(deleteSomeByKey.Ok);

			this.TestGetAll(new List<Ti> { addSome[0], addSome[3] });
        }

        [Test]
        public virtual void TestGetAll()
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var item1 = ItemGenerator.NewItem(bp.Facade);
			var item2 = ItemGenerator.NewItem(bp.Facade);

	        var addSome = this.TestAddSome(new List<Ti> {item1, item2});

			this.TestGetAll(addSome.ToList());
        }

		private IList<Ti> TestGetAll(int expected)
		{
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var credential = this.GetValidCredential();

			Assert.IsNotNull(bc);
			Assert.IsNotNull(credential);
			
			var getAll = bc.GetAll(credential);

			Assert.IsNotNull(getAll);
			Assert.IsNotNull(getAll.Items);
			Assert.IsNotNull(getAll.Ok);
			Assert.That(getAll.Items.Count(), Is.EqualTo(expected));

			return getAll.Items;
		}

		private void TestGetAll(List<Ti> expected)
		{
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();

			Assert.IsNotNull(expected);

			var all = this.TestGetAll(expected.Count());

			foreach (var expectedItem in expected)
			{
				var expectedItemTc = bp.Facade.Map(expectedItem);

				Assert.IsTrue(all.Any(
					foundItem => { 
						var foundItemTc = bp.Facade.Map(foundItem);
						return bp.Facade.AreEqual(foundItemTc, expectedItemTc);
				} ));
			}
		}

        [Test]
        public virtual void TestGetSome()
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var credential = this.GetValidCredential();

			var item1 = ItemGenerator.NewItem(bp.Facade);
			var item2 = ItemGenerator.NewItem(bp.Facade);
			var item3 = ItemGenerator.NewItem(bp.Facade);
			var item4 = ItemGenerator.NewItem(bp.Facade);
			var item5 = ItemGenerator.NewItem(bp.Facade);
			var item6 = ItemGenerator.NewItem(bp.Facade);

			var addSome = this.TestAddSome(new List<Ti>{item1, item2, item3, item4, item5, item6}).ToList();
	        var getSome = bc.GetSome(new Search {SkipItems = 3, TakeItems = 2}, credential);

			Assert.IsNotNull(getSome);
			Assert.IsTrue(getSome.Ok);
			Assert.IsNotNull(getSome.Items);
			Assert.That(getSome.Items.Count(), Is.EqualTo(2));
			Assert.IsTrue(bp.Facade.AreEqual(getSome.Items.First(), bp.Facade.Map(addSome[3])));
			Assert.IsTrue(bp.Facade.AreEqual(getSome.Items.Last(), bp.Facade.Map(addSome[4])));
        }

        [Test]
        public virtual void TestGetByKey()
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var credential = this.GetValidCredential();

			var item1 = ItemGenerator.NewItem(bp.Facade);
			var item2 = ItemGenerator.NewItem(bp.Facade);

			this.TestAddSome(new List<Ti> { item1, item2 });
	        var all = this.TestGetAll(2);

			var first = this.ItemGenerator.CopyItem(bp.Facade.Map(all[0]));
			var second = this.ItemGenerator.CopyItem(bp.Facade.Map(all[1]));

	        bc.DeleteByKey(second, credential);

	        this.TestGetAll(1);

	        var getByKey = bc.GetByKey(first, credential);
			Assert.IsNotNull(getByKey);
			Assert.IsTrue(getByKey.Ok);
			Assert.IsNotNull(getByKey.Item);
			Assert.IsTrue(bp.Facade.AreEqual(getByKey.Item, first));
        }

        [Test]
        public virtual void TestExistsByKey()
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var credential = this.GetValidCredential();

			var item1 = ItemGenerator.NewItem(bp.Facade);
			var item2 = ItemGenerator.NewItem(bp.Facade);

			this.TestAddSome(new List<Ti> { item1, item2 });
			var all = this.TestGetAll(2);

			var first = this.ItemGenerator.CopyItem(bp.Facade.Map(all[0]));
			var second = this.ItemGenerator.CopyItem(bp.Facade.Map(all[1]));

			bc.DeleteByKey(second, credential);

			this.TestGetAll(1);

			var existsByKey = bc.ExistsByKey(first, credential);
			Assert.IsNotNull(existsByKey);
			Assert.IsTrue(existsByKey.Ok);
			Assert.IsTrue(existsByKey.Item);

			existsByKey = bc.ExistsByKey(second, credential);
			Assert.IsNotNull(existsByKey);
			Assert.IsTrue(existsByKey.Ok);
			Assert.IsFalse(existsByKey.Item);
        }

	    public abstract void TestIsAllowed();

	    public abstract void TestUpdate();

	    public abstract void TestSave();

	    public abstract void TestSaveSome();

	    public abstract void TestValidate();
    }
}
