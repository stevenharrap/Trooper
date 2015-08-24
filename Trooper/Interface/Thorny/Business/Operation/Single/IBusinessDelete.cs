//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCrud.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;
using Trooper.Interface.Thorny.Business.Security;
using System.ServiceModel;
using Trooper.Interface.Thorny.Business.Response;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
    [ServiceContract]
	public interface IBusinessDelete<TPoco> : IBusinessRequest<TPoco>
        where TPoco : class
    {
        [OperationContract]
        IResponse DeleteByKey(TPoco item, IIdentity identity);

        [OperationContract]
        IResponse DeleteSomeByKey(IEnumerable<TPoco> items, IIdentity identity);
    }
}