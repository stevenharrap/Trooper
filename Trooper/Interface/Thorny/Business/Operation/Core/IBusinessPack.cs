using System;
using Trooper.Thorny.Interface.DataManager;
using Trooper.Interface.Thorny.Business.Security;
using Autofac;

namespace Trooper.Interface.Thorny.Business.Operation.Core
{
	public interface IBusinessPack<Tc, Ti> : IDisposable
        where Tc : class, Ti, new()
        where Ti : class
    {
        IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        IAuthorization<Tc> Authorization { get; set; }

        IValidation<Tc> Validation { get; set; }

        IFacade<Tc, Ti> Facade { get; set; }

        IUnitOfWork Uow { get; set; }

        IComponentContext Container { get; set; }

        IBusinessPack<TcOther, TiOther> ResolveBusinessPack<TiBusinessCoreOther, TcOther, TiOther>()
            where TiBusinessCoreOther : class, IBusinessCore<TcOther, TiOther>
            where TcOther : class, TiOther, new()
            where TiOther : class;
    }
}
