using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface ISavingSome : IHelping
    {
        #region Items are all valid and exist

        /// <summary>
        ///     Response.Items = added items
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void DoesSaveWhenAllItemsAreValidAndExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotSaveWhenAllItemsAreValidAndExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotSaveWhenAllItemsAreValidAndExistAndIdentityIsNull();

        #endregion

        #region Items are all valid and some do not exist

        /// <summary>
        ///     Response.Items = added items
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void DoesSaveWhenAllItemsAreValidAndSomeDoNotExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotSaveWhenAllItemsAreValidAndSomeDoNotExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotSaveWhenAllItemsAreValidAndSomeDoNotExistAndIdentityIsNull();

        #endregion

        #region Any items are invalid

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Some items invalid]
        /// </summary>
        void DoesNotSaveWhenAnyItemsAreInvalidAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotSaveWhenAnyItemsAreInvalidAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotSaveWhenAnyItemsAreInvalidAndIdentityIsNull();

        #endregion

        #region Any items are null

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Some items null]
        /// </summary>
        void DoesNotSaveWhenAnyItemsAreNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotSaveWhenAnyItemsAreNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied, Itentity not supplied]
        /// </summary>
        void DoesNotSaveWhenAnyItemsAreNullAndIdentityIsNull();

        #endregion

        #region items is null

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotSaveWhenItemsIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotSaveWhenItemsIsNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotSaveWhenItemsIsNullAndIdentityIsNull();

        #endregion

        #region any item already exists

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [An item already exists]
        /// </summary>
        void DoesNotSaveWhenAnyItemAlreadyExistsAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotSaveWhenAnyItemAlreadyExistsAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotSaveWhenAnyItemAlreadyExistslAndIdentityIsNull();

        #endregion
    }
}
