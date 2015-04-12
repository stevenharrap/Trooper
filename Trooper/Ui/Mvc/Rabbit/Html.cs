//--------------------------------------------------------------------------------------
// <copyright file="Html.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Text;
using Trooper.BusinessOperation2.Utility;

namespace Trooper.Ui.Mvc.Rabbit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.WebPages;
    using Trooper.BusinessOperation2;
    using Trooper.BusinessOperation2.Interface.OperationResponse;
    using Trooper.Properties;
    using Trooper.Ui.Interface.Mvc.Cruncher;
    using Trooper.Ui.Mvc.Rabbit.Controllers;
    using Trooper.Ui.Mvc.Rabbit.Controls;
    using Trooper.Ui.Mvc.Cruncher;
    using Trooper.Utility;
    using Trooper.Ui.Interface.Mvc.Rabbit;
    using Trooper.Ui.Interface.Mvc.Rabbit.Controls;
    using Trooper.Ui.Mvc.Utility;

    /// <summary>
    /// Bootstrap is a CSS library from Twitter. It is very good at Html5 layout and provides
    /// flexible device independent display. This class provides none-form related UI elements
    /// and will automatically inject the Jquery and Bootstrap client side requirements using 
    /// Cruncher.
    /// <see cref="http://twitter.github.io/bootstrap/"/>
    /// </summary>
    /// <typeparam name="TModel">
    /// The model type in your view
    /// </typeparam>
    public class Html<TModel> : IHtml
    {
        #region private fields

        private const string registerName = "RabbitControlsRegister";

        private IGoRabbit<TModel> goRabbit;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Html{TModel}"/> class and includes all
        /// Bootstrap JavaScript and CSS requirements.
        /// </summary>
        /// <param name="htmlHelper">
        /// The html helper.
        /// </param>
        public Html(IGoRabbit<TModel> goRabbit)
        {
            this.goRabbit = goRabbit;

            this.IncludeJquery();
			this.IncludeBootstrap();

            if (!this.goRabbit.Cruncher.HasJsItem("trooper"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.trooper_js, "trooper", OrderOptions.Middle);
                this.goRabbit.Cruncher.AddLessInline(Resources.trooper_less, "trooper_less", OrderOptions.Middle);
            }
        }

		#region public properties

        public Dictionary<string, IHtmlControl> ControlsRegister
        {
            get
            {
                if (HttpContext.Current.Items.Contains(registerName))
                {
                    return HttpContext.Current.Items[registerName] as Dictionary<string, IHtmlControl>;
                }

                HttpContext.Current.Items[registerName] = new Dictionary<string, IHtmlControl>();

                return HttpContext.Current.Items[registerName] as Dictionary<string, IHtmlControl>;
            }

            private set
            {
                HttpContext.Current.Items[registerName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the errors that may be present. Providing errors to
        /// any specific control overrides this.
        /// </summary>
        public IList<IMessage> Messages { get; set; }        

		#endregion

		#region public methods

        #region controls
        
        /// <summary>
        /// Makes a panel group where each 
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="errors">
        /// If your page is presenting errors then supply them. ErrorMode will indicate how you want the panel group to react.
        /// </param>
        /// <param name="errorMode">
        /// How should the panel group react to errors.
        /// </param>
        /// <returns>
        /// The <see cref="MvcHtmlString"/>.
        /// </returns>
        public IHtmlString PannelGroup(PannelGroup pgProps)
        {
            this.RegisterControl(pgProps);

            if (!this.goRabbit.Cruncher.HasJsItem("panelGroup_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.panelGroup_js, "panelGroup_js", OrderOptions.Middle);
            }
           
            var result = "<div class=\"panel-group " 
                + (pgProps.WorstMessageLevel == MessageAlertLevel.Error ? "has-errors" : string.Empty)
                + "\" id=\"" + pgProps.Id + "\">\n";
            var p = 0;
            var active = string.Empty;

            foreach (var item in pgProps.Items.Where(i => i != null))
            {
                p++;
                result +=
                    "<div class=\"panel panel-default\">\n<div class=\"panel-heading\">\n<h4 class=\"panel-title\">\n";

                if (item.AlwaysOpen)
                {
                    result += item.Title;
                }
                else
                {
                    result += "<a data-toggle=\"collapse\" data-parent=\"#" + pgProps.Id + "\" href=\"#" + pgProps.Id + "_" + p + "\">"
                              + item.Title + "</a>\n";
                }

                if (item.AlwaysOpen)
                {
                    result += "</h4>\n</div>\n<div class=\"panel-collapse\">\n";
                }
                else
                {
                    result += "</h4>\n</div>\n<div id=\"" + pgProps.Id + "_" + p + "\" class=\"panel-collapse collapse\">\n";    
                }
                
                if (item.Content != null)
                {
                    result += "<div class=\"panel-body\">\n" + item.Content.Invoke(null) + "</div>\n";
                }

                if (item.Table != null)
                {
                    result += item.Table.Invoke(null);
                }

                result += "</div>\n</div>\n\n";

                if (item.Expanded)
                {
                    active = pgProps.Id + "_" + p;
                }
            }

            result += "</div>\n";

            var js = string.Format("new trooper.ui.control.panelGroup({{id:'{0}', active:'{1}', hasErrors: {2}}});",
                pgProps.Id, 
                active,
                RabbitHelper.GetJsBool(pgProps.WorstMessageLevel == MessageAlertLevel.Error));

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

            return new MvcHtmlString(result);
        }
        
        /// <summary>
        /// Generates a button drop down list.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="buttons">
        /// The buttons.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <param name="buttonType">
        /// The button type.
        /// </param>
        /// <param name="icon">
        /// The icon.
        /// </param>
        /// <returns>
        /// The <see cref="MvcHtmlString"/>.
        /// </returns>
        public IHtmlString ButtonDropDown(ButtonDropDown bddProps)
        {
            this.RegisterControl(bddProps);

            var result = string.Format("<div class=\"trooper btn-group drop{0}\">", bddProps.Direction.ToString().ToLower());

            result +=
                string.Format(
                    "<button type=\"button\" class=\"btn {0} dropdown-toggle\" data-toggle=\"dropdown\">",
                    RabbitHelper.ButtonTypeToString(bddProps.ButtonType));

            if (!string.IsNullOrEmpty(bddProps.Icon))
            {
                result += string.Format("<span class=\"glyphicon glyphicon-{0}\"></span> ", bddProps.Icon);
            }

            result += string.Format(" {0} <span class=\"caret\"></span>", bddProps.Title);
            result += "</button><ul class=\"dropdown-menu\" role=\"menu\">";

            foreach (var item in bddProps.Buttons)
            {
                if (item != null)
                {
                    result += string.Format("<li>{0}</li>", item);
                }
            }

            result += "</ul></div>";

            return new MvcHtmlString(result);
        }
        
        /// <summary>
        /// Generates a modal dialog that you can call from a JavaScript function or control such as a button.
        /// <see cref="http://getbootstrap.com/javascript/#modals"/>
        /// </summary>
        /// <example>
        /// <![CDATA[
        ///     @var myHtml = new Html<HomeModel>(Html);  //or use BootstrapForm   
        ///     @myHtml.ModalWindow("yourid", "your title", content : @<text>....your content...</text>)
        ///     <script>
        ///         function OpenWindowFromScript() {
        ///             $('#yourid').model('show')
        ///         }
        ///     </script>
        ///     <button type="button" data-toggle="modal" data-target="#yourid">Open from button</button>
        /// ]]>
        /// </example>
        /// <param name="id">
        /// The id of the window
        /// </param>
        /// <param name="title">
        /// The title of the window
        /// </param>
        /// <param name="buttons">
        /// The buttons that should appear at the bottom of the window
        /// </param>
        /// <param name="incCloseButton">
        /// Include a close button into the bottom of the window
        /// </param>
        /// <param name="content">
        /// The content of the window. If frameUrl is not empty this will be ignored.
        /// </param>
        /// <param name="frameUrl">
        /// The frame URL for the window if you want to call another page as its content.
        /// </param>
        /// <param name="frameHeight">
        /// The frame height if you are using a frame URL
        /// </param>
        /// <returns>
        /// The <see cref="MvcHtmlString"/> html for the window.
        /// </returns>
        public IHtmlString ModalWindow(ModalWindow mwProps)
        {
            this.RegisterControl(mwProps);

            var result = "<div class=\"modal fade\" id=\"" + mwProps.Id
                         + "\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"" + mwProps.Id
                         + "_label\" aria-hidden=\"true\">\n"
                         + "<div class=\"modal-dialog\">\n<div class=\"modal-content\">\n"
                         + "<div class=\"modal-header\">\n";

            if (mwProps.IncCloseButton)
            {
                result += "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>\n";
            }

            result += "<h4 class=\"modal-title\" id=\"" + mwProps.Id + "_label\">" + mwProps.Title + "</h4>\n</div>\n";

            if (!string.IsNullOrEmpty(mwProps.FrameUrl))
            {
                result += "<div class=\"modal-body\" style=\"margin:0; padding:0\">\n"
                          + "<iframe style=\"width:100%; height:" + mwProps.FrameHeight
                          + "px; padding:0\" frameborder=\"0\" src=\"" + mwProps.FrameUrl + "\">\n" + "</iframe>\n" + "</div>\n";
            }
            else
            {
                result += "<div class=\"modal-body\">\n"
                          + (mwProps.Content == null ? string.Empty : mwProps.Content.Invoke(null).ToString())
                          + "</div>\n";
            }

            result += "<div class=\"modal-footer\">\n";

            if (mwProps.IncCloseButton)
            {
                result += "<button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">\n"
                          + "<span class=\"glyphicon glyphicon-remove\"></span> Close\n</button>\n";
            }

            if (mwProps.Buttons != null)
            {
                result = mwProps.Buttons.Aggregate(result, (current, b) => current + (b + "\n"));
            }

            result += "</div>\n</div>\n</div>\n</div>\n";

            return MvcHtmlString.Create(result);
        }

        /// <summary>
        /// Creates a virtual modal window that you can call from a JavaScript. There is now output from this method
        /// rather it inserts the required JavaScript code and instance into the JavaScript stream.
        /// <see cref="http://getbootstrap.com/javascript/#modals"/>
        /// <code>
        /// <![CDATA[
        /// //// in your JavaScript:
        /// /* Show */ $('#id').modal('show');
        /// /* Hide */ $('#id').modal('hide');
        /// ]]>
        /// </code>
        /// </summary>
        /// <param name="id">
        /// The id of the window.
        /// </param>
        /// <param name="title">
        /// The title of the window.
        /// </param>
        /// <param name="frameUrl">
        /// The frame url of the window. If null then you will need to set it by JavaScript.
        /// </param>
        /// <param name="buttons">
        /// The buttons (if any) at the bottom of the window.
        /// </param>
        /// <param name="incCloseButton">
        /// Should window include a close button.
        /// </param>
        /// <param name="frameHeight">
        /// The frame height of the frame.
        /// </param>
        public void ModalVirtualWindow(ModalWindow mwProps)
        {
            this.RegisterControl(mwProps);

            if (!this.goRabbit.Cruncher.HasJsItem("virtualModalWindow_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.virtualModalWindow_js, "virtualModalWindow_js", OrderOptions.Middle);
            }

            var buttonsString = mwProps.Buttons != null
                                    ? mwProps.Buttons.Aggregate(
                                        "new Array('",
                                        (current, b) => current + string.Join("','", b.ToString().Replace("'", "\'")))
                                      + "')"
                                    : string.Empty;

            var js = string.Format(
                "new trooper.ui.control.virtualModalWindow({{ id:'{0}', title:'{1}', frameUrl:'{2}', buttons: {3}, incCloseButton:{4}, frameHeight:{5}  }});",
                mwProps.Id,
                mwProps.Title,
                Conversion.ConvertToString(mwProps.FrameUrl, string.Empty),
                buttonsString,
                RabbitHelper.GetJsBool(mwProps.IncCloseButton),
                mwProps.FrameHeight);

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);
        }

		public IHtmlString MessagesPanel(MessagesPanel mpProps)
		{
			mpProps.Messages = mpProps.Messages ?? this.Messages;

			if (mpProps.Messages == null || !mpProps.Messages.Any())
			{
				return null;
			}

            this.RegisterControl(mpProps);

            if (!this.goRabbit.Cruncher.HasJsItem("messagesPanel_js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.messagesPanel_js, "messagesPanel_js", OrderOptions.Middle);
            }

			var html = new StringBuilder();
			var perColumn = Math.Floor(mpProps.Messages.Count / (double)mpProps.Columns);
			var remainder = mpProps.Messages.Count - (perColumn * mpProps.Columns);
			
			
			var alertLevel = MessageUtility.IsOk(mpProps.Messages)
				? "alert-success"
				: MessageUtility.IsWarning(mpProps.Messages)
					? "alert-warning"
					: "alert-danger";
			
			html.AppendFormat("<div id=\"{0}\" class=\"trooper messages-panel alert {1}\">", mpProps.Id, alertLevel);
			html.Append("<div class=\"row\">");
			
			html.Append("<div class=\"col-md-10\">");
			html.AppendFormat("<h4>{0}</h4>", mpProps.Title);
			html.Append("<div class=\"messages\">");
			html.Append("<hr />");

			html.Append("<div class=\"row\">");

			var d = 0;
			for (var c = 0; c < mpProps.Columns; c++)
			{
				html.AppendFormat("<div class=\"col-md-{0}\">", 12 / (mpProps.Columns));
				html.Append("<ul>");

				var thisColumn = c == mpProps.Columns - 1 ? perColumn + remainder : perColumn;

				for (var m = 0; m < thisColumn && d < mpProps.Messages.Count; m++)
				{
                    switch (mpProps.Messages[d].Level)
                    {
                        case MessageAlertLevel.Error:
                            html.AppendFormat("<li><span class=\"glyphicon glyphicon-wrench text-danger\"></span> {0}</li>",
                            mpProps.Messages[d].Content);
                            break;

                        case MessageAlertLevel.Warning:
                            html.AppendFormat("<li><span class=\"glyphicon glyphicon-warning-sign text-warning\"></span> {0}</li>",
                                mpProps.Messages[d].Content);
                            break;

                        case MessageAlertLevel.Note:
                            html.AppendFormat("<li><span class=\"glyphicon glyphicon-info-sign text-success\"></span> {0}</li>",
                                mpProps.Messages[d].Content);
                            break;
                    }

					d++;
				}

				html.Append("</ul>");
				html.Append("</div>");
			}

			html.Append("</div>");

			html.Append("</div>");
			html.Append("</div>");

			html.Append("<div class=\"col-md-2\">");
            html.Append("<button type=\"button\" class=\"close close-bar\">");
            html.Append("<span class=\"glyphicon glyphicon-remove-circle\"></span>");
            html.Append("</button>");
            html.Append("<button type=\"button\" class=\"close close-messages\">");
            html.Append("<span class=\"glyphicon glyphicon-chevron-up\"></span>");
            html.Append("</button>");                                        
            html.Append("<button type=\"button\" class=\"close open-messages\" style=\"display: none\">");
            html.Append("<span class=\"glyphicon glyphicon-chevron-down\"></span>");
            html.Append("</button>");
			html.Append("</div>");

			html.Append("</div>");
			html.Append("</div>");

            var js = string.Format("new trooper.ui.control.messagesPanel({{id:'{0}'}});", mpProps.Id);

            this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

			return new MvcHtmlString(html.ToString());
		}

		public IHtmlString Popover(Popover poProps)
		{
			this.RegisterControl(poProps);

			if (!this.goRabbit.Cruncher.HasJsItem("popover_js"))
			{
				this.goRabbit.Cruncher.AddJsInline(Resources.popover_js, "popover_js", OrderOptions.Middle);
			}

			var js = string.Format(
				"new trooper.ui.control.popover({{id:'{0}', content:'{1}', title:'{2}', placement:'{3}', placementAutoAssist: {4}, selector:'{5}', behaviour:'{6}'}});",
				poProps.Id,
				poProps.Content == null ? string.Empty : poProps.Content.Replace("'", @"\'"),
				poProps.Title == null ? string.Empty : poProps.Title.Replace("'", @"\'"),
				RabbitHelper.PopoverPlacementToString(poProps.Placement),
                RabbitHelper.GetJsBool(poProps.PlacementAutoAssist),
				poProps.Selector,
				poProps.Behaviour);

			this.goRabbit.Cruncher.AddJsInline(js, OrderOptions.Last);

			return new MvcHtmlString(string.Empty);
		}

        #endregion

        #region support

        public void RegisterControl(IHtmlControl control)
        {
            var idInc = 1;
            var id = control.Id;

            if (!string.IsNullOrWhiteSpace(id) && !this.ControlsRegister.ContainsKey(id))
            {
                this.ControlsRegister.Add(id, control);
                return;
            }

            while (this.ControlsRegister.ContainsKey(string.Format("control_{0}", idInc)))
            {
                idInc++;
            }

            control.Id = string.Format("control_{0}", idInc);

            this.ControlsRegister.Add(control.Id, control);
        }               

	    public void IncludeJquery()
	    {
			if (!this.goRabbit.Cruncher.HasJsItem("jquery"))
			{
				this.goRabbit.Cruncher.AddJsInline(Resources.jquery_min_js, "jquery", OrderOptions.First);
			}
	    }

        public void IncludeJqueryInputMask() {
            if (!this.goRabbit.Cruncher.HasJsItem("jquery.inputmask.js"))
            {
                this.goRabbit.Cruncher.AddJsInline(Resources.jquery_inputmask_js, "jquery.inputmask.js", OrderOptions.Middle);
            }
        }

	    public void IncludeBootstrap()
	    {
			if (!this.goRabbit.Cruncher.HasJsItem("bootstrap"))
			{
				var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

				this.goRabbit.Cruncher.AddJsInline(Resources.bootstrap_min_js, "bootstrap", OrderOptions.First);

				var ghre = RabbitController.MakeAction(urlHelper, "GlyphiconsHalflingsRegularEot");
				var ghrs = RabbitController.MakeAction(urlHelper, "GlyphiconsHalflingsReguarSvg");
				var ghrt = RabbitController.MakeAction(urlHelper, "GlyphiconsHalflingsRegularTtf");
				var ghrw = RabbitController.MakeAction(urlHelper, "GlyphiconsHalflingsRegularWoff");
				var ghrw2 = RabbitController.MakeAction(urlHelper, "GlyphiconsHalflingsRegularWoff2");
				var getBootstrapCssMap = RabbitController.MakeAction(urlHelper, "bootstrap_css_map");

				var css = Resources.bootstrap_css;

				css = css.Replace("../fonts/glyphicons-halflings-regular.eot", ghre);
				css = css.Replace("../fonts/glyphicons-halflings-regular.woff", ghrw);
				css = css.Replace("../fonts/glyphicons-halflings-regular.woff2", ghrw2);
				css = css.Replace("../fonts/glyphicons-halflings-regular.ttf", ghrt);
				css = css.Replace("../fonts/glyphicons-halflings-regular.svg", ghrs);
				css = css.Replace("bootstrap.css.map", getBootstrapCssMap);

				this.goRabbit.Cruncher.AddCssInline(css, "bootstrap_css", OrderOptions.First);
			}
	    }

	    public void IncludeJqueryUi()
	    {
			if (!this.goRabbit.Cruncher.HasJsItem("jquery-ui"))
			{
				this.goRabbit.Cruncher.AddJsInline(Resources.jquery_ui_min_js, "jquery-ui", OrderOptions.Middle);
				this.goRabbit.Cruncher.AddCssInline(Resources.jquery_ui_min_css, "jquery-ui-css", OrderOptions.Middle);

                var jqueryTheme = Resources.jquery_ui_1_10_0_custom_css;
                                
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_flat_0_aaaaaa_40x100.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_bg_flat_0_aaaaaa_40x100_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_glass_55_fbf9ee_1x400.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_bg_glass_55_fbf9ee_1x400_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_glass_65_ffffff_1x400.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_bg_glass_65_ffffff_1x400_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_glass_75_dadada_1x400.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_bg_glass_75_dadada_1x400_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_glass_75_e6e6e6_1x400.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_bg_glass_75_e6e6e6_1x400_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_glass_75_ffffff_1x400.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_bg_glass_75_ffffff_1x400_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_highlight_soft_75_cccccc_1x100.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_bg_highlight_soft_75_cccccc_1x100_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_inset_soft_95_fef1ec_1x100.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_bg_inset_soft_95_fef1ec_1x100_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_222222_256x240.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_icons_222222_256x240_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_2e83ff_256x240.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_icons_2e83ff_256x240_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_454545_256x240.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_icons_454545_256x240_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_888888_256x240.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_icons_888888_256x240_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_cd0a0a_256x240.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_icons_cd0a0a_256x240_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_f6cf3b_256x240.png", RabbitController.MakeAction(this.goRabbit.UrlHelper, "ui_icons_f6cf3b_256x240_png"));

                this.goRabbit.Cruncher.AddCssInline(jqueryTheme, "jquery-ui-theme", OrderOptions.Middle);
			}
	    }

	    public void IncludeMoment()
	    {
		    if (!this.goRabbit.Cruncher.HasJsItem("moment.js"))
		    {
				this.goRabbit.Cruncher.AddJsInline(Resources.moment_js, "moment.js", OrderOptions.Middle);
		    }
	    }

        #endregion

        #endregion       
	}
}
