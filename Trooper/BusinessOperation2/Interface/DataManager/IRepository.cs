namespace Trooper.BusinessOperation2.Interface.DataManager
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    public interface IRepository<T>
        where T : class
    {
        IDbContext DbContext { get; }

        IObjectContextAdapter ObjectContextAdapter { get; }

        IQueryable<T> GetAll();

        EntityState GetState(T item);

        T Add(T item);

        void Delete(T item);

        void DeleteSome(IEnumerable<T> item);

        void Update(T item);

        bool Any();
    }
}
