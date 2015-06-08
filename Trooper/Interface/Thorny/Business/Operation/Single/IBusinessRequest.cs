using System;
using System.ServiceModel;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
    [ServiceContract]
	public interface IBusinessRequest<Tc, Ti> : IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        [OperationContract]
        ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity = null);
    }
}
