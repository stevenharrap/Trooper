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

            this.JsLessLinkage();
        }

        public MvcHtmlString Render()
        {
            var result = new StringBuilder();
            var pagedSource = new PagedList<T>(this.tProps.Source.ToList(), 1, this.tProps.RowsPerPage);

            result.Append("<table class=\"trooper table\">");

            if (!string.IsNullOrWhiteSpace(this.tProps.Caption))
            {
                result.AppendFormat("<caption>{0}</caption>", this.tProps.Caption);
            }

            result.Append("<thead>");
            result.Append("<tr>");
            foreach (var col in this.tProps.Columns)
            {
                MemberExpression body = col.Mapping.Body as MemberExpression;

                result.AppendFormat("<th>{0}</th>", body.Member.Name);
            }
            result.Append("</tr>");
            result.Append("</thead>");

            result.Append("<tbody>");

            
            foreach (var row in pagedSource)
            {
                result.Append("<tr>");
                foreach (var col in this.tProps.Columns)
                {
                    var compiled = col.Mapping.Compile();

                    result.AppendFormat("<td>{0}</td>", compiled(row));
                }

                result.Append("</tr>");
            }

            result.Append("</tbody>");
            
            result.Append("<tfoot>");

            if (this.tProps.CanPage && pagedSource.PageCount > 1)
            {
                result.AppendFormat("<tr><td colspan=\"{0}\">{1}</td></tr>", this.tProps.Columns.Count(), this.FooterLinkage(pagedSource));
            }

            result.Append("</tfoot>");

            result.Append("</table>");

            return new MvcHtmlString(result.ToString());
        }

        private string FooterLinkage(PagedList<T> pagedSource)
        {
            var result = new StringBuilder();

            result.Append("<div class=\"paging\">");
            result.Append("<div class=\"left\">");
            result.Append("<ul class=\"pagination\">");

            if (pagedSource.PageNumber > 10)
            {
                result.Append("<li><a><span class=\"glyphicon glyphicon-triangle-left\"></span></a></li>");
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

            result.Append("</ul>");
            result.Append("</div>");
            result.AppendFormat("<div class=\"right\">{0} items</div>", pagedSource.Count());
            result.Append("</div>");

            return result.ToString();
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