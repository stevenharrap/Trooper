//--------------------------------------------------------------------------------------
// <copyright file="AddResponse.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Business.Response
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using Trooper.BusinessOperation2.Interface.Business.Response;
    
    /// <summary>
    /// The many response is an implementation of the IOperationResponse interface
    /// for responding to requests for 1 entity of a particular type.
    /// </summary>
    /// <typeparam name="TEntityKey">
    /// The key of the inserted entity.
    /// </typeparam>
    [Serializable]
    [DataContract]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class AddResponse<T> : Response, IAddResponse<T>
    {
        /// <summary>
        /// Gets or sets the item being returned.
        /// </summary>
        [DataMember]
        public T Item { get; set; }
    }
}