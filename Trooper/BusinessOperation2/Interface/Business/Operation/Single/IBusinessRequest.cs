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
        /// <summary>
        /// Can the user perform the given action. The search and entities provide the context to what is being attempted. 
        /// You will need to override this method if you have special access checking requirements.
        /// </summary>
        /// <param name="argument">
        /// The argument.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, ICredential credential = null);
    }
}
