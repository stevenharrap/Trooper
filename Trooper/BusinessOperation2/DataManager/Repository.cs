namespace Trooper.BusinessOperation2.Interface
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using Trooper.BusinessOperation2.Interface.DataManager;

    public class Repository<T> : IRepository<T> 
        where T : class
    {
        private readonly IDbSet<T> _dbset;

        public IDbContext DbContext { get; private set; }

        public IObjectContextAdapter ObjectContextAdapter { get; private set; }

        public Repository(IDbContext context)
        {
            this.DbContext = context;
            this.ObjectContextAdapter = this.DbContext as IObjectContextAdapter;
            _dbset = context.Set<T>();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbset;
        }

        public EntityState GetState(T item)
        {
            var entry = this.DbContext.Entry(item);

            return entry.State;
        }

        public virtual T Add(T item)
        {
            return _dbset.Add(item);
        }

        public virtual void Delete(T item)
        {
            var entry = this.DbContext.Entry(item);
            entry.State = EntityState.Deleted;
            _dbset.Remove(item);
        }

        public virtual void DeleteSome(IEnumerable<T> item)
        {
            foreach (var ent in item)
            {
                var entry = this.DbContext.Entry(ent);
                entry.State = EntityState.Deleted;
                _dbset.Remove(ent);
            }
        }

        public virtual void Update(T item)
        {
            var entry = this.DbContext.Entry(item);
            _dbset.Attach(item);
            entry.State = EntityState.Modified;
        }

        public virtual bool Any()
        {
            return _dbset.Any();
        }
    }
}
