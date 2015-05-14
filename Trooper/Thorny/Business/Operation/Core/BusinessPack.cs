using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Operation.Core
{
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
        public IAuthorization<Tc> Authorization { get; set; }

        public IValidation<Tc> Validation { get; set; }

        public IFacade<Tc, Ti> Facade { get; set; }

        public IUnitOfWork Uow { get; set; }

        public void Dispose()
        {
            Uow.Dispose();
        }
    }
}
