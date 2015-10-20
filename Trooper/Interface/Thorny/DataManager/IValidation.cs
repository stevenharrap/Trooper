namespace Trooper.Thorny.Interface.DataManager
{
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Response;

    public interface IValidation<TEnt> 
        where TEnt : class, new()
    {
        IUnitOfWork Uow { get; set; }

        bool IsValid(TEnt item);

        bool AreValid(IEnumerable<TEnt> items);

        bool IsValid(TEnt item, IResponse response);

        bool AreValid(IEnumerable<TEnt> items, IResponse response);

        IResponse Validate(TEnt item);

        IResponse Validate(IEnumerable<TEnt> items);

        bool Validate(TEnt item, IResponse response);

        bool Validate(IEnumerable<TEnt> items, IResponse response);
    }
}
