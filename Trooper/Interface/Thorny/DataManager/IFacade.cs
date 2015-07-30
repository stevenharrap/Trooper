namespace Trooper.Thorny.Interface.DataManager
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public interface IFacade<TEnt, TPoco> 
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        //bool IsKeyAuto();

        PropertyInfo[] KeyProperties { get; }

        IUnitOfWork Uow { get; set; }

        TEnt GetByKey(TEnt item);

        IEnumerable<TEnt> GetSomeByKey(IEnumerable<TEnt> items);

        //TEnt GetByKey(object obj);

        bool Exists(TEnt item);

        //bool Exists(object obj);

	    bool IsDefault(TEnt item);

        bool AreEqual(TEnt item1, TEnt item2);

        //bool AreEqual(object obj, TEnt item2);

        IQueryable<TEnt> GetAll();

        IEnumerable<TEnt> GetSome(ISearch search);

        IEnumerable<TEnt> Limit(IEnumerable<TEnt> items, ISearch search);
        
        TEnt Add(TEnt item);

        IList<TEnt> AddSome(IEnumerable<TEnt> items);

        bool Delete(TEnt item);

        bool DeleteSome(IEnumerable<TEnt> item);

        TEnt Update(TEnt item);

        bool Any();

        TEnt Map(TPoco item);

        IEnumerable<TEnt> Map(IEnumerable<TPoco> items);
    }
}
