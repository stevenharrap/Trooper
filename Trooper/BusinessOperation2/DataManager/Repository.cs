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
        public IDbSet<T> DbSet { get; private set; }

        public IDbContext DbContext { get; private set; }

        public Repository(IDbContext context)
        {
            this.DbContext = context;
            this.DbSet = context.Set<T>();
        }
    }
}
