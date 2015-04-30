namespace Trooper.BusinessOperation2.Interface.Business.Operation.Single
{
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    public interface IBusinessValidate<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        IResponse Validate(Ti item, IIdentity identity = null);       
    }
}
