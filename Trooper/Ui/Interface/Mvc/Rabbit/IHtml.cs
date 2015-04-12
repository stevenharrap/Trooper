namespace Trooper.Ui.Interface.Mvc.Rabbit
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Trooper.BusinessOperation2.Interface.OperationResponse;
    using Trooper.Ui.Interface.Mvc.Cruncher;
    using Trooper.Ui.Interface.Mvc.Rabbit.Controls;
    using Trooper.Ui.Mvc.Rabbit;
    using Trooper.Ui.Mvc.Rabbit.Controls;

    public interface IHtml
    {
        IHtmlString ButtonDropDown(ButtonDropDown bddProps);

        Dictionary<string, IHtmlControl> ControlsRegister { get; }

        void IncludeBootstrap();

        void IncludeJquery();

        void IncludeJqueryInputMask();

        void IncludeJqueryUi();

        void IncludeMoment();

        IList<IMessage> Messages { get; set; }

        IHtmlString MessagesPanel(MessagesPanel mpProps);

        void ModalVirtualWindow(ModalWindow mwProps);

        IHtmlString ModalWindow(ModalWindow mwProps);

        IHtmlString PannelGroup(PannelGroup pgProps);

        IHtmlString Popover(Popover poProps);

        void RegisterControl(IHtmlControl control);
    }
}
