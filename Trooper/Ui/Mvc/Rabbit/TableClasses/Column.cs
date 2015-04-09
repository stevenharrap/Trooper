//--------------------------------------------------------------------------------------
// <copyright file="Column.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Rabbit.TableClasses
{
    using System;
using System.Linq.Expressions;
using System.Web.Helpers;

    /// <summary>
    /// 
    /// </summary>
    public class Column<T> where T : class 
    {
        public Expression<Func<T, object>> ValueExpression { get; set; }
                
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
    }
}
