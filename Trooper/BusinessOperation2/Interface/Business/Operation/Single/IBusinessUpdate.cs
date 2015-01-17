//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCru.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Interface.Business.Operation.Single
{
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface.Business.Response;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    /// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read and Write operations and control
    /// access to those operations.
    /// </summary>
    public interface IBusinessUpdate<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessValidate<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        /// <summary>
        /// The add operation for adding an item to the system if the user can access the method. 
        /// Result and key of the new entity is returned in the response.
        /// </summary>
        /// <param name="entity">
        /// The entity to add
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        IAddResponse<Tc> Add(Ti item, ICredential credential = null);

        /// <summary>
        /// The update operation for updating an item in the system if the user can access the method. 
        /// The result is returned in the response.
        /// </summary>
        /// <param name="entity">
        /// The entity to update.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        IResponse Update(Ti item, ICredential credential = null);

        /// <summary>
        /// The save operation for adding an item to the system if it 
        /// does not exist or update an existing item. 
        /// Requires that user can access Add and update methods. 
        /// Result and key of the new entity is returned in the response
        /// if it is an add. 
        /// </summary>
        /// <param name="entity">
        /// The entity to add or update.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        ISaveResponse<Tc> Save(Ti item, ICredential credential = null);
    }
}