namespace Trooper.Thorny.Interface.DataManager
{
    using System;
    using Trooper.Interface.Thorny.Business.Response;
    using Business.Response;
    using System.Collections.Generic;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() 
            where T : class;

        bool Save(IResponse response);

        void Save();

        IResponse GetValidationResult<T>(T item)
            where T : class;
    }
}
