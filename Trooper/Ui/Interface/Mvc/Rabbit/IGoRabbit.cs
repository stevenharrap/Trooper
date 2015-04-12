namespace Trooper.Ui.Interface.Mvc.Rabbit
{
    using System;
using System.Web;
using System.Web.Mvc;
using Trooper.Ui.Interface.Mvc.Cruncher;
using Trooper.Ui.Mvc.Rabbit.Controls;

    public interface IGoRabbit<TModel>
    {
        ICruncher Cruncher { get; set; }

        HtmlHelper<TModel> HtmlHelper { get; set; }

        UrlHelper UrlHelper { get; set; }

        IForm<TModel> NewForm(FormControl formProps);

        IHtmlString Header();
    }
}
