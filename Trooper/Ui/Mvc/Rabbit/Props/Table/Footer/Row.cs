//--------------------------------------------------------------------------------------
// <copyright file="Row.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Trooper.Ui.Mvc.Rabbit.Props.Table.Footer
{
	/// <summary>
    /// The row contains cells. Each row appears in the footer of the table above any
    /// paging options.
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Gets or sets any extra CSS classes on the TR. By default this will be "footer". Setting this
        /// property will add extra classes. 
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the cells that go in the row.
        /// </summary>
        public IList<Cell> Cells { get; set; }
    }
}