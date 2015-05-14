namespace Trooper.Thorny.Interface.DataManager
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

        Tc GetByKey(Tc item);

        Tc GetByKey(object obj);

        bool Exists(Tc item);

        bool Exists(object obj);

	    bool IsDefault(Tc item);

        bool AreEqual(Tc item1, Tc item2);

        bool AreEqual(object obj, Tc item2);

        IQueryable<Tc> GetAll();

        IEnumerable<Tc> GetSome(ISearch search);

        IEnumerable<Tc> Limit(IEnumerable<Tc> items, ISearch search);
        
        Tc Add(Tc item);

        IList<Tc> AddSome(IEnumerable<Tc> items);

        void Delete(Tc item);

        void DeleteSome(IEnumerable<Tc> item);

        Tc Update(Tc item);

        bool Any();

        Tc Map(Ti item);

        IEnumerable<Tc> Map(IEnumerable<Ti> items);
    }
}
