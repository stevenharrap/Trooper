namespace Trooper.BusinessOperation2.UnitTestBase
{
    using Autofac;
    using NUnit.Framework;
    using System.Linq;
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface.UnitTestBase;

    public abstract class TestFacadeBase<TiBusinessCore, Tc, Ti> : TestBase<TiBusinessCore, Tc, Ti>
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

        #region ---- Facade Tests ----

        [Test]
        public virtual void DeleteAll()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var all = bp.Facade.GetAll();
                bp.Facade.DeleteSome(all);
                bp.Uow.Save();

                all = bp.Facade.GetAll();
                Assert.IsTrue(!all.Any());
            }
        }

        [Test]
        public virtual void TestGetAll()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                this.DeleteAll();
                bp.Facade.Add(new Tc());
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsTrue(result != null && result.Count() == 1, "TestGetAll: GetAll failed.");
            }
        }

        [Test]
        public virtual void TestGetById()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var entity = this.ItemGenerator.ItemFactory();
                var obj = this.ItemGenerator.ItemFactory(true, entity) as object;
                var copy = AutoMapper.Mapper.Map<Tc>(entity);

                bp.Facade.Add(copy);
                bp.Uow.Save();

                var itemByObject = bp.Facade.GetById(obj);
                var itemByEntity = bp.Facade.GetById(entity);

                Assert.IsNotNull(itemByObject, "TestGetByIdentifier: GetByIdentifier(obj) returned null.");
                Assert.IsNotNull(itemByObject, "TestGetByIdentifier: GetByIdentifier(entity) returned null.");
                Assert.IsTrue(bp.Facade.AreEqual(obj, itemByObject), "TestGetByIdentifier: AreEqual(object, object) not matching.");
                Assert.IsTrue(bp.Facade.AreEqual(entity, itemByEntity), "TestGetByIdentifier: AreEqual(entity, entity) not matching.");
            }
        }

        [Test]
        public virtual void TestAreEqual()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var first = this.ItemGenerator.ItemFactory();
                var second = this.ItemGenerator.ItemFactory(true, first);
                var third = this.ItemGenerator.ItemFactory();

                Assert.IsTrue(bp.Facade.AreEqual(first, second));
                Assert.IsTrue(bp.Facade.AreEqual(first as object, second));

                Assert.IsFalse(bp.Facade.AreEqual(first, third));
                Assert.IsFalse(bp.Facade.AreEqual(first as object, third));
            }
        }

        [Test]
        public virtual void TestAdd()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                this.DeleteAll();

                var entity = this.ItemGenerator.ItemFactory();
                var copy = AutoMapper.Mapper.Map<Tc>(entity);
                bp.Facade.Add(entity);
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsTrue(result != null, "TestAdd: null was returned.");
                Assert.IsTrue(result.Count() == 1, "TestAdd: 1 entity was not returned.");
                Assert.IsTrue(bp.Facade.AreEqual(copy, result.First()), "TestAdd: the returned entity is not equal to the orginal.");
            }
        }

        [Test]
        public virtual void TestDelete()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                this.DeleteAll();

                var first = this.ItemGenerator.ItemFactory();
                var second = this.ItemGenerator.ItemFactory(false, first);

                bp.Facade.Add(first);
                bp.Facade.Add(second);
                bp.Uow.Save();
                var result = bp.Facade.GetAll();

                Assert.AreEqual(result.Count(), 2);

                bp.Facade.Delete(second);
                bp.Uow.Save();
                result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.AreEqual(result.Count(), 1);
                Assert.IsTrue(bp.Facade.AreEqual(result.First(), first));
            }
        }

        #endregion        
    }
}
