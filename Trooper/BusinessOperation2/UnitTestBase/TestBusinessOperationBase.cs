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
        public override void Setup(IContainer container, IItemGenerator<Tc, Ti> itemGenerator) 
        {
            base.Setup(container, itemGenerator); 
        }

        public override void Setup(IContainer container)
        {
            base.Setup(container);
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
        public virtual void TestGetBusinessPack() { }

        [Test]
        public virtual void TestDeleteAll() 
        {
            var bc = this.NewBusinessCoreInstance();
            var credential = this.GetInvalidCredential();
            var all = bc.GetAll(credential);

            if (!all.Ok)
            {
                Assert.Fail();
            }

            var delete = bc.DeleteSomeByKey(all.Items, credential);

            if (!delete.Ok)
            {
                Assert.Fail();
            }

            all = bc.GetAll(credential);

            Assert.IsTrue(all.Ok && !all.Items.Any());
        }

        [Test]
        public virtual void TestAdd() 
        {
            this.TestDeleteAll();

            var bc = this.NewBusinessCoreInstance();
            var credential = this.GetInvalidCredential();

            var add = bc.Add(this.ItemGenerator.ItemFactory(), credential);

            if (!add.Ok)
            {
                Assert.Fail();
            }

            var all = bc.GetAll(credential);

            if (!all.Ok)
            {
                Assert.Fail();
            }

            Assert.That(all.Ok && all.Items.Count == 1);
        }

        [Test]
        public virtual void TestAddSome() { }

        [Test]
        public virtual void TestDeleteByKey() {  }

        [Test]
        public virtual void TestDeleteSomeByKey() { }

        [Test]
        public virtual void TestGetAll() { }

        [Test]
        public virtual void TestGetSome() { }

        [Test]
        public virtual void TestGetByKey() { }

        [Test]
        public virtual void TestExistsByKey() { }

        [Test]
        public virtual void TestIsAllowed() { }

        [Test]
        public virtual void TestUpdate() { }

        [Test]
        public virtual void TestSave() { }

        [Test]
        public virtual void TestValidate() { }
    }
}
