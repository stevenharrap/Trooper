//--------------------------------------------------------------------------------------
// <copyright file="AddSomeResponse.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Interface.Business.Response
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    /// <summary>
    /// The many response is an implementation of the IOperationResponse interface
    /// for responding to requests for some entities of a particular type.
    /// </summary>
    /// <typeparam name="TEntityKey">
    /// The keys of the inserted entities.
    /// </typeparam>
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public interface ISaveSomeResponse<T> : IResponse
    {
        /// <summary>
        /// Gets or sets the items being returned.
        /// </summary>
        [DataMember]
        IEnumerable<ISaveSomeItem<T>> Items { get; set; }
    }
}