namespace Trooper.Thorny.Interface.DataManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Trooper.Utility;

    public interface IFacade<TEnt, TPoco> 
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        //bool IsKeyAuto();

        IEnumerable<ClassMapping> Searches { get; }

        IUnitOfWork Uow { get; set; }

        TEnt GetByKey(TEnt item);

        IEnumerable<TEnt> GetSomeByKey(IEnumerable<TEnt> items);

        bool Exists(TEnt item);

	    bool IsDefault(TEnt item);

        bool AreEqual(TEnt item1, TEnt item2);

        bool AreEqual(TPoco item1, TPoco item2);

        IQueryable<TEnt> GetAll();

        void AddSearch<TISearch, TSearch>()
            where TISearch : class, ISearch
            where TSearch : class, TISearch, new();

        void AddSearch<TSearch>()
            where TSearch : class, ISearch, new();            

        bool IsSearchAllowed(ISearch search);

        IEnumerable<TEnt> GetSome(ISearch search);

        IEnumerable<TEnt> Limit(IEnumerable<TEnt> items, ISearch search);
        
        TEnt Add(TEnt item);

        IList<TEnt> AddSome(IEnumerable<TEnt> items);

        bool Delete(TEnt item);

        bool DeleteSome(IEnumerable<TEnt> item);

        TEnt Update(TEnt item);

        bool Any();  

        TPoco ToPoco(TEnt item);

        TPoco ToPoco(TPoco item);

        TEnt ToEnt(TPoco item);

        IEnumerable<TPoco> ToPocos(IEnumerable<TEnt> items);

        IEnumerable<TPoco> ToPocos(IEnumerable<TPoco> items);

        IEnumerable<TEnt> ToEnts(IEnumerable<TPoco> items);
    }
}