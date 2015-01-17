namespace Trooper.BusinessOperation2.Interface.DataManager
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IFacade<T> 
        where T : class, new()
    {
        //bool IsKeyAuto();

        IUnitOfWork Uow { get; set; }

        T GetById(T item);

        T GetById(object obj);

        bool Exists(T item);

        bool Exists(object obj);

        bool AreEqual(T item1, T item2);

        bool AreEqual(object obj, T item2);

        IQueryable<T> GetAll();

        IQueryable<T> GetSome(ISearch search);

        IQueryable<T> Limit(IQueryable<T> items, ISearch search);

        T Add(T item);

        void Delete(T item);

        void DeleteSome(IEnumerable<T> item);

        void Update(T item);

        bool Any();
    }
}
