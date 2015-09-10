using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IGettingSome : IHelping
    {
        #region search is allowed

        /// <summary>
        ///     Response.Items = all items
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void ReturnsExpecteItemsSearchIsAllowedAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void ReturnsExpecteItemsSearchIsAllowedAndIdentityIsDenied();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void ReturnsExpecteItemsSearchIsAllowedAndIdentityIsNull();        

        #endregion

        #region search is not allowed

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = false
        ///     Response.Messages = [Search denied]
        /// </summary>
        void ReportsFailureWhenSearchIsNotAllowedAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void ReportsFailureWhenSearchIsNotAllowedAndIdentityIsDenied();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void ReportsFailureWhenSearchIsNotAllowedAndIdentityIsNull();

        #endregion

        #region search is null

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = false
        ///     Response.Messages = [Search not supplied]
        /// </summary>
        void ReportsFailureWhenSearchIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = false
        ///     Response.Messages = [Search not supplied]
        /// </summary>
        void ReportsFailureWhenSearchIsNullAndIdentityIsDenied();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = false
        ///     Response.Messages = [Search not supplied, Identity not supplied]
        /// </summary>
        void ReportsFailureWhenSearchIsNullAndIdentityIsNull();        

        #endregion
    }
}
