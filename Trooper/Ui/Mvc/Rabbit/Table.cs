using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using Humanizer;
using PagedList;
using Trooper.Properties;
using Trooper.Ui.Interface.Mvc.Cruncher;
using Trooper.Ui.Interface.Mvc.Rabbit;
using Trooper.Ui.Interface.Mvc.Rabbit.Table;
using Trooper.Ui.Mvc.Rabbit.Props;
using Trooper.Ui.Mvc.Utility;
using Trooper.Utility;
using Trooper.Ui.Mvc.Rabbit.Props.Table;
using Trooper.Ui.Mvc.Rabbit.Models.Table;
using Trooper.Ui.Mvc.Rabbit.Props.Table.Body;

namespace Trooper.Ui.Mvc.Rabbit
{
    public class Table<T>
        where T : class
    {
        private TableProps<T> tProps;

        private PagedList<T> pagedSource;

        private IHtml html;

        private ICruncher cruncher;

        public Table(TableProps<T> tProps, IHtml html, ICruncher cruncher)
        {
            this.cruncher = cruncher;
            this.html = html;
            this.tProps = tProps;
            
            this.html.RegisterControl(tProps);
            this.tProps.TableModel.Sorting = tProps.Columns;
            this.pagedSource = this.MakePagedSource();
        }

        public MvcHtmlString Render()
        {
            var result = new StringBuilder();            

            var tableStyle = string.Format(
                "trooper table {0} {1} {2} {3}",
                this.tProps.Striped ? "table-striped" : null,
                this.tProps.Bordered ? "table-bordered" : null,
                this.tProps.Hover ? "table-hover" : null,
                this.tProps.Condensed ? "table-condensed" : null);

            var persistedTable = new PersistedTableModel
            {
                Sorting = this.tProps.Columns
                    .Where(c => c.SortIdentityName != null)
                    .ToDictionary(k => k.SortIdentityName, v => new SortInfo { Direction = v.SortDirection, Importance = v.SortImportance }),
                PageNumber = this.tProps.TableModel.PageNumber,
                Selected = this.tProps.TableModel.Selected.Select(s => this.GetKeyAsDictionary(s)).ToArray()
            };                    

            result.AppendFormat(
                "<textarea class=\"trooper table-persisted-data\" name=\"{0}.PersistedData\"/>{1}</textarea>\r\n",
                this.tProps.Name,
                Json.Encode(persistedTable));

            result.AppendFormat("<table id=\"{0}\" class=\"{1}\">\r\n", this.tProps.Id, tableStyle);

            if (!string.IsNullOrWhiteSpace(this.tProps.Caption))
            {
                result.AppendFormat("<caption>{0}</caption>\r\n", this.tProps.Caption);
            }

            this.RenderHeader(result);

            this.RenderBody(result, pagedSource);            

            this.RenderFooter(result, pagedSource);

            result.Append("</table>\r\n");

            this.JsLessLinkage();

            return new MvcHtmlString(result.ToString());
        }

        private PagedList<T> MakePagedSource()
        {
            var orderBy = from c in this.tProps.Columns
                          where c.SortDirection != null
                          orderby c.SortImportance descending
                          let direction = c.SortDirection == SortDirection.Ascending
                            ? "ASC"
                            : c.SortDirection == SortDirection.Descending
                            ? "DESC"
                            : string.Empty
                          select string.Format("{0} {1}", c.SortIdentityName, direction);

            var ordered = orderBy.Any()
                ? this.tProps.TableModel.Source.AsQueryable().OrderBy(string.Join(", ", orderBy))
                : this.tProps.TableModel.Source.AsQueryable();

            var pagedSource = new PagedList<T>(
                ordered.ToList(),
                this.tProps.TableModel.PageNumber,
                this.tProps.PageSize);

            return pagedSource;
        }

        private void RenderHeader(StringBuilder result)
        {
            result.Append("<thead>\r\n");
            result.Append("<tr>\r\n");

            for (var c = 0; c<this.tProps.Columns.Count; c++)
            {
	            var col = this.tProps.Columns[c];
                var header = this.GetColumnTitle(col);
                var css = RabbitHelper.MakeClassAttribute(new string[] 
                { 
                    string.Format("col-{0}", c), 
                    col.SortIdentity == null ? null : "is-sortable" 
                });

                result.AppendFormat("<th {0}>", css);
                result.AppendFormat("<span class=\"title\">{0}</span>", header);

                if (col.SortIdentityName != null)
                {
                    if (col.SortDirection == SortDirection.Ascending)
                    {
                        result.Append(" <span class=\"glyphicon glyphicon-triangle-top\"></span>");
                    }
                    else if (col.SortDirection == SortDirection.Descending)
                    {
                        result.Append(" <span class=\"glyphicon glyphicon-triangle-bottom\"></span>");
                    }
                }

                result.Append("</th>\r\n");
            }

            result.Append("</tr>\r\n");
            result.Append("</thead>\r\n");
        }

        private void RenderBody(StringBuilder result, PagedList<T> pagedSource)
        {
            result.Append("<tbody>\r\n");

            foreach (var row in pagedSource)
            {
                var keyResult = this.GetKeyAsJson(row, true);
                var rowFormat = this.tProps.RowFormatter == null ? null : this.tProps.RowFormatter(row);

                var css = rowFormat == null ? null : RabbitHelper.MakeClassAttribute(new string[] 
                { 
                    rowFormat.Highlighted != RowHighlight.None ? rowFormat.Highlighted.ToString().ToLower() : null,
                    rowFormat.Bold ? "bold" : null,
                    rowFormat.RowTextHighlightStyle != RowTextHighlight.None ? string.Format("text-{0}", rowFormat.RowTextHighlightStyle.ToString().ToLower()) : null,
                    rowFormat.RuleUnderStyle != RuleStyle.Default ? string.Format("rule-under rule-under-{0}", rowFormat.RuleUnderStyle.ToString().ToLower()) : null,
                    rowFormat.RuleOverStyle != RuleStyle.Default ? string.Format("rule-over rule-over-{0}", rowFormat.RuleOverStyle.ToString().ToLower()) : null,
                    this.tProps.RowSelectionMode == TableRowSelectionModes.None ? null : "is-selectable",
                    this.tProps.TableModel.Selected.Contains(row) ? "selected" : null
                });

                result.AppendFormat("<tr data-value=\"{0}\" {1}>\r\n", keyResult, css);

				for (var c = 0; c < this.tProps.Columns.Count; c++)
				{
					var col = this.tProps.Columns[c];
                    var compiledValue = col.Value.Compile();
                    var value = compiledValue(row);

                    if (!string.IsNullOrEmpty(col.Format))
                    {
                        value = string.Format(col.Format, value);
                    }

                    result.AppendFormat("<td class=\"col-{0}\">{1}</td>", c, value);
                }

                result.Append("\r\n</tr>\r\n");
            }

            result.Append("</tbody>\r\n");
        }

        private string GetKeyAsJson(T row, bool quote)
        {
            var result = this.GetKeyAsDictionary(row);

            if (quote)
            {
                return Json.Encode(result).Replace("\"", "&quot;");
            }

            return Json.Encode(result);
        }

        private Dictionary<string, object> GetKeyAsDictionary(T row)
        {
            if (this.tProps.Keys == null || !this.tProps.Keys.Any())
            {
                return null;
            }

            var keyResult = new Dictionary<string, object>();

            foreach (var k in this.tProps.Keys)
            {
                var name = ReflectionHelper.GetNameFromExpression(k);

                if (name == null)
                {
                    continue;
                }

                var compiledKey = k.Compile();
                keyResult.Add(name, compiledKey(row));
            }

            return keyResult;
        }

        private void RenderFooter(StringBuilder result, PagedList<T> pagedSource)
        {
			result.Append("<tfoot>\r\n");

			this.RenderFooterRows(result);

			this.RenderFooterPaging(result, pagedSource);

			result.Append("</tfoot>\r\n");
        }

	    private void RenderFooterRows(StringBuilder result)
	    {
            if (this.tProps.FooterRows == null)
            {
                return;
            }

			foreach (var r in this.tProps.FooterRows)
			{
				result.AppendFormat("<tr {0}>\r\n", RabbitHelper.MakeClassAttribute(new []{ "footer", r.Class }));

				for (var c = 0; c < r.Cells.Count; c++)
				{
					var cell = r.Cells[c];

					var attrs = new Dictionary<string, string>
					{
						{"class", RabbitHelper.MakeClassAttributeContent(new[] {cell.Class, string.Format("col-{0}", c)})}
					};

					if (cell.ColSpan > 0)
					{
						attrs.Add("colspan", cell.ColSpan.ToString());
					}

					result.AppendFormat("<td {0}>", RabbitHelper.MakeAttributesList(attrs));
					result.Append(cell.Content);
					result.Append("</td>\r\n");
				}

				result.Append("</tr>\r\n");
			}
	    }

	    private void RenderFooterPaging(StringBuilder result, PagedList<T> pagedSource)
	    {
			if (!this.tProps.CanPage || pagedSource.PageCount == 1)
			{
				return;
			}

            var pn = pagedSource.PageNumber - 1;
            var remainder = pn % this.tProps.PagesSize;
            var lower = pn - remainder;
            var higher = lower + this.tProps.PagesSize;
            lower = lower <= 0 ? 1 : lower;
            lower = lower >= this.tProps.PagesSize ? lower + 1 : lower;
            higher = higher > pagedSource.PageCount ? pagedSource.PageCount : higher;

			result.AppendFormat("<tr>\r\n");
			result.AppendFormat("<td colspan=\"{0}\">", this.tProps.Columns.Count());

			result.Append("<div class=\"paging\">\r\n");
			result.Append("<div class=\"left\">\r\n");
			result.Append("<ul class=\"pagination\">\r\n");

            if (pagedSource.PageNumber > this.tProps.PagesSize)
			{
				result.AppendFormat(
					"<li data-value=\"{0}\"><a><span class=\"glyphicon glyphicon-triangle-left\"></span></a></li>\r\n",
					lower - 1);
			}            

			for (var p = lower; p <= higher; p++)
			{
				if (p == pagedSource.PageNumber)
				{
					result.AppendFormat("<li class=\"active\" data-value=\"{0}\"><a>{0} <span class=\"sr-only\">(current)</span></a></li>", p);
				}
				else
				{
					result.AppendFormat("<li data-value=\"{0}\"><a>{0}</a></li>", p);
				}
			}

			if (higher < pagedSource.PageCount)
			{
				result.AppendFormat(
					"<li data-value=\"{0}\"><a>{1} pages <span class=\"glyphicon glyphicon-triangle-right\"></span></a></li>",
					higher + 1,
					pagedSource.PageCount);
			}

			result.Append("\r\n</ul>\r\n");
			result.Append("</div>\r\n");
			result.AppendFormat("<div class=\"right\">{0} items</div>\r\n", pagedSource.TotalItemCount);
			result.Append("</div>\r\n");
			result.Append("</td>\r\n");
			result.Append("</tr>\r\n");
	    }

        private string GetColumnTitle(IColumn<T> column)
        {
            if (!string.IsNullOrEmpty(column.Header))
            {
                return column.Header;
            }

            string result = ReflectionHelper.GetNameFromExpression(column.Value);

            if (result == null)
            {
                return string.Empty;
            }                       

            if ((this.tProps.HumanizeHeaders && column.Humanize == null) || (column.Humanize != null && (bool)column.Humanize))
            {
                result = result.Humanize();
            }

            return result;
        }        

        private void JsLessLinkage()
        {
            if (!this.cruncher.HasJsItem("table_js"))
            {
                this.cruncher.AddJsInline(Resources.table_js, "table_js", OrderOptions.Middle);

                this.cruncher.AddLessInline(Resources.table_less, "table_less", OrderOptions.Middle);
            }

	        var colIndex = 0;

	        var columns =
		        this.tProps.Columns.Select(
			        c =>
				        new
				        {
							ci = colIndex++,
					        h = this.GetColumnTitle(c),
					        hes = c.HeaderExtraSmall,
					        hm = c.HeaderMedium,
					        hp = c.HeaderPrint,
					        hs = c.HeaderSmall,
							vim = c.VisibleInModes == null ? null : c.VisibleInModes.Select(v => v.ToString()),
					        si = ReflectionHelper.GetNameFromExpression(c.SortIdentity)
				        });

            var js = string.Format(
                "new trooper.ui.control.table({{ id: '{0}', formId: '{1}', name: '{2}.PersistedData', rowSelectionMode: '{3}', columns: {4}, postAction: {5} }});",
                this.tProps.Id,
                this.tProps.FormId,
                this.tProps.Name,
                this.tProps.RowSelectionMode,
				Json.Encode(columns),
                RabbitHelper.MakeJsStringParameter(this.tProps.PostAction));


            this.cruncher.AddJsInline(js, OrderOptions.Last);
        }
    }
}