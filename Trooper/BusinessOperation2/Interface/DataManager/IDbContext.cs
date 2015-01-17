namespace Trooper.BusinessOperation2.Interface.DataManager
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;

        DbEntityEntry<T> Entry<T>(T item) where T : class;

        int SaveChanges();

        void Dispose();
    }
}
