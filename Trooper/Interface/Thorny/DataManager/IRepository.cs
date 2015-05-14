namespace Trooper.Thorny.Interface.DataManager
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    public interface IRepository<T>
        where T : class
    {
        IDbContext DbContext { get; }

        IDbSet<T> DbSet { get; }
    }
}
