//--------------------------------------------------------------------------------------
// <copyright file="IBusinessR.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Thorny.Interface.DataManager;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
	/// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read operations and control
    /// access to those operations.
    /// </summary>
    public interface IBusinessRead<Tc, Ti> : IBusinessRequest<Tc, Ti>, IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        IManyResponse<Ti> GetAll(IIdentity identity = null);

        IManyResponse<Ti> GetSome(ISearch search, IIdentity identity = null);

        ISingleResponse<Ti> GetByKey(Ti item, IIdentity identity = null);

        ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity = null);        
    }
}