//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Interface.Business.Operation.Single
{
    using System.Collections.Generic;
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface.Business.Response;
    using Trooper.BusinessOperation2.Interface.Business.Security;

    /// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read and Add operations and control
    /// access to those operations.
    /// </summary>
    /// <typeparam name="TSearch">
    /// The search class to provide parameters to the GetSome method
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The class that contains the entity whole entity (but not navigation properties)
    /// </typeparam>
    /// <typeparam name="TEntityPrp">
    /// The class that contains the entity properties (that are not primary or foreign key properties)
    /// </typeparam>
    /// <typeparam name="TEntityKey">
    /// The key of the entity
    /// </typeparam>
    public interface IBusinessCreate<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessValidate<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        /// <summary>
        /// The add operation for adding an item to the system. The user will need to have access to the method. 
        /// Result and key of the new entity is returned in the response.
        /// </summary>
        /// <param name="entity">
        /// The entity to add
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        IAddResponse<Ti> Add(Ti item, ICredential credential = null);

        /// <summary>
        /// The add operation for adding items to the system. The user will need to have access to the method. 
        /// Result and key of the new entity is returned in the response.
        /// </summary>
        /// <param name="entities">
        /// The entities to add
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, ICredential credential = null);
    }
}