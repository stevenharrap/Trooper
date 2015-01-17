namespace Trooper.App.Ui.StdTemplate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    using Trooper.App.Ui.StdTemplate.Classes;
    using Trooper.BusinessOperation.Business;

    public class StdTemplateHelper
    {
        /// <summary>
        /// The get navigation item link html.
        /// </summary>
        /// <param name="n">
        /// The navigation item.
        /// </param>
        /// <returns>
        /// The <see cref="MvcHtmlString"/>.
        /// </returns>
        public static MvcHtmlString GetNavItemLink(NavItem n)
        {
            var s = string.Format(
                "<a href=\"{0}\" class=\"{1}\" {2}{3}>{4}</a>",
                n.Url,
                n.ProgressBar ? string.Empty : "no-progress-bar",
                string.IsNullOrEmpty(n.Target) ? string.Empty : string.Format(" target=\"{0}\"", n.Target),
                string.IsNullOrEmpty(n.Tooltip) ? string.Empty : string.Format(" title=\"{0}\"", n.Tooltip),
                n.Title);

            return new MvcHtmlString(s);
        }

        public static MvcHtmlString RenderContextMenu(List<NavItem> navItems)
        {
            var sb = new StringBuilder();

            sb.Append("<ul class=\"contextual-nav nav nav-pills nav-stacked\">");

            foreach (var n in navItems)
            {
                if (n.NavItemType == NavItemTypes.Link)
                {
                    sb.Append(string.Format("<li class=\"{0}\">", n.Active ? "active" : string.Empty));
                    sb.Append(GetNavItemLink(n));

                    if (n.NavItems != null && n.NavItems.Any())
                    {
                        sb.Append("<ul class=\"contextual-nav nav nav-pills nav-stacked\">");

                        foreach (var sn in n.NavItems)
                        {
                            if (sn.NavItemType == NavItemTypes.Link)
                            {
                                sb.AppendFormat(
                                    "<li class=\"{0}\">{1}</li>",
                                    n.Active ? "active" : string.Empty,
                                    GetNavItemLink(sn));
                            }
                            else if (sn.NavItemType == NavItemTypes.Break)
                            {
                                sb.Append("<li><hr /></li>");
                            }
                            sb.Append("</ul>");
                        }

                        sb.Append("</li>");
                    }
                    else if (n.NavItemType == NavItemTypes.Break)
                    {
                        sb.Append("<li><hr /></li>");
                    }
                }
            }

            return new MvcHtmlString(sb.ToString());
        }
    }
}
