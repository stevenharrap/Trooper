//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Interface
{
    using System.Collections.Generic;

    using Trooper.BusinessOperation.Response;

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
    public interface IBusinessCr<TSearch, TEntity, TEntityPrp, TEntityKey>
        : IBusinessR<TSearch, TEntity, TEntityPrp, TEntityKey>
        where TSearch : class, ISearch, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
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
        IAddResponse<TEntityKey> Add(TEntity entity);

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
        IAddSomeResponse<TEntityKey> AddSome(IEnumerable<TEntity> entities);
    }
}