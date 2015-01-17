//--------------------------------------------------------------------------------------
// <copyright file="BusinessCore.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Business.Operation.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.BusinessOperation2.Business.Response;
    using Trooper.BusinessOperation2.Business.Security;
    using Trooper.BusinessOperation2.Interface.Business.Operation;
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface.Business.Response;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.BusinessOperation2.Interface.OperationResponse;
    using Trooper.BusinessOperation2.Utility;


    /// <summary>
    /// Provides the means to expose your Model, wrap it in Read and Add operations and control
    /// access to those operations.
    /// </summary>
    public abstract class BusinessCore<Tc, Ti> : IBusinessCore<Tc, Ti> 
        where Tc : class, Ti, new()
        where Ti : class
    {
        public event BusinessPackHandler<Tc, Ti> OnRequestBusinessPack;

        public IBusinessPack<Tc, Ti> GetBusinessPack()
        {
            return this.OnRequestBusinessPack();
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
        public virtual IAddResponse<Ti> Add(Ti item, ICredential credential)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new AddResponse<Tc>();
                var arg = new RequestArg<Tc> { Action = Action.AddAction, Item = item as Tc };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, credential))
                {
                    return response as IAddResponse<Ti>;
                }                
                
                Tc added;

                if (!bp.Facade.Exists(item))
                {
                    added = bp.Facade.Add(item as Tc);

                    bp.Validation.Validate(added, response);
                }
                else
                {
                    MessageUtility.Errors.Add(string.Format("The entity ({0}) could not be added.", typeof(Tc)), response);
                }

                if (response.Ok)
                {
                    bp.Uow.Save();
                    //response.Item = added as Ti; ????????????
                }

                return response as IAddResponse<Ti>;
            }
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
        public virtual IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, ICredential credential)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new AddSomeResponse<Tc>();
                var arg = new RequestArg<Tc> { Action = Action.AddAction, /*Items = items.ToList<Tc>()*/ };
            }






            /*var f = this.FacadeFactory();
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


            return entityNavs;*/

            return null;
        }

        public virtual IResponse Validate(Ti item, ICredential credential)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new Response();

                var added = bp.Facade.Add(item as Tc);

                var arg = new RequestArg<Tc> { Action = Action.ValidateAction, Item = item as Tc };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, credential))
                {
                    return response;
                }

                bp.Validation.Validate(added, response);

                return response;
            }
        }

        public virtual ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, ICredential credential)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new SingleResponse<bool>();

                var arg = new RequestArg<Tc> { Action = Action.IsAllowedAction };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, credential))
                {
                    return response;
                }

                //response.Item = this.OperationRequest.IsAllowed(argument, this.Credential, response);

                return response;
            }
        }

        public virtual IResponse DeleteByKey(Ti item, ICredential credential)
        {
            throw new System.NotImplementedException();
        }

        public virtual IResponse DeleteSomeByKey(IEnumerable<Ti> items, ICredential credential)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new Response();
                var arg = new RequestArg<Tc> { Action = Action.DeleteSomeByKeyAction, Items = items as IList<Tc> };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, credential))
                {
                    return response;
                }

                bp.Facade.DeleteSome(items as IList<Tc>);

                return response;
            }
        }

        public virtual IManyResponse<Ti> GetAll(ICredential credential = null)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new ManyResponse<Ti>();
                var arg = new RequestArg<Tc> { Action = Action.GetAllAction };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, credential))
                {
                    return response;
                }

                response.Items = bp.Facade.GetAll().ToList<Ti>();

                return response;
            }
        }

        public virtual IManyResponse<Ti> GetSome(ISearch search, ICredential credential)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new ManyResponse<Ti>();
                var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Search = search };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, credential))
                {
                    return response;
                }

                response.Items = bp.Facade.GetSome(search).ToList<Ti>();

                return response;
            }
        }

        public virtual ISingleResponse<Ti> GetByKey(Ti item, ICredential credential)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new SingleResponse<Ti>();
                var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Item = item as Tc };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, credential))
                {
                    return response;
                }

                var result = bp.Facade.GetById(item);

                if (result == null)
                {
                    MessageUtility.Errors.Add("The item requested cannot be found.", item, null, response);
                    return response;
                }

                response.Item = result;

                return response;
            }
        }

        public virtual ISingleResponse<bool> ExistsByKey(Ti item, ICredential credential)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new SingleResponse<bool>();
                var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Item = item as Tc };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, credential))
                {
                    return response;
                }

                var result = bp.Facade.GetById(item);
                response.Item = result != null;

                return response;
            }
        }

        public virtual IResponse Update(Ti item, ICredential credential)
        {
            throw new System.NotImplementedException();
        }

        public virtual ISaveResponse<Tc> Save(Ti item, ICredential credential)
        {
            throw new System.NotImplementedException();
        }

        
    }
}