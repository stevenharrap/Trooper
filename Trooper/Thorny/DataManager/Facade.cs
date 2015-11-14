namespace Trooper.Thorny.Interface
{
    //using AutoMapper;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Reflection;
    using Trooper.Thorny.Business.Operation.Core;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Utility;    

    public class Facade<TEnt, TPoco> : IFacade<TEnt, TPoco> 
        where TEnt : class, TPoco, new()
        where TPoco : class, new()
    {
        private static Mapping[] entToPocoMap;
        private static Mapping[] pocoToEntMap;

        static Facade()
        {
            //Mapper.CreateMap<TPoco, TEnt>().IgnorePropertiesOfType(typeof(ICollection));
            //Mapper.CreateMap<TEnt, TPoco>();

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

		    set { this.uow = value; }
	    }

	    public PropertyInfo[] KeyProperties
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
                return false;
            }

            var searchType = search.GetType();

            return this.Searches.Any(s =>  s.Source.IsEquivalentTo(searchType));
        }

        public virtual IEnumerable<TEnt> GetSome(ISearch search)
        {
            if (!this.IsSearchAllowed(search))
            {
                return new List<TEnt>();
            }
            
            return this.GetAll().AsEnumerable();
        }

        public IEnumerable<TEnt> Limit(IEnumerable<TEnt> items, ISearch search)
        {
            if (search.SkipItems > 0)
            {
                items = items.Skip(search.SkipItems);
            }

            return search.TakeItems > 0 ? items.Take(search.TakeItems) : items;
        }

        public virtual TEnt GetByKey(TEnt item)
        {
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
            foreach (var item in items)
            {
                yield return this.GetByKey(item);
            }
        }

        public bool Exists(TEnt item)
        {
            return this.GetByKey(item) != null;
        }
        
	    public bool IsDefault(TEnt item)
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

        public bool AreEqual(TEnt item1, TEnt item2)
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

        public virtual TEnt Add(TEnt item)
        {
            return this.Repository.DbSet.Add(item);
        }

        public IList<TEnt> AddSome(IEnumerable<TEnt> items)
        {
            var result = from i in items
                         select this.Add(i);

            return result.ToList();
        }

        public virtual bool Delete(TEnt item)
        {
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

        public TPoco ToPoco(TEnt item)
        {
            if (item == null) { return null; }

            var result = new TPoco();

            foreach (var m in entToPocoMap)
            {
                m.SetterMethod.Invoke(result, new object[] { m.GetterMethod.Invoke(item, null) });
            }           

            return result;
        }

        public IEnumerable<TPoco> ToPocos(IEnumerable<TEnt> items)
        {
            foreach (var item in items ?? Enumerable.Empty<TEnt>())
            {
                yield return this.ToPoco(item);
            }
        }

        public TEnt ToEnt(TPoco item)
        {
            if (item == null) { return null; }

            var result = new TEnt();

            foreach (var m in pocoToEntMap)
            {
                m.SetterMethod.Invoke(result, new object[] { m.GetterMethod.Invoke(item, null) });
            }

            return result;
        }

        public IEnumerable<TEnt> ToEnts(IEnumerable<TPoco> items)
        {
            foreach (var item in items ?? Enumerable.Empty<TPoco>())
            {
                yield return this.ToEnt(item);
            }
        }

        #endregion

        #region private methods

        private TEnt FindLocal(TEnt item)
        {
            return this.Repository.DbContext.Set<TEnt>().Local.FirstOrDefault(i => this.AreEqual(i, item));
        }

        private class Mapping
        {
            public Mapping(MethodInfo getter, MethodInfo setter)
            {
                this.GetterMethod = getter;
                this.SetterMethod = setter;
            }

            public MethodInfo GetterMethod { get;  }

            public MethodInfo SetterMethod {get;  }
        }

        #endregion        
    }
}
