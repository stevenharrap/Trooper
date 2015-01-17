namespace Trooper.BusinessOperation2.UnitTestBase
{
    using Autofac;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.BusinessOperation2.Business.Operation.Core;
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

        /// <summary>
        ///     Test <see cref="IFacade.GetById(T item)"/> and <see cref="IFacade.GetById(object obj)"/>
        /// </summary>
        [Test]
        public virtual void TestGetById()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var item1 = this.ItemGenerator.ItemFactory();
                var obj1 = this.ItemGenerator.ItemFactory(true, item1) as object;
                var item2 = this.ItemGenerator.MakeItem(false, item1);
                var obj2 = this.ItemGenerator.ItemFactory(true, item2) as object;
                var copy = AutoMapper.Mapper.Map<Tc>(item1);

                bp.Facade.Add(copy);
                bp.Uow.Save();

                var item1ByObject = bp.Facade.GetById(obj1);
                var item1ByEntity = bp.Facade.GetById(item1);
                var item2ByEntity = bp.Facade.GetById(item2);
                var item2ByObject = bp.Facade.GetById(obj2);

                Assert.IsNotNull(item1ByObject);
                Assert.IsNotNull(item1ByEntity);
                Assert.IsNull(item2ByEntity);
                Assert.IsNull(item2ByObject);
                Assert.IsTrue(bp.Facade.AreEqual(obj1, item1ByObject));
                Assert.IsTrue(bp.Facade.AreEqual(item1, item1ByEntity));
            }
        }

        /// <summary>
        ///     Test <see cref="IFacade.Exists(T item)"/> and <see cref="IFacade.Exists(object obj)"/>
        /// </summary>
        [Test]
        public virtual void TestExists()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var item1 = this.ItemGenerator.ItemFactory();
                var obj1 = this.ItemGenerator.ItemFactory(true, item1) as object;
                var item2 = this.ItemGenerator.MakeItem(false, item1);
                var obj2 = this.ItemGenerator.ItemFactory(true, item2) as object;
                var copy = AutoMapper.Mapper.Map<Tc>(item1);

                bp.Facade.Add(copy);
                bp.Uow.Save();

                var item1ByObject = bp.Facade.Exists(obj1);
                var item1ByEntity = bp.Facade.Exists(item1);
                var item2ByEntity = bp.Facade.Exists(item2);
                var item2ByObject = bp.Facade.Exists(obj2);

                Assert.IsTrue(item1ByObject);
                Assert.IsTrue(item1ByEntity);
                Assert.IsFalse(item2ByEntity);
                Assert.IsFalse(item2ByObject);
            }
        }

        /// <summary>
        ///     Test <see cref="IFacade.AreEqual(T item)"/> and <see cref="IFacade.AreEqual(object obj)"/>
        /// </summary>
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

        /// <summary>
        ///     Test <see cref="IFacade.GetAll()"/>
        /// </summary>
        [Test]
        public virtual void TestGetAll()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                this.DeleteAll();
                bp.Facade.Add(new Tc());
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.That(result.Count(), Is.EqualTo(1));
            }
        }

        /// <summary>
        ///     Test <see cref="IFacade.GetSome(ISearch search)"/>
        /// </summary>
        [Test]
        public virtual void TestGetSome()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var item1 = ItemGenerator.ItemFactory();
                var item2 = ItemGenerator.ItemFactory();
                var item3 = ItemGenerator.ItemFactory();
                var item4 = ItemGenerator.ItemFactory();
                var item5 = ItemGenerator.ItemFactory();
                var item6 = ItemGenerator.ItemFactory();

                this.DeleteAll();
                bp.Facade.Add(item1);
                bp.Facade.Add(item2);
                bp.Facade.Add(item3);
                bp.Facade.Add(item4);
                bp.Facade.Add(item5);
                bp.Facade.Add(item6);
                bp.Uow.Save();

                var some = bp.Facade.GetSome(new Search { SkipItems = 3, TakeItems = 2 });

                Assert.IsNotNull(some);
                Assert.That(some.Count(), Is.EqualTo(2));
                Assert.IsTrue(bp.Facade.AreEqual(some.First(), item4));
                Assert.IsTrue(bp.Facade.AreEqual(some.Last(), item5));
            }
        }

        /// <summary>
        ///     Test <see cref="IFacade.Limit(IQueryable<T> items, ISearch search)"/>
        /// </summary>
        [Test]
        public virtual void TestLimit()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var item1 = ItemGenerator.ItemFactory();
                var item2 = ItemGenerator.ItemFactory();
                var item3 = ItemGenerator.ItemFactory();
                var item4 = ItemGenerator.ItemFactory();
                var item5 = ItemGenerator.ItemFactory();
                var item6 = ItemGenerator.ItemFactory();

                this.DeleteAll();
                bp.Facade.Add(item1);
                bp.Facade.Add(item2);
                bp.Facade.Add(item3);
                bp.Facade.Add(item4);
                bp.Facade.Add(item5);
                bp.Facade.Add(item6);
                bp.Uow.Save();

                var all = bp.Facade.GetAll();

                var some = bp.Facade.Limit(all, new Search { SkipItems = 3, TakeItems = 2 });

                Assert.IsNotNull(some);
                Assert.That(some.Count(), Is.EqualTo(2));
                Assert.IsTrue(bp.Facade.AreEqual(some.First(), item4));
                Assert.IsTrue(bp.Facade.AreEqual(some.Last(), item5));
            }
        }

        /// <summary>
        ///     Tests that all items are deleted - used by other tests.
        /// </summary>
        [Test]
        public virtual void DeleteAll()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var all = bp.Facade.GetAll();
                bp.Facade.DeleteSome(all);
                bp.Uow.Save();

                all = bp.Facade.GetAll();
                Assert.IsFalse(all.Any());
            }
        }

        /// <summary>
        ///     Tests <see cref="IFacade.DeleteSome(IEnumerable<T> item)"/>
        /// </summary>
        [Test]
        public virtual void DeleteSome()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var item1 = ItemGenerator.ItemFactory();
                var item2 = ItemGenerator.ItemFactory();
                var item3 = ItemGenerator.ItemFactory();
                var item4 = ItemGenerator.ItemFactory();
                
                this.DeleteAll();
                bp.Facade.Add(item1);
                bp.Facade.Add(item2);
                bp.Facade.Add(item3);
                bp.Facade.Add(item4);
                bp.Uow.Save();

                bp.Facade.DeleteSome(new List<Tc> { item2, item3 });
                bp.Uow.Save();

                var all = bp.Facade.GetAll();
                Assert.IsNotNull(all);
                Assert.That(all.Count(), Is.EqualTo(2));
                Assert.IsTrue(bp.Facade.AreEqual(all.First(), item1));
                Assert.IsTrue(bp.Facade.AreEqual(all.Last(), item4));
            }
        }

        /// <summary>
        ///     Tests <see cref="IFacade.Add(T item)"/>
        /// </summary>
        [Test]
        public virtual void TestAdd()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                this.DeleteAll();

                var item = this.ItemGenerator.ItemFactory();
                var copy = AutoMapper.Mapper.Map<Tc>(item);
                bp.Facade.Add(item);
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.That(result.Count(), Is.EqualTo(1));
                Assert.IsTrue(bp.Facade.AreEqual(copy, result.First()));
            }
        }

        /// <summary>
        ///     Tests <see cref="IFacade.Delete(T item)"/>
        /// </summary>
        [Test]
        public virtual void TestDelete()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                this.DeleteAll();

                var item1 = this.ItemGenerator.ItemFactory();
                var item2 = this.ItemGenerator.ItemFactory(false, item1);

                bp.Facade.Add(item1);
                bp.Facade.Add(item2);
                bp.Uow.Save();
                var result = bp.Facade.GetAll();

                Assert.AreEqual(result.Count(), 2);

                bp.Facade.Delete(item2);
                bp.Uow.Save();
                result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.AreEqual(result.Count(), 1);
                Assert.IsTrue(bp.Facade.AreEqual(result.First(), item1));
            }
        }

        /// <summary>
        ///     Tests <see cref="IFacade.Any()"/>
        /// </summary>
        [Test]
        public virtual void TestAny()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                this.DeleteAll();

                Assert.IsFalse(bp.Facade.Any());

                var item1 = this.ItemGenerator.ItemFactory();
                bp.Facade.Add(item1);
                bp.Uow.Save();

                Assert.IsTrue(bp.Facade.Any());
            }
        }

        #endregion        
    }
}
