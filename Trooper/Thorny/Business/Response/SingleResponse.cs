﻿//--------------------------------------------------------------------------------------
// <copyright file="SingleResponse.cs" company="Trooper Inc">
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
    /// <typeparam name="TEntity">
    /// The type of entity to return
    /// </typeparam>
    [DataContract(Name = "SingleResponseOf{0}")]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class SingleResponse<T> : Response, ISingleResponse<T>
    {
        public SingleResponse() { }

        public SingleResponse(IResponse response) : base(response) { }

        /// <summary>
        /// Gets or sets the item being returned.
        /// </summary>
        [DataMember]
        public T Item { get; set; }
    }
}