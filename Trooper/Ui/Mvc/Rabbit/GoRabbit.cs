using Trooper.Ui.Mvc.Rabbit.Props;

namespace Trooper.Ui.Mvc.Rabbit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Trooper.Ui.Interface.Mvc.Cruncher;
    using Trooper.Ui.Interface.Mvc.Rabbit;
    using Trooper.Ui.Mvc.Rabbit.Controls;

    public class GoRabbit<TModel> : IGoRabbit<TModel>
    {
        public IHtml Html { get; set; }

        public ICruncher Cruncher { get; set; }

        public HtmlHelper<TModel> HtmlHelper { get; set; }

        public UrlHelper UrlHelper { get; set; }

        public GoRabbit(HtmlHelper<TModel> htmlHelper)
        {
            this.HtmlHelper = htmlHelper;
            this.UrlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            this.Cruncher = new Mvc.Cruncher.Cruncher(htmlHelper);
            this.Html = new Html<TModel>(this);
        }

        public GoRabbit(UrlHelper urlHelper, HtmlHelper<TModel> htmlHelper, ICruncher cruncher, IHtml html)
        {
            this.UrlHelper = urlHelper;
            this.HtmlHelper = HtmlHelper;
            this.Cruncher = cruncher;
            this.Html = html;
        }

        public IForm<TModel> NewForm(FormProps formProps)
        {
            return new Form<TModel>(formProps, this, this.Html);
        }

        public IHtmlString Header()
        {
            return this.Cruncher.Header();
        }
    }
}
