namespace Trooper.BusinessOperation2.UnitTestBase
{
    using Autofac;
using NUnit.Framework;
using Trooper.BusinessOperation2.Business.Security;
using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
using Trooper.BusinessOperation2.Interface.Business.Security;
using Trooper.BusinessOperation2.Interface.UnitTestBase;
    using System.Linq;
        
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
        }

        public virtual ICredential GetValidCredential()
        {
            return new Credential
            {
                Username = "TestUser"
            };
        }

        public virtual ICredential GetInvalidCredential()
        {
            return null;
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
            Assert.That(false);
            /*var bc = this.NewBusinessCoreInstance();
            var credential = this.GetValidCredential();
            var all = bc.GetAll(credential);

            Assert.IsNotNull(all);
            Assert.IsTrue(all.Ok);

            var delete = bc.DeleteSomeByKey(all.Items, credential);

            Assert.IsNotNull(delete);
            Assert.IsTrue(delete.Ok);

            all = bc.GetAll(credential);

            Assert.IsNotNull(all);
            Assert.IsFalse(all.Items.Any());*/
        }

        [Test]
        public virtual void TestAdd() 
        {
            Assert.That(false);
            /*this.TestDeleteAll();

            var bc = this.NewBusinessCoreInstance();
            var credential = this.GetValidCredential();

            var add = bc.Add(this.ItemGenerator.NewItem(bc.GetBusinessPack().Facade), credential);

            Assert.IsNotNull(add);
            Assert.IsTrue(add.Ok);

            var all = bc.GetAll(credential);

            Assert.IsNotNull(all);
            Assert.IsTrue(all.Ok);
            Assert.That(all.Items.Count, Is.EqualTo(1));*/
        }

        [Test]
        public virtual void TestAddSome() 
        {
            Assert.That(false);
        }

        [Test]
        public virtual void TestDeleteByKey() 
        {
            Assert.That(false);
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
        public virtual void TestValidate()
        {
            Assert.That(false);
        }
    }
}
