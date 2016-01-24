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
    using Step;

    public class BusinessCore<TEnt, TPoco> : BusinessCore, IBusinessCore<TEnt, TPoco> 
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        #region private fields ==========================================================
        
        private static List<IBusinessProcessStep<TEnt, TPoco>> addingPreSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new IsIdentityValidStep<TEnt, TPoco>(),
            new IsIdentityAllowedStep<TEnt, TPoco>(),
            new IsDataValidStep<TEnt, TPoco>(),
            new DataExistsStep<TEnt, TPoco>()
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> addingSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new AddDataStep<TEnt, TPoco>()
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> defaultAllowedPreSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new IsIdentityValidStep<TEnt, TPoco>(),
            new IsIdentityAllowedStep<TEnt, TPoco>()
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> defaultAllowedAndDataValidPreSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new IsIdentityValidStep<TEnt, TPoco>(),
            new IsIdentityAllowedStep<TEnt, TPoco>(),
            new IsDataValidStep<TEnt, TPoco>(),
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> isAllowedSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new IsAllowedStep<TEnt, TPoco>()
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> getSessionSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new GetSessionStep<TEnt, TPoco>()
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> deletingSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new DeleteDataStep<TEnt, TPoco>()
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> gettingAllSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new GetAllDataStep<TEnt, TPoco>()
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> gettingSomePreSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new IsIdentityValidStep<TEnt, TPoco>(),
            new IsIdentityAllowedStep<TEnt, TPoco>(),
            new IsSearchValidStep<TEnt, TPoco>(),
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> gettingSomeSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new GetSomeStep<TEnt, TPoco>()
        };
        
        private static List<IBusinessProcessStep<TEnt, TPoco>> gettingByKeySteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new GetDataByKeyStep<TEnt, TPoco>()
        };        

        private static List<IBusinessProcessStep<TEnt, TPoco>> existsByKeySteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new ExistsByKeyStep<TEnt, TPoco>()
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> updatingPreSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new IsIdentityValidStep<TEnt, TPoco>(),
            new IsIdentityAllowedStep<TEnt, TPoco>(),
            new IsDataValidStep<TEnt, TPoco>(),
            new NoDataExistsStep<TEnt, TPoco>()
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> updatingSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new UpdatingDataStep<TEnt, TPoco>()
        };

        private static List<IBusinessProcessStep<TEnt, TPoco>> savingSteps = new List<IBusinessProcessStep<TEnt, TPoco>>
        {
            new SaveDataStep<TEnt, TPoco>()
        };

        #endregion

        #region public properties =======================================================

        public event BusinessPackHandler<TEnt, TPoco> OnRequestBusinessPack;

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> AddingPreSteps
        {
            get
            {
                return addingPreSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> AddingSteps
        {
            get
            {
                return addingSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> IsAllowedPreSteps
        {
            get
            {
                return defaultAllowedPreSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> IsAllowedSteps
        {
            get
            {
                return isAllowedSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> GetSessionPreSteps
        {
            get
            {
                return defaultAllowedPreSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> GetSessionSteps
        {
            get
            {
                return getSessionSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> DeletingPreSteps
        {
            get
            {
                return defaultAllowedPreSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> DeletingSteps
        {
            get
            {
                return deletingSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> GettingAllPreSteps
        {
            get
            {
                return defaultAllowedPreSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> GettingAllSteps
        {
            get
            {
                return gettingAllSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> GettingSomePreSteps
        {
            get
            {
                return gettingSomePreSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> GettingByKeySteps
        {
            get
            {
                return gettingByKeySteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> GettingByKeyPreSteps
        {
            get
            {
                return defaultAllowedAndDataValidPreSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> GettingSomeSteps
        {
            get
            {
                return gettingSomeSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> ExistsByKeyPreSteps
        {
            get
            {
                return defaultAllowedAndDataValidPreSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> ExistsByKeySteps
        {
            get
            {
                return existsByKeySteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> UpdatingPreSteps
        {
            get
            {
                return updatingPreSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> UpdatingSteps
        {
            get
            {
                return updatingSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> SavingPreSteps
        {
            get
            {
                return defaultAllowedAndDataValidPreSteps;
            }
        }

        protected virtual IEnumerable<IBusinessProcessStep<TEnt, TPoco>> SavingSteps
        {
            get
            {
                return savingSteps;
            }
        }

        #endregion

        #region public Methos ===========================================================

        #region GetBusinessPack -----------------------------------------------------------

        public IBusinessPack<TEnt, TPoco> GetBusinessPack()
        {
            return this.OnRequestBusinessPack();
        }

        public IBusinessPack<TEnt, TPoco> GetBusinessPack(IUnitOfWork uow)
        {
            return this.OnRequestBusinessPack(uow);
        }

        #endregion

        #region Add -----------------------------------------------------------------------

        public IAddResponse<TPoco> Add(TPoco item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.Add(bp, item == null ? null : bp.Facade.ToEnt(item), identity);

                if (!responseEnt.Ok || !bp.Uow.Save(responseEnt))
                {
                    responseEnt.Item = null;
                }

                var responsePoco = new AddResponse<TPoco>(responseEnt)
                {
                    Item = responseEnt.Item == null ? null : bp.Facade.ToPoco(responseEnt.Item)
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
            var argument = new RequestArg<TPoco>(item) { Action = OperationAction.AddAction };
            
            this.InvokeSteps(
                businessPack, argument, item, identity, response,
                this.AddingPreSteps, this.AddingSteps);

            return response;
        }        

        #endregion

        #region AddSome -------------------------------------------------------------------

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
                    Items = responseEnt.Items == null ? null : bp.Facade.ToPocos(responseEnt.Items)
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
            var argument = new RequestArg<TPoco>(items) { Action = OperationAction.AddSomeAction };

            this.InvokeSteps(
                businessPack, argument, items, identity, response,
                this.AddingPreSteps, this.AddingSteps);

            return response;
        }        

        #endregion

        #region IsAllowed -----------------------------------------------------------------

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

            this.InvokeSteps(
                businessPack, argument, identity, response,
                this.IsAllowedPreSteps, this.IsAllowedSteps);

            return response;
        }

        #endregion

        #region GetSession ----------------------------------------------------------------

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
            var argument = new RequestArg<TPoco> { Action = OperationAction.GetSession };

            this.InvokeSteps(
                businessPack, argument, identity, response,
                this.GetSessionPreSteps, this.GetSessionSteps);

            return response;
        }

        #endregion

        #region DeleteByKey ---------------------------------------------------------------

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
            var argument = new RequestArg<TPoco>(item) { Action = OperationAction.DeleteByKeyAction };

            this.InvokeSteps(
                businessPack, argument, item, identity, response,
                this.DeletingPreSteps, this.DeletingSteps);

            return response;
        }       

        #endregion

        #region DeleteSomeByKey -----------------------------------------------------------

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
            var argument = new RequestArg<TPoco>(items) { Action = OperationAction.DeleteSomeByKeyAction };

            this.InvokeSteps(
                businessPack, argument, items, identity, response,
                this.DeletingPreSteps, this.DeletingSteps);

            return response;
        }        

        #endregion

        #region GetAll --------------------------------------------------------------------

        public IManyResponse<TPoco> GetAll(IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.GetAll(bp, identity);
                var responsePoco = new ManyResponse<TPoco>(responseEnt)
                {
                    Items = responseEnt.Items == null ? null : bp.Facade.ToPocos(responseEnt.Items).ToList()
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
            var argument = new RequestArg<TPoco> { Action = OperationAction.GetAllAction };

            this.InvokeSteps(
                businessPack, argument, identity, response,
                this.GettingAllPreSteps, this.GettingAllSteps);

            return response;
        }


        #endregion

        #region GetSome -------------------------------------------------------------------   

        public IEnumerable<ClassMapping> GetSearches(IBusinessPack<TEnt, TPoco> businessPack)
        {
            return businessPack.Facade.Searches;
        }

        public IManyResponse<TPoco> GetSome(ISearch search, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.GetSome(bp, search, identity);
                var responsePoco = new ManyResponse<TPoco>(responseEnt)
                {
                    Items = bp.Facade.ToPocos(responseEnt.Items).ToList()
                };

                return responsePoco;
            }
        }

        public IManyResponse<TEnt> GetSome(IBusinessPack<TEnt, TPoco> businessPack, ISearch search, IIdentity identity)
        {
            return this.GetSome(businessPack, search, identity, null);
        }

        public IManyResponse<TEnt> GetSome(IBusinessPack<TEnt, TPoco> businessPack, ISearch search, IIdentity identity, IResponse priorResponse)
        {
            var response = MakeResponse<ManyResponse<TEnt>>(priorResponse);
            //Todo: validate search method
            var argument = new RequestArg<TPoco> { Action = OperationAction.GetSomeAction, Search = search };

            this.InvokeSteps(
                businessPack, argument, search, identity, response,
                this.GettingSomePreSteps, this.GettingSomeSteps);

            return response;
        }
        
        #endregion

        #region GetByKey ------------------------------------------------------------------

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
            var argument = new RequestArg<TPoco>(item) { Action = OperationAction.GetSomeAction };

            this.InvokeSteps(
                businessPack, argument, item, identity, response,
                this.GettingByKeyPreSteps, this.GettingByKeySteps);

            return response;
        }        

        #endregion

        #region GetSomeByKey --------------------------------------------------------------

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
            var argument = new RequestArg<TPoco>(items) { Action = OperationAction.GetSomeByKeyAction };            

            this.InvokeSteps(
                businessPack, argument, items, identity, response,
                this.GettingByKeyPreSteps, this.GettingByKeySteps);

            return response;
        }
        
        #endregion

        #region ExistsByKey ---------------------------------------------------------------

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
            var argument = new RequestArg<TPoco>(item) { Action = OperationAction.ExistsByKeyAction };

            this.InvokeSteps(
                businessPack, argument, item, identity, response,
                this.ExistsByKeyPreSteps, this.ExistsByKeySteps);

            return response;            
        }        

        #endregion

        #region Update --------------------------------------------------------------------

        public ISingleResponse<TPoco> Update(TPoco item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.Update(bp, item == null ? null : bp.Facade.ToEnt(item), identity);

                if (!responseEnt.Ok || !bp.Uow.Save(responseEnt))
                {
                    responseEnt.Item = null;
                }

                return new SingleResponse<TPoco>(responseEnt)
                {
                    Item = responseEnt.Item == null ? null : bp.Facade.ToPoco(responseEnt.Item)
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
            var argument = new RequestArg<TPoco>(item) { Action = OperationAction.UpdateAction };

            this.InvokeSteps(
                businessPack, argument, item, identity, response,
                this.UpdatingPreSteps, this.UpdatingSteps);

            return response;
        }

        #endregion

        #region UpdateSome ----------------------------------------------------------------

        public IManyResponse<TPoco> UpdateSome(IEnumerable<TPoco> items, IIdentity identity)
		{
			using (var bp = this.GetBusinessPack())
			{
				var responseEnt = this.UpdateSome(bp, items ==  null ?  null : bp.Facade.ToEnts(items), identity);

                if (!responseEnt.Ok || !bp.Uow.Save(responseEnt))
                {
                    responseEnt.Items = null;
                }

                return new ManyResponse<TPoco>(responseEnt)
                {
                    Items = responseEnt.Items == null ? null : bp.Facade.ToPocos(responseEnt.Items).ToList()
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
            var argument = new RequestArg<TPoco>(items) { Action = OperationAction.UpdateAction };

            this.InvokeSteps(
                businessPack, argument, items, identity, response,
                this.UpdatingPreSteps, this.UpdatingSteps);

            return response;
        }
        
        #endregion

        #region Save ----------------------------------------------------------------------

        public ISaveResponse<TPoco> Save(TPoco item, IIdentity identity)
        {
            using (var bp = this.GetBusinessPack())
            {
                var responseEnt = this.Save(bp, item == null ? null : bp.Facade.ToEnt(item), identity);

                if (!responseEnt.Ok || !bp.Uow.Save(responseEnt))
                {
                    responseEnt.Item = null;
                    responseEnt.Change = SaveChangeType.None;
                }

                return new SaveResponse<TPoco>(responseEnt)
                {
                    Change = responseEnt.Change,
                    Item = responseEnt.Item == null ? null : bp.Facade.ToPoco(responseEnt.Item)
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
            var argument = new RequestArg<TPoco>(item) { Action = OperationAction.SaveAction };

            this.InvokeSteps(
                businessPack, argument, item, identity, response,
                this.SavingPreSteps, this.SavingSteps);

            return response;
        }
        
        #endregion

        #region SaveSome ------------------------------------------------------------------

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
            var argument = new RequestArg<TPoco>(items) { Action = OperationAction.SaveSomeAction };

            this.InvokeSteps(
                businessPack, argument, items, identity, response,
                this.SavingPreSteps, this.SavingSteps);

            return response;
        }

        #endregion

        #endregion       

        #region private =================================================================
        
        private bool InvokeSteps(
            IBusinessPack<TEnt, TPoco> businessPack,
            IRequestArg<TPoco> argument,
            TEnt item,
            IIdentity identity,
            IResponse response,
            params IEnumerable<IBusinessProcessStep<TEnt, TPoco>>[] steps)
        {
            foreach (var list in steps)
                foreach (var step in list)
                {
                    step.Execute(businessPack, argument, item, identity, response);

                    if (!response.Ok)
                    {
                        return false;
                    }
                }

            return true;
        }

        private bool InvokeSteps(
            IBusinessPack<TEnt, TPoco> businessPack,
            IRequestArg<TPoco> argument,
            IEnumerable<TEnt> items,
            IIdentity identity,
            IResponse response,
            params IEnumerable<IBusinessProcessStep<TEnt, TPoco>>[] steps)
        {
            foreach (var list in steps)
                foreach (var step in list)
                {
                    step.Execute(businessPack, argument, items, identity, response);

                    if (!response.Ok)
                    {
                        return false;
                    }
                }

            return true;
        }

        private bool InvokeSteps(
            IBusinessPack<TEnt, TPoco> businessPack,
            IRequestArg<TPoco> argument,
            IIdentity identity,
            IResponse response,
            params IEnumerable<IBusinessProcessStep<TEnt, TPoco>>[] steps)
        {
            foreach (var list in steps)
                foreach (var step in list)
                {
                    step.Execute(businessPack, argument, identity, response);

                    if (!response.Ok)
                    {
                        return false;
                    }
                }

            return true;
        }

        private bool InvokeSteps(
            IBusinessPack<TEnt, TPoco> businessPack,
            IRequestArg<TPoco> argument,
            ISearch search,
            IIdentity identity,
            IResponse response,
            params IEnumerable<IBusinessProcessStep<TEnt, TPoco>>[] steps)
        {
            foreach (var list in steps)
                foreach (var step in list)
                {
                    step.Execute(businessPack, argument, search, identity, response);

                    if (!response.Ok)
                    {
                        return false;
                    }
                }

            return true;
        }

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

        //private static bool ParameterCheck(object data, IIdentity identity, IResponse response)
        //{
        //    if (data == null)
        //    {
        //        MessageUtility.Errors.Add("The item(s) have not been supplied.", InvalidDataCode, response);
        //    }

        //    if (identity == null)
        //    {
        //        MessageUtility.Errors.Add("The identity has not been supplied.", InvalidIdentityCode, response);
        //    }

        //    return response.Ok;
        //}

        #endregion
    }

    public class BusinessCore
    {
        public const string InvalidIdentityCode = Constants.BusinessCoreErrorCodeRoot + ".InvalidIdentity";

        public const string AddFailedCode = Constants.BusinessCoreErrorCodeRoot + ".AddFailed";

        public const string InvalidSearchCode = Constants.BusinessCoreErrorCodeRoot + ".InvalidSearch";

        public const string DeniedSearchCode = Constants.BusinessCoreErrorCodeRoot + ".DeniedSearch";

        public const string NoRecordCode = Constants.BusinessCoreErrorCodeRoot + ".NoReocrd";

        public const string UpdateFailedCode = Constants.BusinessCoreErrorCodeRoot + ".UpdateFailed";

        public const string SaveFailedCode = Constants.BusinessCoreErrorCodeRoot + ".SaveFailed";

        public const string UserDeniedCode = Constants.AuthorizationErrorCodeRoot + ".UserDenied";

        public const string InvalidDataCode = Constants.ValidationErrorCodeRoot + ".InvalidData";
    }
}