﻿//--------------------------------------------------------------------------------------
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
    [DataContract(Name = "SaveSomeResponseOf{0}")]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class SaveSomeResponse<T> : Response, ISaveSomeResponse<T>
    {
        public SaveSomeResponse() { }

        public SaveSomeResponse(IResponse response) : base(response) { }

        [DataMember]
        public IEnumerable<ISaveSomeItem<T>> Items { get; set; }
    }
}