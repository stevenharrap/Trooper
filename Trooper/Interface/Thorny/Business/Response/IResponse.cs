//--------------------------------------------------------------------------------------
// <copyright file="IOperationResponse.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Interface.Thorny.Business.Response
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Trooper.Thorny;
    using Trooper.Thorny.Business.Response;

    /// <summary>
    /// The Operation Response interface. This defines the base requirements
    /// for a response.
    /// </summary>
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public interface IResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the response is ok. A false
        /// response is any response where the are Messages with Alert level as Error.
        /// A true response is any response where the are no Messages with Alert level as Error.
        /// </summary>
        bool Ok { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether warnings have been issued against the operation.
        /// </summary>
        bool Warn { get; set; }

        /// <summary>
        /// Gets or sets the any messages.
        /// </summary>
        List<Message> Messages { get; set; }
    }
}