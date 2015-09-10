using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface ISaving : IHelping
    {
        #region Item is valid and does exist

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

        #region Item is valid and does not exist

        /// <summary>
        ///     Response.Item = added item
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void DoesAddWhenItemIsValidAndDoesNotExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access Denied]
        /// </summary>
        void DoesNotAddWhenItemIsValidAndDoesNotExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotAddWhenItemIsValidAndDoesNotExistAndIdentityIsNull();

        #endregion

        #region Item is invalid and exists

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
        void DoesNotUpdateAddWhenItemIsInvalidAndExistsAndIdentityIsNull();

        #endregion

        #region Item is invalid and does not exist

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Invalid item]
        /// </summary>
        void DoesNotAddWhenItemIsInvalidAndDoesNotExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access Denied]
        /// </summary>
        void DoesNotAddWhenItemIsInvalidAndDoesNotExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotAddWhenItemIsInvalidAndDoesNotExistAndIdentityIsNull();

        #endregion

        #region item is null

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied]
        /// </summary>
        void DoesNotSaveWhenItemIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied]
        /// </summary>
        void DoesNotSaveWhenItemIsNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied, Itentity not supplied]
        /// </summary>
        void DoesNotSaveWhenItemIsNullAndIdentityIsNull();

        #endregion
    }
}
