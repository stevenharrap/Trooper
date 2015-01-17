//--------------------------------------------------------------------------------------
// <copyright file="BusinessCru.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation
{
    using System.Data.Entity;

    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.DataManager;
    using Trooper.BusinessOperation.Interface;
    using Trooper.BusinessOperation.Response;
    using Trooper.BusinessOperation.Utility;

    /// <summary>
    /// Provides the means to expose your Model, wrap it in Read and Write operations and control
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
    /// The class that contains the entity whole entity (but not navigation properties)
    /// </typeparam>
    /// <typeparam name="TEntityPrp">
    /// The class that contains the entity properties (that are not primary or foreign key properties)
    /// </typeparam>
    /// <typeparam name="TEntityKey">
    /// The key of the entity
    /// </typeparam>
    public class BusinessCru<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
        : BusinessCr<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>,
        IBusinessCru<TSearch, TEntity, TEntityPrp, TEntityKey>
        where TDbContext : DbContext, new()
        where TSearch : class, ISearch, new()
        where TEntityNav : class, TEntity, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessCru{TDbContext,TSearch,TEntityNav,TEntity,TEntityPrp,TEntityKey}"/> class.
        /// </summary>
        /// <param name="username">
        /// The user.
        /// </param>
        public BusinessCru(string username = null)
            : base(username)       
        {
        }

        /// <summary>
        /// The update operation for updating an item in the system if the user can access the method. 
        /// The result is returned in the response. This method does NOT use the Facade.Save method.
        /// </summary>
        /// <param name="entity">
        /// The entity to update.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        public virtual IOperationResponse Update(TEntity entity)
        {
            var response = new OperationResponse();            

            using (var scope = new UnitOfWorkScope())
            {
                this.Update(entity, response);

                if (response.Ok)
                {
                    scope.SaveAllChanges();
                }
            }

            return response;
        }

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
        public SaveResponse<TEntity> Save(TEntity entity)
        {
            var exists = this.ExistsByKey(entity);
            var saveResponse = new SaveResponse<TEntity>();
            TEntityKey entityKey = null;
            
            if (!exists.Ok)
            {
                MessageUtility.Add(exists, saveResponse);
                
                return saveResponse;
            }
            
            if (exists.Item)
            {
                var update = this.Update(entity);

                if (!update.Ok)
                {
                    MessageUtility.Add(update, saveResponse);
                
                    return saveResponse;
                }

                entityKey = entity;

                saveResponse.Change = SaveChangeType.Update;
            }

            if (!exists.Item)
            {
                var add = this.Add(entity);
                
                if (!add.Ok)
                {
                    MessageUtility.Add(add, saveResponse);

                    return saveResponse;
                }

                entityKey = add.EntityKey;

                saveResponse.Change = SaveChangeType.Add;
            }

            var get = this.GetByKey(entityKey);

            AutoMapper.Mapper.CreateMap<SingleResponse<TEntity>, SaveResponse<TEntity>>();

            AutoMapper.Mapper.Map(get, saveResponse);

            return saveResponse;
        }

        /// <summary>
        /// The update operation for updating an item in the system if the user can access the method. 
        /// This method does NOT use the Facade.Save method. The entity nav is updated if the response indicates success. 
        /// No changes are saved as this needs to be done by the calling method.
        /// </summary>
        /// <param name="entity">
        /// The entity to update.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        protected TEntityNav Update(TEntity entity, IOperationResponse response)
        {
            var f = this.FacadeFactory();
            var cua = f.CanUserArgFactory();
            var en = f.Map(entity);

            en = f.Update(en);

            if (en == null)
            {
                MessageUtility.Errors.Add(
                    string.Format("The entity ({0}) could not be updated.", typeof(TEntity)),
                    null,
                    response);

                return null;
            }

            f.ValidateEntity(en, response);

            if (!response.Ok)
            {
                f.GuardFault(response);

                return null;
            }

            cua.Action = UserAction.UpdateAction;
            cua.Entity = en;

            if (!f.CanUser(cua, response))
            {
                f.GuardFault(response);

                return null;
            }

            return en;
        }
    }
}