//--------------------------------------------------------------------------------------
// <copyright file="NavItem.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Ui.StdTemplate.Classes
{
    using System.Collections.Generic;

    /// <summary>
    ///     The navigation item types.
    /// </summary>
    public enum NavItemTypes
    {
        /// <summary>
        /// The break.
        /// </summary>
        Break, 

        /// <summary>
        /// The link.
        /// </summary>
        Link
    }

    /// <summary>
    ///     The navigation item.
    /// </summary>
    public class NavItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NavItem" /> class.
        /// </summary>
        public NavItem()
        {
            this.Url = "#";
            this.NavItemType = NavItemTypes.Link;
            this.ProgressBar = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the navigation item type.
        /// </summary>
        public NavItemTypes NavItemType { get; set; }

        /// <summary>
        ///     Gets or sets the navigation sub items.
        /// </summary>
        public List<NavItem> NavItems { get; set; }

        /// <summary>
        ///     Gets or sets the target of the link.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        ///     Gets or sets the title of the link.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the tooltip of the link.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        ///     Gets or sets the url of the link.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item should appear as the active page.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether progress bar.
        /// </summary>
        public bool ProgressBar { get; set; }

        #endregion
    }
}