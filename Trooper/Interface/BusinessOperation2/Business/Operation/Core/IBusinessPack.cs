using System;
using Trooper.BusinessOperation2.Interface.DataManager;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.Interface.BusinessOperation2.Business.Operation.Core
{
	public interface IBusinessPack<Tc, Ti> : IDisposable
        where Tc : class, Ti, new()
        where Ti : class
    {
        IAuthorization<Tc> Authorization { get; set; }

        IValidation<Tc> Validation { get; set; }

        IFacade<Tc, Ti> Facade { get; set; }

        IUnitOfWork Uow { get; set; }
    }
}
