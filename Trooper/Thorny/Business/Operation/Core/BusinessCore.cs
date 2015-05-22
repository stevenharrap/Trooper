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

        #region Methods

        #region public

        public IBusinessPack<Tc, Ti> GetBusinessPack()
        {
            return this.OnRequestBusinessPack();
        }

        public virtual IAddResponse<Ti> Add(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = this.Add(bp, item, identity);

                if (!response.Ok || !bp.Uow.Save(response))
                {
                    response.Item = null;
                }

                return response;
            }
        }

        public virtual IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = this.AddSome(bp, items, identity);

                if (!response.Ok || !bp.Uow.Save(response))
                {
                    response.Items = null;
                }

                return response;
            }
        }

        public virtual ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.IsAllowed(bp, argument, identity);
            }
        }

        public virtual ISingleResponse<System.Guid> GetSession(IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetSession(bp, identity);
            }
        }

        public virtual IResponse DeleteByKey(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = this.DeleteByKey(bp, item, identity);

                if (response.Ok)
                {
                    bp.Uow.Save(response);
                }

                return response;
            }
        }

        public virtual IResponse DeleteSomeByKey(IEnumerable<Ti> items, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = this.DeleteSomeByKey(bp, items, identity);

                if (response.Ok)
                {
                    bp.Uow.Save(response);
                }
                return response;
            }
        }

        public virtual IManyResponse<Ti> GetAll(IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetAll(bp, identity);
            }
        }

        public virtual IManyResponse<Ti> GetSome(ISearch search, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetSome(bp, search, identity, true);
            }
        }

        public virtual ISingleResponse<Ti> GetByKey(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetByKey(bp, item, identity);
            }
        }

        public virtual ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.ExistsByKey(bp, item, identity);
            }
        }
        
        public virtual ISingleResponse<Ti> Update(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = this.Update(bp, item, identity);

                if (!response.Ok || !bp.Uow.Save(response))
                {
                    response.Item = null;
                }

                return response;
            }
        }

        public virtual ISaveResponse<Ti> Save(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = this.Save(bp, item, identity);

                if (!response.Ok || !bp.Uow.Save(response))
                {
                    response.Item = null;
                    response.Change = SaveChangeType.None;
                }

                return response;
            }
        }

        public virtual ISaveSomeResponse<Ti> SaveSome(IEnumerable<Ti> items, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = this.SaveSome(bp, items, identity);

                if (!response.Ok || bp.Uow.Save(response))
                {
                    response.Items = null;
                }

                return response;
            }
        }

        #endregion

        #region protected

        protected virtual IAddResponse<Ti> Add(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IAddResponse<Ti> response = null)
        {
            response = response ?? new AddResponse<Ti>();

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var itemAsTc = businessPack.Facade.Map(item);
            var errorMessage = string.Format("The entity ({0}) could not be added.", typeof(Tc));

            if (businessPack.Facade.Exists(item))
            {
                MessageUtility.Errors.Add(errorMessage, AddFailedCode, response);
                return response;
            }

            var added = businessPack.Facade.Add(itemAsTc);

            if (added == null)
            {
                MessageUtility.Errors.Add(errorMessage, AddFailedCode, response);
                return response;
            }

            businessPack.Validation.Validate(added, response);

            var arg = new RequestArg<Tc> { Action = Action.AddAction, Item = added };

            if (businessPack.Authorization != null)
            {
                businessPack.Authorization.IsAllowed(arg, identity, response);
            }

            response.Item = added;

            return response;
        }

        protected virtual IAddSomeResponse<Ti> AddSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IAddSomeResponse<Ti> response = null)
        {
            response = response ?? new AddSomeResponse<Ti>();

            if (items == null)
            {
                MessageUtility.Errors.Add("The items have not been supplied.", NullItemCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var itemsTc = businessPack.Facade.Map(items);
            var added = businessPack.Facade.AddSome(itemsTc);

            foreach (var item in added)
            {
                businessPack.Validation.Validate(item, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var arg = new RequestArg<Tc> { Action = Action.AddSomeAction, Items = added };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            response.Items = added;

            return response;
        }

        protected virtual ISingleResponse<bool> IsAllowed(IBusinessPack<Tc, Ti> businessPack, IRequestArg<Ti> argument, IIdentity identity, ISingleResponse<bool> response = null)
        {
            response = response ?? new SingleResponse<bool> { Item = false };

            if (argument == null)
            {
                MessageUtility.Errors.Add("The argument has not been supplied.", NullArgumentCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var arg = new RequestArg<Tc> { Action = Action.IsAllowedAction };

            if (businessPack.Authorization != null && businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            var testArg = new RequestArg<Tc> { Action = argument.Action };

            var testOutcome = businessPack.Authorization.IsAllowed(testArg, identity);

            response.Item = testOutcome;

            return response;
        }

        protected virtual ISingleResponse<System.Guid> GetSession(IBusinessPack<Tc, Ti> businessPack, IIdentity identity, ISingleResponse<System.Guid> response = null)
        {
            response = response ?? new SingleResponse<System.Guid>();

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            var arg = new RequestArg<Tc> { Action = Security.Action.GetSession };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            var session = System.Guid.NewGuid();

            sessions.Add(session);
            response.Item = session;

            return response;
        }

        protected virtual IResponse DeleteByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse response = null)
        {
            response = response ?? new Response();

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var itemAsTc = businessPack.Facade.Map(item);
            var errorMessage = string.Format("The entity ({0}) could not be added.", typeof(Tc));

            var arg = new RequestArg<Tc> { Action = Action.DeleteByKeyAction, Item = itemAsTc };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            businessPack.Facade.Delete(itemAsTc);

            return response;
        }

        protected virtual IResponse DeleteSomeByKey(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse response = null)
        {
            response = response ?? new Response();

            if (items == null)
            {
                MessageUtility.Errors.Add("The items have not been supplied.", NullItemsCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var itemsAsListTc = businessPack.Facade.Map(items);

            var arg = new RequestArg<Tc> { Action = Action.DeleteSomeByKeyAction, Items = itemsAsListTc.ToList() };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            businessPack.Facade.DeleteSome(itemsAsListTc);

            return response;
        }

        protected virtual IManyResponse<Ti> GetAll(IBusinessPack<Tc, Ti> businessPack, IIdentity identity, IManyResponse<Ti> response = null)
        {
            response = response ?? new ManyResponse<Ti>();

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            var arg = new RequestArg<Tc> { Action = Action.GetAllAction };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            response.Items = businessPack.Facade.GetAll().ToList<Ti>();

            return response;
        }

        protected virtual IManyResponse<Ti> GetSome(IBusinessPack<Tc, Ti> businessPack, ISearch search, IIdentity identity, bool limit, IManyResponse<Ti> response = null)
        {
            response = response ?? new ManyResponse<Ti>();

            if (search == null)
            {
                MessageUtility.Errors.Add("The search has not been supplied.", NullSearchCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Search = search };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            var some = businessPack.Facade.GetSome(search);

            if (limit)
            {
                response.Items = businessPack.Facade.Limit(some, search).ToList<Ti>();
            }
            else
            {
                response.Items = some.ToList<Ti>();
            }

            return response;
        }

        protected virtual ISingleResponse<Ti> GetByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, ISingleResponse<Ti> response = null)
        {
            response = response ?? new SingleResponse<Ti>();

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Item = item as Tc };
            var errorMessage = string.Format("The ({0}) could not be found.", typeof(Tc));

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            var result = businessPack.Facade.GetByKey(item);

            if (result == null)
            {
                MessageUtility.Errors.Add(errorMessage, NoRecordCode, item, null, response);
                return response;
            }

            response.Item = result;

            return response;
        }

        protected virtual ISingleResponse<bool> ExistsByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, ISingleResponse<bool> response = null)
        {
            response = response ?? new SingleResponse<bool>();

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var arg = new RequestArg<Tc> { Action = Action.GetSomeAction, Item = item as Tc };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            var result = businessPack.Facade.GetByKey(item);
            response.Item = result != null;

            return response;
        }

        protected virtual ISingleResponse<Ti> Update(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, ISingleResponse<Ti> response = null)
        {
            response = response ?? new SingleResponse<Ti>();

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var itemAsTc = businessPack.Facade.Map(item);
            var errorMessage = string.Format("The ({0}) could not be updated.", typeof(Tc));
            var updated = businessPack.Facade.Exists(itemAsTc) ? businessPack.Facade.Update(itemAsTc) : null;

            if (updated == null)
            {
                MessageUtility.Errors.Add(errorMessage, NoRecordCode, response);
                return response;
            }

            businessPack.Validation.Validate(updated, response);

            var arg = new RequestArg<Tc> { Action = Action.UpdateAction, Item = updated };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            response.Item = updated;

            return response;
        }

        protected virtual ISaveResponse<Ti> Save(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, ISaveResponse<Ti> response = null)
        {
            response = response ?? new SaveResponse<Ti> { Change = SaveChangeType.None };

            if (item == null)
            {
                MessageUtility.Errors.Add("The item has not been supplied.", NullItemCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var itemAsTc = businessPack.Facade.Map(item);
            var errorMessage = string.Format("The ({0}) could not be saved.", typeof(Tc));
            var exists = businessPack.Facade.Exists(itemAsTc);
            var saved = exists ? businessPack.Facade.Update(itemAsTc) : businessPack.Facade.Add(itemAsTc);

            if (saved == null)
            {
                MessageUtility.Errors.Add(errorMessage, SaveFailedCode, response);
                return response;
            }

            businessPack.Validation.Validate(saved, response);

            var arg = new RequestArg<Tc> { Action = exists ? Action.UpdateAction : Action.AddAction, Item = saved };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            response.Item = saved;
            response.Change = exists ? SaveChangeType.Update : SaveChangeType.Add;

            return response;
        }

        protected virtual ISaveSomeResponse<Ti> SaveSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, ISaveSomeResponse<Ti> response = null)
        {
            response = response ?? new SaveSomeResponse<Ti>();

            if (items == null)
            {
                MessageUtility.Errors.Add("The items have not been supplied.", NullItemsCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            var itemsTc = businessPack.Facade.Map(items);
            var saved = (from i in itemsTc
                         let exists = businessPack.Facade.Exists(i)
                         let item = exists ? businessPack.Facade.Update(i) : businessPack.Facade.Add(i)
                         select new { Item = item, Exists = exists }).ToList();

            foreach (var i in saved)
            {
                businessPack.Validation.Validate(i.Item, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            foreach (var i in saved)
            {
                var arg = new RequestArg<Tc> { Action = i.Exists ? Action.UpdateAction : Action.AddAction, Item = i.Item };

                if (businessPack.Authorization != null)
                {
                    businessPack.Authorization.IsAllowed(arg, identity, response);
                }
            }

            if (!response.Ok)
            {
                return response;
            }

            response.Items = saved.Select(i => new SaveSomeItem<Ti>
            {
                Change = i.Exists ? SaveChangeType.Update : SaveChangeType.Add,
                Item = i.Item
            });

            return response;
        }

        #endregion

        #endregion
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