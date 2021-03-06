﻿//--------------------------------------------------------------------------------------
// <copyright file="Error.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Thorny.Business.Response
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using Trooper.Interface.Thorny.Business.Response;

    /// <summary>
    /// Defines an error that can be returned in a service response.
    /// </summary>
    [DataContract]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class Message
    {
        /// <summary>
        /// Gets or sets the descriptive message of the error.
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// An alphanumeric code that can uniquely identify the message regardless of culture
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the entity (if any) that the error is related to.
        /// </summary>
        [DataMember]
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets the property of the entity (if any) that the error is related to.
        /// </summary>
        [DataMember]
        public string Property { get; set; }

        /// <summary>
        /// Gets or a sets a value indicate the nature of the message.
        /// </summary>
        [DataMember]
        public MessageAlertLevel Level { get; set; }        
    }
}