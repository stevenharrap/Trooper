//--------------------------------------------------------------------------------------
// <copyright file="BusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.DataManager;
    using Trooper.BusinessOperation.Interface;
    using Trooper.BusinessOperation.Response;
    using Trooper.BusinessOperation.Utility;

    /// <summary>
    /// Provides the means to expose your Model, wrap it in Read and Add operations and control
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
    public class BusinessCr<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
        : BusinessR<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>,
        IBusinessCr<TSearch, TEntity, TEntityPrp, TEntityKey>
        where TDbContext : DbContext, new()
        where TSearch : class, ISearch, new()
        where TEntityNav : class, TEntity, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessCr{TDbContext,TSearch,TEntityNav,TEntity,TEntityPrp,TEntityKey}"/> class.
        /// </summary>
        /// <param name="username">
        /// The user.
        /// </param>
        public BusinessCr(string username = null)
            : base(username)       
        {
        }

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
        public virtual IAddResponse<TEntityKey> Add(TEntity entity)
        {
            var response = new AddResponse<TEntityKey>();            

            using (var scope = new UnitOfWorkScope())
            {
                var en = this.Add(entity, response);

                if (response.Ok)
                {
                    scope.SaveAllChanges();
                    response.EntityKey = en;
                }
            }

            return response;
        }

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
        public virtual IAddSomeResponse<TEntityKey> AddSome(IEnumerable<TEntity> entities)
        {
            var response = new AddSomeResponse<TEntityKey>();

            using (var scope = new UnitOfWorkScope())
            {
                response.EntityKeys = this.AddSome(entities, response);

                if (response.Ok)
                {
                    scope.SaveAllChanges();
                }
            }

            return response;
        }

        /// <summary>
        /// The add operation for adding an item to the system if the user can access the method. 
        /// The added entity nav is returned if the response indicates success. No changes are
        /// saved as this needs to be done by the calling method.
        /// </summary>
        /// <param name="entity">
        /// The entity to add
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        protected TEntityNav Add(TEntity entity, IOperationResponse response)
        {
            var f = this.FacadeFactory();
            var cua = f.CanUserArgFactory();

            AutoMapper.Mapper.CreateMap<TEntity, TEntityNav>();

            var en = AutoMapper.Mapper.Map<TEntityNav>(entity);

            if (en != null && !f.Exists(entity))
            {
                en = f.Add(en);

                if (en != null)
                {
                    f.ValidateEntity(en, response);
                }
                else
                {
                    MessageUtility.Errors.Add(
                        string.Format("The entity ({0}) could not be added.", typeof(TEntity)),
                        response);
                }

                if (!response.Ok)
                {
                    f.GuardFault(response);

                    return null;
                }
            }
            else
            {
                MessageUtility.Errors.Add(string.Format("The entity ({0}) could not be added.", typeof(TEntity)), response);

                return null;
            }

            cua.Action = UserAction.AddAction;
            cua.Entity = en;

            //// Check access if all good
            if (!f.CanUser(cua, response))
            {
                f.GuardFault(response);

                return null;
            }

            return en;
        }

        /// <summary>
        /// The add operation for adding items to the system. The user will need to have access to the method. 
        /// The added entity navs are returned if the response indicates success. No changes are
        /// saved as this needs to be done by the calling method.
        /// </summary>
        /// <param name="entities">
        /// The entities to add
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        protected IEnumerable<TEntityNav> AddSome(IEnumerable<TEntity> entities, IOperationResponse response)
        {
            var f = this.FacadeFactory();
            var cua = f.CanUserArgFactory();

            AutoMapper.Mapper.CreateMap<TEntity, TEntityNav>();
            var entityNavs = AutoMapper.Mapper.Map<List<TEntityNav>>(entities);

            entityNavs = f.AddSome(entityNavs).ToList();

            foreach (var en in entityNavs)
            {
                f.ValidateEntity(en, response);

                if (!response.Ok)
                {
                    f.GuardFault(response);

                    return null;
                }
            }

            cua.Action = UserAction.AddSomeAction;
            cua.Entities = entityNavs;

            //// Check access if data correct
            if (!f.CanUser(cua, response))
            {
                f.GuardFault(response);

                return null;
            }


            return entityNavs;
        }
    }
}