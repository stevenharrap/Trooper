using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IAdding : IHelping
    {
        #region Item is valid

        /// <summary>
        ///     Response.Item = added item
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void DoesAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access Denied]
        /// </summary>
        void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNull();

        #endregion

        #region Item is invalid

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Invalid item]
        /// </summary>
        void DoesNotAddWhenItemIsInvalidAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access Denied]
        /// </summary>
        void DoesNotAddWhenItemIsInvalidAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotAddWhenItemIsInvalidAndIdentityIsNull();

        #endregion

        #region item is null

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied]
        /// </summary>
        void DoesNotAddWhenItemIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied]
        /// </summary>
        void DoesNotAddWhenItemIsNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied, Itentity not supplied]
        /// </summary>
        void DoesNotAddWhenItemIsNullAndIdentityIsNull();

        #endregion

        #region item already exists

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Add failed]
        /// </summary>
        void DoesNotAddWhenItemAlreadyExistsAndIdentityIsAllowed();        

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotAddWhenItemAlreadyExistsAndIdentityIsNotAllowed();        

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        ///     
        ///     The existing item should not change
        /// </summary>
        void DoesNotAddWhenItemAlreadyExistslAndIdentityIsNull();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Add failed]
        /// </summary>
        void ItemDoesNotChangeWhenAddedItemAlreadyExistsAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void ItemDoesNotChangeWhenAddedItemAlreadyExistsAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item already exists]
        /// </summary>
        void ItemDoesNotChangeWhenAddedItemAlreadyExistslAndIdentityIsNull();

        #endregion
    }
}
