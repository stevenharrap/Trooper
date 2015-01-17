//--------------------------------------------------------------------------------------
// <copyright file="Facade.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using AutoMapper;
    using AutoMapper.Mappers;

    using Microsoft.Practices.EnterpriseLibrary.Validation;

    using Trooper.ActiveDirectory;
    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.DataManager;
    using Trooper.BusinessOperation.Interface;
    using Trooper.BusinessOperation.Response;
    using Trooper.BusinessOperation.Utility;

    /// <summary>
    /// <para>
    /// Provides the CRUD and associated operations for getting data in and out of your model.
    /// It is assumed that this will always be instantiated and used from within a "using DBContext" scope. 
    /// </para>
    /// <para>
    /// The BusinessOperation project provides a default implementation of this interface which
    /// you can extend and override where you see fit to provide specific behavior. 
    /// </para>
    /// </summary>
    /// <typeparam name="TDbContext">
    /// The DbContext from the model that this facade is managing.
    /// </typeparam>
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
    public class Facade<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> :
        IFacade<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
        where TDbContext : DbContext, new()
        where TSearch : class, ISearch, new()
        where TEntityNav : class, TEntity, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// The active directory user.
        /// </summary>
        private ActiveDirectoryUser adUser;

        /// <summary>
        /// The username.
        /// </summary>
        private string username;

        /// <summary>
        /// Initializes a new instance of the <see cref="Facade{TDbContext,TSearch,TEntityNav,TEntity,TEntityPrp,TEntityKey}"/> class.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        public Facade(string username)
        {
            this.username = username;

            // ReSharper disable UnusedVariable
            // This ensures that AutoMapper is included in your business class and UI.
            var useless = new ListSourceMapper();
            // ReSharper restore UnusedVariable
        }

        /// <summary>
        /// Gets or sets the default error message to supply when nothing else is available
        /// </summary>
        /// <returns></returns>
        public string DefaultErrorMessage
        {
            get
            {
                return "The operation could not be completed.";
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the username of the current user.
        /// </summary>
        public string Username
        {
            get
            {
                if (string.IsNullOrEmpty(this.username))
                {
                    this.username = this.AdUser.UserName;
                }

                return this.username;
            }

            set
            {
                this.username = value;
                this.adUser = null;
            }
        }

        /// <summary>
        /// Gets or sets the access list that defines which groups and users have access to which operations.
        /// This may be ignored if you override the CanUser method with your own code.
        /// </summary>
        public virtual List<AdRole> AccessList { get; set; }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        public TDbContext DbContext
        {
            get
            {
                if (this.DbContextManager == null)
                {
                    this.InstantiateDbContextManager();
                }

                return this.DbContextManager != null ? this.DbContextManager.GetDbContext<TDbContext>() : null;
            }
        }

        /// <summary>
        /// Gets the object context from the DB context.
        /// This is useful for some of the more obscure EF functionality
        /// </summary>
        public ObjectContext ObjectContext
        {
            get
            {
                return (this.DbContext as IObjectContextAdapter).ObjectContext;
            }
        }

        /// <summary>
        /// Gets the active directory user. This is cached so you can use it multiple times with out much impact on performance.
        /// </summary>
        protected virtual ActiveDirectoryUser AdUser
        {
            get
            {
                return this.adUser
                       ?? (this.adUser =
                           string.IsNullOrEmpty(this.username) ? new ActiveDirectoryUser() : new ActiveDirectoryUser(this.username));
            }
        }

        /// <summary>
        /// Gets or sets the DbContextManager.
        /// </summary>
        /// <value>The DbContextManager.</value>
        private DbContextManager DbContextManager { get; set; }

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
        public virtual bool IsAddDataAction(string action)
        {
            return action == UserAction.AddAction || action == UserAction.AddSomeAction;
        }

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
        public virtual bool IsUpdateDataAction(string action)
        {
            return action == UserAction.UpdateAction;
        }

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
        public virtual bool IsRemoveDataAction(string action)
        {
            return action == UserAction.DeleteByKeyAction
                || action == UserAction.DeleteSomeByKeyAction;
        }
        
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
        public virtual bool IsChangeAction(string action)
        {
            return this.IsAddDataAction(action) 
                || this.IsUpdateDataAction(action) 
                || this.IsRemoveDataAction(action);
        }

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
        public virtual bool IsReadAction(string action)
        {
            return action == UserAction.GetAllAction
                || action == UserAction.GetSomeAction
                || action == UserAction.GetByKeyAction
                || action == UserAction.ExistsByKeyAction;
        }

        /// <summary>
        /// Returns the actions string appropriate for the supplied SaveChangeType
        /// </summary>
        /// <param name="saveChangeType">
        /// The save change type.
        /// </param>
        /// <returns>
        /// The appropriate <see cref="string"/> action string.
        /// </returns>
        public string GetChangeTypeAction(SaveChangeType saveChangeType)
        {
            switch (saveChangeType)
            {
                case SaveChangeType.Add:
                    return UserAction.AddAction;

                case SaveChangeType.Update:
                    return UserAction.UpdateAction;

                case SaveChangeType.None:
                    return string.Empty;

                default:
                    return string.Empty;
            }
        }
        
        /// <summary>
        /// Provides the means of getting some of the rows from your table based on the properties
        /// of the search object and how you implement GetSome.
        /// Without specific overriding in your class this will provide the same result as the
        /// GetAll method limited to the MaxItems value of the searchObject. 
        /// <para>
        /// If you override this method then your GetSome should NOT call base.GetSome(search).
        /// To apply the record limitation call 'Limit' on your record set after your implementation
        /// has performed its filtering.
        /// </para> 
        /// </summary> 
        /// <param name="search">
        /// The search object to provide guidance on what to return.
        /// </param>
        /// <returns>
        /// A list of TEntity
        /// </returns>
        public virtual IEnumerable<TEntityNav> GetSome(TSearch search)
        {
            return this.Limit(this.GetAll(), search);
        }

        /// <summary>
        /// Limits the number of returned items in the record set based on the Skip and Take
        /// values in the search.
        /// </summary>
        /// <param name="entityNavs">
        /// The entityNavs.
        /// </param>
        /// <param name="search">
        /// The search.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<TEntityNav> Limit(IEnumerable<TEntityNav> entityNavs, TSearch search)
        {
            if (search.SkipItems > 0)
            {
                entityNavs = entityNavs.Skip(search.SkipItems);
            }

            return search.TakeItems > 0 ? entityNavs.Take(search.TakeItems) : entityNavs;
        }

        /// <summary>
        /// Gets all entities from your table. Overriding this method to implement partitioning (e.g. never getting
        /// items with a "Deleted" property being true is useful here. This scenario will also ensure that GetSome
        /// observes the partitioning as GetSome uses GetAll as its source.
        /// <para>
        /// Note on expanding result set to connected entities:
        /// Your record may have detail records and it may be efficient to ensure that they are included in then
        /// result set. This means that the SQL will be a larger join but you will have all the data from one
        /// query rather than many.
        /// <code>
        /// <![CDATA[
        ///     using System.Linq.Data;
        /// 
        ///     var data = /* from d in this.DbContext.YourTable */;
        ///     return data.Include(t => t.DetailTableNav)
        /// ]]>
        /// </code>
        /// </para>
        /// </summary>
        /// <returns>Set of TEntity.</returns>
        public virtual IEnumerable<TEntityNav> GetAll()
        {
            return this.DbContext.Set<TEntityNav>();
        }

        /// <summary>
        /// Gets the entity by the specified entity key.
        /// </summary>
        /// <param name="entityKey">
        /// The entity key.
        /// </param>
        /// <returns>
        /// Entity matching specified entity key or null if not found.
        /// </returns>
        public virtual TEntityNav GetByKey(TEntityKey entityKey)
        {
            var oc = this.ObjectContext;
            var os = oc.CreateObjectSet<TEntityNav>();
            var es = os.EntitySet;
            var entitySetName = oc.DefaultContainerName + "." + es.Name;

            var keyPairs = (from km in es.ElementType.KeyMembers
                            let value = entityKey.GetType().GetProperty(km.Name).GetValue(entityKey)
                            select new KeyValuePair<string, object>(km.Name, value)).ToList();

            var nullKey = keyPairs.FirstOrDefault(kp => kp.Key == null || kp.Value == null);

            if (!nullKey.Equals(default(KeyValuePair<string, object>)))
            {
                return null;
            }

            var key = new EntityKey(entitySetName, keyPairs);
            object result;

            if (oc.TryGetObjectByKey(key, out result))
            {
                return result as TEntityNav;
            }

            return null;
        }

        /// <summary>
        /// Does a record exist in the table for the given entity key. This uses GetByKey to determine
        /// existence and is not overridable.
        /// </summary>
        /// <param name="entityKey">
        /// The entity key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Exists(TEntityKey entityKey)
        {
            return this.GetByKey(entityKey) != null;
        }

        /// <summary>
        /// <para>
        /// 1. If the entity key is null then false is returned. Its a null key which is not set.
        /// 2. If the entity key appears to represent an existing record but no record is retrievable and
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
        public bool NotExistsAndNotNew(bool allowNullKey, TEntityKey entityKey, TEntityNav entityNav)
        {
            //// If the key is null then we assume that its a null key 
            //// and that record does not need to exist. We return false
            //// because the record does not need to exist.
            if (allowNullKey && entityKey == null)
            {
                return false;
            }

            if (!allowNullKey && entityKey == null)
            {
                return true;
            }

            //// If the key really does represent an existing record then all is good.
            if (this.Exists(entityKey))
            {
                return false;
            }

            //// We now need the key to support IsKeyAuto to continue
            if (!entityKey.IsKeyAuto())
            {
                throw new Exception(Constants.NotAutoKeyMsg);
            }

            //// It's a new key and thus a new record
            if (entityKey.IsEntityNew())
            {
                return false;
            } 

            //// We can only check the nav now and if its not null we'll return true to be safe if it is.
            if (entityNav == null)
            {
                return true;
            }

            //// If the entity really represents an existing record then all is good.
            if (this.Exists(entityNav))
            {
                return false;
            }

            //// If the state of nav is added all good.
            if (this.ObjectContext.ObjectStateManager.GetObjectStateEntry(entityNav).State == EntityState.Added)
            {
                return false;
            }

            //// Not sure now so lets be safe.
            return true;
        }

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
        public bool NotExistsAndNotNew(TEntityKey entityKey, TEntityNav entityNav, ValidationResults validationResults, string propertyName = null)
        {
            if (this.NotExistsAndNotNew(false, entityKey, entityNav))
            {
                validationResults.AddResult(
                    new ValidationResult(
                        string.Format("The {0} must belong to a record.", propertyName ?? "value"),
                        entityNav,
                        propertyName,
                        null,
                        null));

                return true;
            }

            return false;
        }

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
        public bool NullableNotExistsAndNotNew(TEntityKey entityKey, TEntityNav entityNav, ValidationResults validationResults, string propertyName = null)
        {
            if (this.NotExistsAndNotNew(true, entityKey, entityNav))
            {
                validationResults.AddResult(
                    new ValidationResult(
                        string.Format("The {0} must belong to a record.", propertyName ?? "value"),
                        entityNav,
                        propertyName,
                        null,
                        null));

                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Adds a new entity object to the context.
        /// </summary>
        /// <param name="newEntityNav">
        /// An entity that has not yet been added to the context.
        /// </param>
        /// <returns>
        /// The added <see cref="TEntityNav"/>.
        /// </returns>
        public virtual TEntityNav Add(TEntityNav newEntityNav)
        {
            if (newEntityNav == null)
            {
                return null;
            }

            return this.DbContext.Set<TEntityNav>().Add(newEntityNav);
        }

        /// <summary>
        /// Adds new entity objects to the context.
        /// </summary>
        /// <param name="newEntityNavs">
        /// Entities that have not yet been added to the context.
        /// </param>
        /// <returns>
        /// The added <see cref="TEntityNav"/>.
        /// </returns>
        public virtual IEnumerable<TEntityNav> AddSome(IEnumerable<TEntityNav> newEntityNavs)
        {
            if (newEntityNavs == null)
            {
                return null;
            }

            return this.DbContext.Set<TEntityNav>().AddRange(newEntityNavs);
        }

        /// <summary>
        /// Gets the original value of the property on the entity before any of your code changed it.
        /// If you do GetByKey twice your second reference will be to the first retrieval. So using that to 
        /// see if a property has changed will be misleading. 
        /// </summary>
        /// <param name="entityNav">
        /// The entity nav to find the original value of.
        /// </param>
        /// <param name="propertyExpression">
        /// The property expression. I.e "p => p.YourEntityProperty"
        /// </param>
        /// <typeparam name="TPropertyType">
        /// The type of the property
        /// </typeparam>
        /// <returns>
        /// The <see cref="TPropertyType"/>.
        /// Returns the value property
        /// </returns>
        public TPropertyType GetOriginalValue<TPropertyType>(TEntityNav entityNav, Expression<Func<TEntityNav, TPropertyType>> propertyExpression)
        {
            var entry = this.DbContext.Entry(entityNav);

            return entry.State == EntityState.Added 
                ? default(TPropertyType) 
                : entry.Property(propertyExpression).OriginalValue;
        }

        /// <summary>
        /// <para>
        /// Finds an existing row in the DB for the supplied entity navigation and maps the values of the properties into that retrieved entity.
        /// If no row exists then null is returned.
        /// </para>
        /// <para>
        /// If you are overriding this method then your code should map the entity to an entityNav and hand the entityNav to the override.
        /// </para> 
        /// </summary>
        /// <param name="entityNav">
        /// The entity.
        /// </param>
        /// <returns>
        /// The updated <see cref="TEntityNav"/>.
        /// </returns>
        public virtual TEntityNav Update(TEntityNav entityNav)
        {
            return this.Map(entityNav, this.GetByKey(entityNav));
        }

        /// <summary>
        /// If the entity nav exists then it is saved to its existing record
        /// and the update returned. If update returned null then the entity
        /// is added and the added entity is returned. This method cannot be
        /// overriden because it uses the Add and Update facade methods.
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
        public TEntityNav Save(TEntityNav entityNav, out SaveChangeType saveChangeType)
        {
            if (entityNav == null)
            {
                saveChangeType = SaveChangeType.None;
                return null;
            }

            var updated = this.Update(entityNav);

            if (updated != null)
            {
                saveChangeType = SaveChangeType.Update;

                return updated;
            }

            saveChangeType = SaveChangeType.Add;

            return this.Add(entityNav);
        }

        /// <summary>
        /// If the entity nav exists then it is saved to its existing record
        /// and the update returned. If update returned null then the entity
        /// is added and the added entity is returned. This method cannot be
        /// overriden because it uses the Add and Update facade methods.
        /// </summary>
        /// <param name="entityNav">
        /// The entity nav to update or save.
        /// </param>
        /// <returns>
        /// The updated or added entity.<see cref="TEntityNav"/>.
        /// </returns>
        public TEntityNav Save(TEntityNav entityNav)
        {
            SaveChangeType saveChangeType;

            return this.Save(entityNav, out saveChangeType);
        }

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
        public TEntityNav Map(TEntity entity, TEntityNav entityNav)
        {
            if (entity == null || entityNav == null)
            {
                return null;
            }

            Mapper.CreateMap<TEntity, TEntityNav>();

            Mapper.Map(entity, entityNav);

            return entityNav;
        }

        /// <summary>
        /// Maps the values of the entity into a new entity navigation
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The new <see cref="TEntityNav"/>.
        /// </returns>
        public TEntityNav Map(TEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var entityNav = new TEntityNav();

            Mapper.CreateMap<TEntity, TEntityNav>();

            Mapper.Map(entity, entityNav);

            return entityNav;
        }

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
        public TEntityNav Map(TEntityPrp entityPrp, TEntityNav entityNav)
        {
            if (entityPrp == null || entityNav == null)
            {
                return null;
            }

            Mapper.CreateMap<TEntityPrp, TEntityNav>();

            Mapper.Map(entityPrp, entityNav);

            return entityNav;
        }

        /// <summary>
        /// Maps the values of the entity into a new entity property.
        /// If the entity is null then null will be returned.
        /// </summary>
        /// <param name="entityPrp">
        /// The entity properties.
        /// </param>
        /// <returns>
        /// The new <see cref="TEntityNav"/>.
        /// </returns>
        public TEntityNav Map(TEntityPrp entityPrp)
        {
            if (entityPrp == null)
            {
                return null;
            }

            var entityNav = new TEntityNav();

            Mapper.CreateMap<TEntityPrp, TEntityNav>();

            Mapper.Map(entityPrp, entityNav);

            return entityNav;
        }

        /// <summary>
        /// Maps the property values of the entity key into the entity navigation. 
        /// If mapping can't occur then null is returned.  
        /// </summary>
        /// <param name="entityKey">
        /// The entity key.
        /// </param>
        /// <param name="entityNav">
        /// The entity navigation.
        /// </param>
        /// <returns>
        /// The mapped <see cref="TEntityNav"/>.
        /// </returns>
        public TEntityNav Map(TEntityKey entityKey, TEntityNav entityNav)
        {
            if (entityKey == null || entityNav == null)
            {
                return null;
            }

            Mapper.CreateMap<TEntityKey, TEntityNav>();

            Mapper.Map(entityKey, entityNav);

            return entityNav;
        }

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
        public TEntityNav Map(TEntityKey entityKey)
        {
            if (entityKey == null)
            {
                return null;
            }

            var entityNav = new TEntityNav();

            Mapper.CreateMap<TEntityKey, TEntityNav>();

            Mapper.Map(entityKey, entityNav);

            return entityNav;
        }

        /// <summary>
        /// <para>
        /// If the entity is new then it is added else the existing is retrieved.
        /// If the entity is null then null will be returned.
        /// </para>
        /// <para>
        /// If the entity already exists then it is retrieved, the values from the supplied
        /// entity are copied into the retrieved item and then retrieved item is returned.
        /// </para>
        /// <para>
        /// Update is NOT called if the entity exists. If the entity does not have an auto
        /// key then this will throw an exception.
        /// </para>
        /// </summary>
        /// <param name="entityNav">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntityNav"/>.
        /// </returns>
        public TEntityNav Load(TEntityNav entityNav)
        {
            if (entityNav == null)
            {
                return null;
            }

            if (!entityNav.IsKeyAuto())
            {
                throw new Exception(Constants.NotAutoKeyMsg);
            }

            if (entityNav.IsEntityNew())
            {
                return this.Add(this.Map(entityNav));
            }

            var existing = this.GetByKey(entityNav);

            return this.Map(entityNav, existing);
        }

        /// <summary>
        /// <para>
        /// If the entity is new then it is added else the existing is retrieved.
        /// If the entity is null then null will be returned.
        /// </para>
        /// <para>
        /// If the entity already exists then it is retrieved, the values from the supplied
        /// entity are copied into the retrieved item and then retrieved item is returned.
        /// </para>
        /// <para>
        /// Update is NOT called if the entity exists. If the entity does not have an auto
        /// key then this will throw an exception.
        /// </para>
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntityNav"/>.
        /// </returns>
        public TEntityNav Load(TEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            if (!entity.IsKeyAuto())
            {
                throw new Exception(Constants.NotAutoKeyMsg);
            }

            if (entity.IsEntityNew())
            {
                return this.Add(this.Map(entity));
            }

            var entityNav = this.GetByKey(entity);

            return this.Map(entity, entityNav);
        }

        /// <summary>
        /// <para>
        /// If the entity is new then it is added else the existing is retrieved.
        /// If the entity is null then null will be returned.
        /// </para>
        /// <para>
        /// If the entity already exists then it is retrieved, the values from the supplied
        /// entity are copied into the retrieved item and then retrieved item is returned.
        /// </para>
        /// <para>
        /// Update is NOT called if the entity exists. If the entity does not have an auto
        /// key then this will throw an exception.
        /// </para>
        /// </summary>
        /// <param name="entityPrp">
        /// The entity properties.
        /// </param>
        /// <returns>
        /// The <see cref="TEntityNav"/>.
        /// </returns>
        public TEntityNav Load(TEntityPrp entityPrp)
        {
            if (entityPrp == null)
            {
                return null;
            }

            if (!entityPrp.IsKeyAuto())
            {
                throw new Exception(Constants.NotAutoKeyMsg);
            }

            if (entityPrp.IsEntityNew())
            {
                return this.Add(this.Map(entityPrp));
            }

            var entityNav = this.GetByKey(entityPrp);

            return this.Map(entityPrp, entityNav);
        }

        /// <summary>
        /// Replaces the existing entity in the DB with the supplied entity. Based on the parameters 
        /// the behavior of the replacement can be effected. Using this method with entities that
        /// do not have Auto Keys will cause an exception!
        /// </summary>
        /// <param name="entity">
        /// The entity to add or update into the DB
        /// </param>
        /// <param name="existingKey">
        /// The key of the existing entity that should be replaced.
        /// </param>
        /// <param name="deleteExisting">
        /// If there exists an entity with the "existingKey" and it does not equal the entity then it will be deleted. This is ignored
        /// if keysMustMatch is true - in this case you are just updating.
        /// </param>
        /// <param name="keysMustMatch">
        /// Should the entity key and existingKey match. If not the operation will not take place. This
        /// ensure that you only update the existing entity.
        /// </param>
        /// <returns>
        /// The <see cref="TEntityNav"/> or null.
        /// </returns>
        public virtual TEntityNav Replace(TEntity entity, TEntityKey existingKey, bool deleteExisting, bool keysMustMatch)
        {
            if (entity == null)
            {
                return null;
            }

            if (!entity.IsKeyAuto())
            {
                throw new Exception(Constants.NotAutoKeyMsg);
            }

            if (entity.IsEntityNew())
            {
                var entityNav = this.Map(entity);

                if (entityNav == null)
                {
                    return null;
                }

                return this.Add(entityNav);
            }

            if (existingKey != null && keysMustMatch && !entity.Equals(existingKey))
            {
                return null;
            }

            if (existingKey != null && deleteExisting && !existingKey.IsEntityNew() && !existingKey.Equals(entity))
            {
                this.DeleteByKey(existingKey);
            }

            return this.Update(this.Map(entity));
        }

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
        public ICollection<TOtherEntityNav> SetMany<TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey>(
            ICollection<TOtherEntityNav> otherEntityNavs,
            List<TOtherEntityKey> setEntityKeys,
            IFacade<TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey> facade = null,
            bool clearAll = true)
            where TOtherSearch : class, ISearch, new()
            where TOtherEntityNav : class, TOtherEntity, new()
            where TOtherEntity : class, TOtherEntityPrp, new()
            where TOtherEntityPrp : class, TOtherEntityKey, new()
            where TOtherEntityKey : class, IEntityKey<TOtherEntityKey>, new()
        {
            if (setEntityKeys != null)
            {
                if (facade == null)
                {
                    facade = new Facade<TDbContext, TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey>(this.Username);
                }
                
                if (otherEntityNavs == null)
                {
                    otherEntityNavs = new Collection<TOtherEntityNav>();
                }
                else
                {
                    if (clearAll)
                    {
                        otherEntityNavs.Clear();
                    }
                }

                foreach (var k in setEntityKeys)
                {
                    var nav = facade.GetByKey(k);

                    if (clearAll)
                    {
                        otherEntityNavs.Add(nav);
                    }
                    else if (!otherEntityNavs.Any(e => e.Equals(k)))
                    {
                        otherEntityNavs.Add(nav);
                    }
                }
            }

            return otherEntityNavs;
        }

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
        public ICollection<TOtherEntityNav> SetMany<TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey>(
            ICollection<TOtherEntityNav> otherEntityNavs,
            List<TOtherEntityNav> setOtherEntityNavs,
            bool clearAll = true)
            where TOtherSearch : class, ISearch, new()
            where TOtherEntityNav : class, TOtherEntity, new()
            where TOtherEntity : class, TOtherEntityPrp, new()
            where TOtherEntityPrp : class, TOtherEntityKey, new()
            where TOtherEntityKey : class, IEntityKey<TOtherEntityKey>, new()
        {
            if (setOtherEntityNavs == null)
            {
                return otherEntityNavs;
            }

            if (otherEntityNavs == null)
            {
                otherEntityNavs = new Collection<TOtherEntityNav>();
            }
            else
            {
                if (clearAll)
                {
                    otherEntityNavs.Clear();
                }
            }

            foreach (var nav in setOtherEntityNavs)
            {
                if (clearAll)
                {
                    otherEntityNavs.Add(nav);
                }
                else if (!otherEntityNavs.Any(e => e.Equals(nav)))
                {
                    otherEntityNavs.Add(nav);
                }
            }

            return otherEntityNavs;
        }

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
        public ICollection<TOtherEntityNav> ClearMany<TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey>(
            ICollection<TOtherEntityNav> otherEntityNavs,
            IFacade<TOtherSearch, TOtherEntityNav, TOtherEntity, TOtherEntityPrp, TOtherEntityKey> facade = null)
            where TOtherSearch : class, ISearch, new()
            where TOtherEntityNav : class, TOtherEntity, new()
            where TOtherEntity : class, TOtherEntityPrp, new()
            where TOtherEntityPrp : class, TOtherEntityKey, new()
            where TOtherEntityKey : class, IEntityKey<TOtherEntityKey>, new()
        {
            if (otherEntityNavs == null)
            {
                otherEntityNavs = new Collection<TOtherEntityNav>();
            }
            else
            {
                otherEntityNavs.Clear();
            }

            return otherEntityNavs;
        }

        /// <summary>
        /// Deletes the entity for the specified entity key.
        /// You will need to ensure any child objects are deleted first.
        /// </summary>
        /// <param name="entityKey">
        /// The entity key.
        /// </param>
        /// <returns>
        /// Returns true if the item was found and thus deleted.
        /// </returns>
        public virtual bool DeleteByKey(TEntityKey entityKey)
        {
            if (entityKey == null)
            {
                return false;
            }

            var entity = this.GetByKey(entityKey);

            if (entity != null)
            {
                this.DbContext.Set<TEntityNav>().Remove(entity);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes the entities specified by the entity keys.
        /// </summary>
        /// <param name="entityKeys">
        /// The entity keys.
        /// </param>
        /// <returns>
        /// Returns true if the items were found and thus deleted.
        /// </returns>
        public virtual bool DeleteSomeByKey(IEnumerable<TEntityKey> entityKeys)
        {
            if (entityKeys == null)
            {
                return false;
            }

            foreach (var ek in entityKeys)
            {
                var ekn = this.Map(ek);
                var entry = this.DbContext.Entry(ekn);

                try
                {
                    entry.State = EntityState.Deleted;
                }
                catch
                {
                    var x = this.GetByKey(ek);
                    this.DbContext.Set<TEntityNav>().Remove(x);
                }
            }

            return true;
        }

        /// <summary>
        /// Validates the entity and returns any issues in the validation results.
        /// You will need to override this method if you have special rules to check against.
        /// If you are overriding this call this base implementation first as it will check against
        /// null parameters and fire appropriate exceptions.
        /// </summary>
        /// <param name="entityNav">
        /// The entity navigation to validate.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// Returns true if validation returned no error level messages.
        /// </returns>
        public virtual bool ValidateEntity(TEntityNav entityNav, IOperationResponse response)
        {
            var vr = Validation.Validate(entityNav);

            if (response == null || vr.IsValid)
            {
                return vr.IsValid;
            }

            foreach (var v in vr)
            {
                MessageUtility.Errors.Add(v.Message, v.Target, v.Key, response);
            }

            return vr.IsValid;
        }

        /// <summary>
        /// Can the user perform the given action. The search, entityKey, entity and object parameters provide a
        /// context to what is being attempted. The order of searching for a role is:
        /// 1. Look for a role that matches the the supplied action.
        /// 2. If no role found and the action is an DeleteSome or Delete then look for a "All Remove Actions" role.
        /// 3. If no role found and the action is an Add, AddSome, Update, DeleteSome or Delete then look for a "All Change Actions" role.
        /// 4. If no role found and the action is an GetAll, GetSome, GetByKey or GetSomeByKey then look for a "All Read Actions" role.
        /// 5. If no role found then look an "All Actions" role.
        /// 6. If no role can be found then it is assumed that the user is allowed to proceed.
        /// 7. If a role is found but the current user is not in that role then access is denied
        /// 8. If a role is found then access if the role.Allow is true
        /// 9. Access is denied
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
        public virtual bool CanUser(IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> argument, IOperationResponse response)
        {
            if (this.AccessList == null)
            {
                return true;
            }

            if (!this.GuardCanUser(response))
            {
                return false;
            }

            var role = this.AccessList.FirstOrDefault(ag => ag.Action == argument.Action);

            if (role == null && this.IsRemoveDataAction(argument.Action))
            {
                role = this.AccessList.FirstOrDefault(ag => ag.Action == UserAction.AllRemoveActions);
            }

            if (role == null && this.IsChangeAction(argument.Action))
            {
                role = this.AccessList.FirstOrDefault(ag => ag.Action == UserAction.AllChangeActions);
            }

            if (role == null && this.IsReadAction(argument.Action))
            {
                role = this.AccessList.FirstOrDefault(ag => ag.Action == UserAction.AllReadActions);
            }

            if (role == null)
            {
                role = this.AccessList.FirstOrDefault(ag => ag.Action == UserAction.AllActions);
            }

            //// No action found to check against so it's ok.
            if (role == null)
            {
                return true;
            }

            var hasUser = (role.Users != null && role.Users.Contains(this.AdUser.UserName))
                          || (role.UserGroups != null && role.UserGroups.Any(ug => this.AdUser.Groups.Contains(ug)));

            if (hasUser && role.Allow)
            {
                return true;
            }

            if (response != null)
            {
                MessageUtility.Errors.Add(
                string.Format(
                    "The user {0} is not allowed to perform action {1}.",
                    this.AdUser.UserName,
                    argument.Action),
                null,
                response);
            }

            return false;
        }

        /// <summary>
        /// Create CanUserArg instances. Override this if you have special CanUserTypes
        /// </summary>
        /// <returns>
        /// An instance of FacadeCanUserArg by default
        /// </returns>
        public virtual IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> CanUserArgFactory()
        {
            return new FacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>();
        }

        /// <summary>
        /// Map and load entities of the CanUserArg with the supplied entity key
        /// </summary>
        /// <param name="canUserArg">The IFacadeCanUserArg instance to update</param>
        /// <param name="entityKey">The entity key to load from</param>
        public void LoadCanUserArgData(IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> canUserArg, TEntityKey entityKey)
        {
            if (canUserArg == null || entityKey == null)
            {
                return;
            }

            canUserArg.Entity = this.GetByKey(entityKey);
        }

        /// <summary>
        /// Map and load entities of the CanUserArg with the supplied entity keys
        /// </summary>
        /// <param name="canUserArg">The IFacadeCanUserArg instance to update</param>
        /// <param name="entityKeys">The entity key to load from</param>
        public void LoadCanUserArgData(IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> canUserArg, List<TEntityKey> entityKeys)
        {
            if (canUserArg == null || entityKeys == null)
            {
                return;
            }

            var data = from k in entityKeys let e = this.GetByKey(k) where e != null select e;

            canUserArg.Entities = data.ToList();
        }

        /// <summary>
        /// Map and load entities of the CanUserArg with the supplied entity
        /// </summary>
        /// <param name="canUserArg">The IFacadeCanUserArg instance to update</param>
        /// <param name="entity">The entity key to load from</param>
        public void LoadCanUserArgData(IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> canUserArg, TEntity entity)
        {
            if (canUserArg == null || entity == null)
            {
                return;
            }

            var e = this.GetByKey(entity);

            if (e == null)
            {
                return;
            }

            Mapper.Map(entity, e);

            canUserArg.Entity = e;
        }

        /// <summary>
        /// Map and load entities of the CanUserArg with the supplied entities
        /// </summary>
        /// <param name="canUserArg">The IFacadeCanUserArg instance to update</param>
        /// <param name="entities">The entity key to load from</param>
        public void LoadCanUserArgData(IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> canUserArg, List<TEntity> entities)
        {
            if (canUserArg == null || entities == null)
            {
                return;
            }

            var data = from e in entities 
                       let n = this.GetByKey(e) 
                       where n != null 
                       let ln = Mapper.Map(e, n) 
                       select ln;

            canUserArg.Entities = data.ToList();
        }

        /// <summary>
        /// Convert a IBusinessCanUserArg to a IFacadeCanUserArg
        /// </summary>
        /// <param name="businessCanUserArg">
        /// The instance of IBusinessCanUserArg to convert to an IFacadeCanUserArg using the CanUserArgFactory</param>
        /// <returns>
        /// The generated instance implementing IFacadeCanUserArg
        /// </returns>
        public virtual IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
            ConvertToFacadeCanUserArg(IBusinessCanUserArg<TSearch, TEntity, TEntityPrp, TEntityKey> businessCanUserArg)
        {
            if (businessCanUserArg == null)
            {
                return this.CanUserArgFactory();
            }

            var argument = this.CanUserArgFactory();
            argument.Action = businessCanUserArg.Action;
            argument.Search = businessCanUserArg.Search;

            if (businessCanUserArg.Entities != null)
            {
                this.LoadCanUserArgData(argument, businessCanUserArg.Entities.ToList());
            }

            return argument;
        }

        /// <summary>
        /// In the event that an operation cannot complete but no message supplied as to why. 
        /// </summary>
        /// <param name="response">
        /// The operation response
        /// </param>
        /// <returns>
        /// The <see cref="IOperationResponse"/>.
        /// </returns>
        public IOperationResponse GuardFault(IOperationResponse response)
        {
            if (response == null)
            {
                response = new OperationResponse();
            }

            if (response.Ok)
            {
                MessageUtility.Errors.Add(this.DefaultErrorMessage, response);
            }

            return response;
        }

        /// <summary>
        /// In the event that an CanUser is passed a null response or response in error. 
        /// </summary>
        /// <param name="response">
        /// The operation response
        /// </param>
        /// <returns>
        /// Returns true if guarding does not result in an error.
        /// </returns>
        public bool GuardCanUser(IOperationResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return response.Ok;
        }

        /// <summary>
        /// Instantiates a new DbContextManager based on application configuration settings.
        /// </summary>
        private void InstantiateDbContextManager()
        {
            /* Retrieve DbContextManager configuration settings: */
            var contextManagerConfiguration = ConfigurationManager.GetSection("DbContext") as Hashtable;

            if (contextManagerConfiguration == null)
            {
                throw new ConfigurationErrorsException("A Facade.DbContext tag or its managerType attribute is missing in the configuration.");
            }

            if (!contextManagerConfiguration.ContainsKey("managerType"))
            {
                throw new ConfigurationErrorsException("dbManagerConfiguration does not contain key 'managerType'.");
            }

            var managerTypeName = contextManagerConfiguration["managerType"] as string;

            if (string.IsNullOrEmpty(managerTypeName))
            {
                throw new ConfigurationErrorsException("The managerType attribute is empty.");
            }

            managerTypeName = managerTypeName.Trim().ToUpperInvariant();

            try
            {
                /* Try to create a type based on it's name: */
                var frameworkAssembly = Assembly.GetAssembly(typeof(DbContextManager));

                var managerType = frameworkAssembly.GetType(managerTypeName, true, true);

                /* Try to create a new instance of the specified DbContextManager type: */
                this.DbContextManager = Activator.CreateInstance(managerType) as DbContextManager;
            }
            catch (Exception e)
            {
                throw new ConfigurationErrorsException("The managerType specified in the configuration is not valid.", e);
            }
        }
    }
}