﻿//--------------------------------------------------------------------------------------
// <copyright file="ManyResponse.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Interface.Business.Response
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.ServiceModel;

    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.Interface;

    /// <summary>
    /// The many response is an implementation of the IOperationResponse interface
    /// from responding to requests for many items of a particular type.
    /// </summary>
    /// <typeparam name="TEntity">The Type of the entities that will be returned.
    /// </typeparam>
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public interface IManyResponse<T> : IOperationResponse
    {
        /// <summary>
        /// Gets or sets the items being returned.
        /// </summary>
        [DataMember]
        IList<T> Items { get; set; }
    }
}
