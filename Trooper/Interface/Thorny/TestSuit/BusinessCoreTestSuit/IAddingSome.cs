using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IAddingSome : IHelping
    {
        #region Items are all valid

        /// <summary>
        ///     Response.Items = added items
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void DoesAddWhenAllItemsDoNotExistAndAreValidAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotAddWhenAllItemsDoNotExistAndAreAreValidAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotAddWhenAllItemsDoNotExistAndAreValidAndIdentityIsNull();

        #endregion

        #region Any items are invalid

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Some items invalid]
        /// </summary>
        void DoesNotAddWhenAnyItemsAreInvalidAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotAddWhenAnyItemsAreInvalidAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotAddWhenAnyItemsAreInvalidAndIdentityIsNull();

        #endregion

        #region Any items are null

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Some items null]
        /// </summary>
        void DoesNotAddWhenAnyItemsAreNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotAddWhenAnyItemsAreNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied, Itentity not supplied]
        /// </summary>
        void DoesNotAddWhenAnyItemsAreNullAndIdentityIsNull();

        #endregion

        #region items is null

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotAddWhenItemsIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotAddWhenItemsIsNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotAddWhenItemsIsNullAndIdentityIsNull();

        #endregion

        #region any item already exists

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [An item already exists]
        /// </summary>
        void DoesNotAddWhenAnyItemAlreadyExistsAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotAddWhenAnyItemAlreadyExistsAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotAddWhenAnyItemAlreadyExistslAndIdentityIsNull();

        #endregion
    }
}
