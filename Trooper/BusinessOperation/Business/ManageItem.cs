//--------------------------------------------------------------------------------------
// <copyright file="ManageItem.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Business
{
    /// <summary>
    /// The manage item.
    /// </summary>
    /// <typeparam name="TItem">
    /// The type of the item you are managing.
    /// </typeparam>
    public class ManageItem<TItem>
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public ManageAction Action { get; set; }

        /// <summary>
        /// Gets or sets the item of the ManageItem
        /// </summary>
        public TItem Item { get; set; }
    }
}
