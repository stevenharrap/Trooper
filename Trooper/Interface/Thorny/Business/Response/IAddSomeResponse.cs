//--------------------------------------------------------------------------------------
// <copyright file="AddSomeResponse.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using Trooper.Thorny;
using Trooper.Thorny.Interface.OperationResponse;

namespace Trooper.Interface.Thorny.Business.Response
{
	/// <summary>
    /// The many response is an implementation of the IOperationResponse interface
    /// for responding to requests for some entities of a particular type.
    /// </summary>
    /// <typeparam name="TEntityKey">
    /// The keys of the inserted entities.
    /// </typeparam>
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public interface IAddSomeResponse<T> : IResponse
    {
        /// <summary>
        /// Gets or sets the items being returned.
        /// </summary>
        [DataMember]
        IEnumerable<T> Items { get; set; }
    }
}