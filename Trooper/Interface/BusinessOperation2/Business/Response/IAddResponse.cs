//--------------------------------------------------------------------------------------
// <copyright file="AddResponse.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.ServiceModel;
using Trooper.BusinessOperation.Business;
using Trooper.BusinessOperation2.Interface.OperationResponse;

namespace Trooper.Interface.BusinessOperation2.Business.Response
{
	/// <summary>
    /// The many response is an implementation of the IOperationResponse interface
    /// for responding to requests for 1 entity of a particular type.
    /// </summary>
    /// <typeparam name="TEntityKey">
    /// The key of the inserted entity.
    /// </typeparam>
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public interface IAddResponse<T> : IResponse
    {
        /// <summary>
        /// Gets or sets the item being returned.
        /// </summary>
        [DataMember]
        T Item { get; set; }
    }
}