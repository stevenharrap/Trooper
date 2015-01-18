namespace Trooper.BusinessOperation2.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Reflection;
    using Trooper.BusinessOperation2.Interface.DataManager;

    public abstract class Facade<Tc, Ti> : IFacade<Tc, Ti> 
        where Tc : class, Ti, new()
        where Ti : class
    {
        #region fields

        #region public

        public IUnitOfWork Uow { get; set; }

        public PropertyInfo[] KeyProperties
        {
            get
            {
                if (this.keyProperties == null)
                {
                    var entityType = new Tc().GetType();
                    var oc = this.Repository.ObjectContextAdapter.ObjectContext;
                    var os = oc.CreateObjectSet<Tc>();
                    var es = os.EntitySet;

                    this.keyProperties = es.ElementType.KeyMembers.Select(km => entityType.GetProperty(km.Name)).ToArray();
                }

                return this.keyProperties;
            }
        }        

        #endregion

        #region private

        private IRepository<Tc> repository;

        private IRepository<Tc> Repository
        {
            get
            {
                if (this.Uow == null)
                {
                    throw new NullReferenceException("The UnitOfWork property (Uow) for this Facade instance has not been set.");
                }

                if (this.repository == null)
                {
                    this.repository = this.Uow.GetRepository<Tc>();
                }

                return this.repository;
            }
        }

        private PropertyInfo[] keyProperties;

        #endregion

        #endregion

        #region methods               

        public virtual IQueryable<Tc> GetAll()
        {
            return this.Repository.GetAll();
        }

        public IQueryable<Tc> GetSome(ISearch search)
        {
            return this.Limit(this.GetAll(), search);
        }

        public IQueryable<Tc> Limit(IQueryable<Tc> items, ISearch search)
        {
            if (search.SkipItems > 0)
            {
                items = items.Skip(search.SkipItems);
            }

            return search.TakeItems > 0 ? items.Take(search.TakeItems) : items;
        }

        public virtual Tc GetById(Tc item)
        {
            var oc = this.Repository.ObjectContextAdapter.ObjectContext;
            var os = oc.CreateObjectSet<Tc>();
            var es = os.EntitySet;
            var entitySetName = oc.DefaultContainerName + "." + es.Name;

            var keyPairs = this.KeyProperties.Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(item))).ToList();
            var nullKey = keyPairs.FirstOrDefault(kp => kp.Key == null || kp.Value == null);

            if (!nullKey.Equals(default(KeyValuePair<string, object>)))
            {
                return null;
            }

            var key = new EntityKey(entitySetName, keyPairs);
            object result;

            if (oc.TryGetObjectByKey(key, out result))
            {
                return result as Tc;
            }

            return null;
        }
        
        public Tc GetById(object obj)
        {
            var item = AutoMapper.Mapper.Map<Tc>(obj);

            return this.GetById(item);
        }

        public bool Exists(Tc item)
        {
            return this.GetById(item) != null;
        }

        public bool Exists(object obj)
        {
            var item = AutoMapper.Mapper.Map<Tc>(obj);

            return this.Exists(item);
        }

        public bool AreEqual(Tc item1, Tc item2)
        {
            if (item1 == null || item2 == null)
            {
                return false;
            }

            foreach (var p in this.KeyProperties)
            {
                if (!p.GetValue(item1).Equals(p.GetValue(item2)))
                {
                    return false;
                }
            }

            return true;
        }

        public bool AreEqual(object obj, Tc item2)
        {
            var entity1 = AutoMapper.Mapper.Map<Tc>(obj);

            return this.AreEqual(entity1, item2);
        }

        public virtual Tc Add(Tc item)
        {
            return this.Repository.Add(item);
        }

        public virtual void Delete(Tc item)
        {
            if (this.Repository.GetState(item) == EntityState.Detached)
            {
                item = this.GetById(item);
            }

            this.Repository.Delete(item);
        }

        public virtual void DeleteSome(IEnumerable<Tc> items)
        {
            var input = from i in items
                        select this.Repository.GetState(i) == EntityState.Detached ? this.GetById(i) : i;

            this.Repository.DeleteSome(input);
        }

        public virtual void Update(Tc item)
        {
            this.Repository.Update(item);
        }

        public virtual bool Any()
        {
            return this.Repository.Any();
        }

        public Tc Map(Ti item)
        {
            return AutoMapper.Mapper.Map<Tc>(item);
        }

        public IEnumerable<Tc> Map(IEnumerable<Ti> items)
        {
            return AutoMapper.Mapper.Map<IEnumerable<Tc>>(items);
        }

        #endregion

        #region private methods

        #endregion        
    }
}
