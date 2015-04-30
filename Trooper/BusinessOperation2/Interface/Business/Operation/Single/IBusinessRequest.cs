namespace Trooper.BusinessOperation2.Interface.Business.Operation.Single
{
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface.Business.Response;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    public interface IBusinessRequest<Tc, Ti> : IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity = null);
    }
}
