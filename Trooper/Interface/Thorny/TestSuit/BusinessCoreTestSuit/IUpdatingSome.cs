using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IUpdatingSome
    {
        #region Items are all valid and exist

        /// <summary>
        ///     Response.Items = updated items
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void DoesUpdateWhenAllItemsAreValidAndExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotUpdateWhenAllItemsAreValidAndExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotUpdateWhenAllItemsAreValidAndExistAndIdentityIsNull();

        #endregion

        #region Items are all valid and some dont exist

        /// <summary>
        ///     Response.Items = updated items
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void DoesUpdateWhenAllItemsAreValidAndSomeDoNotExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotUpdateWhenAllItemsAreValidAndSomeDoNotExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotUpdateWhenAllItemsAreValidAndSomeDoNotExistAndIdentityIsNull();

        #endregion

        #region Any items are invalid and all exist

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Some items invalid]
        /// </summary>
        void DoesNotUpdateWhenAnyItemsAreInvalidAndAllExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotUpdateWhenAnyItemsAreInvalidAllExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotUpdateWhenAnyItemsAreInvalidAndAllExistAndIdentityIsNull();

        #endregion

        #region Any items are invalid and some don't exist

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Some items invalid]
        /// </summary>
        void DoesNotUpdateWhenAnyItemsAreInvalidAndSomeDoNotExistAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Access denied]
        /// </summary>
        void DoesNotUpdateWhenAnyItemsAreInvalidSomeDoNotExistAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void DoesNotUpdateWhenAnyItemsAreInvalidAndSomeDoNotExistAndIdentityIsNull();

        #endregion

        #region Any items are null

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Some items null]
        /// </summary>
        void DoesNotUpdateWhenAnyItemsAreNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = true
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotUpdateWhenAnyItemsAreNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Item not supplied, Itentity not supplied]
        /// </summary>
        void DoesNotUpdateWhenAnyItemsAreNullAndIdentityIsNull();

        #endregion

        #region items is null

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotUpdateWhenItemsIsNullAndIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotUpdateWhenItemsIsNullAndIdentityIsNotAllowed();

        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = [Items not supplied]
        /// </summary>
        void DoesNotUpdateWhenItemsIsNullAndIdentityIsNull();

        #endregion        
    }
}
