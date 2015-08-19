using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IUpdating
    {
        #region Item exists and is valid

        /// <summary>
        ///     Response.Item = added item
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void DoesUpdateWhenItemIsValidAndExistsAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access Denied]
        /// </summary>
        void DoesNotUpdateWhenItemIsValidAndExistsAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotUpdateWhenItemIsValidAndExistsAndIdentityIsNull();

        #endregion

        #region Item exista and is invalid

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Invalid item]
        /// </summary>
        void DoesNotUpdateWhenItemIsInvalidAndExistsAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access Denied]
        /// </summary>
        void DoesNotUpdateWhenItemIsInvalidAndExistsAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotUpdateWhenItemIsInvalidAndExistsAndIdentityIsNull();

        #endregion

        #region item is null

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied]
        /// </summary>
        void DoesNotUpdateWhenItemIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied]
        /// </summary>
        void DoesNotUpdateWhenItemIsNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied, Itentity not supplied]
        /// </summary>
        void DoesNotUpdateWhenItemIsNullAndIdentityIsNull();

        #endregion

        #region item does not exist

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item already exists]
        /// </summary>
        void DoesNotUpdateWhenItemDoesNotExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotUpdateWhenItemDoesNotExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotUpdateWhenItemDoesNotExistAndIdentityIsNull();

        #endregion
    }
}
