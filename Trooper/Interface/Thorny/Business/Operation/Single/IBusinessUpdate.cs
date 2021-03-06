﻿//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCru.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;
using System.ServiceModel;
using System.Collections.Generic;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
	/// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read and Write operations and control
    /// access to those operations.
    /// </summary>
    [ServiceContract]
    public interface IBusinessUpdate<TPoco> : IBusinessRequest<TPoco>
        where TPoco : class
    {
        [OperationContract]
        ISingleResponse<TPoco> Update(TPoco item, IIdentity identity);

        [OperationContract]
        IManyResponse<TPoco> UpdateSome(IEnumerable<TPoco> items, IIdentity identity);
    }
}