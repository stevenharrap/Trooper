//--------------------------------------------------------------------------------------
// <copyright file="IBusinessR.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Interface.Business.Operation.Single
{
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface.Business.Response;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    /// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read operations and control
    /// access to those operations.
    /// </summary>
    public interface IBusinessRead<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        /// <summary>
        /// Gets all entities for the given type if the user can access the method. 
        /// </summary> 
        /// <returns>
        /// GetManyResponse of TEntity.
        /// </returns>
        IManyResponse<Ti> GetAll(ICredential credential = null);        

        /// <summary>
        /// The get some entities based on the search object in the ItemsRequest
        /// if the user can access the method.
        /// </summary>
        /// <param name="search">
        /// The search.
        /// </param>
        /// <returns>
        /// The GetManyResponse of TEntity
        /// </returns>
        IManyResponse<Ti> GetSome(ISearch search, ICredential credential = null);  

        /// <summary>
        /// Get the entity by its key if the user can access the method.
        /// </summary>
        /// <param name="itemKey">
        /// The item Key.
        /// </param>
        /// <returns>
        /// The GetSingleResponse containing the entity
        /// </returns>
        ISingleResponse<Ti> GetByKey(Ti item, ICredential credential = null);

        /// <summary>
        /// Determines if the entity exists by its key and that the user can access the method.
        /// If the user does not have access then false will be returned
        /// </summary>
        /// <param name="itemKey">
        /// The item Key.
        /// </param>
        /// <returns>
        /// The GetSingleResponse containing the entity
        /// </returns>
        ISingleResponse<bool> ExistsByKey(Ti item, ICredential credential = null);
        
    }
}