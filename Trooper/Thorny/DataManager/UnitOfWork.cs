namespace Trooper.Thorny.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using Trooper.Thorny.DataManager;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Thorny.Interface.OperationResponse;
    using Trooper.Thorny.Utility;

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

        public void Save(IResponse response)
        {
            try
            {
                _ctx.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    foreach (var vr in eve.ValidationErrors)
                    {                        
                        MessageUtility.Errors.Add(vr.ErrorMessage, Validation.InvalidPropertyCode, eve.Entry.Entity, vr.PropertyName, response);
                    }                    
                }
            }
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
