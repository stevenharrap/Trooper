//--------------------------------------------------------------------------------------
// <copyright file="BusinessCrud.cs" company="Trooper Inc">
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
    /// Provides the means to expose your Model and Facade, wrap it in Read, Write and delete operations and control
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
    public class BusinessCrud<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
        : BusinessCru<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>,
        IBusinessCrud<TSearch, TEntity, TEntityPrp, TEntityKey>
        where TDbContext : DbContext, new()
        where TSearch : class, ISearch, new()
        where TEntityNav : class, TEntity, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessCrud{TDbContext,TSearch,TEntityNav,TEntity,TEntityPrp,TEntityKey}"/> class.
        /// </summary>
        /// <param name="username">
        /// The user.
        /// </param>
        public BusinessCrud(string username = null)
            : base(username)       
        {
        }

        /// <summary>
        /// The delete operation for deleting an item in the system if the use can access the method.
        /// </summary>
        /// <param name="entityKey">
        /// The key of the entity to delete.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        public virtual IOperationResponse DeleteByKey(TEntityKey entityKey)
        {
            var response = new OperationResponse();                     

            using (var scope = new UnitOfWorkScope())
            {
                this.DeleteByKey(entityKey, response);

                if (response.Ok)
                {
                    scope.SaveAllChanges();
                }

                return response;
            }
        }

        /// <summary>
        /// The delete operation for deleting items in the system if the use can access the method.
        /// </summary>
        /// <param name="entityKeys">
        /// The keys of the entity to delete.
        /// </param>
        /// <returns>
        /// The <see cref="OperationResponse"/>.
        /// </returns>
        public virtual IOperationResponse DeleteSomeByKey(IEnumerable<TEntityKey> entityKeys)
        {
            var response = new OperationResponse();

            using (var scope = new UnitOfWorkScope())
            {
                this.DeleteSomeByKey(entityKeys, response);

                if (response.Ok)
                {
                    scope.SaveAllChanges();
                }

                return response;
            }
        }

        /// <summary>
        /// The delete operation for deleting an item in the system if the use can access the method.
        /// The calling method must call SaveChanges to for deleted to take effect.
        /// </summary>
        /// <param name="entityKey">
        /// The key of the entity to delete.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        protected void DeleteByKey(TEntityKey entityKey, IOperationResponse response)
        {
            var f = this.FacadeFactory();
            var cua = f.CanUserArgFactory();
            var en = f.GetByKey(entityKey);

            if (en == null)
            {
                MessageUtility.Errors.Add("The item could not be deleted.", response);
                return;
            }

            cua.Action = UserAction.DeleteByKeyAction;
            cua.Entity = en;

            if (!f.CanUser(cua, response))
            {
                f.GuardFault(response);

                return;
            }

            var ok = f.DeleteByKey(entityKey);

            if (!ok)
            {
                MessageUtility.Errors.Add("The item could not be deleted", response);
            }
        }

        /// <summary>
        /// The delete operation for deleting items in the system if the use can access the method.
        /// The calling method must call SaveChanges to for deleted to take effect.
        /// </summary>
        /// <param name="entityKeys">
        /// The keys of the entity to delete.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        protected void DeleteSomeByKey(IEnumerable<TEntityKey> entityKeys, IOperationResponse response)
        {
            var f = this.FacadeFactory();
            var cua = f.CanUserArgFactory();
            var entityNavs = (from ek in entityKeys let en = f.GetByKey(ek) where en != null select en).ToList();

            cua.Action = UserAction.DeleteSomeByKeyAction;
            cua.Entities = entityNavs;

            if (!f.CanUser(cua, response))
            {
                f.GuardFault(response);

                return;
            }
            
            var ok = f.DeleteSomeByKey(entityNavs);

            if (!ok)
            {
                MessageUtility.Errors.Add("The items could not be deleted.", response);
            }
        }
    }
}