//--------------------------------------------------------------------------------------
// <copyright file="PanelGroupItem.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Rabbit
{
    using System;
    using System.Web.WebPages;

    /// <summary>
    /// The panel group item.
    /// </summary>
    public class PanelGroupItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the panel is expanded (visible).
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the panel should always be open always open.
        /// </summary>
        public bool AlwaysOpen { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public Func<object, HelperResult> Content { get; set; }

        /// <summary>
        /// Gets or sets the panel table. Appears below the content.
        /// </summary>
        public Func<object, HelperResult> Table { get; set; }
    }
}