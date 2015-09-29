//--------------------------------------------------------------------------------------
// <copyright file="SaveResponse.cs" company="Trooper Inc">
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
    [DataContract(Name = "SaveResponseOf{0}")]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class SaveResponse<T> : Response, ISaveResponse<T>
    {
        public SaveResponse() { }

        public SaveResponse(IResponse response) : base(response) { }

        /// <summary>
        /// Gets or sets the item being returned.
        /// </summary>
        [DataMember]
        public T Item { get; set; }

        /// <summary>
        /// Gets or sets the the change type which indicates if it was an Add or Update.
        /// Null indicates failure.
        /// </summary>
        [DataMember]
        public SaveChangeType Change { get; set; }
    }
}