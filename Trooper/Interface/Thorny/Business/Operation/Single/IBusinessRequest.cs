using System;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
	public interface IBusinessRequest<Tc, Ti> : IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity = null);
    }
}
