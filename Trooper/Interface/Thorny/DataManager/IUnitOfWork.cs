namespace Trooper.Thorny.Interface.DataManager
{
    using System;
    using Trooper.Interface.Thorny.Business.Response;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() 
            where T : class;

        bool Save(IResponse response);

        void Save();
    }
}
