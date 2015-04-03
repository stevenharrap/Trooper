using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trooper.Ui.Mvc.Rabbit.Controls;

namespace Trooper.Ui.Mvc.Rabbit
{
    public class Table<T> : HtmlControl
        where T : class
    {
        private TableControl<T> tProps; 

        public Table(TableControl<T> tProps)
        {
            this.tProps = tProps;
        }

        public MvcHtmlString Render()
        {
            var result = new StringBuilder();

            result.Append("<table class=\"table\">");

            if (!string.IsNullOrWhiteSpace(this.tProps.Caption))
            {
                result.AppendFormat("<caption>{0}</caption>", this.tProps.Caption);
            }

            result.Append("<thead>");
            result.Append("<tr>");
            foreach (var col in this.tProps.Columns)
            {
                result.AppendFormat("<th>{0}</th>", "C");
            }
            result.Append("</tr>");
            result.Append("</thead>");

            result.Append("<tbody>");

            foreach (var row in this.tProps.Source)
            {
                result.Append("<tr>");
                foreach (var col in this.tProps.Columns)
                {
                    result.AppendFormat("<td>{0}</td>", "D");
                }

                result.Append("</tr>");
            }

            result.Append("</tbody>");

            result.Append("<tfoot>");
            result.Append("</tfoot>");

            result.Append("</table>");

            return new MvcHtmlString(result.ToString());
        }
    }
}
