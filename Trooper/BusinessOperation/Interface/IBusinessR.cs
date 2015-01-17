//--------------------------------------------------------------------------------------
// <copyright file="IBusinessR.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Interface
{
    using Trooper.BusinessOperation.Response;

    /// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read operations and control
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
    public interface IBusinessR<TSearch, TEntity, TEntityPrp, TEntityKey> 
        where TSearch : class, ISearch, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// Gets or sets the username of the user who is doing these operations. This
        /// may not be the logged-in person.
        /// </summary>
        string Username { get; set; }
        
        /// <summary>
        /// Gets all entities for the given type if the user can access the method. 
        /// </summary> 
        /// <returns>
        /// GetManyResponse of TEntity.
        /// </returns>
        ManyResponse<TEntity> GetAll();        

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
        ManyResponse<TEntity> GetSome(TSearch search);  

        /// <summary>
        /// Get the entity by its key if the user can access the method.
        /// </summary>
        /// <param name="itemKey">
        /// The item Key.
        /// </param>
        /// <returns>
        /// The GetSingleResponse containing the entity
        /// </returns>
        SingleResponse<TEntity> GetByKey(TEntityKey itemKey);

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
        SingleResponse<bool> ExistsByKey(TEntityKey itemKey);

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
        OperationResponse Validate(TEntity entity);

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
        OperationResponse CanUser(IBusinessCanUserArg<TSearch, TEntity, TEntityPrp, TEntityKey> argument);

        /// <summary>
        /// Creates a CanUserArgument for use by presentation layers.
        /// </summary>
        /// <returns>IBusinessCanUserArg&lt;TSearch, TEntity&gt;</returns>
        IBusinessCanUserArg<TSearch, TEntity, TEntityPrp, TEntityKey> MakeCanUserArg();
    }
}