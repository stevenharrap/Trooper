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
    public delegate IBusinessPack<Tc, Ti> BusinessPackHandler<Tc, Ti>(IUnitOfWork uow = null)        
        where Tc : class, Ti, new()
        where Ti : class;

    public interface IBusinessCore<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        event BusinessPackHandler<Tc, Ti> OnRequestBusinessPack;

        #region GetBusinessPack

        IBusinessPack<Tc, Ti> GetBusinessPack();
        
        IBusinessPack<Tc, Ti> GetBusinessPack(IUnitOfWork uow);

        #endregion

        #region Add

        IAddResponse<Ti> Add(Ti item, IIdentity identity);

        IAddResponse<Ti> Add(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity);

        IAddResponse<Ti> Add(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region AddSome

        IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, IIdentity identity);

        IAddSomeResponse<Ti> AddSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity);

        IAddSomeResponse<Ti> AddSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse priorResponse);

        #endregion

        #region DeleteByKey

        IResponse DeleteByKey(Ti item, IIdentity identity);

        IResponse DeleteByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity);

        IResponse DeleteByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region DeleteSomeByKey

        IResponse DeleteSomeByKey(IEnumerable<Ti> items, IIdentity identity);

        IResponse DeleteSomeByKey(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity);

        IResponse DeleteSomeByKey(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse priorResponse);

        #endregion

        #region GetAll

        IManyResponse<Ti> GetAll(IIdentity identity);

        IManyResponse<Ti> GetAll(IBusinessPack<Tc, Ti> businessPack, IIdentity identity);

        IManyResponse<Ti> GetAll(IBusinessPack<Tc, Ti> businessPack, IIdentity identity, IResponse priorResponse);

        #endregion

        #region GetSome

        IManyResponse<Ti> GetSome(ISearch search, IIdentity identity);

        IManyResponse<Ti> GetSome(IBusinessPack<Tc, Ti> businessPack, ISearch search, IIdentity identity, bool limit);

        IManyResponse<Ti> GetSome(IBusinessPack<Tc, Ti> businessPack, ISearch search, IIdentity identity, IResponse priorResponse, bool limit);

        #endregion

        #region GetByKey

        ISingleResponse<Ti> GetByKey(Ti item, IIdentity identity);

        ISingleResponse<Ti> GetByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity);

        ISingleResponse<Ti> GetByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region GetSomeByKey

        IManyResponse<Ti> GetSomeByKey(IEnumerable<Ti> items, IIdentity identity);

        IManyResponse<Ti> GetSomeByKey(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity);

        IManyResponse<Ti> GetSomeByKey(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse priorResponse);

        #endregion

        #region ExistsByKey

        ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity);

        ISingleResponse<bool> ExistsByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity);

        ISingleResponse<bool> ExistsByKey(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region IsAllowed

        ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity);

        ISingleResponse<bool> IsAllowed(IBusinessPack<Tc, Ti> businessPack, IRequestArg<Ti> argument, IIdentity identity);

        ISingleResponse<bool> IsAllowed(IBusinessPack<Tc, Ti> businessPack, IRequestArg<Ti> argument, IIdentity identity, IResponse priorResponse);

        #endregion

        #region GetSession

        ISingleResponse<Guid> GetSession(IIdentity identity);

        ISingleResponse<System.Guid> GetSession(IBusinessPack<Tc, Ti> businessPack, IIdentity identity);

        ISingleResponse<System.Guid> GetSession(IBusinessPack<Tc, Ti> businessPack, IIdentity identity, IResponse priorResponse);

        #endregion

        #region Update

        ISingleResponse<Ti> Update(Ti item, IIdentity identity);

        ISingleResponse<Ti> Update(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity);

        ISingleResponse<Ti> Update(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region UpdateSome

        IManyResponse<Ti> UpdateSome(IEnumerable<Ti> items, IIdentity identity);

        IManyResponse<Ti> UpdateSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity);

        IManyResponse<Ti> UpdateSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse priorResponse);

        #endregion

        #region Save

        ISaveResponse<Ti> Save(Ti item, IIdentity identity);

        ISaveResponse<Ti> Save(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity);

        ISaveResponse<Ti> Save(IBusinessPack<Tc, Ti> businessPack, Ti item, IIdentity identity, IResponse priorResponse);

        #endregion

        #region SaveSome

        ISaveSomeResponse<Ti> SaveSome(IEnumerable<Ti> items, IIdentity identity);

        ISaveSomeResponse<Ti> SaveSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity);

        ISaveSomeResponse<Ti> SaveSome(IBusinessPack<Tc, Ti> businessPack, IEnumerable<Ti> items, IIdentity identity, IResponse priorResponse);

        #endregion
    }
}