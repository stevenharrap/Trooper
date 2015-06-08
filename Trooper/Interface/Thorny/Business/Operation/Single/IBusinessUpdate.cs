//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCru.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Thorny.Interface.OperationResponse;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;
using System.ServiceModel;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
	/// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read and Write operations and control
    /// access to those operations.
    /// </summary>
    [ServiceContract]
    public interface IBusinessUpdate<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        [OperationContract]
        IAddResponse<Ti> Add(Ti item, IIdentity identity = null);

        [OperationContract]
        ISingleResponse<Ti> Update(Ti item, IIdentity identity = null);

        [OperationContract]
        ISaveResponse<Ti> Save(Ti item, IIdentity identity = null);
    }
}