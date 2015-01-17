//--------------------------------------------------------------------------------------
// <copyright file="BusinessR.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation
{
    using System.Data.Entity;
    using System.Linq;

    using Trooper.ActiveDirectory;
    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.DataManager;
    using Trooper.BusinessOperation.Interface;
    using Trooper.BusinessOperation.Response;
    using Trooper.BusinessOperation.Utility;

    /// <summary>
    /// Provides the means to expose your Model, wrap it in Read operations and control
    /// access to those operations.
    /// </summary>
    /// <typeparam name="TDbContext">
    /// The DbContext from your model
    /// </typeparam>
    /// <typeparam name="TSearch">
    /// The search class to provide parameters to the GetSome method
    /// </typeparam>
    /// <typeparam name="TEntityNav">
    /// The class that extends the entity with navigation properties
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The type of entity
    /// </typeparam>
    /// <typeparam name="TEntityPrp">
    /// The class that contains the entity properties (that are not primary or foreign key properties)
    /// </typeparam>
    /// <typeparam name="TEntityKey">
    /// The key of the entity
    /// </typeparam>
    public class BusinessR<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> :
        IBusinessR<TSearch, TEntity, TEntityPrp, TEntityKey>
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
        /// Initializes a new instance of the <see cref="BusinessR{TDbContext,TSearch,TEntityNav,TEntity,TEntityPrp,TEntityKey}"/> class.
        /// </summary>
        /// <param name="username">
        /// The user.
        /// </param>
        public BusinessR(string username = null)
        {
            this.username = username;
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
        /// Can the user perform the given action. The search and entities provide the context to what is being attempted. 
        /// You will need to override this method if you have special access checking requirements.
        /// </summary>
        /// <param name="argument">
        /// The argument.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        public virtual OperationResponse CanUser(IBusinessCanUserArg<TSearch, TEntity, TEntityPrp, TEntityKey> argument)
        {
            var response = new OperationResponse();

            using (new UnitOfWorkScope())
            {
                var f = this.FacadeFactory();
                var cua = f.ConvertToFacadeCanUserArg(argument);
                f.CanUser(cua, response);
            }

            return response;
        }

        /// <summary>
        /// Creates a CanUserArgument for use by presentation layers. Override this if you have 
        /// a special CanUserArg class.
        /// </summary>
        /// <returns>IBusinessCanUserArg&lt;TSearch, TEntity&gt;</returns>
        public virtual IBusinessCanUserArg<TSearch, TEntity, TEntityPrp, TEntityKey> MakeCanUserArg()
        {
            return new BusinessCanUserArg<TSearch, TEntity, TEntityPrp, TEntityKey>();
        }
        
        /// <summary>
        /// Gets all entities for the given type if the user can access the method. 
        /// </summary> 
        /// <returns>
        /// GetManyResponse of TEntity.
        /// </returns>
        public virtual ManyResponse<TEntity> GetAll()
        {
            var response = new ManyResponse<TEntity>();
            
            using (new UnitOfWorkScope())
            {
                var f = this.FacadeFactory();
                var cua = f.CanUserArgFactory();

                cua.Action = UserAction.GetAllAction;

                if (!f.CanUser(cua, response))
                {
                    f.GuardFault(response);

                    return response;
                }

                response.Items = f.GetAll().ToList<TEntity>();

                return response;
            }
        }

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
        public virtual ManyResponse<TEntity> GetSome(TSearch search)
        {
            var response = new ManyResponse<TEntity>();                      

            using (new UnitOfWorkScope())
            {
                var f = this.FacadeFactory();
                var cua = f.CanUserArgFactory();

                cua.Action = UserAction.GetSomeAction;
                cua.Search = search;

                if (!f.CanUser(cua, response))
                {
                    f.GuardFault(response);

                    return response;
                }

                response.Items = f.GetSome(search).ToList<TEntity>();

                return response;
            }
        }

        /// <summary>
        /// Get the entity by its key if the user can access the method.
        /// </summary>
        /// <param name="itemKey">
        /// The item Key.
        /// </param>
        /// <returns>
        /// The GetSingleResponse containing the entity
        /// </returns>
        public virtual SingleResponse<TEntity> GetByKey(TEntityKey itemKey)
        {
            var response = new SingleResponse<TEntity>();                       

            using (new UnitOfWorkScope())
            {
                var f = this.FacadeFactory();
                var cua = f.CanUserArgFactory();
                var item = f.GetByKey(itemKey);

                if (item == null)
                {
                    MessageUtility.Errors.Add("The item requested cannot be found.", itemKey, null, response);
                }

                cua.Action = UserAction.GetByKeyAction;
                cua.Entity = item;

                if (!f.CanUser(cua, response))
                {
                    f.GuardFault(response);

                    return response;
                }

                response.Item = item;

                return response;
            }
        }

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
        public virtual SingleResponse<bool> ExistsByKey(TEntityKey itemKey)
        {
            var response = new SingleResponse<bool>();

            using (new UnitOfWorkScope())
            {
                var f = this.FacadeFactory();
                var cua = f.CanUserArgFactory();
                var item = f.GetByKey(itemKey);

                cua.Action = UserAction.ExistsByKeyAction;
                cua.Entity = item;

                if (!f.CanUser(cua, response))
                {
                    f.GuardFault(response);

                    response.Item = false;
                    return response;
                }

                response.Item = item != null;

                return response;
            }
        }

        /// <summary>
        /// The validate operation for validating an item - no changes should be made to the system.
        /// Any attempt to Add or update items is always validated. This method allows for testing
        /// potential operations by the user. The user will need to have access to the method. 
        /// </summary>
        /// <param name="entity">
        /// The entity to validate.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>. Ok will be true if there are no validation issues.
        /// </returns>
        public virtual OperationResponse Validate(TEntity entity)
        {
            var response = new OperationResponse();

            using (new UnitOfWorkScope())
            {
                AutoMapper.Mapper.CreateMap<TEntity, TEntityNav>();
                
                var f = this.FacadeFactory();
                var cua = f.CanUserArgFactory();
                var en = AutoMapper.Mapper.Map<TEntityNav>(entity);

                //// We add the entity to the context but never save
                //// this allows validation to work correctly.
                en = f.Add(en);
                
                cua.Action = UserAction.ValidateAction;
                cua.Entity = en;

                //// First check access
                if (!f.CanUser(cua, response))
                {
                    f.GuardFault(response);

                    return response;
                }

                f.ValidateEntity(en, response);
            }

            return response;
        }
        
        /// <summary>
        /// The facade factory that you should override to give your custom business class its custom facade (if you require)
        /// </summary>
        /// <returns>
        /// Returns the new facade that can be used within the context.
        /// </returns>
        protected virtual IFacade<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> FacadeFactory()
        {
            return new Facade<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>(this.Username);
        }
    }
}