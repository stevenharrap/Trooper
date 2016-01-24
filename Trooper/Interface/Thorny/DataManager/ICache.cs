namespace Trooper.Interface.Thorny.DataManager
{
    using System.Collections.Generic;
    using Trooper.Thorny.Interface.DataManager;

    public interface ICache<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        IUnitOfWork Uow { get; set; }

        void Put(TPoco item);

        void Put(IEnumerable<TPoco> items);

        TPoco Get(TPoco item);

        IEnumerable<TPoco> Get(IEnumerable<TPoco> items);
    }
}