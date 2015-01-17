//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCru.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Interface
{
    using Trooper.BusinessOperation.Response;

    /// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read and Write operations and control
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
    public interface IBusinessCru<TSearch, TEntity, TEntityPrp, TEntityKey>
        : IBusinessR<TSearch, TEntity, TEntityPrp, TEntityKey>
        where TSearch : class, ISearch, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
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
        IAddResponse<TEntityKey> Add(TEntity entity);

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
        IOperationResponse Update(TEntity entity);

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
        SaveResponse<TEntity> Save(TEntity entity);
    }
}