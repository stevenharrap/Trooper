//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Trooper.Thorny.Interface.DataManager;
using Trooper.Thorny.Interface.OperationResponse;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Core
{
    public delegate IBusinessPack<TEnt, TPoco> BusinessPackHandler<TEnt, TPoco>(IUnitOfWork uow = null)        
        where TEnt : class, TPoco, new()
        where TPoco : class;

    public interface IBusinessCore<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        event BusinessPackHandler<TEnt, TPoco> OnRequestBusinessPack;

        #region GetBusinessPack

        IBusinessPack<TEnt, TPoco> GetBusinessPack();
        
        IBusinessPack<TEnt, TPoco> GetBusinessPack(IUnitOfWork uow);

        #endregion

        #region Add

        IAddResponse<TPoco> Add(TPoco item, IIdentity identity);

        IAddResponse<TPoco> Add(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity);

        IAddResponse<TPoco> Add(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region AddSome

        IAddSomeResponse<TPoco> AddSome(IEnumerable<TPoco> items, IIdentity identity);

        IAddSomeResponse<TPoco> AddSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity);

        IAddSomeResponse<TPoco> AddSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity, IResponse priorResponse);

        #endregion

        #region DeleteByKey

        IResponse DeleteByKey(TPoco item, IIdentity identity);

        IResponse DeleteByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity);

        IResponse DeleteByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region DeleteSomeByKey

        IResponse DeleteSomeByKey(IEnumerable<TPoco> items, IIdentity identity);

        IResponse DeleteSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity);

        IResponse DeleteSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity, IResponse priorResponse);

        #endregion

        #region GetAll

        IManyResponse<TPoco> GetAll(IIdentity identity);

        IManyResponse<TPoco> GetAll(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity);

        IManyResponse<TPoco> GetAll(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity, IResponse priorResponse);

        #endregion

        #region GetSome

        IManyResponse<TPoco> GetSome(ISearch search, IIdentity identity);

        IManyResponse<TPoco> GetSome(IBusinessPack<TEnt, TPoco> businessPack, ISearch search, IIdentity identity, bool limit);

        IManyResponse<TPoco> GetSome(IBusinessPack<TEnt, TPoco> businessPack, ISearch search, IIdentity identity, IResponse priorResponse, bool limit);

        #endregion

        #region GetByKey

        ISingleResponse<TPoco> GetByKey(TPoco item, IIdentity identity);

        ISingleResponse<TPoco> GetByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity);

        ISingleResponse<TPoco> GetByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region GetSomeByKey

        IManyResponse<TPoco> GetSomeByKey(IEnumerable<TPoco> items, IIdentity identity);

        IManyResponse<TPoco> GetSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity);

        IManyResponse<TPoco> GetSomeByKey(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity, IResponse priorResponse);

        #endregion

        #region ExistsByKey

        ISingleResponse<bool> ExistsByKey(TPoco item, IIdentity identity);

        ISingleResponse<bool> ExistsByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity);

        ISingleResponse<bool> ExistsByKey(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region IsAllowed

        ISingleResponse<bool> IsAllowed(IRequestArg<TPoco> argument, IIdentity identity);

        ISingleResponse<bool> IsAllowed(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity);

        ISingleResponse<bool> IsAllowed(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity, IResponse priorResponse);

        #endregion

        #region GetSession

        ISingleResponse<Guid> GetSession(IIdentity identity);

        ISingleResponse<System.Guid> GetSession(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity);

        ISingleResponse<System.Guid> GetSession(IBusinessPack<TEnt, TPoco> businessPack, IIdentity identity, IResponse priorResponse);

        #endregion

        #region Update

        ISingleResponse<TPoco> Update(TPoco item, IIdentity identity);

        ISingleResponse<TPoco> Update(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity);

        ISingleResponse<TPoco> Update(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region UpdateSome

        IManyResponse<TPoco> UpdateSome(IEnumerable<TPoco> items, IIdentity identity);

        IManyResponse<TPoco> UpdateSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity);

        IManyResponse<TPoco> UpdateSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity, IResponse priorResponse);

        #endregion

        #region Save

        ISaveResponse<TPoco> Save(TPoco item, IIdentity identity);

        ISaveResponse<TPoco> Save(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity);

        ISaveResponse<TPoco> Save(IBusinessPack<TEnt, TPoco> businessPack, TPoco item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region SaveSome

        ISaveSomeResponse<TPoco> SaveSome(IEnumerable<TPoco> items, IIdentity identity);

        ISaveSomeResponse<TPoco> SaveSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity);

        ISaveSomeResponse<TPoco> SaveSome(IBusinessPack<TEnt, TPoco> businessPack, IEnumerable<TPoco> items, IIdentity identity, IResponse priorResponse);

        #endregion
    }
}