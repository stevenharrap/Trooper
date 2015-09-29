//--------------------------------------------------------------------------------------
// <copyright file="AddResponse.cs" company="Trooper Inc">
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
    /// for responding to requests for 1 entity of a particular type.
    /// </summary>
    /// <typeparam name="TEntityKey">
    /// The key of the inserted entity.
    /// </typeparam>
    [DataContract(Name = "AddResponseOf{0}")]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class AddResponse<T> : Response, IAddResponse<T>
    {
        public AddResponse() { }

        public AddResponse(IResponse response) : base(response) { } 

        /// <summary>
        /// Gets or sets the item being returned.
        /// </summary>
        [DataMember]
        public T Item { get; set; }
    }
}