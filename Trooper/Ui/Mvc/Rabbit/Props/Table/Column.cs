//--------------------------------------------------------------------------------------
// <copyright file="Column.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trooper.Ui.Interface.Mvc.Rabbit.Table;

namespace Trooper.Ui.Mvc.Rabbit.Props.Table
{
	/// <summary>
    /// 
    /// </summary>
    public class Column<T> : IColumn<T> where T : class 
    {
        public Expression<Func<T, object>> SortIdentity { get; set; }

        public Expression<Func<T, object>> Value { get; set; }
                
        /// <summary>
        /// Gets or sets a value indicating whether content in the column is allowed to wrap.
        /// By default header large is used.
        /// </summary>
        public bool NoWrap { get; set; }

        public bool? Humanize { get; set; }

        public string Format { get; set; }

        public string Header { get; set; }

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

	    public IList<ScreenMode> VisibleInModes { get; set; }
    }
}
