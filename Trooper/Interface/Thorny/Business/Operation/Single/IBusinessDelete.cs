//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCrud.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Security;
using System.ServiceModel;
using Trooper.Interface.Thorny.Business.Response;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
    [ServiceContract]
	public interface IBusinessDelete<TEnt, TPoco> : IBusinessRequest<TEnt, TPoco>, IBusinessOperation<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        [OperationContract]
        IResponse DeleteByKey(TPoco item, IIdentity identity = null);

        [OperationContract]
        IResponse DeleteSomeByKey(IEnumerable<TPoco> items, IIdentity identity = null);
    }
}