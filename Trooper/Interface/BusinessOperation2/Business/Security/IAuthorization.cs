using System.Collections.Generic;
using Trooper.BusinessOperation2.Interface.DataManager;
using Trooper.BusinessOperation2.Interface.OperationResponse;

namespace Trooper.Interface.BusinessOperation2.Business.Security
{
	public interface IAuthorization<Tc> 
        where Tc : class, new()
    {
        IUnitOfWork Uow { get; set; }

        IList<IUserRole> Roles { get; set; }

        ICredential ResolveCredential(IIdentity identity);

        bool IsAddDataAction(string action);

        bool IsUpdateDataAction(string action);

        bool IsRemoveDataAction(string action);

        bool IsChangeAction(string action);

        bool IsReadAction(string action);

		bool IsAllowed(IRequestArg<Tc> arg, IIdentity identity);

        bool IsAllowed(IRequestArg<Tc> arg, ICredential credential);

		bool IsAllowed(IRequestArg<Tc> arg, IIdentity identity, IResponse response);

        bool IsAllowed(IRequestArg<Tc> arg, ICredential credential, IResponse response);
    }
}
