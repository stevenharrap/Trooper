//--------------------------------------------------------------------------------------
// <copyright file="SingleResponse.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.ServiceModel;
using Trooper.BusinessOperation2;
using Trooper.BusinessOperation2.Interface.OperationResponse;

namespace Trooper.Interface.BusinessOperation2.Business.Response
{
	/// <summary>
    /// The many response is an implementation of the IOperationResponse interface
    /// for responding to requests for 1 entity of a particular type.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity to return
    /// </typeparam>
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public interface ISingleResponse<T> : IResponse
    {
        /// <summary>
        /// Gets or sets the item being returned.
        /// </summary>
        [DataMember]
        T Item { get; set; }
    }
}