//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Trooper.BusinessOperation2.Interface.DataManager;
using Trooper.BusinessOperation2.Interface.OperationResponse;
using Trooper.Interface.BusinessOperation2.Business.Response;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.Interface.BusinessOperation2.Business.Operation.Core
{
	public delegate IBusinessPack<Tc, Ti> BusinessPackHandler<Tc, Ti>()        
        where Tc : class, Ti, new()
        where Ti : class;

    public interface IBusinessCore<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        event BusinessPackHandler<Tc, Ti> OnRequestBusinessPack;

        IBusinessPack<Tc, Ti> GetBusinessPack();

        IAddResponse<Ti> Add(Ti item, IIdentity identity);

        IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, IIdentity identity);

        IResponse DeleteByKey(Ti item, IIdentity identity);

        IResponse DeleteSomeByKey(IEnumerable<Ti> items, IIdentity identity);

        IManyResponse<Ti> GetAll(IIdentity identity);

        IManyResponse<Ti> GetSome(ISearch search, IIdentity identity);

        ISingleResponse<Ti> GetByKey(Ti item, IIdentity identity);

        ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity);

        ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity);

        ISingleResponse<Guid> GetSession(IIdentity identity);

        IResponse Update(Ti item, IIdentity identity);

        ISaveResponse<Ti> Save(Ti item, IIdentity identity);

        ISaveSomeResponse<Ti> SaveSome(IEnumerable<Ti> items, IIdentity identity);

        ISingleResponse<bool> Validate(Ti item, IIdentity identity);       
    }
}