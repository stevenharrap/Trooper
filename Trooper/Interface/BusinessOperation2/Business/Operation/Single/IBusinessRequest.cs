using Trooper.Interface.BusinessOperation2.Business.Operation.Core;
using Trooper.Interface.BusinessOperation2.Business.Response;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.Interface.BusinessOperation2.Business.Operation.Single
{
	public interface IBusinessRequest<Tc, Ti> : IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity = null);
    }
}
