//--------------------------------------------------------------------------------------
// <copyright file="ManyResponse.cs" company="Trooper Inc">
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
    /// from responding to requests for many items of a particular type.
    /// </summary>
    /// <typeparam name="TEntity">The Type of the entities that will be returned.
    /// </typeparam>
    [DataContract(Name = "ManyResponseOf{0}")]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class ManyResponse<T> : Response, IManyResponse<T>
    {
        public ManyResponse() { }

        public ManyResponse(IResponse response) : base(response)
        {
        }

        /// <summary>
        /// Gets or sets the items being returned.
        /// </summary>
        [DataMember]
        public IList<T> Items { get; set; }
    }
}
