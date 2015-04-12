using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trooper.Properties;
using Trooper.Ui.Interface.Mvc.Cruncher;
using Trooper.Ui.Mvc.Rabbit.Controls;
using Trooper.Ui.Mvc.Rabbit.TableClasses;
using Humanizer;
using System.Web.Helpers;
using Trooper.Ui.Interface.Mvc.Rabbit;
using Trooper.Ui.Interface.Mvc.Rabbit.Controls;
using Trooper.Utility;

namespace Trooper.Ui.Mvc.Rabbit
{
    public class Table<T>
        where T : class
    {
        private TableControl<T> tProps;

        private IHtml html;

        private ICruncher cruncher;

        public Table(TableControl<T> tProps, IHtml html, ICruncher cruncher)
        {
            this.cruncher = cruncher;
            this.tProps = tProps;
            this.html = html;

            this.html.RegisterControl(tProps);
        }

        public MvcHtmlString Render()
        {
            var result = new StringBuilder();
            var pagedSource = new PagedList<T>(this.tProps.Source.ToList(), this.tProps.TableModel.PageNumber, this.tProps.PageSize);

            var tableStyle = string.Format(
                "trooper table {0} {1} {2} {3}",
                this.tProps.Striped ? "table-striped" : null,
                this.tProps.Bordered ? "table-bordered" : null,
                this.tProps.Hover ? "table-hover" : null,
                this.tProps.Condensed ? "table-condensed" : null);

            var persistData =
                new
                {
                    this.tProps.TableModel.PageNumber,
                };

            result.AppendFormat(
                "<textarea style=\"display:none\" name=\"{0}.PersistedData\"/>{1}</textarea>\r\n",
                this.tProps.Name,
                Json.Encode(persistData));

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

        private void RenderHeader(StringBuilder result)
        {
            result.Append("<thead>\r\n");
            result.Append("<tr>\r\n");

            foreach (var col in this.tProps.Columns)
            {
                var header = this.GetColumnTitle(col);

                var dictionary = new Dictionary<string, string> {
                    { "si", ReflectionHelper.GetNameFromExpression(col.SortIdentity) },
                    { "h", header },
                    { "hm", col.HeaderMedium },
                    { "hs", col.HeaderSmall },
                    { "hes", col.HeaderExtraSmall },
                    { "hp", col.HeaderPrint }
                };
                                
                var dataValue = Json.Encode(dictionary).Replace("\"", "&quot;");

                result.AppendFormat("<th data-value=\"{0}\">{1}</th>\r\n", dataValue, header);
            }

            result.Append("</tr>\r\n");
            result.Append("</thead>\r\n");
        }

        private void RenderBody(StringBuilder result, PagedList<T> pagedSource)
        {
            result.Append("<tbody>\r\n");

            foreach (var row in pagedSource)
            {
                var keyResult = this.GetKeyAsJson(row);                

                result.AppendFormat("<tr data-value=\"{0}\">\r\n", keyResult);

                foreach (var col in this.tProps.Columns)
                {
                    var compiledValue = col.Value.Compile();
                    var value = compiledValue(row);

                    if (!string.IsNullOrEmpty(col.Format))
                    {
                        value = string.Format(col.Format, value);
                    }

                    result.AppendFormat("<td>{0}</td>", value);
                }

                result.Append("\r\n</tr>\r\n");
            }

            result.Append("</tbody>\r\n");
        }

        private string GetKeyAsJson(T row)
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

            return Json.Encode(keyResult).Replace("\"", "&quot;");
        }

        private void RenderFooter(StringBuilder result, PagedList<T> pagedSource)
        {
            if (!this.tProps.CanPage || pagedSource.PageCount == 1) {
                return;
            }

            result.Append("<tfoot>\r\n");

            result.AppendFormat("<tr>\r\n");
            result.AppendFormat("<td colspan=\"{0}\">", this.tProps.Columns.Count());

            result.Append("<div class=\"paging\">\r\n");
            result.Append("<div class=\"left\">\r\n");
            result.Append("<ul class=\"pagination\">\r\n");

            if (pagedSource.PageNumber > 10)
            {
                result.AppendFormat(
                    "<li data-value=\"{0}\"><a><span class=\"glyphicon glyphicon-triangle-left\"></span></a></li>\r\n", 
                    pagedSource.PageNumber - 10);
            }

            for (var p = pagedSource.PageNumber; p < pagedSource.PageCount && p < pagedSource.PageNumber + 10; p++)
            {
                if (p == pagedSource.PageNumber)
                {
                    result.AppendFormat("<li class=\"active\" data-value=\"{0}\"><a>{0} <span class=\"sr-only\">(current)</span></a></li>", p);
                }
                else {
                    result.AppendFormat("<li data-value=\"{0}\"><a>{0}</a></li>", p);
                }                
            }

            if (pagedSource.PageNumber + 10 < pagedSource.PageCount)
            {
                result.AppendFormat(
                    "<li data-value=\"{0}\"><a>{1} pages <span class=\"glyphicon glyphicon-triangle-right\"></span></a></li>", 
                    pagedSource.PageNumber + 10, 
                    pagedSource.PageCount);
            }

            result.Append("\r\n</ul>\r\n");
            result.Append("</div>\r\n");
            result.AppendFormat("<div class=\"right\">{0} items</div>\r\n", pagedSource.TotalItemCount);
            result.Append("</div>\r\n");
            result.Append("</td>\r\n");
            result.Append("</tr>\r\n");

            result.Append("</tfoot>\r\n");
        }

        private string GetColumnTitle(Column<T> column)
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

            var js = string.Format(
                "new trooper.ui.control.table({{ id: '{0}', rowSelectionMode: '{1}' }});",
                this.tProps.Id,
                this.tProps.RowSelectionMode);

            this.cruncher.AddJsInline(js, OrderOptions.Last);
        }
    }
}