using System;
using Trooper.Thorny.Interface.DataManager;
using Trooper.Interface.Thorny.Business.Security;
using Autofac;

namespace Trooper.Interface.Thorny.Business.Operation.Core
{
	public interface IBusinessPack<TEnt, TPoco> : IDisposable
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        IBusinessCore<TEnt, TPoco> BusinessCore { get; set; }

        IAuthorization<TPoco> Authorization { get; set; }

        IValidation<TEnt> Validation { get; set; }

        IFacade<TEnt, TPoco> Facade { get; set; }

        IUnitOfWork Uow { get; set; }

        IComponentContext Container { get; set; }

        IBusinessPack<TcOther, TiOther> ResolveBusinessPack<TiBusinessCoreOther, TcOther, TiOther>()
            where TiBusinessCoreOther : class, IBusinessCore<TcOther, TiOther>
            where TcOther : class, TiOther, new()
            where TiOther : class;
    }
}
