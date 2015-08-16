using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IGettingByKey
    {
        #region key exists

        /// <summary>
        ///     Response.Item = expected item
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void GetsExpectedItemWhenKeyExistsAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void ReportsFailureWhenKeyExistsAndIdentityIsDenied();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void ReportsFailureWhenKeyExistsAndIdentityIsNull();

        #endregion 

        #region key does not exist

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item does not exist]
        /// </summary>
        void ReportsFailureWhenKeyDoesNotExistAndIdentityIsAllowed();
        
        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void ReportsFailureWhenKeyDoesNotExistAndIdentityIsDenied();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void ReportsFailureWhenKeyDoesNotExistAndIdentityIsNull();

        #endregion

        #region key is null

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Key not supplied]
        /// </summary>
        void ReportsFailureWhenKeyIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Key not supplied]
        /// </summary>
        void ReportsFailureWhenKeyIsNullAndIdentityIsDenied();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [key not supplied, Identity not supplied]
        /// </summary>
        void ReportsFailureWhenKeyIsNullAndIdentityIsNull();

        #endregion
    }
}
