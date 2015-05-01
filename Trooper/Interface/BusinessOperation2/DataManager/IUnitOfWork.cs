namespace Trooper.BusinessOperation2.Interface.DataManager
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() 
            where T : class;

        void Save();
    }
}
