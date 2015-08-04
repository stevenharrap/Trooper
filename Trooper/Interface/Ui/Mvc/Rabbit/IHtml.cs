using System.Collections.Generic;
using System.Web;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Ui.Interface.Mvc.Rabbit.Props;
using Trooper.Ui.Mvc.Rabbit.Props;

namespace Trooper.Ui.Interface.Mvc.Rabbit
{
	public interface IHtml
    {
        IHtmlString ButtonDropDown(ButtonDropDownProps bddProps);

		Dictionary<string, IElementProps> ControlsRegister { get; }

        void IncludeBootstrap();

        void IncludeJquery();

        void IncludeJqueryInputMask();

        void IncludeJqueryUi();

        void IncludeMoment();

        IList<IMessage> Messages { get; set; }

        IHtmlString MessagesPanel(MessagesPanelProps mpProps);

        void ModalVirtualWindow(ModalWindowProps mwProps);

        IHtmlString ModalWindow(ModalWindowProps mwProps);

        IHtmlString PannelGroup(PannelGroupProps pgProps);

        IHtmlString Popover(PopoverProps poProps);

		void RegisterControl(IElementProps control);
    }
}
