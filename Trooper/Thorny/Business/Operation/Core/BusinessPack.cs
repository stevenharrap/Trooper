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

    public class BusinessPack<Tc, Ti> : IBusinessPack<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public IAuthorization<Tc> Authorization { get; set; }

        public IValidation<Tc> Validation { get; set; }

        public IFacade<Tc, Ti> Facade { get; set; }

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
