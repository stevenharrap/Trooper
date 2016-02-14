namespace Trooper.Thorny.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Reflection;
    using Business.Operation.Core;
    using DataManager;
    using Trooper.Utility;    

    public class Facade<TEnt, TPoco> : IFacade<TEnt, TPoco> 
        where TEnt : class, TPoco, new()
        where TPoco : class, new()
    {
        private static Mapping[] entToPocoMap;
        private static Mapping[] pocoToEntMap;
        private static Mapping[] pocoToPocoMap;
        private static Mapping[] entToEntMap;

        static Facade()
        {
            var result = new TPoco();

            var mappings = from pocoProp in typeof(TPoco).GetProperties()
                           let entProp = typeof(TEnt).GetProperties().FirstOrDefault(
                               p => p.Name == pocoProp.Name
                               && p.PropertyType.IsEquivalentTo(pocoProp.PropertyType))
                           where entProp != null
                           let pocoGetter = pocoProp.GetGetMethod()
                           let pocoSetter = pocoProp.GetSetMethod()
                           let entGetter = entProp.GetGetMethod()
                           let entSetter = entProp.GetSetMethod()
                           where pocoGetter != null && pocoSetter != null && entGetter != null && entSetter != null
                           select new { pocoGetter, pocoSetter, entGetter, entSetter };

            entToPocoMap = mappings.Select(m => new Mapping(m.entGetter, m.pocoSetter)).ToArray();
            pocoToEntMap = mappings.Select(m => new Mapping(m.pocoGetter, m.entSetter)).ToArray();
            entToEntMap = mappings.Select(m => new Mapping(m.entGetter, m.entSetter)).ToArray();
            pocoToPocoMap = mappings.Select(m => new Mapping(m.pocoGetter, m.pocoSetter)).ToArray();
        }

        public Facade()
        {
            this.AddSearch<ISearch, Search>();
        }

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

		    set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.uow = value;
            }
	    }

	    private PropertyInfo[] KeyProperties
        {
            get
            {
                if (this.keyProperties == null)
                {
                    var entityType = new TEnt().GetType();
                    var oc = this.ObjectContextAdapter.ObjectContext;
                    var os = oc.CreateObjectSet<TEnt>();
                    var es = os.EntitySet;

                    this.keyProperties = es.ElementType.KeyMembers.Select(km => entityType.GetProperty(km.Name)).ToArray();
                }

                return this.keyProperties;
            }
        }

        public IEnumerable<ClassMapping> Searches
        {
            get
            {
                return this.searches;
            }
        }

        #endregion

        #region private

        private List<ClassMapping> searches;

        private IRepository<TEnt> repository;

        private IRepository<TEnt> Repository
        {
            get
            {
                if (this.Uow == null)
                {
                    throw new NullReferenceException("The UnitOfWork property (Uow) for this Facade instance has not been set.");
                }

                if (this.repository == null)
                {
                    this.repository = this.Uow.GetRepository<TEnt>();
                }

                return this.repository;
            }
        }

        private PropertyInfo[] keyProperties;

	    private IUnitOfWork uow;

        #endregion

        #endregion

        #region methods

        public virtual IQueryable<TEnt> GetAll()
        {
            return this.Repository.DbSet;
        }               

        public void AddSearch<TISearch, TSearch>()
            where TISearch : class, ISearch
            where TSearch : class, TISearch, new()
        {
            var searchType = typeof(TSearch);

            if (this.searches == null)
            {
                this.searches = new List<ClassMapping>();
            }

            if (this.searches.Any(s => s.Source.IsEquivalentTo(searchType)))
            {
                return;
            }

            this.searches.Add(ClassMapping.Make<TISearch, TSearch>());
        }

        public void AddSearch<TSearch>()
            where TSearch : class, ISearch, new()
        {
            this.AddSearch<TSearch, TSearch>();
        }

        public void ClearSearches()
        {
            this.searches = new List<ClassMapping>();
        }

        public bool IsSearchAllowed(ISearch search)
        {
            if (search == null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            var searchType = search.GetType();

            return this.Searches.Any(s =>  s.Source.IsEquivalentTo(searchType));
        }

        public virtual IEnumerable<TEnt> GetSome(ISearch search)
        {
            if (search == null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            if (!this.IsSearchAllowed(search))
            {
                return new List<TEnt>();
            }
            
            return this.GetAll().AsEnumerable();
        }

        public IEnumerable<TEnt> Limit(IEnumerable<TEnt> items, ISearch search)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (search == null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            if (search.SkipItems > 0)
            {
                items = items.Skip(search.SkipItems);
            }

            return search.TakeItems > 0 ? items.Take(search.TakeItems) : items;
        }

        public virtual TEnt GetByKey(TEnt item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var oc = this.ObjectContextAdapter.ObjectContext;
            var os = oc.CreateObjectSet<TEnt>();
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
                return result as TEnt;
            }
            
            return null;
        }        

        public virtual IEnumerable<TEnt> GetSomeByKey(IEnumerable<TEnt> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                yield return this.GetByKey(item);
            }
        }

        public bool Exists(TEnt item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return this.GetByKey(item) != null;
        }
        
	    public bool IsDefault(TEnt item)
	    {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
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

        public bool AreEqual(TEnt item1, TEnt item2)
        {
            if (item1 == null) throw new ArgumentNullException(nameof(item1));
            if (item2 == null) throw new ArgumentNullException(nameof(item2));
            
            foreach (var p in this.KeyProperties)
            {
                if (!p.GetValue(item1).Equals(p.GetValue(item2)))
                {
                    return false;
                }
            }

            return true;
        }

        public bool AreEqual(TPoco item1, TPoco item2)
        {
            if (item1 == null) throw new ArgumentNullException(nameof(item1));
            if (item2 == null) throw new ArgumentNullException(nameof(item2));

            var poco1 = this.ToEnt(item1);
            var poco2 = this.ToEnt(item2);

            foreach (var p in this.KeyProperties)
            {
                if (!p.GetValue(poco1).Equals(p.GetValue(poco2)))
                {
                    return false;
                }
            }

            return true;
        }

        public virtual TEnt Add(TEnt item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return this.Repository.DbSet.Add(item);
        }

        public IList<TEnt> AddSome(IEnumerable<TEnt> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var result = from i in items
                         select this.Add(i);

            return result.ToList();
        }

        public virtual bool Delete(TEnt item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (this.IsDefault(item))
	        {
		        return false;
	        }

            var local = this.FindLocal(item);

            if (local == null)
            {
                local = this.Repository.DbSet.Attach(item);
            }

            var entry = this.Repository.DbContext.Entry(local);

            entry.State = EntityState.Deleted;

	        return true;
        }

        public bool DeleteSome(IEnumerable<TEnt> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var result = true;

            foreach (var item in items)
            {
	            if (!this.Delete(item))
	            {
		            result = false;
	            }
            }

	        return result;
        }

        public virtual TEnt Update(TEnt item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

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
                this.CopyEnt(item, local);
                return local;
            }                        
        }

        public bool Any()
        {
            return this.GetAll().Any();
        }        

        public TPoco ToPoco(TEnt item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var result = new TPoco();

            foreach (var m in entToPocoMap)
            {
                m.SetterMethod.Invoke(result, new object[] { m.GetterMethod.Invoke(item, null) });
            }           

            return result;
        }

        public TPoco ToPoco(TPoco item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var result = new TPoco();

            foreach (var m in pocoToPocoMap)
            {
                m.SetterMethod.Invoke(result, new object[] { m.GetterMethod.Invoke(item, null) });
            }

            return result;
        }

        public IEnumerable<TPoco> ToPocos(IEnumerable<TEnt> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items ?? Enumerable.Empty<TEnt>())
            {
                yield return this.ToPoco(item);
            }
        }

        public IEnumerable<TPoco> ToPocos(IEnumerable<TPoco> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items ?? Enumerable.Empty<TPoco>())
            {
                yield return this.ToPoco(item);
            }
        }

        public TEnt ToEnt(TPoco item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var result = new TEnt();

            foreach (var m in pocoToEntMap)
            {
                m.SetterMethod.Invoke(result, new object[] { m.GetterMethod.Invoke(item, null) });
            }

            return result;
        }

        public IEnumerable<TEnt> ToEnts(IEnumerable<TPoco> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items ?? Enumerable.Empty<TPoco>())
            {
                yield return item == null ? null : this.ToEnt(item);
            }
        }

        #endregion

        #region private methods

        private TEnt FindLocal(TEnt item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return this.Repository.DbContext.Set<TEnt>().Local.FirstOrDefault(i => this.AreEqual(i, item));
        }

        private void CopyEnt(TEnt source, TEnt destination)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            foreach (var m in entToEntMap)
            {
                m.SetterMethod.Invoke(destination, new object[] { m.GetterMethod.Invoke(source, null) });
            }
        }

        private class Mapping
        {
            public Mapping(MethodInfo getter, MethodInfo setter)
            {
                if (getter == null)
                {
                    throw new ArgumentNullException(nameof(getter));
                }

                if (setter == null)
                {
                    throw new ArgumentNullException(nameof(setter));
                }

                this.GetterMethod = getter;
                this.SetterMethod = setter;
            }

            public MethodInfo GetterMethod { get;  }

            public MethodInfo SetterMethod {get;  }
        }

        #endregion        
    }
}
