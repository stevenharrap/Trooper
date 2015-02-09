namespace Trooper.BusinessOperation2.UnitTestBase
{
    using Autofac;
using NUnit.Framework;
using Trooper.BusinessOperation2.Business.Security;
using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
using Trooper.BusinessOperation2.Interface.Business.Security;
using Trooper.BusinessOperation2.Interface.UnitTestBase;
    using System.Linq;
    using System;
    using System.Collections.Generic;
        
    public class TestBusinessOperationBase<TiBusinessCore, Tc, Ti> : TestBase<TiBusinessCore, Tc, Ti>
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

	    [Test]
	    public virtual void TestAddSome(List<Tc> expected)
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

			foreach (var expectedItem in expected)
			{
				Assert.IsTrue(addSome.Items.Any(foundItem => bp.Facade.AreEqual(foundItem, expectedItem)));
			}

		    this.TestGetAll(expected);
	    }
		
        [Test]
        public virtual void TestAddSome() 
        {
            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();
            var item1 = this.ItemGenerator.NewItem(bp.Facade);
            var item2 = this.ItemGenerator.NewItem(bp.Facade);
            var item3 = this.ItemGenerator.NewItem(bp.Facade);

			this.TestAddSome(new List<Tc> { item1, item2, item3 });
        }

        [Test]
        public virtual void TestDeleteByKey() 
        {
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();
			var item1 = this.ItemGenerator.NewItem(bp.Facade);
			var item2 = this.ItemGenerator.NewItem(bp.Facade);
			var credential = this.GetValidCredential();

			this.TestAddSome(new List<Tc> { item1, item2 });

	        var deleteByKey = bc.DeleteByKey(item2, credential);
			Assert.IsNotNull(deleteByKey);
			Assert.IsTrue(deleteByKey.Ok);

	        this.TestGetAll(new List<Tc> {item1});
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

			this.TestAddSome(new List<Tc> { item1, item2, item3 });

			/*bp.Facade.DeleteSome(new List<Tc> { item2, item3 });
			

			var all = bp.Facade.GetAll().ToList();
			Assert.IsNotNull(all);
			Assert.That(all.Count(), Is.EqualTo(2));
			Assert.IsTrue(bp.Facade.AreEqual(all[0], item1));
			Assert.IsTrue(bp.Facade.AreEqual(all[1], item4));*/
        }

        [Test]
        public virtual void TestGetAll()
        {
            Assert.That(false);
        }

		[Test]
		public virtual IList<Ti> TestGetAll(int expected)
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

		[Test]
		public virtual void TestGetAll(List<Tc> expected)
		{
			var bc = this.NewBusinessCoreInstance();
			var bp = bc.GetBusinessPack();

			Assert.IsNotNull(expected);

			var all = this.TestGetAll(expected.Count());

			foreach (var expectedItem in expected)
			{
				Assert.IsTrue(all.Any(foundItem => bp.Facade.AreEqual(foundItem, expectedItem)));
			}
		}

        [Test]
        public virtual void TestGetSome()
        {
            Assert.That(false);
        }

        [Test]
        public virtual void TestGetByKey()
        {
            Assert.That(false);
        }

        [Test]
        public virtual void TestExistsByKey()
        {
            Assert.That(false);
        }

        [Test]
        public virtual void TestIsAllowed()
        {
            Assert.That(false);
        }

        [Test]
        public virtual void TestUpdate()
        {
            Assert.That(false);
        }

        [Test]
        public virtual void TestSave()
        {
            Assert.That(false);
        }

        [Test]
        public virtual void TestSaveSome()
        {
            Assert.That(false);
        }

        [Test]
        public virtual void TestValidate()
        {
            Assert.That(false);
        }
    }
}
