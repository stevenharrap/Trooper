using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Operation.Core
{
    using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Thorny.Interface.DataManager;

    public class BusinessPack<TEnt, TPoco> : IBusinessPack<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public IBusinessCore<TEnt, TPoco> BusinessCore { get; set; }

        public IAuthorization<TPoco> Authorization { get; set; }

        public IValidation<TEnt> Validation { get; set; }

        public IFacade<TEnt, TPoco> Facade { get; set; }

        public IUnitOfWork Uow { get; set; }

        public IComponentContext Container { get; set; }

        public IBusinessPack<TcOther, TiOther> ResolveBusinessPack<TiBusinessCoreOther, TcOther, TiOther>()
            where TiBusinessCoreOther : class, IBusinessCore<TcOther, TiOther>
            where TcOther : class, TiOther, new()
            where TiOther : class
        {
            var core = this.Container.Resolve<TiBusinessCoreOther>();
            return core.GetBusinessPack(this.Uow);            
        }

        public void Dispose()
        {
            Uow.Dispose();
        }
    }
}
