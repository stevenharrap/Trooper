namespace Trooper.BusinessOperation.Utility
{
    using Trooper.BusinessOperation.Business;

    public class ManageItemUtility
    {
        /// <summary>
        /// The delete.
        /// </summary>
        /// <returns>
        /// A ManageItem that is signaling that its item should be deleted.
        /// </returns>
        public static ManageItem<TItem> Delete<TItem>()
        {
            return new ManageItem<TItem> { Action = ManageAction.Delete };
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// A ManageItem that is signaling that its item should be deleted.
        /// </returns>
        public static ManageItem<TItem> Delete<TItem>(TItem item)
        {
            return new ManageItem<TItem> { Action = ManageAction.Delete, Item = item };
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <returns>
        /// A ManageItem that is signaling that its item should be removed as reference. I.e remove
        /// a record from a many-to-many situation.
        /// </returns>
        public static ManageItem<TItem> Remove<TItem>()
        {
            return new ManageItem<TItem> { Action = ManageAction.Remove };
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// A ManageItem that is signaling that its item should be removed as reference. I.e remove
        /// a record from a many-to-many situation.
        /// </returns>
        public static ManageItem<TItem> Remove<TItem>(TItem item)
        {
            return new ManageItem<TItem> { Action = ManageAction.Remove, Item = item };
        }

        /// <summary>
        /// The change.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// A ManageItem that is signaling that its item is changing or being added.
        /// </returns>
        public static ManageItem<TItem> Change<TItem>(TItem item)
        {
            return new ManageItem<TItem> { Action = ManageAction.Change, Item = item };
        }

        /// <summary>
        /// The is change.
        /// </summary>
        /// <param name="manageItem">
        /// The manage item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsChange<TItem>(ManageItem<TItem> manageItem)
        {
            return manageItem != null && manageItem.Action == ManageAction.Change;
        }

        /// <summary>
        /// The is delete.
        /// </summary>
        /// <param name="manageItem">
        /// The manage item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsDelete<TItem>(ManageItem<TItem> manageItem)
        {
            return manageItem != null && manageItem.Action == ManageAction.Delete;
        }

        /// <summary>
        /// The is remove.
        /// </summary>
        /// <param name="manageItem">
        /// The manage item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsRemove<TItem>(ManageItem<TItem> manageItem)
        {
            return manageItem != null && manageItem.Action == ManageAction.Remove;
        }

        /// <summary>
        /// The is remove.
        /// </summary>
        /// <param name="manageItem">
        /// The manage item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool HasItem<TItem>(ManageItem<TItem> manageItem)
        {
            return !object.ReferenceEquals(manageItem.Item, null);
        }
    }
}
