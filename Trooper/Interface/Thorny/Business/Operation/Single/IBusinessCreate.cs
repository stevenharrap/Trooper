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
	public interface IBusinessCreate<TPoco> : IBusinessRequest<TPoco>
        where TPoco : class
    {
        [OperationContract]
        IAddResponse<TPoco> Add(TPoco item, IIdentity identity);

        [OperationContract]
        IAddSomeResponse<TPoco> AddSome(IEnumerable<TPoco> items, IIdentity identity);
    }
}