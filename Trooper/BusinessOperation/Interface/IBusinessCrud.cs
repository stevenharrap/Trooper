//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCrud.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Interface
{
    using System.Collections.Generic;

    using Trooper.BusinessOperation.Response;

    /// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read, Write and delete operations and control
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
    public interface IBusinessCrud<TSearch, TEntity, TEntityPrp, TEntityKey>
        : IBusinessCru<TSearch, TEntity, TEntityPrp, TEntityKey>
        where TSearch : class, ISearch, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// The delete operation for deleting an item in the system if the use can access the method.
        /// </summary>
        /// <param name="entityKey">
        /// The key of the entity to delete.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        IOperationResponse DeleteByKey(TEntityKey entityKey);

        /// <summary>
        /// The delete operation for deleting items in the system if the use can access the method.
        /// </summary>
        /// <param name="entityKeys">
        /// The keys of the entity to delete.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        IOperationResponse DeleteSomeByKey(IEnumerable<TEntityKey> entityKeys);
    }
}