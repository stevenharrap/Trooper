//--------------------------------------------------------------------------------------
// <copyright file="IBusinessR.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Thorny.Interface.DataManager;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;
using System.ServiceModel;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
	/// <summary>
    /// Provides the means to expose your Model and Facade, wrap it in Read operations and control
    /// access to those operations.
    /// </summary>
    [ServiceContract]
    public interface IBusinessRead<TPoco> : IBusinessRequest<TPoco>
        where TPoco : class
    {
        [OperationContract]
        IManyResponse<TPoco> GetAll(IIdentity identity);

        [OperationContract]
        IManyResponse<TPoco> GetSome(ISearch search, IIdentity identity);

        [OperationContract]
        ISingleResponse<TPoco> GetByKey(TPoco item, IIdentity identity);

        [OperationContract]
        ISingleResponse<bool> ExistsByKey(TPoco item, IIdentity identity);        
    }
}