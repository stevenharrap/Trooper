using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IIsAllowing : IHelping
    {
        /// <summary>
        ///     Response.Item = false
        ///     Response.Ok = true
        ///     Response.Messages = [No access]
        /// </summary>
        void ReportFailureWhenArgIsValidAndUserCannotUseOperation();

        /// <summary>
        ///     Response.Item = false
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void ReportFailureWhenArgIsValidAndIdentityIsNull();

        /// <summary>
        ///     Response.Item = true
        ///     Response.Ok = true
        ///     Response.Messages = emtpy
        /// </summary>
        void ReportSuccessWhenArgIsValidAndUserCanUseOperation();

        /// <summary>
        ///     Response.Item = false
        ///     Response.Ok = false
        ///     Response.Messages = [Arg not supplied]
        /// </summary>
        void ReportFailureWhenArgIsNullAndIdentityIsValid();

        /// <summary>
        ///     Response.Item = false
        ///     Response.Ok = false
        ///     Response.Messages = [Arg not supplied, Identity not supplied]
        /// </summary>
        void ReportFailureWhenArgIsNullAndIdentityIsNull();        
    }
}