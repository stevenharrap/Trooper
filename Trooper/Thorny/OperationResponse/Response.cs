//--------------------------------------------------------------------------------------
// <copyright file="OperationResponse.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------


namespace Trooper.Thorny.OperationResponse
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using Trooper.Thorny.Interface.OperationResponse;
	using Trooper.Thorny.Utility;
    
    /// <summary>
    /// Implements the OperationResponse interface for replying to requests
    /// where the action nothing is required other than a positive of negative response.
    /// </summary>
    [Serializable]
    [DataContract]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class Response : IResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the response is ok. A false
        /// response is any response where the are Messages with Alert level as Error.
        /// A true response is any response where the are no Messages with Alert level as Error.
        /// </summary>
        [DataMember]
        public bool Ok 
        {
            get { return MessageUtility.IsOk(this.Messages); }
        }

        /// <summary>
        /// Gets or sets the errors that prevented the response from providing the items. 
        /// </summary>
        [DataMember]
        public List<IMessage> Messages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether warnings have been issued against the operation.
        /// </summary>
        [DataMember]
        public bool Warn
        {
            get { return MessageUtility.IsWarning(this.Messages); }
        }
    }
}
