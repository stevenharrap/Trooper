using System;
using Trooper.Thorny.Interface.DataManager;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Core
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
