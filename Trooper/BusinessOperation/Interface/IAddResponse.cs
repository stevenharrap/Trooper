﻿//--------------------------------------------------------------------------------------
// <copyright file="IAddResponse.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Interface
{
    using System.ServiceModel;

    using Trooper.BusinessOperation.Business;

    /// <summary>
    /// The Operation Response interface. 
    /// </summary>
    /// <typeparam name="TEntityKey">
    /// Returned in the response and indicates the key of the inserted item.
    /// </typeparam>
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public interface IAddResponse<TEntityKey> : IOperationResponse
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// Gets or sets the key of the entity that has been inserted.
        /// </summary>
        TEntityKey EntityKey { get; set; }
    }
}