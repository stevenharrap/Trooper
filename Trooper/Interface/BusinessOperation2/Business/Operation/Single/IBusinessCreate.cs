//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;
using Trooper.Interface.BusinessOperation2.Business.Operation.Core;
using Trooper.Interface.BusinessOperation2.Business.Response;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.Interface.BusinessOperation2.Business.Operation.Single
{
	public interface IBusinessCreate<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessValidate<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        IAddResponse<Ti> Add(Ti item, IIdentity identity = null);

        IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, IIdentity identity = null);
    }
}