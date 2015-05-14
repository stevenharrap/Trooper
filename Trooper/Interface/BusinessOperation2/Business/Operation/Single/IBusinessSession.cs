using System;
using Trooper.Interface.BusinessOperation2.Business.Operation.Core;
using Trooper.Interface.BusinessOperation2.Business.Response;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.Interface.BusinessOperation2.Business.Operation.Single
{
	public interface IBusinessSession
    {
        ISingleResponse<Guid> GetSession(IIdentity identity);
    }
}
