//--------------------------------------------------------------------------------------
// <copyright file="BusinessCore.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Thorny.Business.Operation.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using Response;
	using Security;
	using Interface.DataManager;
	using Utility;
    using System;
    using Trooper.Utility;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;

    public class BusinessCore<TEnt, TPoco> : BusinessCore, IBusinessCore<TEnt, TPoco> 
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        private static List<Guid> sessions = new List<Guid>();        

        public event BusinessPackHandler<TEnt, TPoco> OnRequestBusinessPack;        

        #region Methods

        #region public

        #region GetBusinessPack

        public IBusinessPack<TEnt, TPoco> GetBusinessPack()
        {
            return this.OnRequestBusinessPack();
        }

        public IBusinessPack<TEnt, TPoco> GetBusinessPack(IUnitOfWork uow)
        {
            return this.OnRequestBusinessPack(uow);
        }

        #endregion

        #region Add

        public IAddResponse<TPoco> Add(TPoco item, IIdentity identity)
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

        public IAddResponse<TPoco> Add(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity)
        {
            return this.Add(businessPack, item, identity, null);
        }        

        public virtual IAddResponse<TPoco> Add(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<AddResponse<TPoco>>(priorResponse);

            if (!ParameterCheck(item, identity, response))
            {
                return response;
            }

            var arg = new RequestArg<TPoco> { Action = OperationAction.AddAction, Item = item };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }            

            var itemEnt = businessPack.Facade.Map(item);
            var errorMessage = string.Format("The entity ({0}) could not be added.", typeof(TEnt));

            if (businessPack.Facade.Exists(itemEnt))
            {
                MessageUtility.Errors.Add(errorMessage, AddFailedCode, response);
                return response;
            }

            var added = businessPack.Facade.Add(itemEnt);

            if (added == null)
            {
                MessageUtility.Errors.Add(errorMessage, AddFailedCode, response);
                return response;
            }

            businessPack.Validation.Validate(added, response);            

            response.Item = added;

            return response;
        }

        #endregion

        #region AddSome

        public IAddSomeResponse<TPoco> AddSome(IEnumerable<TPoco> items, IIdentity identity)
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

        public IAddSomeResponse<TPoco> AddSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity)
        {
            return this.AddSome(businessPack, items, identity, null);
        }

        public virtual IAddSomeResponse<TPoco> AddSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<AddSomeResponse<TPoco>>(priorResponse);

            if (!ParameterCheck(items, identity, response))
            {
                return response;
            }

            var arg = new RequestArg<TPoco> { Action = OperationAction.AddSomeAction, Items = items.ToList() };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
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

            response.Items = added;

            return response;
        }

        #endregion

        #region IsAllowed

        public ISingleResponse<bool> IsAllowed(IRequestArg<TPoco> argument, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.IsAllowed(bp, argument, identity);
            }
        }

        public ISingleResponse<bool> IsAllowed(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity)
        {
            return this.IsAllowed(businessPack, argument, identity, null);
        }

        public virtual ISingleResponse<bool> IsAllowed(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity, IResponse priorResponse)
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

            var arg = new RequestArg<TPoco> { Action = OperationAction.IsAllowedAction };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            var testArg = new RequestArg<TPoco> { Action = argument.Action };

            var testOutcome = businessPack.Authorization.IsAllowed(testArg, identity);

            response.Item = testOutcome;

            return response;
        }

        #endregion
        
        #region GetSession

        public ISingleResponse<Guid> GetSession(IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetSession(bp, identity);
            }
        }

        public ISingleResponse<Guid> GetSession(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity)
        {
            return this.GetSession(businessPack, identity, null);
        }

        public virtual ISingleResponse<Guid> GetSession(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity, IResponse priorResponse) 
        {
            var response = MakeResponse<SingleResponse<Guid>>(priorResponse);

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            var arg = new RequestArg<TPoco> { Action = OperationAction.GetSession };

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

        public IResponse DeleteByKey(TPoco item, IIdentity identity)
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

        public IResponse DeleteByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity)
        {
            return this.DeleteByKey(businessPack, item, identity, null);
        }        

        public virtual IResponse DeleteByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<Response>(priorResponse);

            if (!ParameterCheck(item, identity, response))
            {
                return response;
            }

            var arg = new RequestArg<TPoco> { Action = OperationAction.DeleteByKeyAction, Item = item };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }
            
            var itemAsTc = businessPack.Facade.Map(item);
            var errorMessage = string.Format("The entity ({0}) could not be deleted.", typeof(TEnt));            

            if (!businessPack.Facade.Delete(itemAsTc))
            {
                MessageUtility.Errors.Add(errorMessage, NoRecordCode, response);
            }

            return response;
        }

        #endregion

        #region DeleteSomeByKey

        public IResponse DeleteSomeByKey(IEnumerable<TPoco> items, IIdentity identity)
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

        public IResponse DeleteSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity)
        {
            return this.DeleteSomeByKey(businessPack, items, identity, null);
        }

        public virtual IResponse DeleteSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<Response>(priorResponse);

            if (!ParameterCheck(items, identity, response))
            {
                return response;
            }

            var arg = new RequestArg<TPoco> { Action = OperationAction.DeleteSomeByKeyAction, Items = items.ToList() };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            var itemsAsListTc = businessPack.Facade.Map(items);
            var errorMessage = string.Format("At least one of the entities ({0}) could not be deleted.", typeof(TEnt));                    

            if (!businessPack.Facade.DeleteSome(itemsAsListTc))
            {
                MessageUtility.Errors.Add(errorMessage, BusinessCore.NoRecordCode, response);
            }

            return response;
        }

        #endregion

        #region GetAll

        public IManyResponse<TPoco> GetAll(IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetAll(bp, identity);
            }
        }

        public IManyResponse<TPoco> GetAll(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity)
        {
            return this.GetAll(businessPack, identity, null);
        }

        public virtual IManyResponse<TPoco> GetAll(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<ManyResponse<TPoco>>(priorResponse);

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
                return response;
            }

            var arg = new RequestArg<TPoco> { Action = OperationAction.GetAllAction };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            response.Items = businessPack.Facade.GetAll().ToList<TPoco>();

            return response;
        }

        #endregion

        #region GetSome        

        public IEnumerable<ClassMapping> GetSearches(IBusinessPack<TEnt, TPoco> businessPack)
        {
            return businessPack.Facade.Searches;
        }

        public IManyResponse<TPoco> GetSome(ISearch search, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetSome(bp, search, identity, true);
            }
        }

        public IManyResponse<TPoco> GetSome(IBusinessPack<TEnt, TPoco> businessPack, ISearch search, IIdentity identity, bool limit)
        {
            return this.GetSome(businessPack, search, identity, null, limit);
        }

        public virtual IManyResponse<TPoco> GetSome(IBusinessPack<TEnt, TPoco> businessPack, ISearch search, IIdentity identity, IResponse priorResponse, bool limit)
        {
            var response = MakeResponse<ManyResponse<TPoco>>(priorResponse);

            if (search == null)
            {
                MessageUtility.Errors.Add("The search has not been supplied.", NullSearchCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            if (!businessPack.Facade.IsSearchAllowed(search))
            {
                MessageUtility.Errors.Add("The search type cannot be used for searching.", DeniedSearchCode, response);
            }

            if (!response.Ok)
            {
                return response;
            }  

            var arg = new RequestArg<TPoco> { Action = OperationAction.GetSomeAction, Search = search };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }                      

            var some = businessPack.Facade.GetSome(search);

            if (limit)
            {
                response.Items = businessPack.Facade.Limit(some, search).ToList<TPoco>();
            }
            else
            {
                response.Items = some.ToList<TPoco>();
            }

            return response;
        }

        #endregion

        #region GetByKey

        public ISingleResponse<TPoco> GetByKey(TPoco item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.GetByKey(bp, item, identity);
            }
        }

        public ISingleResponse<TPoco> GetByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity)
        {
            return this.GetByKey(businessPack, item, identity, null);
        }

        public virtual ISingleResponse<TPoco> GetByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<TPoco>>(priorResponse);

            if (!ParameterCheck(item, identity, response))
            {
                return response;
            }

            var arg = new RequestArg<TPoco> { Action = OperationAction.GetSomeAction, Item = item };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }
            
            var itemEnt = businessPack.Facade.Map(item);            
            var errorMessage = string.Format("The ({0}) could not be found.", typeof(TEnt));   
            var result = businessPack.Facade.GetByKey(itemEnt);

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

        public IManyResponse<TPoco> GetSomeByKey(IEnumerable<TPoco> items, IIdentity identity)
		{
			using (var bp = this.GetBusinessPack())
			{
				return this.GetSomeByKey(bp, items, identity);
			}
		}

        public IManyResponse<TPoco> GetSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity)
        {
            return this.GetSomeByKey(businessPack, items, identity, null);
        }

        public virtual IManyResponse<TPoco> GetSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<ManyResponse<TPoco>>(priorResponse);

            if (!ParameterCheck(items, identity, response))
            {
                return response;
            }

            var arg = new RequestArg<TPoco> { Action = OperationAction.GetSomeByKeyAction, Items = items.ToList() };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }
            
            var itemsTc = businessPack.Facade.Map(items).ToList();
            response.Items = businessPack.Facade.GetSomeByKey(itemsTc).ToList<TPoco>();

            return response;
        }

        #endregion

        #region ExistsByKey

        public ISingleResponse<bool> ExistsByKey(TPoco item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.ExistsByKey(bp, item, identity);
            }
        }

        public ISingleResponse<bool> ExistsByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity)
        {
            return this.ExistsByKey(businessPack, item, identity, null);
        }

        public virtual ISingleResponse<bool> ExistsByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<bool>>(priorResponse);

            if (!ParameterCheck(item, identity, response))
            {
                return response;
            }

            var arg = new RequestArg<TPoco> { Action = OperationAction.GetSomeAction, Item = item };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }            

            var itemEnt = businessPack.Facade.Map(item);
            var result = businessPack.Facade.GetByKey(itemEnt);
            response.Item = result != null;

            return response;
        }

        #endregion

        #region Update

        public ISingleResponse<TPoco> Update(TPoco item, IIdentity identity)
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

        public ISingleResponse<TPoco> Update(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity)
        {
            return this.Update(businessPack, item, identity, null);
        }

        public virtual ISingleResponse<TPoco> Update(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<TPoco>>(priorResponse);

            if (!ParameterCheck(item, identity, response))
            {
                return response;
            }

            var arg = new RequestArg<TPoco> { Action = OperationAction.UpdateAction, Item = item };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }

            var itemAsTc = businessPack.Facade.Map(item);
            var errorMessage = string.Format("The ({0}) could not be updated.", typeof(TEnt));
            var updated = businessPack.Facade.Exists(itemAsTc) ? businessPack.Facade.Update(itemAsTc) : null;

            if (updated == null)
            {
                MessageUtility.Errors.Add(errorMessage, NoRecordCode, response);
                return response;
            }

            businessPack.Validation.Validate(updated, response);

            response.Item = updated;

            return response;
        }

        #endregion

        #region UpdateSome

        public IManyResponse<TPoco> UpdateSome(IEnumerable<TPoco> items, IIdentity identity)
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

        public IManyResponse<TPoco> UpdateSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity)
        {
            return this.UpdateSome(businessPack, items, identity, null);
        }

        public virtual IManyResponse<TPoco> UpdateSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<ManyResponse<TPoco>>(priorResponse);

            if (!ParameterCheck(items, identity, response))
            {
                return response;
            }

            foreach (var i in items)
            {
                var arg = new RequestArg<TPoco> { Action = OperationAction.UpdateAction, Item = i };

                if (businessPack.Authorization != null)
                {
                    businessPack.Authorization.IsAllowed(arg, identity, response);
                }
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

            response.Items = updated.ToList<TPoco>();

            return response;
        }

        #endregion

        #region Save

        public ISaveResponse<TPoco> Save(TPoco item, IIdentity identity)
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

        public ISaveResponse<TPoco> Save(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity)
        {
            return this.Save(businessPack, item, identity, null);
        }

        public virtual ISaveResponse<TPoco> Save(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SaveResponse<TPoco>>(priorResponse);

            response.Change = SaveChangeType.None;

            if (!ParameterCheck(item, identity, response))
            {
                return response;
            }

            var itemAsTc = businessPack.Facade.Map(item);
            var exists = businessPack.Facade.Exists(itemAsTc);

            var arg = new RequestArg<TPoco> { Action = exists ? OperationAction.UpdateAction : OperationAction.AddAction, Item = item };

            if (businessPack.Authorization != null && !businessPack.Authorization.IsAllowed(arg, identity, response))
            {
                return response;
            }
            
            var errorMessage = string.Format("The ({0}) could not be saved.", typeof(TEnt));            
            var saved = exists ? businessPack.Facade.Update(itemAsTc) : businessPack.Facade.Add(itemAsTc);

            if (saved == null)
            {
                MessageUtility.Errors.Add(errorMessage, SaveFailedCode, response);
                return response;
            }

            businessPack.Validation.Validate(saved, response);            

            response.Item = saved;
            response.Change = exists ? SaveChangeType.Update : SaveChangeType.Add;

            return response;
        }

        #endregion

        #region SaveSome

        public ISaveSomeResponse<TPoco> SaveSome(IEnumerable<TPoco> items, IIdentity identity)
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

        public ISaveSomeResponse<TPoco> SaveSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity)
        {
            return this.SaveSome(businessPack, items, identity, null);
        }

        public virtual ISaveSomeResponse<TPoco> SaveSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SaveSomeResponse<TPoco>>(priorResponse);

            if (!ParameterCheck(items, identity, response))
            {
                return response;
            }

            var itemsTc = businessPack.Facade.Map(items);
            var saved = (from i in itemsTc
                         let exists = businessPack.Facade.Exists(i)
                         select new { Item = i, Exists = exists }).ToList();

            foreach (var i in saved)
            {
                var arg = new RequestArg<TPoco> { Action = i.Exists ? OperationAction.UpdateAction : OperationAction.AddAction, Item = i.Item };

                if (businessPack.Authorization != null)
                {
                    businessPack.Authorization.IsAllowed(arg, identity, response);
                }
            }

            if (!response.Ok)
            {
                return response;
            }
            
            foreach (var a in saved)
            {
                var item = a.Item;

                if (a.Exists)
                {
                    businessPack.Facade.Update(a.Item);
                }
                else
                {
                    businessPack.Facade.Add(a.Item);
                }
            }
            
            foreach (var i in saved)
            {
                businessPack.Validation.Validate(i.Item, response);
            }

            if (!response.Ok)
            {
                return response;
            }            

            response.Items = saved.Select(i => new SaveSomeItem<TPoco>
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

        private static bool ParameterCheck(object data, IIdentity identity, IResponse response)
        {
            if (data == null)
            {
                MessageUtility.Errors.Add("The item(s) have not been supplied.", NullDataCode, response);
            }

            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }

            return response.Ok;
        }

        #endregion

        #endregion
    }

    public class BusinessCore
    {
        public const string NullDataCode = Constants.BusinessCoreErrorCodeRoot + ".NullData";

        public const string NullIdentityCode = Constants.BusinessCoreErrorCodeRoot + ".NullIdentity";

        public const string NullArgumentCode = Constants.BusinessCoreErrorCodeRoot + ".:NullArgument";

        public const string AddFailedCode = Constants.BusinessCoreErrorCodeRoot + ".AddFailed";

        public const string NullSearchCode = Constants.BusinessCoreErrorCodeRoot + ".NullSearch";

        public const string DeniedSearchCode = Constants.BusinessCoreErrorCodeRoot + ".DeniedSearch";

        public const string NoRecordCode = Constants.BusinessCoreErrorCodeRoot + ".NoReocrd";

        public const string SaveFailedCode = Constants.BusinessCoreErrorCodeRoot + ".SaveFailed";

        public const string UserDeniedCode = Constants.AuthorizationErrorCodeRoot + ".UserDenied";

        public const string InvalidPropertyCode = Constants.ValidationErrorCodeRoot + ".InvalidProperty";
    }
}