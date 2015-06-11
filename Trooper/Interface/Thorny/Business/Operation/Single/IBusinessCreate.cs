//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ServiceModel;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
    [ServiceContract]
	public interface IBusinessCreate<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        [OperationContract]
        IAddResponse<Ti> Add(Ti item, IIdentity identity);

        [OperationContract]
        IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, IIdentity identity);
    }
}