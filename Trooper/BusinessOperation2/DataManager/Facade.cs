namespace Trooper.BusinessOperation2.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Reflection;
    using Trooper.BusinessOperation2.Interface.DataManager;

    public abstract class Facade<T> : IFacade<T> 
        where T : class, new()
    {
        #region public fields

        public IUnitOfWork Uow { get; set; }

        private IRepository<T> repository;

        private IRepository<T> Repository
        {
            get
            {
                if (this.Uow == null)
                {
                    throw new NullReferenceException("The UnitOfWork property (Uow) for this Facade instance has not been set.");
                }

                if (this.repository == null)
                {
                    this.repository = this.Uow.GetRepository<T>();
                }

                return this.repository;
            }
        }

        private PropertyInfo[] keyProperties;

        #endregion

        #region public methods        

        private PropertyInfo[] KeyProperties
        {            
            get
            {
                if (this.keyProperties == null)
                {
                    var entityType = new T().GetType();
                    var oc = this.Repository.ObjectContextAdapter.ObjectContext;
                    var os = oc.CreateObjectSet<T>();
                    var es = os.EntitySet;

                    this.keyProperties = es.ElementType.KeyMembers.Select(km => entityType.GetProperty(km.Name)).ToArray();
                }

                return this.keyProperties;
            }
        }        

        public virtual IQueryable<T> GetAll()
        {
            return this.Repository.GetAll();
        }

        public IQueryable<T> GetSome(ISearch search)
        {
            return this.Limit(this.GetAll(), search);
        }

        public IQueryable<T> Limit(IQueryable<T> items, ISearch search)
        {
            if (search.SkipItems > 0)
            {
                items = items.Skip(search.SkipItems);
            }

            return search.TakeItems > 0 ? items.Take(search.TakeItems) : items;
        }

        public virtual T GetById(T item)
        {
            var oc = this.Repository.ObjectContextAdapter.ObjectContext;
            var os = oc.CreateObjectSet<T>();
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
                return result as T;
            }

            return null;
        }
        
        public T GetById(object obj)
        {
            var entity = AutoMapper.Mapper.Map<T>(obj);

            return this.GetById(entity);
        }

        public bool Exists(T item)
        {
            return this.GetById(item) != null;
        }

        public bool Exists(object obj)
        {
            return this.Exists(obj);
        }

        public bool AreEqual(T item1, T item2)
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

        public bool AreEqual(object obj, T item2)
        {
            var entity1 = AutoMapper.Mapper.Map<T>(obj);

            return this.AreEqual(entity1, item2);
        }

        public virtual T Add(T item)
        {
            return this.Repository.Add(item);
        }

        public virtual void Delete(T item)
        {
            this.Repository.Delete(item);
        }

        public virtual void DeleteSome(IEnumerable<T> item)
        {
            this.Repository.DeleteSome(item);
        }

        public virtual void Update(T item)
        {
            this.Repository.Update(item);
        }

        public virtual bool Any()
        {
            return this.Repository.Any();
        }

        #endregion

        #region private methods

        #endregion        
    }
}
