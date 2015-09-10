using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IDeletingByKey : IHelping
    {
        #region Any item exists

        /// <summary>
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void DoesRemoveItemWhenItemExistsAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotRemoveItemWhenItemExistsAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotRemoveItemWhenItemExistsAndIdentityIsNull();

        #endregion

        #region An item does not exist

        /// <summary>
        ///     Response.Ok = true
        ///     Response.Messages = emtpty
        /// </summary>
        void ReportSuccessWhenItemDoesNotExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void ReportsFailureWhenItemDoesNotExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void ReportsFailureWhenItemDoesNotExistAndIdentityIsNull();

        #endregion

        #region An item is null

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied]
        /// </summary>
        void ReportsFailureWhenItemIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied]
        /// </summary>
        void ReportsFailuredWhenItemIsNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied, Identity not supplied]
        /// </summary>
        void ReportsFailureWhenItemIsNullAndIdentityIsNull();

        #endregion        
    }
}
