//--------------------------------------------------------------------------------------
// <copyright file="AddSomeResponse.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Interface.Thorny.Business.Response;

namespace Trooper.Thorny.Business.Response
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.ServiceModel;

	/// <summary>
    /// The many response is an implementation of the IOperationResponse interface
    /// for responding to requests for some entities of a particular type.
    /// </summary>
    /// <typeparam name="TEntityKey">
    /// The keys of the inserted entities.
    /// </typeparam>
    [DataContract(Name = "AddSomeResponseOf{0}")]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class AddSomeResponse<T> : Response, IAddSomeResponse<T>
    {
        /// <summary>
        /// Gets or sets the item being returned.
        /// </summary>
        [DataMember]
        public IEnumerable<T> Items { get; set; }
    }
}