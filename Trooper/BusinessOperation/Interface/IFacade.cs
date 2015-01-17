//--------------------------------------------------------------------------------------
// <copyright file="IFacade.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Interface
{
    using System.Collections.Generic;

    using Microsoft.Practices.EnterpriseLibrary.Validation;

    using Trooper.BusinessOperation.Business;

    /// <summary>
    /// <para>
    /// Provides the CRUD and associated operations for getting data in and out of your model.
    /// It is assumed that the class that implements this interface will always be instantiated
    /// and used from within a "using DBContext" scope.
    /// </para>
    /// <para>
    /// The BusinessOperation project provides a default implementation of this interface which
    /// you can extend and override where you see fit to provide specific behavior. 
    /// </para>
    /// </summary>
    /// <typeparam name="TSearch">
    /// The search class to provide parameters to the GetSome method
    /// </typeparam>
    /// <typeparam name="TEntityNav">
    /// The class that contains the entity navigation properties
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
    public interface IFacade<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
        where TSearch : class, ISearch, new()
        where TEntityNav : class, TEntity, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// Gets or sets the username of the current user.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets the default error message supplied when no message available for the user.
        /// </summary>
        string DefaultErrorMessage { get; set; }

        /// <summary>
        /// Does the action result in data being add.
        /// Override this method to include your own special action names that result in adding records to the system.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsAddDataAction(string action);

        /// <summary>
        /// Does the action result in data being updated.
        /// Override this method to include your own special action names that result in update to the system records.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsUpdateDataAction(string action);

        /// <summary>
        /// Does the action in data being removed.
        /// Override this method to include your own special action names that result in change to the system.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsRemoveDataAction(string action);

        /// <summary>
        /// Does the action result in any change in the system.
        /// By default this action uses <see cref="IsAddDataAction"/>, 
        /// <see cref="IsUpdateDataAction"/> and, <see cref="IsRemoveDataAction"/>
        /// so override those with your special action names.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsChangeAction(string action);

        /// <summary>
        /// Does the action result in a read of information but no change.
        /// Override this method to include your own special action names that result in reads from the system.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsReadAction(string action);

        /// <summary>
        /// Returns the actions string appropriate for the supplied SaveChangeType
        /// </summary>
        /// <param name="saveChangeType">
        /// The save change type.
        /// </param>
        /// <returns>
        /// The appropriate <see cref="string"/> action string.
        /// </returns>
        string GetChangeTypeAction(SaveChangeType saveChangeType);
       
        /// <summary>
        /// Provides the means of getting some of the rows from your table based on the properties
        /// of the search object and how you implement GetSome.
        /// Without specific overriding in your class this will provide the same result as the
        /// GetAll method limited to the MaxItems value of the searchObject. 
        /// </summary> 
        /// <param name="search">
        /// The search object to provide guidance on what to return.
        /// </param>
        /// <returns>
        /// A list of TEntity
        /// </returns>
        IEnumerable<TEntityNav> GetSome(TSearch search);

        /// <summary>
        /// Gets all entities from your table.
        /// </summary>
        /// <returns>Set of TEntity.</returns>
        IEnumerable<TEntityNav> GetAll();

        /// <summary>
        /// Gets the entity by the specified entity key.
        /// </summary>
        /// <param name="entityKey">
        /// The entity key.
        /// </param>
        /// <returns>
        /// Entity matching specified entity key or null if not found.
        /// </returns>
        TEntityNav GetByKey(TEntityKey entityKey);

        /// <summary>
        /// Does a record exist in the table for the given entity key. 
        /// </summary>
        /// <param name="entityKey">
        /// The entity key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Exists(TEntityKey entityKey);

        /// <summary>
        /// <para>
        /// 1. If the entity key is null then false is returned. Its a null key which is not set.
        /// 2. If the entity key appears to represent an existing record but no record is retrievable and
        /// the entity does not represent a new record that has just been added then true is returned else false.
        /// If the entity key appears to represent an existing record but no record is retrievable and
        /// the entity does not represent a new record that has just been added then true is returned else false.
        /// </para>
        /// <para>
        /// This method is useful when determining if the current item is being added along with its parent item
        /// at the same time.
        /// </para> 
        /// <para>
        /// Useful in validation when you want to know if the parent record exists or has just been added.
        /// </para>
        /// </summary>
        /// <param name="allowNullKey">
        /// If the foreign key is optional and no child record is being recorded then entityKey should be supplied
        /// as null. To signal that this is allowable allowNullKey should be true.
        /// </param>
        /// <param name="entityKey">
        /// The entity key. Does it appear to represent an existing record but record cannot be retrieved?
        /// </param>
        /// <param name="entityNav">
        /// The entity nav. Does it appear to represent an existing record but record cannot be retrieved?
        /// </param>
        /// <returns>
        /// Returns true if the entity does not exist and has not just been added.
        /// </returns>
        bool NotExistsAndNotNew(bool allowNullKey, TEntityKey entityKey, TEntityNav entityNav);

        /// <summary>
        /// <para>
        /// 1. It is assumed that the key is not allowed to be null. If so then true.
        /// 2. If the entity key appears to represent an existing record but no record is retrievable and
        /// the entity does not represent a new record that has just been added then true is returned else false.
        /// If the entity key appears to represent an existing record but no record is retrievable and
        /// the entity does not represent a new record that has just been added then true is returned else false.
        /// </para>
        /// <para>
        /// This method is useful when determining if the current item is being added along with its parent item
        /// at the same time.
        /// </para> 
        /// <para>
        /// Useful in validation when you want to know if the parent record exists or has just been added.
        /// </para>
        /// </summary>
        /// <param name="entityKey">
        /// The entity key. Does it appear to represent an existing record but record cannot be retrieved?
        /// </param>
        /// <param name="entityNav">
        /// The entity nav. Does it appear to represent an existing record but record cannot be retrieved?
        /// </param>
        /// <param name="validationResults">
        /// The validation results to populate with an error if this turns out to be true.
        /// </param>
        /// <param name="propertyName">
        /// The field that is required.
        /// </param>
        /// <returns>
        /// Returns true if the entity does not exist and has not just been added.
        /// </returns>
        bool NotExistsAndNotNew(
            TEntityKey entityKey,
            TEntityNav entityNav,
            ValidationResults validationResults,
            string propertyName = null);

        /// <summary>
        /// <para>
        /// 1. It is assumed that the key is allowed to be null. If null then false.
        /// 2. If the entity key appears to represent an existing record but no record is retrievable and
        /// the entity does not represent a new record that has just been added then true is returned else false.
        /// If the entity key appears to represent an existing record but no record is retrievable and
        /// the entity does not represent a new record that has just been added then true is returned else false.
        /// </para>
        /// <para>
        /// This method is useful when determining if the current item is being added along with its parent item
        /// at the same time.
        /// </para> 
        /// <para>
        /// Useful in validation when you want to know if the parent record exists or has just been added.
        /// </para>
        /// </summary>
        /// <param name="entityKey">
        /// The entity key. Does it appear to represent an existing record but record cannot be retrieved?
        /// </param>
        /// <param name="entityNav">
        /// The entity nav. Does it appear to represent an existing record but record cannot be retrieved?
        /// </param>
        /// <param name="validationResults">
        /// The validation results to populate with an error if this turns out to be true.
        /// </param>
        /// <param name="propertyName">
        /// The field that is required.
        /// </param>
        /// <returns>
        /// Returns true if the entity does not exist and has not just been added.
        /// </returns>
        bool NullableNotExistsAndNotNew(
            TEntityKey entityKey,
            TEntityNav entityNav,
            ValidationResults validationResults,
            string propertyName = null);

        /// <summary>
        /// Adds a new entity object to the context.
        /// </summary>
        /// <param name="newEntityNav">
        /// An entity that has not yet been added to the context.
        /// </param>
        /// <returns>
        /// The added <see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Add(TEntityNav newEntityNav);

        /// <summary>
        /// Adds new entity objects to the context.
        /// </summary>
        /// <param name="newEntityNavs">
        /// Entities that have not yet been added to the context.
        /// </param>
        /// <returns>
        /// The added <see cref="TEntityNav"/>.
        /// </returns>
        IEnumerable<TEntityNav> AddSome(IEnumerable<TEntityNav> newEntityNavs);

        /// <summary>
        /// Finds an existing row in the DB for the supplied entity navigation and maps the values of the properties into that retrieved entity.
        /// If no row exists then null is returned.
        /// </summary>
        /// <param name="entityNav">
        /// The entity.
        /// </param>
        /// <returns>
        /// The updated <see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Update(TEntityNav entityNav);

        /// <summary>
        /// If the entity nav exists then it is saved to its existing record
        /// and the update returned. If update returned null then the entity
        /// is added and the added entity is returned.
        /// </summary>
        /// <param name="entityNav">
        /// The entity nav to update or save.
        /// </param>
        /// <param name="saveChangeType">
        /// Did the operation result is a save or update.
        /// </param>
        /// <returns>
        /// The updated or added entity.<see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Save(TEntityNav entityNav, out SaveChangeType saveChangeType);

        /// <summary>
        /// If the entity nav exists then it is saved to its existing record
        /// and the update returned. If update returned null then the entity
        /// is added and the added entity is returned.
        /// </summary>
        /// <param name="entityNav">
        /// The entity nav to update or save.
        /// </param>
        /// <returns>
        /// The updated or added entity.<see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Save(TEntityNav entityNav);

        /// <summary>
        /// If the entity is new then it is added else the existing entity is updated
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Load(TEntity entity);

        /// <summary>
        /// If the entity is new then it is added else the existing entity is updated
        /// </summary>
        /// <param name="entityNav">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Load(TEntityNav entityNav);

        /// <summary>
        /// If the entity is new then it is added else the existing entity is updated
        /// </summary>
        /// <param name="entityPrp">
        /// The entity properties.
        /// </param>
        /// <returns>
        /// The <see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Load(TEntityPrp entityPrp);

        /// <summary>
        /// Maps the property values of the entity into the entity navigation. 
        /// If mapping can't occur then null is returned.  
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="entityNav">
        /// The entity navigation.
        /// </param>
        /// <returns>
        /// The mapped <see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Map(TEntity entity, TEntityNav entityNav);

        /// <summary>
        /// Maps the property values of the entity properties into the entity navigation. 
        /// If mapping can't occur then null is returned.  
        /// </summary>
        /// <param name="entityPrp">
        /// The entity properties.
        /// </param>
        /// <param name="entityNav">
        /// The entity navigation.
        /// </param>
        /// <returns>
        /// The mapped <see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Map(TEntityPrp entityPrp, TEntityNav entityNav);

        /// <summary>
        /// Maps the values of the entity into a new entity navigation
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The new <see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Map(TEntity entity);

        /// <summary>
        /// Maps the values of the entity into a new entity property
        /// </summary>
        /// <param name="entityPrp">
        /// The entity properties.
        /// </param>
        /// <returns>
        /// The new <see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Map(TEntityPrp entityPrp);

        /// <summary>
        /// Maps the values of the entity into a new entity property.
        /// If the entity is null then null will be returned.
        /// </summary>
        /// <param name="entityKey">
        /// The entity key.
        /// </param>
        /// <returns>
        /// The new <see cref="TEntityNav"/>.
        /// </returns>
        TEntityNav Map(TEntityKey entityKey);

        /// <summary>
        /// Replaces the existing entity in the DB with the supplied entity. Based on the parameters 
        /// the behavior of the replacement can be effected.
        /// </summary>
        /// <param name="entity">
        /// The entity to add or update into the DB
        /// </param>
        /// <param name="existingKey">
        /// The key of the existing entity that should be replaced.
        /// </param>
        /// <param name="deleteExisting">
        /// If there exists an entity with the "existingKey" then it will be deleted.
        /// </param>
        /// <param name="keysMustMatch">
        /// Should the entity key and existingKey match. If not the operation will not take place. This
        /// ensure that you only update the existing entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntityNav"/> or null.
        /// </returns>
        TEntityNav Replace(TEntity entity, TEntityKey existingKey, bool deleteExisting, bool keysMustMatch);

        /// <summary>
        /// Deletes the entity for the specified entity key.
        /// </summary>
        /// <param name="entityKey">
        /// The entity key.
        /// </param>
        /// <returns>
        /// Returns true if the item was found and thus deleted.
        /// </returns>
        bool DeleteByKey(TEntityKey entityKey);

        /// <summary>
        /// Deletes the entities specified by the entity keys.
        /// </summary>
        /// <param name="entityKeys">
        /// The entity keys.
        /// </param>
        /// <returns>
        /// Returns true if the items were found and thus deleted.
        /// </returns>
        bool DeleteSomeByKey(IEnumerable<TEntityKey> entityKeys);

        /// <summary>
        /// If this entity for this facade has a many-to-many relationship with another entity and the
        /// middle table has no extraneous fields then this will allow you to add, update or delete the
        /// connecting rows.
        /// </summary>
        /// <param name="otherEntityNavs">
        /// The navigation property from this entity that defines the collection of the other entity nav.
        /// </param>
        /// <param name="setEntityKeys">
        /// The set of entity keys that represents the other entities that should be associated with this entity.
        /// </param>
        /// <param name="facade">
        /// The facade that manages the CRUD operations of the other entity. If null then a default facade will be automatically
        /// generated.
        /// </param>
        /// <param name="clearAll">
        /// If true then all current items in the collection will first be cleared. This is true by default.
        /// Setting to true if you want to ensure that list is completely refreshed against your input.
        /// </param>
        /// <typeparam name="TOtherSearch">
        /// The search class to provide parameters to the GetSome method
        /// </typeparam>
        /// <typeparam name="TOtherEntityNav">
        /// The class that contains the entity navigation properties
        /// </typeparam>
        /// <typeparam name="TOtherEntity">
        /// The class that contains the entity whole entity (but not navigation properties)
        /// </typeparam>
        /// <typeparam name="TOtherEntityPrp">
        /// The class that contains the entity properties (that are not primary or foreign key properties)
        /// </typeparam>
        /// <typeparam name="TOtherEntityKey">
        /// The key of the entity
        /// </typeparam>
        /// <returns>
        /// The collection of the other entity associated with this entity that now exists.
        /// </returns>
        ICollection<TOtherEntityNav> SetMany<TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey>(
            ICollection<TOtherEntityNav> otherEntityNavs,
            List<TOtherEntityKey> setEntityKeys,
            IFacade<TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey> facade = null, 
            bool clearAll = true)
            where TOtherSearch : class, ISearch, new()
            where TOtherEntityNav : class, TOtherEntity, new()
            where TOtherEntity : class, TOtherEntityPrp, new()
            where TOtherEntityPrp : class, TOtherEntityKey, new()
            where TOtherEntityKey : class, IEntityKey<TOtherEntityKey>, new();

        /// <summary>
        /// If this entity for this facade has a many-to-many relationship with another entity and the
        /// middle table has no extraneous fields then this will allow you to add, update or delete the
        /// connecting rows.  
        /// </summary>
        /// <param name="otherEntityNavs">
        /// The navigation property from this entity that defines the collection of the other entity nav.
        /// </param>
        /// <param name="setOtherEntityNavs">
        /// The set of entity nav that should be associated with this entity. These navs should be new or
        /// fully connected into the context.
        /// </param>
        /// <param name="clearAll">
        /// If true then all current items in the collection will first be cleared. This is true by default.
        /// Setting to true if you want to ensure that list is completely refreshed against your input.
        /// </param>
        /// <typeparam name="TOtherSearch">
        /// The search class to provide parameters to the GetSome method
        /// </typeparam>
        /// <typeparam name="TOtherEntityNav">
        /// The class that contains the entity navigation properties
        /// </typeparam>
        /// <typeparam name="TOtherEntity">
        /// The class that contains the entity whole entity (but not navigation properties)
        /// </typeparam>
        /// <typeparam name="TOtherEntityPrp">
        /// The class that contains the entity properties (that are not primary or foreign key properties)
        /// </typeparam>
        /// <typeparam name="TOtherEntityKey">
        /// The key of the entity
        /// </typeparam>
        /// <returns>
        /// The collection of the other entity associated with this entity that now exists.
        /// </returns>
        ICollection<TOtherEntityNav> SetMany<TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey>(
            ICollection<TOtherEntityNav> otherEntityNavs,
            List<TOtherEntityNav> setOtherEntityNavs,
            bool clearAll = true)
            where TOtherSearch : class, ISearch, new()
            where TOtherEntityNav : class, TOtherEntity, new()
            where TOtherEntity : class, TOtherEntityPrp, new()
            where TOtherEntityPrp : class, TOtherEntityKey, new()
            where TOtherEntityKey : class, IEntityKey<TOtherEntityKey>, new();

        /// <summary>
        /// If this entity for this facade has a many-to-many relationship with another entity and the
        /// middle table has no extraneous fields then this will allow you to clear all associations.
        /// </summary>
        /// <param name="otherEntityNavs">
        /// The navigation property from this entity that defines the collection of the other entity nav.
        /// </param>
        /// <param name="facade">
        /// The facade that manages the CRUD operations of the other entity. If null then a default facade will be automatically
        /// generated.
        /// </param>
        /// <typeparam name="TOtherSearch">
        /// The search class to provide parameters to the GetSome method
        /// </typeparam>
        /// <typeparam name="TOtherEntityNav">
        /// The class that contains the entity navigation properties
        /// </typeparam>
        /// <typeparam name="TOtherEntity">
        /// The class that contains the entity whole entity (but not navigation properties)
        /// </typeparam>
        /// <typeparam name="TOtherEntityPrp">
        /// The class that contains the entity properties (that are not primary or foreign key properties)
        /// </typeparam>
        /// <typeparam name="TOtherEntityKey">
        /// The key of the entity
        /// </typeparam>
        /// <returns>
        /// The collection of the other entity associated with this entity that now exists.
        /// </returns>
        ICollection<TOtherEntityNav> ClearMany<TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey>(
            ICollection<TOtherEntityNav> otherEntityNavs,
            IFacade<TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey> facade = null)
            where TOtherSearch : class, ISearch, new()
            where TOtherEntityNav : class, TOtherEntity, new()
            where TOtherEntity : class, TOtherEntityPrp, new()
            where TOtherEntityPrp : class, TOtherEntityKey, new()
            where TOtherEntityKey : class, IEntityKey<TOtherEntityKey>, new();

        /// <summary>
        /// Validates the entity and returns any issues to the response.
        /// </summary>
        /// <param name="entityNav">
        /// The entity navigation to validate.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// Return true if there were no error level messages.
        /// </returns>
        bool ValidateEntity(TEntityNav entityNav, IOperationResponse response);
        
        /// <summary>
        /// Can the user perform the given action. The parameters of the argument provide a
        /// context to what is being attempted. The entities of the argument must all be fully connected. The response
        /// must not be null and must not be in error. I.e. the state of the change must be valid.
        /// loaded items.
        /// </summary>
        /// <param name="argument">
        /// The argument.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// True if the the operation is allowed.
        /// </returns>
        bool CanUser(IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> argument, IOperationResponse response);

        /// <summary>
        /// Create CanUserArg instances
        /// </summary>
        /// <returns>An instance that implements IFacadeCanUserArg</returns>
        IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> CanUserArgFactory();

        /// <summary>
        /// Map and load entities of the CanUserArg with the supplied entity key
        /// </summary>
        /// <param name="canUserArg">The IFacadeCanUserArg instance to update</param>
        /// <param name="entityKey">The entity key to load from</param>
        void LoadCanUserArgData(IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> canUserArg, TEntityKey entityKey);

        /// <summary>
        /// Map and load entities of the CanUserArg with the supplied entity keys
        /// </summary>
        /// <param name="canUserArg">The IFacadeCanUserArg instance to update</param>
        /// <param name="entityKeys">The entity key to load from</param>
        void LoadCanUserArgData(IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> canUserArg, List<TEntityKey> entityKeys);

        /// <summary>
        /// Map and load entities of the CanUserArg with the supplied entity
        /// </summary>
        /// <param name="canUserArg">The IFacadeCanUserArg instance to update</param>
        /// <param name="entity">The entity key to load from</param>
        void LoadCanUserArgData(IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> canUserArg, TEntity entity);

        /// <summary>
        /// Map and load entities of the CanUserArg with the supplied entities
        /// </summary>
        /// <param name="canUserArg">The IFacadeCanUserArg instance to update</param>
        /// <param name="entities">The entity key to load from</param>
        void LoadCanUserArgData(IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> canUserArg, List<TEntity> entities);

        /// <summary>
        /// In the event that an operation cannot complete but no message supplied as to why. 
        /// </summary>
        /// <param name="response">
        /// The operation response
        /// </param>
        /// <returns>
        /// The <see cref="IOperationResponse"/>.
        /// </returns>
        IOperationResponse GuardFault(IOperationResponse response);

        /// <summary>
        /// In the event that an CanUser is passed a null response or response in error. 
        /// </summary>
        /// <param name="response">
        /// The operation response
        /// </param>
        /// <returns>
        /// Returns true if guarding does not result in an error.
        /// </returns>
        bool GuardCanUser(IOperationResponse response);

        /// <summary>
        /// Convert a IBusinessCanUserArg to a IFacadeCanUserArg
        /// </summary>
        /// <param name="businessCanUserArg">
        /// The instance of IBusinessCanUserArg to convert to an IFacadeCanUserArg using the CanUserArgFactory</param>
        /// <returns>
        /// The generated instance implementing IFacadeCanUserArg
        /// </returns>
        IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> 
            ConvertToFacadeCanUserArg(IBusinessCanUserArg<TSearch, TEntity, TEntityPrp, TEntityKey> businessCanUserArg);
    }
}