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
        public virtual void TestAddSome() 
        {
            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();
            var item1 = this.ItemGenerator.NewItem(bp.Facade);
            var item2 = this.ItemGenerator.NewItem(bp.Facade);
            var item3 = this.ItemGenerator.NewItem(bp.Facade);
            var credential = this.GetValidCredential();

            var addSome = bc.AddSome(new List<Tc>{ item1, item2, item3 }, credential);

            Assert.IsNotNull(addSome);
            Assert.IsTrue(addSome.Ok);
            Assert.IsNotNull(addSome.Items);
            Assert.That(addSome.Items.Count(), Is.EqualTo(3));
           
            var all = bc.GetAll(credential);

            Assert.IsNotNull(all);
            Assert.IsTrue(all.Ok);
            Assert.That(all.Items.Count(), Is.EqualTo(3));
        }

        [Test]
        public virtual void TestDeleteByKey() 
        {
            Assert.That(false);

            var bc = this.NewBusinessCoreInstance();
            var bp = bc.GetBusinessPack();
            var credential = this.GetValidCredential();
            var item1 = this.ItemGenerator.NewItem(bp.Facade);
            var item2 = this.ItemGenerator.NewItem(bp.Facade);

            var item1Add = bc.Add(item1, credential);
            var item2Add = bc.Add(item2, credential);

            var getAllResult = bc.GetAll(credential);

            Assert.That(getAllResult.Ok);
            Assert.IsNotNull(getAllResult.Items);
            Assert.That(getAllResult.Items.Count(), Is.EqualTo(2));

            var deleteResult = bc.DeleteByKey(item2Add.Item, credential);
            Assert.That(deleteResult.Ok);

            getAllResult = bc.GetAll(credential);
            Assert.That(getAllResult.Ok);
            Assert.IsNotNull(getAllResult.Items);
            Assert.That(getAllResult.Items.Count(), Is.EqualTo(1));
            Assert.IsTrue(bp.Facade.AreEqual(bp.Facade.Map(getAllResult.Items.First()), bp.Facade.Map(item1Add.Item)));

            deleteResult = bc.DeleteByKey(item1Add.Item, credential);
            Assert.That(deleteResult.Ok);

            getAllResult = bc.GetAll(credential);
            Assert.That(getAllResult.Ok);
            Assert.IsNotNull(getAllResult.Items);
            Assert.That(getAllResult.Items.Count(), Is.EqualTo(0));            
        }

        [Test]
        public virtual void TestDeleteSomeByKey()
        {
            Assert.That(false);
        }

        [Test]
        public virtual void TestGetAll()
        {
            Assert.That(false);
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
