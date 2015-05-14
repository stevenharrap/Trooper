//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCrud.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;
using Trooper.Thorny.Interface.OperationResponse;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
	public interface IBusinessDelete<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        IResponse DeleteByKey(Ti item, IIdentity identity = null);

        IResponse DeleteSomeByKey(IEnumerable<Ti> items, IIdentity identity = null);
    }
}