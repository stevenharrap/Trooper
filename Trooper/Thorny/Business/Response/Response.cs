//--------------------------------------------------------------------------------------
// <copyright file="Response.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Thorny.Business.Response
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using Trooper.Thorny.Utility;
    using Trooper.Interface.Thorny.Business.Response;

    /// <summary>
    /// Implements the OperationResponse.Response interface for replying to requests
    /// where the action nothing is required other than a positive of negative response.
    /// </summary>
    [DataContract]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class Response : IResponse
    {
        private bool? ok;

        private bool? warn;

        /// <summary>
        /// Gets or sets a value indicating whether the response is ok. A false
        /// response is any response where the are Messages with Alert level as Error.
        /// A true response is any response where the are no Messages with Alert level as Error.
        /// </summary>
        [DataMember]
        public bool Ok 
        {
            get
            {
                if (this.ok != null)
                {
                    return (bool)this.ok;
                }
                
                return MessageUtility.IsOk(this.Messages);
            }

            set
            {
                this.ok = value;
            }
        }

        /// <summary>
        /// Gets or sets the errors that prevented the response from providing the items. 
        /// </summary>
        [DataMember]
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether warnings have been issued against the operation.
        /// </summary>
        [DataMember]
        public bool Warn
        {
            get
            {
                if (this.warn != null)
                {
                    return (bool)this.warn;
                }

                return MessageUtility.IsWarning(this.Messages);
            }

            set
            {
                this.warn = value;
            }
        }
    }
}
