using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trooper.Ui.Mvc.Rabbit;

namespace Trooper.Ui.Interface.Mvc.Rabbit.Table
{
	public interface IColumn<T>
		where T : class
	{
		Expression<Func<T, object>> SortIdentity { get; set; }

        string SortIdentityName { get; }

		Expression<Func<T, object>> Value { get; set; }

        SortDirection? SortDirection { get; set; }

        int SortImportance { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether content in the column is allowed to wrap.
		/// By default header large is used.
		/// </summary>
		bool NoWrap { get; set; }

		bool? Humanize { get; set; }

		string Format { get; set; }

		string Header { get; set; }

		/// <summary>
		/// Gets or sets the column header text when the screen is in the medium mode.
		/// </summary>
		string HeaderMedium { get; set; }

		/// <summary>
		/// Gets or sets the column header text when the screen is in the small mode.
		/// </summary>
		string HeaderSmall { get; set; }

		/// <summary>
		/// Gets or sets the column header text when the screen is in the medium mode.
		/// </summary>
		string HeaderExtraSmall { get; set; }

		/// <summary>
		/// Gets or sets the column header text when the screen is in the print mode.
		/// </summary>
		string HeaderPrint { get; set; }

		IList<ScreenMode> VisibleInModes { get; set; }
	}
}
