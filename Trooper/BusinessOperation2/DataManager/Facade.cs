namespace Trooper.BusinessOperation2.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Reflection;
    using Trooper.BusinessOperation2.Interface.DataManager;

    public abstract class Facade<Tc, Ti> : IFacade<Tc, Ti> 
        where Tc : class, Ti, new()
        where Ti : class
    {
        #region fields

        #region public

        private IObjectContextAdapter ObjectContextAdapter
        {
            get
            {
                return this.Repository.DbContext as IObjectContextAdapter;
            }
        }

	    public IUnitOfWork Uow
	    {
		    get
		    {
			    if (this.uow == null)
			    {
				    throw new NullReferenceException("The Unit of Work (Uow) property is null. Always ensure that this property is populated.");
			    }

			    return this.uow;
		    }

		    set { this.uow = value; }
	    }

	    public PropertyInfo[] KeyProperties
        {
            get
            {
                if (this.keyProperties == null)
                {
                    var entityType = new Tc().GetType();
                    var oc = this.ObjectContextAdapter.ObjectContext;
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

	    private IUnitOfWork uow;

        #endregion

        #endregion

        #region methods

        public virtual IQueryable<Tc> GetAll()
        {
            return this.Repository.DbSet;
        }

        public virtual IEnumerable<Tc> GetSome(ISearch search)
        {            
            return this.GetAll().AsEnumerable();
        }

        public IEnumerable<Tc> Limit(IEnumerable<Tc> items, ISearch search)
        {
            if (search.SkipItems > 0)
            {
                items = items.Skip(search.SkipItems);
            }

            return search.TakeItems > 0 ? items.Take(search.TakeItems) : items;
        }

        public virtual Tc GetByKey(Tc item)
        {
            var oc = this.ObjectContextAdapter.ObjectContext;
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
        
        public Tc GetByKey(object obj)
        {
            var item = AutoMapper.Mapper.DynamicMap<Tc>(obj);

            return this.GetByKey(item);
        }

        public bool Exists(Tc item)
        {
            return this.GetByKey(item) != null;
        }

        public bool Exists(object obj)
        {
            var item = AutoMapper.Mapper.DynamicMap<Tc>(obj);

            return this.Exists(item);
        }

	    public bool IsDefault(Tc item)
	    {
			if (item == null)
			{
				return true;
			}

			foreach (var p in this.KeyProperties)
			{
				var defaultValue = Activator.CreateInstance(p.PropertyType);

				if (p.GetValue(item).Equals(defaultValue))
				{
					return true;
				}
			}

			return false;
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
            var entity1 = AutoMapper.Mapper.DynamicMap<Tc>(obj);

            return this.AreEqual(entity1, item2);
        }

        public virtual Tc Add(Tc item)
        {
            return this.Repository.DbSet.Add(item);
        }

        public IList<Tc> AddSome(IEnumerable<Tc> items)
        {
            var result = from i in items
                         select this.Add(i);

            return result.ToList();
        }

        public virtual void Delete(Tc item)
        {
	        if (this.IsDefault(item))
	        {
		        return;
	        }

            var local = this.FindLocal(item);

            var entry = this.Repository.DbContext.Entry(local ?? item);

            entry.State = EntityState.Deleted;
        }

        public void DeleteSome(IEnumerable<Tc> items)
        {
            foreach (var item in items)
            {
                this.Delete(item);
            }
        }

        public virtual Tc Update(Tc item)
        {
            var local = this.FindLocal(item);

            if (local == null)
            {
                var entry = this.Repository.DbContext.Entry(item);
                var attached = this.Repository.DbSet.Attach(item);
                entry.State = EntityState.Modified;
                return attached;
            }
            else
            {
                var entry = this.Repository.DbContext.Entry(local);
                entry.State = EntityState.Modified;
                AutoMapper.Mapper.Map(item, local);
                return local;
            }                        
        }

        public bool Any()
        {
            return this.GetAll().Any();
        }

        public Tc Map(Ti item)
        {
            return AutoMapper.Mapper.Map<Tc>(item);
        }

        public IEnumerable<Tc> Map(IEnumerable<Ti> items)
        {
            //return AutoMapper.Mapper.Map<IEnumerable<Tc>>(items);

            foreach (var i in items)
            {
                yield return this.Map(i);
            }
        }

        #endregion

        #region private methods

        private Tc FindLocal(Tc item)
        {
            return this.Repository.DbContext.Set<Tc>().Local.FirstOrDefault(i => this.AreEqual(i, item));
        }

        #endregion        
    }
}
