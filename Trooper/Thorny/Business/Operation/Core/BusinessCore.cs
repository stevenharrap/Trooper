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

        #region GetBusinessPack

        public IBusinessPack<Tc, Ti> GetBusinessPack()
        {
            return this.OnRequestBusinessPack();
        }

        public IBusinessPack<Tc, Ti> GetBusinessPack(IUnitOfWork uow)
        {
            return this.OnRequestBusinessPack(uow);
        }

        #endregion

        #region Add

        public IAddResponse<Ti> Add(Ti item, IIdentity identity)
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

        public IAddResponse<Ti> Add(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity)
        {
            return this.Add(businessPack, item, identity, null);
        }        

        public virtual IAddResponse<Ti> Add(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<AddResponse<Ti>>(priorResponse);

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

        #endregion

        #region AddSome

        public IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, IIdentity identity)
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

        public IAddSomeResponse<Ti> AddSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity)
        {
            return this.AddSome(businessPack, items, identity, null);
        }

        public virtual IAddSomeResponse<Ti> AddSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<AddSomeResponse<Ti>>(priorResponse);
            
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

        #endregion

        #region IsAllowed

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.IsAllowed(bp, argument, identity);
            }
        }

        public ISingleResponse<bool> IsAllowed(IBusinessPack<Tc, Ti> businessPack, IRequestArg<Ti> argument, IIdentity identity)
        {
            return this.IsAllowed(businessPack, argument, identity, null);
        }

        public virtual ISingleResponse<bool> IsAllowed(IBusinessPack<Tc, Ti> businessPack, IRequestArg<Ti> argument, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<bool>>(priorResponse);

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

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            var testArg = new RequestArg<Tc> { Action = argument.Action };

            var testOutcome = businessPack.Authorization.IsAllowed(testArg, identity);

            response.Item = testOutcome;

            return response;
        }

        #endregion
        
        #region GetSession

        public ISingleResponse<System.Guid> GetSession(IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetSession(bp, identity);
            }
        }

        public ISingleResponse<System.Guid> GetSession(IBusinessPack<Tc, Ti> businessPack, IIdentity identity)
        {
            return this.GetSession(businessPack, identity, null);
        }

        public virtual ISingleResponse<System.Guid> GetSession(IBusinessPack<Tc, Ti> businessPack, IIdentity identity, IResponse priorResponse) 
        {
            var response = MakeResponse<SingleResponse<System.Guid>>(priorResponse);

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

        #endregion

        #region DeleteByKey

        public IResponse DeleteByKey(Ti item, IIdentity identity)
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

        public IResponse DeleteByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity)
        {
            return this.DeleteByKey(businessPack, item, identity, null);
        }        

        public virtual IResponse DeleteByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<Response>(priorResponse);            

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
            var errorMessage = string.Format("The entity ({0}) could not be deleted.", typeof(Tc));

            var arg = new RequestArg<Tc> { Action = Action.DeleteByKeyAction, Item = itemAsTc };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            if (!businessPack.Facade.Delete(itemAsTc))
            {
                MessageUtility.Errors.Add(errorMessage, BusinessCore.NoRecordCode, response);
            }

            return response;
        }

        #endregion

        #region DeleteSomeByKey

        public IResponse DeleteSomeByKey(IEnumerable<Ti> items, IIdentity identity)
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

        public IResponse DeleteSomeByKey(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity)
        {
            return this.DeleteSomeByKey(businessPack, items, identity, null);
        }

        public virtual IResponse DeleteSomeByKey(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<Response>(priorResponse);

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
            var errorMessage = string.Format("At least one of the entities ({0}) could not be deleted.", typeof(Tc));

            var arg = new RequestArg<Tc> { Action = Action.DeleteSomeByKeyAction, Items = itemsAsListTc.ToList() };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            if (!businessPack.Facade.DeleteSome(itemsAsListTc))
            {
                MessageUtility.Errors.Add(errorMessage, BusinessCore.NoRecordCode, response);
            }

            return response;
        }

        #endregion

        #region GetAll

        public IManyResponse<Ti> GetAll(IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetAll(bp, identity);
            }
        }

        public IManyResponse<Ti> GetAll(IBusinessPack<Tc, Ti> businessPack, IIdentity identity)
        {
            return this.GetAll(businessPack, identity, null);
        }

        public virtual IManyResponse<Ti> GetAll(IBusinessPack<Tc, Ti> businessPack, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<ManyResponse<Ti>>(priorResponse);

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

        #endregion

        #region GetSome

        public IManyResponse<Ti> GetSome(ISearch search, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetSome(bp, search, identity, true);
            }
        }

        public IManyResponse<Ti> GetSome(IBusinessPack<Tc, Ti> businessPack, ISearch search, IIdentity identity, bool limit)
        {
            return this.GetSome(businessPack, search, identity, null, limit);
        }

        public virtual IManyResponse<Ti> GetSome(IBusinessPack<Tc, Ti> businessPack, ISearch search, IIdentity identity, IResponse priorResponse, bool limit)
        {
            var response = MakeResponse<ManyResponse<Ti>>(priorResponse);

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

        #endregion

        #region GetByKey

        public ISingleResponse<Ti> GetByKey(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetByKey(bp, item, identity);
            }
        }

        public ISingleResponse<Ti> GetByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity)
        {
            return this.GetByKey(businessPack, item, identity, null);
        }

        public virtual ISingleResponse<Ti> GetByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<Ti>>(priorResponse);

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

        #endregion

        #region GetSomeByKey

        public IManyResponse<Ti> GetSomeByKey(IEnumerable<Ti> items, IIdentity identity)
		{
			using (var bp = this.GetBusinessPack())
			{
				return this.GetSomeByKey(bp, items, identity);
			}
		}

        public IManyResponse<Ti> GetSomeByKey(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity)
        {
            return this.GetSomeByKey(businessPack, items, identity, null);
        }

        public virtual IManyResponse<Ti> GetSomeByKey(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<ManyResponse<Ti>>(priorResponse);

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

            var itemsTc = businessPack.Facade.Map(items).ToList();
            var arg = new RequestArg<Tc> { Action = Action.GetSomeByKeyAction, Items = itemsTc };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            response.Items = businessPack.Facade.GetSomeByKey(itemsTc).ToList<Ti>();

            return response;
        }

        #endregion

        #region ExistsByKey

        public ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.ExistsByKey(bp, item, identity);
            }
        }

        public ISingleResponse<bool> ExistsByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity)
        {
            return this.ExistsByKey(businessPack, item, identity, null);
        }

        public virtual ISingleResponse<bool> ExistsByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<bool>>(priorResponse);

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

        #endregion

        #region Update

        public ISingleResponse<Ti> Update(Ti item, IIdentity identity)
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

        public ISingleResponse<Ti> Update(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity)
        {
            return this.Update(businessPack, item, identity, null);
        }

        public virtual ISingleResponse<Ti> Update(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<Ti>>(priorResponse);

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

        #endregion

        #region UpdateSome

        public IManyResponse<Ti> UpdateSome(IEnumerable<Ti> items, IIdentity identity)
		{
			using (var bp = this.GetBusinessPack())
			{
				var response = this.UpdateSome(bp, items, identity);

                if (!response.Ok || !bp.Uow.Save(response))
                {
                    response.Items = null;
                }

                return response;
			}
		}

        public IManyResponse<Ti> UpdateSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity)
        {
            return this.UpdateSome(businessPack, items, identity, null);
        }

        public virtual IManyResponse<Ti> UpdateSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<ManyResponse<Ti>>(priorResponse);

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
            var updated = itemsTc.Select(i => businessPack.Facade.Update(i)).ToList();

            foreach (var i in updated)
            {
                businessPack.Validation.Validate(i, response);
            }

            if (!response.Ok)
            {
                return response;
            }

            foreach (var i in updated)
            {
                var arg = new RequestArg<Tc> { Action = Action.UpdateAction, Item = i };

                if (businessPack.Authorization != null)
                {
                    businessPack.Authorization.IsAllowed(arg, identity, response);
                }
            }

            if (!response.Ok)
            {
                return response;
            }

            response.Items = updated.ToList<Ti>();

            return response;
        }

        #endregion

        #region Save

        public ISaveResponse<Ti> Save(Ti item, IIdentity identity)
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

        public ISaveResponse<Ti> Save(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity)
        {
            return this.Save(businessPack, item, identity, null);
        }

        public virtual ISaveResponse<Ti> Save(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SaveResponse<Ti>>(priorResponse);

            response.Change = SaveChangeType.None;

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

        #endregion

        #region SaveSome

        public ISaveSomeResponse<Ti> SaveSome(IEnumerable<Ti> items, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = this.SaveSome(bp, items, identity);

                if (!response.Ok || !bp.Uow.Save(response))
                {
                    response.Items = null;
                }

                return response;
            }
        }

        public ISaveSomeResponse<Ti> SaveSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity)
        {
            return this.SaveSome(businessPack, items, identity, null);
        }

        public virtual ISaveSomeResponse<Ti> SaveSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SaveSomeResponse<Ti>>(priorResponse);

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

        #region private

        private static TResponse MakeResponse<TResponse>(IResponse priorResponse)
            where TResponse : class, IResponse, new()
        {
            var response = new TResponse();

            if (priorResponse != null)
            {
                MessageUtility.Add(priorResponse, response);
            }

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

        public const string UserDeniedCode = Constants.AuthorizationErrorCodeRoot + ".UserDenied";

        public const string InvalidPropertyCode = Constants.ValidationErrorCodeRoot + ".InvalidProperty";
    }
}