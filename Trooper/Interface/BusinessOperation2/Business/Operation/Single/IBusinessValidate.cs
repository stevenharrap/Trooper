using Trooper.BusinessOperation2.Interface.OperationResponse;
using Trooper.Interface.BusinessOperation2.Business.Operation.Core;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.Interface.BusinessOperation2.Business.Operation.Single
{
	public interface IBusinessValidate<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        IResponse Validate(Ti item, IIdentity identity = null);       
    }
}
