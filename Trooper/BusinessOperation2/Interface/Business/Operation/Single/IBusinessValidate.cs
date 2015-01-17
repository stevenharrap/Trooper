namespace Trooper.BusinessOperation2.Interface.Business.Operation.Single
{
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    public interface IBusinessValidate<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        /// <summary>
        /// The validate operation for validating an item - no changes should be made to the system.
        /// Any attempt to Add or update items is always validated so there should be no
        /// need for the UI to call this method before execute other actions. 
        /// This method allows for testing potential operations by the user. 
        /// The user will need to have access to the method. 
        /// </summary>
        /// <param name="entity">
        /// The entity to validate.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>. Ok will be true if there are no validation issues.
        /// </returns>
        IResponse Validate(Ti item, ICredential credential = null);       
    }
}
