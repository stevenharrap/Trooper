using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IDeletingSomeByKey : IHelping
    {
        #region Item exists

        /// <summary>
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void DoesRemoveItemWhenAnItemExistsAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotRemoveItemWhenAnItemExistsAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotRemoveItemWhenAnItemExistsAndIdentityIsNull();

        #endregion

        #region Item does not exist

        /// <summary>
        ///     Response.Ok = true
        ///     Response.Messages = emtpty
        /// </summary>
        void ReportSuccessWhenAbItemDoesNotExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void ReportsFailureWhenAnItemDoesNotExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void ReportsFailureWhenAnItemDoesNotExistAndIdentityIsNull();

        #endregion

        #region item is null

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied]
        /// </summary>
        void ReportsFailureWhenAnyItemIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied]
        /// </summary>
        void ReportsFailuredWhenAnyItemIsNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied, Identity not supplied]
        /// </summary>
        void ReportsFailureWhenAnyItemIsNullAndIdentityIsNull();

        #endregion              
    }
}
