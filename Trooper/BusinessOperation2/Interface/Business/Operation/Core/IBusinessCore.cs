//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Interface.Business.Operation.Core
{
    using Autofac;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Trooper.BusinessOperation2.Interface.Business.Response;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    public delegate IBusinessPack<Tc, Ti> BusinessPackHandler<Tc, Ti>()        
        where Tc : class, Ti, new()
        where Ti : class;

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
    public interface IBusinessCore<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        event BusinessPackHandler<Tc, Ti> OnRequestBusinessPack;
        
        IBusinessPack<Tc, Ti> GetBusinessPack();

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
        IAddResponse<Ti> Add(Ti item, ICredential credential);

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
        IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, ICredential credential);

        /// <summary>
        /// The delete operation for deleting an item in the system if the use can access the method.
        /// </summary>
        /// <param name="entityKey">
        /// The key of the entity to delete.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        IResponse DeleteByKey(Ti item, ICredential credential);

        /// <summary>
        /// The delete operation for deleting items in the system if the use can access the method.
        /// </summary>
        /// <param name="entityKeys">
        /// The keys of the entity to delete.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        IResponse DeleteSomeByKey(IEnumerable<Ti> items, ICredential credential);

        /// <summary>
        /// Gets all entities for the given type if the user can access the method. 
        /// </summary> 
        /// <returns>
        /// GetManyResponse of TEntity.
        /// </returns>
        IManyResponse<Ti> GetAll(ICredential credential);

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
        IManyResponse<Ti> GetSome(ISearch search, ICredential credential);

        /// <summary>
        /// Get the entity by its key if the user can access the method.
        /// </summary>
        /// <param name="itemKey">
        /// The item Key.
        /// </param>
        /// <returns>
        /// The GetSingleResponse containing the entity
        /// </returns>
        ISingleResponse<Ti> GetByKey(Ti item, ICredential credential);

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
        ISingleResponse<bool> ExistsByKey(Ti item, ICredential credential);

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
        ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, ICredential credential);

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
        IResponse Update(Ti item, ICredential credential);

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
        ISaveResponse<Ti> Save(Ti item, ICredential credential);

        ISaveSomeResponse<Ti> SaveSome(IEnumerable<Ti> items, ICredential credential);


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
        IResponse Validate(Ti item, ICredential credential);       
    }
}