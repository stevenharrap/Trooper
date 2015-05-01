//--------------------------------------------------------------------------------------
// <copyright file="BusinessCore.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Business.Operation.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using Response;
	using Security;
	using Interface.Business.Operation.Core;
	using Interface.Business.Response;
	using Interface.Business.Security;
	using Interface.DataManager;
	using Interface.OperationResponse;
	using Utility;

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
        
        public virtual IAddResponse<Ti> Add(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {                
                var response = new AddResponse<Ti>();
                var itemAsTc = bp.Facade.Map(item);
                var errorMessage = string.Format("The entity ({0}) could not be added.", typeof(Tc));

                if (bp.Facade.Exists(item))
                {
                    MessageUtility.Errors.Add(errorMessage, response);
                    return response;
                }

                var added = bp.Facade.Add(itemAsTc);

                if (added == null)
                {
                    MessageUtility.Errors.Add(errorMessage, response);
                    return response;
                }

                bp.Validation.Validate(added, response);

                var arg = new RequestArg<Tc> { Action = Action.AddAction, Item = added };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                bp.Uow.Save();

                response.Item = added;

                return response;
            }
        }

        public virtual IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new AddSomeResponse<Ti>();
                var itemsTc = bp.Facade.Map(items);
                var added = bp.Facade.AddSome(itemsTc);

                foreach (var item in added)
                {
                    bp.Validation.Validate(item, response);
                }

                if (!response.Ok)
                {
                    return response;
                }

                var arg = new RequestArg<Tc> { Action = Action.AddSomeAction, Items = added };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                bp.Uow.Save();

                response.Items = added;

                return response;
            }
        }

        public virtual IResponse Validate(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new Response();

                var added = bp.Facade.Add(item as Tc);

                var arg = new RequestArg<Tc> { Action = Action.ValidateAction, Item = item as Tc };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                bp.Validation.Validate(added, response);

                return response;
            }
        }

        public virtual ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new SingleResponse<bool>();

                var arg = new RequestArg<Tc> { Action = Action.IsAllowedAction };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                return response;
            }
        }

        public virtual IResponse DeleteByKey(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new Response();
                var itemAsTc = bp.Facade.Map(item);
                var errorMessage = string.Format("The entity ({0}) could not be added.", typeof(Tc));

                var arg = new RequestArg<Tc> { Action = Action.DeleteByKeyAction, Item = itemAsTc };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                bp.Facade.Delete(itemAsTc);

                bp.Uow.Save();

                return response;
            }
        }

        public virtual IResponse DeleteSomeByKey(IEnumerable<Ti> items, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var itemsAsListTc = bp.Facade.Map(items);
                var response = new Response();
                var arg = new RequestArg<Tc> { Action = Action.DeleteSomeByKeyAction, Items = itemsAsListTc.ToList() };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                bp.Facade.DeleteSome(itemsAsListTc);

                bp.Uow.Save();

                return response;
            }
        }

        public virtual IManyResponse<Ti> GetAll(IIdentity identity = null)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new ManyResponse<Ti>();
                var arg = new RequestArg<Tc> { Action = Action.GetAllAction };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                response.Items = bp.Facade.GetAll().ToList<Ti>();

                return response;
            }
        }

        public virtual IManyResponse<Ti> GetSome(ISearch search, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new ManyResponse<Ti>();
                var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Search = search };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                var some = bp.Facade.GetSome(search);

                response.Items = bp.Facade.Limit(some, search).ToList<Ti>();

                return response;
            }
        }

        public virtual ISingleResponse<Ti> GetByKey(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new SingleResponse<Ti>();
                var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Item = item as Tc };
                var errorMessage = string.Format("The ({0}) could not be found.", typeof(Tc));

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                var result = bp.Facade.GetByKey(item);

                if (result == null)
                {
                    MessageUtility.Errors.Add(errorMessage, item, null, response);
                    return response;
                }

                response.Item = result;

                return response;
            }
        }

        public virtual ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new SingleResponse<bool>();
                var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Item = item as Tc };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                var result = bp.Facade.GetByKey(item);
                response.Item = result != null;

                return response;
            }
        }

        public virtual IResponse Update(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new AddResponse<Ti>();
                var itemAsTc = bp.Facade.Map(item);
                var errorMessage = string.Format("The ({0}) could not be updated.", typeof(Tc));
                var updated = bp.Facade.Exists(itemAsTc) ? bp.Facade.Update(itemAsTc) : null;

                if (updated == null)
                {
                    MessageUtility.Errors.Add(errorMessage, response);
                    return response;
                }

                bp.Validation.Validate(updated, response);

                var arg = new RequestArg<Tc> { Action = Action.UpdateAction, Item = updated };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                bp.Uow.Save();

                response.Item = updated;

                return response;
            }
        }

        public virtual ISaveResponse<Ti> Save(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new SaveResponse<Ti> { Change = SaveChangeType.None };
                var itemAsTc = bp.Facade.Map(item);
                var errorMessage = string.Format("The ({0}) could not be saved.", typeof(Tc));
                var exists = bp.Facade.Exists(itemAsTc);
                var saved = exists ? bp.Facade.Update(itemAsTc) : bp.Facade.Add(itemAsTc);

                if (saved == null)
                {
                    MessageUtility.Errors.Add(errorMessage, response);
                    return response;
                }

                bp.Validation.Validate(saved, response);

                var arg = new RequestArg<Tc> { Action = exists ? Action.UpdateAction :  Action.AddAction, Item = saved };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity))
                {
                    return response;
                }

                bp.Uow.Save();

                response.Item = saved;
                response.Change = exists ? SaveChangeType.Update : SaveChangeType.Add;

                return response;
            }
        }

        public virtual ISaveSomeResponse<Ti> SaveSome(IEnumerable<Ti> items, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = new SaveSomeResponse<Ti>();
                var itemsTc = bp.Facade.Map(items);
                var saved = from i in itemsTc
                            let exists = bp.Facade.Exists(i)
                            let item = exists ? bp.Facade.Update(i) : bp.Facade.Add(i)
                            select new { Item = item, Exists = exists };

                foreach (var i in saved)
                {
                    bp.Validation.Validate(i.Item, response);
                }

                if (!response.Ok)
                {
                    return response;
                }

                foreach (var i in saved)
                {
                    var arg = new RequestArg<Tc> { Action = i.Exists ? Action.UpdateAction : Action.AddAction, Item = i.Item };

                    if (bp.Authorization != null)
                    {
                        bp.Authorization.IsAllowed(arg, identity);
                    }
                }

                if (!response.Ok)
                {
                    return response;
                }                

                bp.Uow.Save();

                response.Items = saved.Select(i => new SaveSomeItem<Ti>
                {
                    Change = i.Exists ? SaveChangeType.Update : SaveChangeType.Add,
                    Item = i.Item
                });

                return response;
            }
        }        
    }
}