using Trooper.BusinessOperation2.Interface.DataManager;

namespace Trooper.BusinessOperation2.UnitTestBase
{
    using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trooper.BusinessOperation2.Business.Operation.Core;
using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
using Trooper.BusinessOperation2.Interface.UnitTestBase;

    
    public abstract class TestFacadeBase<TiBusinessCore, Tc, Ti> : TestBase<TiBusinessCore, Tc, Ti>
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
            this.DeleteAll();
        }
        
        #region ---- Facade Tests ----

        /// <summary>
		///     Test <see cref="IFacade{Tc,Ti}.GetByKey(Tc)"/> and <see cref="IFacade{Tc,Ti}.GetByKey(object)"/>
        /// </summary>
        [Test]
        public virtual void TestGetByKey()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                bp.Facade.Add(this.ItemGenerator.NewItem(bp.Facade));
                bp.Facade.Add(this.ItemGenerator.NewItem(bp.Facade));
                bp.Uow.Save();            

                var all = bp.Facade.GetAll().ToList();
                Assert.NotNull(all);
                Assert.That(all.Count(), Is.EqualTo(2));

                var first = this.ItemGenerator.CopyItem(all.ElementAt(0));
                var second = this.ItemGenerator.CopyItem(all.ElementAt(1));

                bp.Facade.Delete(this.ItemGenerator.CopyItem(second));
                bp.Uow.Save();

                all = bp.Facade.GetAll().ToList();
                Assert.NotNull(all);
                Assert.That(all.Count(), Is.EqualTo(1));
                
                var item1ByObject = bp.Facade.GetByKey(first as object);
                var item1ByEntity = bp.Facade.GetByKey(first);
                var item2ByObject = bp.Facade.GetByKey(second as object);
                var item2ByEntity = bp.Facade.GetByKey(second);                

                Assert.IsNotNull(item1ByObject);
                Assert.IsNotNull(item1ByEntity);
                Assert.IsNull(item2ByEntity);
                Assert.IsNull(item2ByObject);
                Assert.IsTrue(bp.Facade.AreEqual(first as object, item1ByObject));
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
                bp.Facade.Add(this.ItemGenerator.NewItem(bp.Facade));
                bp.Facade.Add(this.ItemGenerator.NewItem(bp.Facade));
                bp.Uow.Save();

                var all = bp.Facade.GetAll().ToList();
                Assert.NotNull(all);
                Assert.That(all.Count(), Is.EqualTo(2));

                var first = this.ItemGenerator.CopyItem(all.ElementAt(0));
                var second = this.ItemGenerator.CopyItem(all.ElementAt(1));

                bp.Facade.Delete(this.ItemGenerator.CopyItem(second));
                bp.Uow.Save();

                all = bp.Facade.GetAll().ToList();
                Assert.NotNull(all);
                Assert.That(all.Count(), Is.EqualTo(1));

                var item1ByObject = bp.Facade.Exists(first as object);
                var item1ByEntity = bp.Facade.Exists(first);
                var item2ByObject = bp.Facade.Exists(second as object);
                var item2ByEntity = bp.Facade.Exists(second);

                Assert.IsTrue(item1ByObject);
                Assert.IsTrue(item1ByEntity);
                Assert.IsFalse(item2ByObject);
                Assert.IsFalse(item2ByEntity);
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
                bp.Facade.Add(this.ItemGenerator.NewItem(bp.Facade));
                bp.Facade.Add(this.ItemGenerator.NewItem(bp.Facade));
                bp.Uow.Save();

                var all = bp.Facade.GetAll().ToList();

                Assert.That(all.Count(), Is.EqualTo(2));

                var first = all[0];
                var second = this.ItemGenerator.CopyItem(first);
                var third = all[1];

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
                var item = this.ItemGenerator.NewItem(bp.Facade);
                bp.Facade.Add(item);
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
                var item1 = ItemGenerator.NewItem(bp.Facade);
                var item2 = ItemGenerator.NewItem(bp.Facade);
                var item3 = ItemGenerator.NewItem(bp.Facade);
                var item4 = ItemGenerator.NewItem(bp.Facade);
                var item5 = ItemGenerator.NewItem(bp.Facade);
                var item6 = ItemGenerator.NewItem(bp.Facade);
                
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
                var item1 = ItemGenerator.NewItem(bp.Facade);
                var item2 = ItemGenerator.NewItem(bp.Facade);
                var item3 = ItemGenerator.NewItem(bp.Facade);
                var item4 = ItemGenerator.NewItem(bp.Facade);
                var item5 = ItemGenerator.NewItem(bp.Facade);
                var item6 = ItemGenerator.NewItem(bp.Facade);
                
                bp.Facade.Add(item1);
                bp.Facade.Add(item2);
                bp.Facade.Add(item3);
                bp.Facade.Add(item4);
                bp.Facade.Add(item5);
                bp.Facade.Add(item6);
                bp.Uow.Save();

                var all = bp.Facade.GetAll().AsEnumerable();

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
                var item1 = ItemGenerator.NewItem(bp.Facade);
                var item2 = ItemGenerator.NewItem(bp.Facade);
                var item3 = ItemGenerator.NewItem(bp.Facade);
                var item4 = ItemGenerator.NewItem(bp.Facade);
                
                bp.Facade.Add(item1);
                bp.Facade.Add(item2);
                bp.Facade.Add(item3);
                bp.Facade.Add(item4);
                bp.Uow.Save();

                bp.Facade.DeleteSome(new List<Tc> { item2, item3 });
                bp.Uow.Save();

                var all = bp.Facade.GetAll().ToList();
                Assert.IsNotNull(all);
                Assert.That(all.Count(), Is.EqualTo(2));
                Assert.IsTrue(bp.Facade.AreEqual(all[0], item1));
                Assert.IsTrue(bp.Facade.AreEqual(all[1], item4));
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
                var item = this.ItemGenerator.NewItem(bp.Facade);
                bp.Facade.Add(item);
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.That(result.Count(), Is.EqualTo(1));
            }
        }

        /// <summary>
        ///     Tests <see cref="IFacade.AddSome(IEnumerable<T> items)"/>
        /// </summary>
        [Test]
        public virtual void TestAddSome()
        {
            using (var bp = this.NewBusinessCoreInstance().GetBusinessPack())
            {
                var item1 = this.ItemGenerator.NewItem(bp.Facade);
                var item2 = this.ItemGenerator.NewItem(bp.Facade);
                var item3 = this.ItemGenerator.NewItem(bp.Facade);
                bp.Facade.AddSome(new List<Tc>{ item1, item2, item3 });
                bp.Uow.Save();

                var result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.That(result.Count(), Is.EqualTo(3));
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
                var item1 = this.ItemGenerator.NewItem(bp.Facade);
                var item2 = this.ItemGenerator.NewItem(bp.Facade);

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

                var anItem = bp.Facade.GetByKey(item1);
                bp.Facade.Delete(anItem);
                bp.Uow.Save();

                result = bp.Facade.GetAll();

                Assert.IsNotNull(result);
                Assert.AreEqual(result.Count(), 0);
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
                Assert.IsFalse(bp.Facade.Any());

                var item1 = this.ItemGenerator.NewItem(bp.Facade);
                bp.Facade.Add(item1);
                bp.Uow.Save();

                Assert.IsTrue(bp.Facade.Any());
            }
        }

        public abstract void TestUpdate();

        #endregion        
    }
}
