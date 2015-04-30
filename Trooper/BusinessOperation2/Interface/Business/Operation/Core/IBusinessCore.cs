//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Interface.Business.Operation.Core
{
    using Autofac;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Trooper.BusinessOperation2.Interface.Business.Response;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    public delegate IBusinessPack<Tc, Ti> BusinessPackHandler<Tc, Ti>()        
        where Tc : class, Ti, new()
        where Ti : class;

    /// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read and Add operations and control
    /// access to those operations.
    /// </summary>
    /// <typeparam name="TSearch">
    /// The search class to provide parameters to the GetSome method
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The class that contains the entity whole entity (but not navigation properties)
    /// </typeparam>
    /// <typeparam name="TEntityPrp">
    /// The class that contains the entity properties (that are not primary or foreign key properties)
    /// </typeparam>
    /// <typeparam name="TEntityKey">
    /// The key of the entity
    /// </typeparam>
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

        IResponse Update(Ti item, IIdentity identity);

        ISaveResponse<Ti> Save(Ti item, IIdentity identity);

        ISaveSomeResponse<Ti> SaveSome(IEnumerable<Ti> items, IIdentity identity);

        IResponse Validate(Ti item, IIdentity identity);       
    }
}