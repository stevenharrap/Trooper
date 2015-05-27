﻿using System.Collections.Generic;
using Trooper.Thorny.Interface.DataManager;
using Trooper.Thorny.Interface.OperationResponse;

namespace Trooper.Interface.Thorny.Business.Security
{
	public interface IAuthorization<Tc> 
        where Tc : class, new()
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

		bool IsAllowed(IRequestArg<Tc> arg, IIdentity identity);

        bool IsAllowed(IRequestArg<Tc> arg, ICredential credential);

		bool IsAllowed(IRequestArg<Tc> arg, IIdentity identity, IResponse response);

        bool IsAllowed(IRequestArg<Tc> arg, ICredential credential, IResponse response);
    }
}
