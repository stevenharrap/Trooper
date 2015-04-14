﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Trooper.Ui.Interface.Mvc.Rabbit.Table;
using Trooper.Ui.Mvc.Rabbit.Models;
using Trooper.Ui.Mvc.Rabbit.TableClasses;
using Trooper.Ui.Mvc.Rabbit.TableClasses.Footer;

namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    public class TableControl<T> : FormControl
        where T : class
    {
        public TableControl()
        {
            this.CanPage = true;
            this.PageSize = 50;
        }

        public List<Expression<Func<T, object>>> Keys { get; set; }

        public void AddKey(Expression<Func<T, object>> keyExpression)
        {
            if (this.Keys == null)
            {
                this.Keys = new List<Expression<Func<T, object>>>();
            }

            this.Keys.Add(keyExpression);
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        public IEnumerable<T> Source { get; set; }

        public IList<IColumn<T>> Columns {get; set;}

		/// <summary>
		/// Gets or sets the footer rows that appear above the paging options. This is just a collection
		/// of rows and columns - there is no relationship to the data types or data source that has been 
		/// provided to the table.
		/// </summary>
		public IList<Row> FooterRows { get; set; }

        public TableModel TableModel { get; set; }

        public IColumn<T> AddColumn(IColumn<T> column)
        {
            if (this.Columns == null)
            {
                this.Columns = new List<IColumn<T>>();
            }

            this.Columns.Add(column);

            return column;
        }               

        public string FormId { get; set; }

        public bool HumanizeHeaders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table can be sorted.
        /// </summary>
        public bool CanSort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table should appear striped.
        /// </summary>
        public bool Striped { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the rows should be bordered.
        /// </summary>
        public bool Bordered { get; set; }

        /// <summary>
        /// Gets or sets the number of rows per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table should allow paging. True by default.
        /// </summary>
        public bool CanPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether rows should be light up on mouse hover.
        /// </summary>
        public bool Hover { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the table should condensed to save space.
        /// </summary>
        public bool Condensed { get; set; }

        /// <summary>
        /// Gets or sets the row selection mode. Allows multiple row selection.
        /// </summary>
        public TableRowSelectionModes RowSelectionMode { get; set; }

        /// <summary>
        /// Gets or sets the caption for the table.
        /// </summary>
        public string Caption { get; set; }
    }
}
