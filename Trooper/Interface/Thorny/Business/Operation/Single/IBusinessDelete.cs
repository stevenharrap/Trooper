//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCrud.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;
using Trooper.Thorny.Interface.OperationResponse;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Security;
using System.ServiceModel;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
    [ServiceContract]
	public interface IBusinessDelete<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        [OperationContract]
        IResponse DeleteByKey(Ti item, IIdentity identity = null);

        [OperationContract]
        IResponse DeleteSomeByKey(IEnumerable<Ti> items, IIdentity identity = null);
    }
}