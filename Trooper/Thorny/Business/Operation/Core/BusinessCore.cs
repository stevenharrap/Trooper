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
                var responseEnt = this.Add(bp, bp.Facade.ToEnt(item), identity);

                if (!responseEnt.Ok || !bp.Uow.Save(responseEnt))
                {
                    responseEnt.Item = null;
                }

                var responsePoco = new AddResponse<TPoco>(responseEnt)
                {
                    Item = bp.Facade.ToPoco(responseEnt.Item)
                };                

                return responsePoco;
            }
        }

        public IAddResponse<TEnt> Add(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity)
        {
            return this.Add(businessPack, item, identity, null);
        }        

        public IAddResponse<TEnt> Add(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<AddResponse<TEnt>>(priorResponse);
            var prepocessors = this.AddMethodPreprocessors(businessPack, item, identity, response);
            var processors = this.AddMethodProcessors(businessPack, item, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> AddMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse response)
        {
            var argument = new RequestArg<TPoco>(item) { Action = OperationAction.AddAction };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(item, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, argument, identity, response),
                () => this.IsDataValid(businessPack, item, response),
                () =>
                {
                    if (businessPack.Facade.Exists(item))
                    {
                        var errorMessage = string.Format("The item ({0}) already exists.", typeof(TEnt));

                        MessageUtility.Errors.Add(errorMessage, AddFailedCode, response);
                    }
                }
            };
        }

        protected virtual IEnumerable<Action> AddMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, AddResponse<TEnt> response)
        {
            return new List<Action>
            {
                () => {
                    var added = businessPack.Facade.Add(item);

                    if (added == null)
                    {
                        var errorMessage = string.Format("The entity ({0}) could not be added.", typeof(TEnt));

                        MessageUtility.Errors.Add(errorMessage, AddFailedCode, response);
                    }

                    response.Item = added;
                }
            };
        }

        #endregion

        #region AddSome

        public IAddSomeResponse<TPoco> AddSome(IEnumerable<TPoco> items, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.AddSome(bp, bp.Facade.ToEnts(items), identity);

                if (!responseEnt.Ok || !bp.Uow.Save(responseEnt))
                {
                    responseEnt.Items = null;
                }

                var responsePoco = new AddSomeResponse<TPoco>(responseEnt)
                {
                    Items = bp.Facade.ToPocos(responseEnt.Items)
                };

                return responsePoco;
            }
        }

        public IAddSomeResponse<TEnt> AddSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity)
        {
            return this.AddSome(businessPack, items, identity, null);
        }

        public IAddSomeResponse<TEnt> AddSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<AddSomeResponse<TEnt>>(priorResponse);
            var prepocessors = this.AddSomeMethodPreprocessors(businessPack, items, identity, response);
            var processors = this.AddSomeMethodProcessors(businessPack, items, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> AddSomeMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity, IResponse response)
        {
            var argument = new RequestArg<TPoco>(items) { Action = OperationAction.AddSomeAction };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(items, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, argument, identity, response),
                () => this.IsDataValid(businessPack, items, response)
            };
        }

        protected virtual IEnumerable<Action> AddSomeMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, AddSomeResponse<TEnt> response)
        {
            return new List<Action>
            {
                () => 
                {
                    var added = businessPack.Facade.AddSome(items);

                    response.Items = added;
                }
            };
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

        public ISingleResponse<bool> IsAllowed(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<bool>>(priorResponse);
            var prepocessors = this.GetIsAllowedPreprocessors(businessPack, argument, identity, response);
            var processors = this.GetIsAllowedProcessors(businessPack, argument, identity, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> GetIsAllowedPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity, IResponse response)
        {
            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(argument, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, argument, identity, response)
            };
        }

        protected virtual IEnumerable<Action> GetIsAllowedProcessors(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity, SingleResponse<bool> response)
        {
            return new List<Action>
            {
                () =>
                {
                    var testArg = new RequestArg<TPoco> { Action = argument.Action };
                    var testOutcome = businessPack.Authorization.IsAllowed(testArg, identity);

                    response.Item = testOutcome;
                }
            };
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
            var prepocessors = this.GetSessionMethodPreprocessors(businessPack, identity, response);
            var processors = this.GetSessionMethodProcessors(businessPack, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> GetSessionMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity, IResponse response)
        {
            var arg = new RequestArg<TPoco> { Action = OperationAction.GetSession };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, arg, identity, response)
            };
        }

        protected virtual IEnumerable<Action> GetSessionMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, SingleResponse<Guid> response)
        {
            return new List<Action>
            {
                () =>
                {
                    var session = System.Guid.NewGuid();

                    sessions.Add(session);
                    response.Item = session;
                }
            };
        }


        #endregion

        #region DeleteByKey

        public IResponse DeleteByKey(TPoco item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = this.DeleteByKey(bp, bp.Facade.ToEnt(item), identity);

                if (response.Ok)
                {
                    bp.Uow.Save(response);
                }

                return response;
            }
        }

        public IResponse DeleteByKey(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity)
        {
            return this.DeleteByKey(businessPack, item, identity, null);
        }        

        public IResponse DeleteByKey(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<Response>(priorResponse);
            var prepocessors = this.DeleteByKeyMethodPreprocessors(businessPack, item, identity, response);
            var processors = this.DeleteByKeyMethodProcessors(businessPack, item, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> DeleteByKeyMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse response)
        {
            var arg = new RequestArg<TPoco>(item) { Action = OperationAction.DeleteByKeyAction };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(item, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, arg, identity, response)
            }; 
        }

        protected virtual IEnumerable<Action> DeleteByKeyMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, Response response)
        {
            return new List<Action>
            {
                () => 
                {
                    if (!businessPack.Facade.Delete(item))
                    {
                        var errorMessage = string.Format("The entity ({0}) could not be deleted.", typeof(TEnt));
                        MessageUtility.Errors.Add(errorMessage, NoRecordCode, response);
                    }
                }
            };
        }

        #endregion

        #region DeleteSomeByKey

        public IResponse DeleteSomeByKey(IEnumerable<TPoco> items, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var response = this.DeleteSomeByKey(bp, bp.Facade.ToEnts(items), identity);

                if (response.Ok)
                {
                    bp.Uow.Save(response);
                }
                return response;
            }
        }

        public IResponse DeleteSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity)
        {
            return this.DeleteSomeByKey(businessPack, items, identity, null);
        }

        public IResponse DeleteSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<Response>(priorResponse);
            var prepocessors = this.DeleteSomeByKeyMethodPreprocessors(businessPack, items, identity, response);
            var processors = this.DeleteSomeByKeyMethodProcessors(businessPack, items, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> DeleteSomeByKeyMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity, IResponse response)
        {
            var arg = new RequestArg<TPoco>(items) { Action = OperationAction.DeleteSomeByKeyAction };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(items, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, arg, identity, response)
            };
        }

        protected virtual IEnumerable<Action> DeleteSomeByKeyMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, Response response)
        {
            return new List<Action>
            {
                () =>
                {
                    if (!businessPack.Facade.DeleteSome(items))
                    {
                        var errorMessage = string.Format("At least one of the entities ({0}) could not be deleted.", typeof(TEnt));
                        MessageUtility.Errors.Add(errorMessage, NoRecordCode, response);
                    }
                }
            };
        }

        #endregion

        #region GetAll

        public IManyResponse<TPoco> GetAll(IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.GetAll(bp, identity);
                var responsePoco = new ManyResponse<TPoco>(responseEnt)
                {
                    Items = bp.Facade.ToPocos(responseEnt.Items).ToList()
                };
                

                return responsePoco;
            }
        }

        public IManyResponse<TEnt> GetAll(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity)
        {
            return this.GetAll(businessPack, identity, null);
        }

        public IManyResponse<TEnt> GetAll(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<ManyResponse<TEnt>>(priorResponse);
            var prepocessors = this.GetAllMethodPreprocessors(businessPack, identity, response);
            var processors = this.GetAllMethodProcessors(businessPack, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;            
        }

        protected virtual IEnumerable<Action> GetAllMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity, IResponse response)
        {
            var arg = new RequestArg<TPoco> { Action = OperationAction.GetAllAction };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, arg, identity, response)
            };
        }

        protected virtual IEnumerable<Action> GetAllMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, ManyResponse<TEnt> response)
        {
            return new List<Action>
            {
                () => {
                    response.Items = businessPack.Facade.GetAll().ToList();
                }
            };
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
                var responseEnt = this.GetSome(bp, search, identity, true);
                var responsePoco = new ManyResponse<TPoco>(responseEnt)
                {
                    Items = bp.Facade.ToPocos(responseEnt.Items).ToList()
                };

                return responsePoco;
            }
        }

        public IManyResponse<TEnt> GetSome(IBusinessPack<TEnt, TPoco> businessPack, ISearch search, IIdentity identity, bool limit)
        {
            return this.GetSome(businessPack, search, identity, null, limit);
        }

        public IManyResponse<TEnt> GetSome(IBusinessPack<TEnt, TPoco> businessPack, ISearch search, IIdentity identity, IResponse priorResponse, bool limit)
        {
            var response = MakeResponse<ManyResponse<TEnt>>(priorResponse);
            var prepocessors = this.GetSomeMethodPreprocessors(businessPack, search, identity, response);
            var processors = this.GetSomeMethodProcessors(businessPack, search, response, limit);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> GetSomeMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, ISearch search, IIdentity identity, IResponse response)
        {
            var argument = new RequestArg<TPoco> { Action = OperationAction.GetSomeAction, Search = search };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => {
                    if (search == null)
                    {
                        MessageUtility.Errors.Add("The search has not been supplied.", NullSearchCode, response);
                    }
                },
                () =>
                {
                    if (!businessPack.Facade.IsSearchAllowed(search))
                    {
                        MessageUtility.Errors.Add("The search type cannot be used for searching.", DeniedSearchCode, response);
                    }
                },
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, argument, identity, response)
            };            
        }

        protected virtual IEnumerable<Action> GetSomeMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, ISearch search, ManyResponse<TEnt> response, bool limit)
        {
            return new List<Action>
            {
                () => {
                    var some = businessPack.Facade.GetSome(search);

                    if (limit)
                    {
                        response.Items = businessPack.Facade.Limit(some, search).ToList();
                    }
                    else
                    {
                        response.Items = some.ToList();
                    }
                }
            };
        }

        #endregion

        #region GetByKey

        public ISingleResponse<TPoco> GetByKey(TPoco item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.GetByKey(bp, bp.Facade.ToEnt(item), identity);

                return new SingleResponse<TPoco>(responseEnt)
                {
                    Item = bp.Facade.ToPoco(responseEnt.Item)
                };
            }
        }

        public ISingleResponse<TEnt> GetByKey(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity)
        {
            return this.GetByKey(businessPack, item, identity, null);
        }

        public ISingleResponse<TEnt> GetByKey(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<TEnt>>(priorResponse);
            var prepocessors = this.GetByKeyMethodPreprocessors(businessPack, item, identity, response);
            var processors = this.GetByKeyMethodProcessors(businessPack, item, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> GetByKeyMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse response)
        {
            var argument = new RequestArg<TPoco>(item) { Action = OperationAction.GetSomeAction };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(item, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, argument, identity, response),
                () => this.IsDataValid(businessPack, item, response)
            };
        }

        protected virtual IEnumerable<Action> GetByKeyMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, SingleResponse<TEnt> response)
        {
            return new List<Action>
            {
                () => {
                    response.Item = businessPack.Facade.GetByKey(item);

                    if (response.Item == null)
                    {
                        var errorMessage = string.Format("The ({0}) could not be found.", typeof(TEnt));
                        MessageUtility.Errors.Add(errorMessage, NoRecordCode, item, null, response);
                    }
                }
            };
        }

        #endregion

        #region GetSomeByKey

        public IManyResponse<TPoco> GetSomeByKey(IEnumerable<TPoco> items, IIdentity identity)
		{
			using (var bp = this.GetBusinessPack())
			{
                var responseEnt = this.GetSomeByKey(bp, bp.Facade.ToEnts(items), identity);

                return new ManyResponse<TPoco>(responseEnt)
                {
                    Items = bp.Facade.ToPocos(responseEnt.Items).ToList()
                };
			}
		}

        public IManyResponse<TEnt> GetSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity)
        {
            return this.GetSomeByKey(businessPack, items, identity, null);
        }

        public IManyResponse<TEnt> GetSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<ManyResponse<TEnt>>(priorResponse);
            var prepocessors = this.GetSomeByKeyMethodPreprocessors(businessPack, items, identity, response);
            var processors = this.GetSomeByKeyMethodProcessors(businessPack, items, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> GetSomeByKeyMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity, IResponse response)
        {
            var argument = new RequestArg<TPoco>(items) { Action = OperationAction.GetSomeByKeyAction };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(items, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, argument, identity, response),
                () => this.IsDataValid(businessPack, items, response)
            };
        }

        protected virtual IEnumerable<Action> GetSomeByKeyMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, ManyResponse<TEnt> response)
        {
            return new List<Action>
            {
                () => {
                    response.Items = businessPack.Facade.GetSomeByKey(items).ToList();
                }
            };
        }
        
        #endregion

        #region ExistsByKey

        public ISingleResponse<bool> ExistsByKey(TPoco item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                return this.ExistsByKey(bp, bp.Facade.ToEnt(item), identity);
            }
        }

        public ISingleResponse<bool> ExistsByKey(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity)
        {
            return this.ExistsByKey(businessPack, item, identity, null);
        }

        public ISingleResponse<bool> ExistsByKey(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<bool>>(priorResponse);
            var prepocessors = this.ExistsByKeyMethodPreprocessors(businessPack, item, identity, response);
            var processors = this.ExistsByKeyMethodProcessors(businessPack, item, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> ExistsByKeyMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse response)
        {
            var argument = new RequestArg<TPoco>(item) { Action = OperationAction.GetSomeAction };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(item, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, argument, identity, response),
                () => this.IsDataValid(businessPack, item, response)
            };
        }

        protected virtual IEnumerable<Action> ExistsByKeyMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, SingleResponse<bool> response)
        {
            return new List<Action>
            {
                () => {
                    var result = businessPack.Facade.GetByKey(item);
                    response.Item = result != null;
                }
            };
        }

        #endregion

        #region Update

        public ISingleResponse<TPoco> Update(TPoco item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.Update(bp, bp.Facade.ToEnt(item), identity);

                if (!responseEnt.Ok || !bp.Uow.Save(responseEnt))
                {
                    responseEnt.Item = null;
                }

                return new SingleResponse<TPoco>(responseEnt)
                {
                    Item = bp.Facade.ToPoco(responseEnt.Item)
                };
            }
        }

        public ISingleResponse<TEnt> Update(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity)
        {
            return this.Update(businessPack, item, identity, null);
        }

        public ISingleResponse<TEnt> Update(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SingleResponse<TEnt>>(priorResponse);
            var prepocessors = this.UpdateMethodPreprocessors(businessPack, item, identity, response);
            var processors = this.UpdateMethodProcessors(businessPack, item, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> UpdateMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse response)
        {
            var argument = new RequestArg<TPoco>(item) { Action = OperationAction.UpdateAction };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(item, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, argument, identity, response),
                () => this.IsDataValid(businessPack, item, response),
                () =>
                {
                    if (!businessPack.Facade.Exists(item))
                    {
                        var errorMessage = string.Format("The item ({0}) does not exist.", typeof(TEnt));

                        MessageUtility.Errors.Add(errorMessage, NoRecordCode, response);
                    }
                }
            };
        }

        protected virtual IEnumerable<Action> UpdateMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, SingleResponse<TEnt> response)
        {
            return new List<Action>
            {
                () => {
                    response.Item = businessPack.Facade.Update(item);

                    if (response.Item == null)
                    {
                        var errorMessage = string.Format("The item ({0}) could not be updated.", typeof(TEnt));

                        MessageUtility.Errors.Add(errorMessage, UpdatFailedCode, response);
                    }
                }
            };
        }

        #endregion

        #region UpdateSome

        public IManyResponse<TPoco> UpdateSome(IEnumerable<TPoco> items, IIdentity identity)
		{
			using (var bp = this.GetBusinessPack())
			{
				var responseEnt = this.UpdateSome(bp, bp.Facade.ToEnts(items), identity);

                if (!responseEnt.Ok || !bp.Uow.Save(responseEnt))
                {
                    responseEnt.Items = null;
                }

                return new ManyResponse<TPoco>(responseEnt)
                {
                    Items = bp.Facade.ToPocos(responseEnt.Items).ToList()
                };
			}
		}

        public IManyResponse<TEnt> UpdateSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity)
        {
            return this.UpdateSome(businessPack, items, identity, null);
        }

        public IManyResponse<TEnt> UpdateSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<ManyResponse<TEnt>>(priorResponse);
            var prepocessors = this.UpdateSomeMethodPreprocessors(businessPack, items, identity, response);
            var processors = this.UpdateSomeMethodProcessors(businessPack, items, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> UpdateSomeMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity, IResponse response)
        {
            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(items, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => 
                {
                        items.All(i => {
                            var argument = new RequestArg<TPoco>(i) { Action = OperationAction.UpdateAction };
                            this.IsIdentityAllowed(businessPack, argument, identity, response);
                            return response.Ok; });
                },
                () => this.IsDataValid(businessPack, items, response),
                () => 
                {
                    items.All(i => {
                        if (!businessPack.Facade.Exists(i))
                        {
                            var errorMessage = string.Format("The item ({0}) does not exist.", typeof(TEnt));

                            MessageUtility.Errors.Add(errorMessage, NoRecordCode, response);
                            return false;
                        }

                        return true;
                    });
                }
            };
        }

        protected virtual IEnumerable<Action> UpdateSomeMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, ManyResponse<TEnt> response)
        {
            return new List<Action>
            {
                () => 
                {
                    response.Items = items.Select(i => 
                    {
                        var item = businessPack.Facade.Update(i);

                        if (item == null)
                        {
                            var errorMessage = string.Format("The item ({0}) could not be updated.", typeof(TEnt));

                            MessageUtility.Errors.Add(errorMessage, UpdatFailedCode, response);
                        }

                        return item;
                    }).ToList();
                }
            };
        }

        #endregion

        #region Save

        public ISaveResponse<TPoco> Save(TPoco item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.Save(bp, bp.Facade.ToEnt(item), identity);

                if (!responseEnt.Ok || !bp.Uow.Save(responseEnt))
                {
                    responseEnt.Item = null;
                    responseEnt.Change = SaveChangeType.None;
                }

                return new SaveResponse<TPoco>(responseEnt)
                {
                    Change = responseEnt.Change,
                    Item = bp.Facade.ToPoco(responseEnt.Item)
                };
            }
        }

        public ISaveResponse<TEnt> Save(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity)
        {
            return this.Save(businessPack, item, identity, null);
        }

        public ISaveResponse<TEnt> Save(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SaveResponse<TEnt>>(priorResponse);
            var exists = item == null ? false : businessPack.Facade.Exists(item);
            var prepocessors = this.SaveMethodPreprocessors(businessPack, item, identity, response, exists);
            var processors = this.SaveMethodProcessors(businessPack, item, response, exists);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;
        }

        protected virtual IEnumerable<Action> SaveMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IIdentity identity, IResponse response, bool exists)
        {            
            var argument = new RequestArg<TPoco>(item) { Action = exists ? OperationAction.UpdateAction : OperationAction.AddAction };

            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(item, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () => this.IsIdentityAllowed(businessPack, argument, identity, response),
                () => this.IsDataValid(businessPack, item, response)
            };
        }

        protected virtual IEnumerable<Action> SaveMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, SaveResponse<TEnt> response, bool exists)
        {
            return new List<Action>
            {
                () => {
                    response.Item = exists ? businessPack.Facade.Update(item) : businessPack.Facade.Add(item);

                    if (response.Item == null)
                    {
                        var errorMessage = string.Format("The ({0}) could not be saved.", typeof(TEnt));

                        MessageUtility.Errors.Add(errorMessage, SaveFailedCode, response);
                        response.Change = SaveChangeType.None;
                    }
                    else
                    {
                        response.Change = exists ? SaveChangeType.Update : SaveChangeType.Add;
                    }
                }
            };
        }

        #endregion

        #region SaveSome

        public ISaveSomeResponse<TPoco> SaveSome(IEnumerable<TPoco> items, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.SaveSome(bp, bp.Facade.ToEnts(items), identity);

                if (!responseEnt.Ok || !bp.Uow.Save(responseEnt))
                {
                    responseEnt.Items = null;
                }

                return new SaveSomeResponse<TPoco>(responseEnt)
                {
                    Items = responseEnt.Items.Select(i => new SaveSomeItem<TPoco> { Change = i.Change, Item = bp.Facade.ToPoco(i.Item) }).ToList()
                };
            }
        }

        public ISaveSomeResponse<TEnt> SaveSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity)
        {
            return this.SaveSome(businessPack, items, identity, null);
        }

        public ISaveSomeResponse<TEnt> SaveSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<SaveSomeResponse<TEnt>>(priorResponse);
            var prepocessors = this.SaveSomeMethodPreprocessors(businessPack, items, identity, response);
            var processors = this.SaveSomeMethodProcessors(businessPack, items, response);

            this.InvokeProcessors(response, prepocessors, processors);

            return response;            
        }

        protected virtual IEnumerable<Action> SaveSomeMethodPreprocessors(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IIdentity identity, IResponse response)
        {
            //Todo: prevent multiple lookups to see that the item exists
            return new List<Action>
            {
                () => this.IsIdentityNull(identity, response),
                () => this.IsDataNull(items, response),
                () => this.IsIdentityValid(businessPack, identity, response),
                () =>
                {
                    items.All(i => {
                        var exists = businessPack.Facade.Exists(i);
                        var argument = new RequestArg<TPoco>(i) { Action = exists ? OperationAction.UpdateAction : OperationAction.AddAction };
                        this.IsIdentityAllowed(businessPack, argument, identity, response);
                        return response.Ok; });
                },
                () => items.All(i =>
                {
                    this.IsDataValid(businessPack, i, response);
                    return response.Ok;
                })
            };
        }

        protected virtual IEnumerable<Action> SaveSomeMethodProcessors(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, SaveSomeResponse<TEnt> response)
        {
            //Todo: prevent multiple lookups to see that the item exists
            return new List<Action>
            {
                () => {
                    var saved = new List<SaveSomeItem<TEnt>>();

                    items.All(i =>
                    {
                        var exists = businessPack.Facade.Exists(i);
                        var result = new SaveSomeItem<TEnt>
                        {
                            Item = exists ? businessPack.Facade.Update(i) : businessPack.Facade.Add(i),
                            Change = exists ? SaveChangeType.Update : SaveChangeType.Add
                        };                        

                        if (result.Item == null)
                        {
                            var errorMessage = string.Format("The ({0}) could not be saved.", typeof(TEnt));

                            MessageUtility.Errors.Add(errorMessage, SaveFailedCode, response);
                            result.Change = SaveChangeType.None;
                        }

                        saved.Add(result);                        

                        return response.Ok;
                    });

                    response.Items = saved; 
                }
            };
        }


        #endregion

        #endregion

        #region protected        

        protected bool InvokeProcessors(IResponse response, params IEnumerable<Action>[] processorList)
        {
            foreach (var list in processorList)
            {
                list.All(p => { p.Invoke(); return response.Ok; });

                if (!response.Ok)
                {
                    return false;
                }
            }

            return true; 
        }

        protected void IsIdentityNull(IIdentity identity, IResponse response)
        {
            if (identity == null)
            {
                MessageUtility.Errors.Add("The identity has not been supplied.", NullIdentityCode, response);
            }
        }

        protected void IsDataNull(TEnt item, IResponse response)
        {
            if (item == null)
            {
                MessageUtility.Errors.Add("The item(s) have not been supplied.", NullDataCode, response);
            }
        }

        protected void IsDataNull(IRequestArg<TPoco> argument, IResponse response)
        {
            if (argument == null)
            {
                MessageUtility.Errors.Add("The item(s) have not been supplied.", NullDataCode, response);
            }
        }

        protected void IsDataNull(IEnumerable<TEnt> items, IResponse response)
        {
            if (items == null || items.Any(i => i == null))
            {
                MessageUtility.Errors.Add("The item(s) have not been supplied.", NullDataCode, response);
            }
        }

        protected void IsIdentityValid(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity, IResponse response)
        {
            businessPack.Authorization.IsValid(identity, response);
        }

        protected void IsIdentityAllowed(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity, IResponse response)
        {
            businessPack.Authorization.IsAllowed(argument, identity, response);
        }        

        protected void IsDataValid(IBusinessPack<TEnt, TPoco> businessPack, TEnt item, IResponse response)
        {
            businessPack.Validation.Validate(item, response);
        }

        protected void IsDataValid(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TEnt> items, IResponse response)
        {
            items.All(i => businessPack.Validation.IsValid(i, response));
        }

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

        public const string InvalidIdentityCode = Constants.BusinessCoreErrorCodeRoot + ".InvalidIdentity";

        public const string NullArgumentCode = Constants.BusinessCoreErrorCodeRoot + ".:NullArgument";

        public const string AddFailedCode = Constants.BusinessCoreErrorCodeRoot + ".AddFailed";

        public const string NullSearchCode = Constants.BusinessCoreErrorCodeRoot + ".NullSearch";

        public const string DeniedSearchCode = Constants.BusinessCoreErrorCodeRoot + ".DeniedSearch";

        public const string NoRecordCode = Constants.BusinessCoreErrorCodeRoot + ".NoReocrd";

        public const string UpdatFailedCode = Constants.BusinessCoreErrorCodeRoot + ".UpdateFailed";

        public const string SaveFailedCode = Constants.BusinessCoreErrorCodeRoot + ".SaveFailed";

        public const string UserDeniedCode = Constants.AuthorizationErrorCodeRoot + ".UserDenied";

        public const string InvalidDataCode = Constants.ValidationErrorCodeRoot + ".InvalidData";
    }
}