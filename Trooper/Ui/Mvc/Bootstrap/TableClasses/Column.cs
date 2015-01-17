﻿//--------------------------------------------------------------------------------------
// <copyright file="Column.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Bootstrap.TableClasses
{
    using System.Web.Helpers;

    /// <summary>
    /// Overrides the default WebGridColumn and provide more control over column layout.
    /// </summary>
    public class Column : WebGridColumn
    {
        /// <summary>
        /// Gets or sets a value indicating whether content in the column is allowed to wrap.
        /// By default header large is used.
        /// </summary>
        public bool NoWrap { get; set; }

        /// <summary>
        /// Gets a value indicating whether the column has screen mode specific headers.
        /// </summary>
        public bool HasSpecificHeaders
        {
            get
            {
                return !string.IsNullOrEmpty(this.HeaderMedium)
                    || !string.IsNullOrEmpty(this.HeaderSmall)
                    || !string.IsNullOrEmpty(this.HeaderExtraSmall)
                    || !string.IsNullOrEmpty(this.HeaderPrint);
            }
        }

        /// <summary>
        /// Gets or sets the column header text when the screen is in the medium mode.
        /// </summary>
        public string HeaderMedium { get; set; }

        /// <summary>
        /// Gets or sets the column header text when the screen is in the small mode.
        /// </summary>
        public string HeaderSmall { get; set; }

        /// <summary>
        /// Gets or sets the column header text when the screen is in the medium mode.
        /// </summary>
        public string HeaderExtraSmall { get; set; }

        /// <summary>
        /// Gets or sets the column header text when the screen is in the print mode.
        /// </summary>
        public string HeaderPrint { get; set; }
    }
}
