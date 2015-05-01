//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCru.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.BusinessOperation2.Interface.OperationResponse;
using Trooper.Interface.BusinessOperation2.Business.Operation.Core;
using Trooper.Interface.BusinessOperation2.Business.Response;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.Interface.BusinessOperation2.Business.Operation.Single
{
	/// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read and Write operations and control
    /// access to those operations.
    /// </summary>
    public interface IBusinessUpdate<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessValidate<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        IAddResponse<Tc> Add(Ti item, IIdentity identity = null);

        IResponse Update(Ti item, IIdentity identity = null);

        ISaveResponse<Tc> Save(Ti item, IIdentity identity = null);
    }
}