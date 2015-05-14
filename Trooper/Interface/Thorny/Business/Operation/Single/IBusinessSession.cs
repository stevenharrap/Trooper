using System;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
	public interface IBusinessSession
    {
        ISingleResponse<Guid> GetSession(IIdentity identity);
    }
}
