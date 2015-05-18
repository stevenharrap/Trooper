//--------------------------------------------------------------------------------------
// <copyright file="BusinessCore.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Operation.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using Response;
	using Security;
	using Interface.DataManager;
	using Interface.OperationResponse;
	using Utility;

	/// <summary>
    /// Provides the means to expose your Model, wrap it in Read and Add operations and control
    /// access to those operations.
    /// </summary>
    public class BusinessCore<Tc, Ti> : BusinessCore, IBusinessCore<Tc, Ti> 
        where Tc : class, Ti, new()
        where Ti : class
    {
        private static List<System.Guid> sessions = new List<System.Guid>();

        public event BusinessPackHandler<Tc, Ti> OnRequestBusinessPack;

        public IBusinessPack<Tc, Ti> GetBusinessPack()
        {
            return this.OnRequestBusinessPack();
        }               
        
        public virtual IAddResponse<Ti> Add(Ti item, IIdentity identity)
        {
            var response = new AddResponse<Ti>();

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode ,response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {                
                
                var itemAsTc = bp.Facade.Map(item);
                var errorMessage = string.Format("The entity ({0}) could not be added.", typeof(Tc));

                if (bp.Facade.Exists(item))
                {
                    MessageUtility.Errors.Add(errorMessage, AddFailedCode, response);
                    return response;
                }

                var added = bp.Facade.Add(itemAsTc);

                if (added == null)
                {
                    MessageUtility.Errors.Add(errorMessage, AddFailedCode, response);
                    return response;
                }

                bp.Validation.Validate(added, response);

                var arg = new RequestArg<Tc> { Action = Action.AddAction, Item = added };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
                {
                    return response;
                }

                bp.Uow.Save(response);

                response.Item = added;

                return response;
            }
        }

        public virtual IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, IIdentity identity)
        {
            var response = new AddSomeResponse<Ti>();

            if (items == null)
            {
                MessageUtility.Errors.Add("The items have not been supplied.", NullItemCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {                
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

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
                {
                    return response;
                }

                bp.Uow.Save(response);

                response.Items = added;

                return response;
            }
        }

        public virtual ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity)
        {
            var response = new SingleResponse<bool> { Item = true };

            if (argument == null)
            {
                MessageUtility.Errors.Add("The argument has not been supplied.", NullArgumentCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {
                var arg = new RequestArg<Tc> { Action = Action.IsAllowedAction };

                if (bp.Authorization != null)
                {
                    response.Item = bp.Authorization.IsAllowed(arg, identity, response);
                }

                return response;
            }
        }

        public virtual ISingleResponse<System.Guid> GetSession(IIdentity identity)
        {
            var response = new SingleResponse<System.Guid>();

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {
                var arg = new RequestArg<Tc> { Action = Security.Action.GetSession };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
                {
                    return response;
                }

                var session = System.Guid.NewGuid();

                sessions.Add(session);
                response.Item = session;

                return response;
            }
        }

        public virtual IResponse DeleteByKey(Ti item, IIdentity identity)
        {
            var response = new Response();

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {                
                var itemAsTc = bp.Facade.Map(item);
                var errorMessage = string.Format("The entity ({0}) could not be added.", typeof(Tc));

                var arg = new RequestArg<Tc> { Action = Action.DeleteByKeyAction, Item = itemAsTc };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
                {
                    return response;
                }

                bp.Facade.Delete(itemAsTc);

                bp.Uow.Save(response);

                return response;
            }
        }

        public virtual IResponse DeleteSomeByKey(IEnumerable<Ti> items, IIdentity identity)
        {
            var response = new Response();

            if (items == null)
            {
                MessageUtility.Errors.Add("The items have not been supplied.", NullItemsCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {
                var itemsAsListTc = bp.Facade.Map(items);
                
                var arg = new RequestArg<Tc> { Action = Action.DeleteSomeByKeyAction, Items = itemsAsListTc.ToList() };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
                {
                    return response;
                }

                bp.Facade.DeleteSome(itemsAsListTc);

                bp.Uow.Save(response);

                return response;
            }
        }

        public virtual IManyResponse<Ti> GetAll(IIdentity identity = null)
        {
            var response = new ManyResponse<Ti>();

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {
                
                var arg = new RequestArg<Tc> { Action = Action.GetAllAction };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
                {
                    return response;
                }

                response.Items = bp.Facade.GetAll().ToList<Ti>();

                return response;
            }
        }

        public virtual IManyResponse<Ti> GetSome(ISearch search, IIdentity identity)
        {
            var response = new ManyResponse<Ti>();

            if (search == null)
            {
                MessageUtility.Errors.Add("The search has not been supplied.", NullSearchCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {                
                var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Search = search };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
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
            var response = new SingleResponse<Ti>();

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {
               
                var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Item = item as Tc };
                var errorMessage = string.Format("The ({0}) could not be found.", typeof(Tc));

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
                {
                    return response;
                }

                var result = bp.Facade.GetByKey(item);

                if (result == null)
                {
                    MessageUtility.Errors.Add(errorMessage, NoRecordCode, item, null, response);
                    return response;
                }

                response.Item = result;

                return response;
            }
        }

        public virtual ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity)
        {
            var response = new SingleResponse<bool>();

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {                
                var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Item = item as Tc };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
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
            var response = new AddResponse<Ti>();

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {                
                var itemAsTc = bp.Facade.Map(item);
                var errorMessage = string.Format("The ({0}) could not be updated.", typeof(Tc));
                var updated = bp.Facade.Exists(itemAsTc) ? bp.Facade.Update(itemAsTc) : null;

                if (updated == null)
                {
                    MessageUtility.Errors.Add(errorMessage, NoRecordCode, response);
                    return response;
                }

                bp.Validation.Validate(updated, response);

                var arg = new RequestArg<Tc> { Action = Action.UpdateAction, Item = updated };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
                {
                    return response;
                }

                bp.Uow.Save(response);

                response.Item = updated;

                return response;
            }
        }

        public virtual ISaveResponse<Ti> Save(Ti item, IIdentity identity)
        {
            var response = new SaveResponse<Ti> { Change = SaveChangeType.None };

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {                
                var itemAsTc = bp.Facade.Map(item);
                var errorMessage = string.Format("The ({0}) could not be saved.", typeof(Tc));
                var exists = bp.Facade.Exists(itemAsTc);
                var saved = exists ? bp.Facade.Update(itemAsTc) : bp.Facade.Add(itemAsTc);

                if (saved == null)
                {
                    MessageUtility.Errors.Add(errorMessage, SaveFailedCode, response);
                    return response;
                }

                bp.Validation.Validate(saved, response);

                var arg = new RequestArg<Tc> { Action = exists ? Action.UpdateAction :  Action.AddAction, Item = saved };

                if (bp.Authorization != null && !bp.Authorization.IsAllowed(arg, identity, response))
                {
                    return response;
                }

                bp.Uow.Save(response);

                response.Item = saved;
                response.Change = exists ? SaveChangeType.Update : SaveChangeType.Add;

                return response;
            }
        }

        public virtual ISaveSomeResponse<Ti> SaveSome(IEnumerable<Ti> items, IIdentity identity)
        {
            var response = new SaveSomeResponse<Ti>();

            if (items == null)
            {
                MessageUtility.Errors.Add("The items have not been supplied.", NullItemsCode, response);
                return response;
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            using (var bp = this.GetBusinessPack())
            {                
                var itemsTc = bp.Facade.Map(items);
                var saved = (from i in itemsTc
                            let exists = bp.Facade.Exists(i)
                            let item = exists ? bp.Facade.Update(i) : bp.Facade.Add(i)
                            select new { Item = item, Exists = exists }).ToList();

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
                        bp.Authorization.IsAllowed(arg, identity, response);
                    }
                }

                if (!response.Ok)
                {
                    return response;
                }                

                bp.Uow.Save(response);

                response.Items = saved.Select(i => new SaveSomeItem<Ti>
                {
                    Change = i.Exists ? SaveChangeType.Update : SaveChangeType.Add,
                    Item = i.Item
                });

                return response;
            }
        }        
    }

    public class BusinessCore
    {
        public const string NullItemCode = Constants.BusinessCoreErrorCodeRoot + ".NullItem";

        public const string NullItemsCode = Constants.BusinessCoreErrorCodeRoot + ".NullItems";

        public const string NullIdentityCode = Constants.BusinessCoreErrorCodeRoot + ".NullIdentity";

        public const string NullArgumentCode = Constants.BusinessCoreErrorCodeRoot + ".:NullArgument";

        public const string AddFailedCode = Constants.BusinessCoreErrorCodeRoot + ".AddFailed";

        public const string NullSearchCode = Constants.BusinessCoreErrorCodeRoot + ".NullSearch";

        public const string NoRecordCode = Constants.BusinessCoreErrorCodeRoot + ".NoReocrd";

        public const string SaveFailedCode = Constants.BusinessCoreErrorCodeRoot + ".SaveFailed";
    }
}