//--------------------------------------------------------------------------------------
// <copyright file="Error.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Business
{
    using System;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
    using System.ServiceModel;

    using Microsoft.Ajax.Utilities;

    using Trooper.BusinessOperation.Interface;

    /// <summary>
    /// Defines an error that can be returned in a service response.
    /// </summary>
    [Serializable]
    [DataContract]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class Message : IMessage
    {
        /// <summary>
        /// Gets or sets the descriptive message of the error.
        /// </summary>
        [DataMember]
        public string Content { get; set; }

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
        public MessageAlertLevel Level { get; set; }        
    }
}