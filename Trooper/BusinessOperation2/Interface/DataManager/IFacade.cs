namespace Trooper.BusinessOperation2.Interface.DataManager
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public interface IFacade<Tc, Ti> 
        where Tc : class, Ti, new()
        where Ti : class
    {
        //bool IsKeyAuto();

        PropertyInfo[] KeyProperties { get; }

        IUnitOfWork Uow { get; set; }

        Tc GetById(Tc item);

        Tc GetById(object obj);

        bool Exists(Tc item);

        bool Exists(object obj);

        bool AreEqual(Tc item1, Tc item2);

        bool AreEqual(object obj, Tc item2);

        IQueryable<Tc> GetAll();

        IQueryable<Tc> GetSome(ISearch search);

        IQueryable<Tc> Limit(IQueryable<Tc> items, ISearch search);

        Tc Add(Tc item);

        void Delete(Tc item);

        void DeleteSome(IEnumerable<Tc> item);

        void Update(Tc item);

        bool Any();

        Tc Map(Ti item);

        IEnumerable<Tc> Map(IEnumerable<Ti> items);
    }
}
