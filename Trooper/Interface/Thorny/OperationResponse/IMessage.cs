//--------------------------------------------------------------------------------------
// <copyright file="IMessage.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Thorny.Interface.OperationResponse
{
    using System.ServiceModel;

    /// <summary>
    /// Defines an error that can be returned in a service response.
    /// </summary>
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public interface IMessage
    {
        /// <summary>
        /// Gets or sets the descriptive message of the error.
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// An alphanumeric code that can uniquely identify the message regardless of culture
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// Gets or sets the entity (if any) that the error is related to.
        /// </summary>
        string Entity { get; set; }

        /// <summary>
        /// Gets or sets the property of the entity (if any) that the error is related to.
        /// </summary>
        string Property { get; set; }

        /// <summary>
        /// Gets or sets a value indicate the nature of the message.
        /// </summary>
        MessageAlertLevel Level { get; set; }
    }
}