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

namespace Trooper.Ui.Mvc.Rabbit
{
    public class Table<T> : HtmlControl
        where T : class
    {
        private TableControl<T> tProps;

        private ICruncher cruncher;

        public Table(TableControl<T> tProps, ICruncher cruncher)
        {
            this.tProps = tProps;
            this.cruncher = cruncher;            
        }

        public MvcHtmlString Render()
        {
            var result = new StringBuilder();
            var pagedSource = new PagedList<T>(this.tProps.Source.ToList(), 1, this.tProps.RowsPerPage);

            result.Append("<table class=\"trooper table\">\r\n");

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

                result.AppendFormat("<th>{0}</th>\r\n", header);
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
                    var compiledValue = col.ValueExpression.Compile();
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
                var name = this.GetNameFromExpression(k);

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
                result.Append("<li><a><span class=\"glyphicon glyphicon-triangle-left\"></span></a></li>\r\n");
            }

            for (var p = pagedSource.PageNumber; p < pagedSource.PageCount && p < pagedSource.PageNumber + 10; p++)
            {
                result.Append("<li>");
                result.AppendFormat("<a>{0}</a>", p);
                result.Append("</li>");
            }

            if (pagedSource.PageNumber + 10 < pagedSource.PageCount)
            {
                result.AppendFormat("<li><a>{0} pages <span class=\"glyphicon glyphicon-triangle-right\"></span></a></li>", pagedSource.PageCount);
            }

            result.Append("\r\n</ul>\r\n");
            result.Append("</div>\r\n");
            result.AppendFormat("<div class=\"right\">{0} items</div>\r\n", pagedSource.Count());
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

            string result = this.GetNameFromExpression(column.ValueExpression);

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

        private string GetNameFromExpression(Expression<Func<T, object>> expression)
        {
            var me = expression.Body as MemberExpression;
            var ue = expression.Body as UnaryExpression;
            string result = null;

            if (me != null)
            {
                result = me.Member.Name;
            }
            else if (ue != null)
            {
                result = (ue.Operand as MemberExpression).Member.Name;
            }

            return result;
        }

        private void JsLessLinkage()
        {
            if (!cruncher.HasJsItem("table_js"))
            {
                //cruncher.AddJsInline(Resources.table_js, "table_js", OrderOptions.Middle);

                cruncher.AddLessInline(Resources.table_less, "table_less", OrderOptions.Middle);
            }

            /*var js = string.Format(
                "new trooper.ui.control.table({{ id: '{0}', rowSelectionMode: '{1}' }});",
                this.Id,
                this.tProps.RowSelectionMode);

            cruncher.AddJsInline(js, OrderOptions.Last);*/
        }
    }
}