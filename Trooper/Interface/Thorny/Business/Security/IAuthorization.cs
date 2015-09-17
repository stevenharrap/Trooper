using System.Collections.Generic;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Thorny.Interface.DataManager;

namespace Trooper.Interface.Thorny.Business.Security
{
	public interface IAuthorization<TPoco> 
        where TPoco : class
    {
        IUnitOfWork Uow { get; set; }

        IList<IAssignment> Assignments { get; }

		IList<string> AllActions { get; }

        ICredential ResolveCredential(IIdentity identity);

        bool IsAddDataAction(string action);

        bool IsRemoveDataAction(string action);

        bool IsChangeAction(string action);

		bool IsUpdateAction(string action);

        bool IsReadAction(string action);

		bool IsAllowed(IRequestArg<TPoco> arg, IIdentity identity);

        bool IsAllowed(IRequestArg<TPoco> arg, ICredential credential);

		bool IsAllowed(IRequestArg<TPoco> arg, IIdentity identity, IResponse response);

        bool IsAllowed(IRequestArg<TPoco> arg, ICredential credential, IResponse response);
    }
}
