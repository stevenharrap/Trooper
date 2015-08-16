using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IGettingSomeByKey
    {
        #region All keys exist

        /// <summary>
        ///     Response.Items = expected items
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void GetsExpectedItemWhenAllKeysExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void ReportsFailureWhenAllKeysExistAndIdentityIsDenied();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void ReportsFailureWhenAllKeysExistAndIdentityIsNull();

        #endregion

        #region some keys do not exist

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Some items are null]
        /// </summary>
        void ReportsFailureWhenSomeKeysDoNotExistAndIdentityIsAllowed();
        
        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void ReportsFailureWhenSomeKeysDoNotExistAndIdentityIsDenied();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void ReportsFailureWhenSomeKeysDoNotExistAndIdentityIsNull();

        #endregion

        #region some keys are null

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Somes keys not supplied]
        /// </summary>
        void ReportsFailureWhenSomesKeysAreNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Some keys not supplied]
        /// </summary>
        void ReportsFailureWhenSomesKeysAreNullAndIdentityIsDenied();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [some keys not supplied, Identity not supplied]
        /// </summary>
        void ReportsFailureWhenSomesKeysAreNullAndIdentityIsNull();

        #endregion

        #region keys are null

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Keys not supplied]
        /// </summary>
        void ReportsFailureWhenItemsIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Keys not supplied]
        /// </summary>
        void ReportsFailureWhenItemsIsNullAndIdentityIsDenied();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [keys not supplied, Identity not supplied]
        /// </summary>
        void ReportsFailureWhenItemsIsNullAndIdentityIsNull();

        #endregion
    }
}
