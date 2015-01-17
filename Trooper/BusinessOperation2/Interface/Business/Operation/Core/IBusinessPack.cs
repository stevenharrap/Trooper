namespace Trooper.BusinessOperation2.Interface.Business.Operation.Core
{
    using System;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.DataManager;

    public interface IBusinessPack<Tc, Ti> : IDisposable
        where Tc : class, Ti, new()
        where Ti : class
    {
        IAuthorization<Tc> Authorization { get; set; }

        IValidation<Tc> Validation { get; set; }

        IFacade<Tc> Facade { get; set; }

        IUnitOfWork Uow { get; set; }
    }
}
