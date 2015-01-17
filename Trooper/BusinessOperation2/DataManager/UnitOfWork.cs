namespace Trooper.BusinessOperation2.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.BusinessOperation2.Interface.DataManager;

    public class UnitOfWork<TContext> : IUnitOfWork 
        where TContext : IDbContext, new()
    {
        private readonly IDbContext _ctx;
        private Dictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork()
        {
            _ctx = new TContext();
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }

        public IRepository<T> GetRepository<T>() where T : 
            class
        {
            // Checks if the Dictionary Key contains the Model class
            if (_repositories.Keys.Contains(typeof(T)))
            {
                // Return the repository for that Model class
                return _repositories[typeof(T)] as IRepository<T>;
            }

            // If the repository for that Model class doesn't exist, create it
            var repository = new Repository<T>(_ctx);

            // Add it to the dictionary
            _repositories.Add(typeof(T), repository);

            return repository;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }

                this._disposed = true;
            }
        }
    }
}
