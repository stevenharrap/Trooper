//--------------------------------------------------------------------------------------
// <copyright file="Html.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Text;
using Trooper.BusinessOperation2.Utility;

namespace Trooper.Ui.Mvc.Bootstrap
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
    using Trooper.Ui.Mvc.Bootstrap.Controllers;
    using Trooper.Ui.Mvc.Bootstrap.Controls;
    using Trooper.Ui.Mvc.Cruncher;
    using Trooper.Utility;

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
    public class Html<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Html{TModel}"/> class and includes all
        /// Bootstrap JavaScript and CSS requirements.
        /// </summary>
        /// <param name="htmlHelper">
        /// The html helper.
        /// </param>
        public Html(HtmlHelper<TModel> htmlHelper)
        {
            this.HtmlHelper = htmlHelper;
            this.ControlsRegister = new Dictionary<string, HtmlControl>();
            this.UrlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            this.Cruncher = new Cruncher(htmlHelper);

            this.IncludeJquery();
			this.IncludeBootstrap();

            if (!this.Cruncher.HasJsItem("trooper"))
            {
                this.Cruncher.AddJsInline(Resources.trooper_js, "trooper", OrderOptions.Middle);
				this.Cruncher.AddLessInline(Resources.trooper_less, "trooper_less", OrderOptions.Middle);
            }
        }

		#region public properties

		/// <summary>
        /// Gets the html helper from your View
        /// </summary>
        public HtmlHelper<TModel> HtmlHelper { get; private set; }

        public Dictionary<string, HtmlControl> ControlsRegister { get; private set; }

        /// <summary>
        /// Gets or sets the errors that may be present. Providing errors to
        /// any specific control overrides this.
        /// </summary>
        public List<IMessage> Messages { get; set; }               

        /// <summary>
        /// Gets or sets the url helper from your View
        /// </summary>
        public UrlHelper UrlHelper { get; set; }

        /// <summary>
        /// Gets or sets the Cruncher instance into which methods may inject JavaScript and CSS
        /// </summary>
        public Cruncher Cruncher { get; set; }

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
        public MvcHtmlString PannelGroup(PannelGroup pgProps)
        {
            this.RegisterControl(pgProps);

            if (!this.Cruncher.HasJsItem("panelGroup_js"))
            {
                this.Cruncher.AddJsInline(Resources.panelGroup_js, "panelGroup_js", OrderOptions.Middle);
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
                this.GetJsBool(pgProps.WorstMessageLevel == MessageAlertLevel.Error));

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

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
        public MvcHtmlString ButtonDropDown(ButtonDropDown bddProps)
        {
            this.RegisterControl(bddProps);

            var result = string.Format("<div class=\"trooper btn-group drop{0}\">", bddProps.Direction.ToString().ToLower());

            result +=
                string.Format(
                    "<button type=\"button\" class=\"btn {0} dropdown-toggle\" data-toggle=\"dropdown\">",
                    this.ButtonTypeToString(bddProps.ButtonType));

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
        public MvcHtmlString ModalWindow(ModalWindow mwProps)
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

            if (!this.Cruncher.HasJsItem("virtualModalWindow_js"))
            {
                this.Cruncher.AddJsInline(Resources.virtualModalWindow_js, "virtualModalWindow_js", OrderOptions.Middle);
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
                this.GetJsBool(mwProps.IncCloseButton),
                mwProps.FrameHeight);

            this.Cruncher.AddJsInline(js, OrderOptions.Last);
        }

		public MvcHtmlString MessagesPanel(MessagesPanel mpProps)
		{
			mpProps.Messages = mpProps.Messages ?? this.Messages;

			if (mpProps.Messages == null || !mpProps.Messages.Any())
			{
				return null;
			}

            this.RegisterControl(mpProps);

            if (!this.Cruncher.HasJsItem("messagesPanel_js"))
            {
                this.Cruncher.AddJsInline(Resources.messagesPanel_js, "messagesPanel_js", OrderOptions.Middle);
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

            this.Cruncher.AddJsInline(js, OrderOptions.Last);

			return new MvcHtmlString(html.ToString());
		}

		public MvcHtmlString Popover(Popover poProps)
		{
			this.RegisterControl(poProps);

			if (!this.Cruncher.HasJsItem("popover_js"))
			{
				this.Cruncher.AddJsInline(Resources.popover_js, "popover_js", OrderOptions.Middle);
			}

			var js = string.Format(
				"new trooper.ui.control.popover({{id:'{0}', content:'{1}', title:'{2}', placement:'{3}', placementAutoAssist: {4}, selector:'{5}', behaviour:'{6}'}});",
				poProps.Id,
				poProps.Content == null ? string.Empty : poProps.Content.Replace("'", @"\'"),
				poProps.Title == null ? string.Empty : poProps.Title.Replace("'", @"\'"),
				PopoverPlacementToString(poProps.Placement),
				GetJsBool(poProps.PlacementAutoAssist),
				poProps.Selector,
				poProps.Behaviour);

			this.Cruncher.AddJsInline(js, OrderOptions.Last);

			return new MvcHtmlString(string.Empty);
		}

        #endregion

        #region support

        /// <summary>
        /// This initiates the HTML class by instantiating it and returning the instance.
        /// Includes all Bootstrap JavaScript and CSS requirements.
        /// </summary>
        /// <param name="htmlHelper">
        /// The html helper.
        /// </param>
        /// <returns>
        /// Returns the new instance.
        /// </returns>
        public static Html<TModel> Init(HtmlHelper<TModel> htmlHelper)
        {
            return new Html<TModel>(htmlHelper);
        }

        public void RegisterControl(HtmlControl control)
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
        
        /// <summary>
        /// Gets the value of the expression from the property.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <typeparam name="TValue">
        /// The data type of the expression
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public TValue GetExpressionValue<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, this.HtmlHelper.ViewData);

            if (metaData == null || metaData.Model == null)
            {
                return default(TValue);
            }

            var value = (TValue)metaData.Model;

            return value;
        }

        /// <summary>
        /// Converts the buttonType to a string suitable for Bootstrap classes
        /// </summary>
        /// <param name="buttonType">
        /// The button type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string ButtonTypeToString(ButtonTypes buttonType)
        {
            if (buttonType == ButtonTypes.None)
            {
                return string.Empty;
            }

            return "btn-" + buttonType.ToString().ToLower();
        }

        /// <summary>
        /// Converts the placement to a string suitable for Bootstrap classes
        /// </summary>
        /// <param name="placement">
        /// The placement.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string PopoverPlacementToString(PopoverPlacements placement)
        {
            return placement.ToString().ToLower();
        }

	    public void IncludeJquery()
	    {
			if (!this.Cruncher.HasJsItem("jquery"))
			{
				this.Cruncher.AddJsInline(Resources.jquery_min_js, "jquery", OrderOptions.First);
			}
	    }

	    public void IncludeBootstrap()
	    {
			if (!this.Cruncher.HasJsItem("bootstrap"))
			{
				var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

				this.Cruncher.AddJsInline(Resources.bootstrap_min_js, "bootstrap", OrderOptions.First);

				var ghre = BootstrapController.MakeAction(urlHelper, "GlyphiconsHalflingsRegularEot");
				var ghrs = BootstrapController.MakeAction(urlHelper, "GlyphiconsHalflingsReguarSvg");
				var ghrt = BootstrapController.MakeAction(urlHelper, "GlyphiconsHalflingsRegularTtf");
				var ghrw = BootstrapController.MakeAction(urlHelper, "GlyphiconsHalflingsRegularWoff");
				var ghrw2 = BootstrapController.MakeAction(urlHelper, "GlyphiconsHalflingsRegularWoff2");
				var getBootstrapCssMap = BootstrapController.MakeAction(urlHelper, "bootstrap_css_map");

				var css = Resources.bootstrap_css;

				css = css.Replace("../fonts/glyphicons-halflings-regular.eot", ghre);
				css = css.Replace("../fonts/glyphicons-halflings-regular.woff", ghrw);
				css = css.Replace("../fonts/glyphicons-halflings-regular.woff2", ghrw2);
				css = css.Replace("../fonts/glyphicons-halflings-regular.ttf", ghrt);
				css = css.Replace("../fonts/glyphicons-halflings-regular.svg", ghrs);
				css = css.Replace("bootstrap.css.map", getBootstrapCssMap);

				this.Cruncher.AddCssInline(css, "bootstrap_css", OrderOptions.First);
			}
	    }

	    public void IncludeJqueryUi()
	    {
			if (!this.Cruncher.HasJsItem("jquery-ui"))
			{
				this.Cruncher.AddJsInline(Resources.jquery_ui_min_js, "jquery-ui", OrderOptions.Middle);
				this.Cruncher.AddCssInline(Resources.jquery_ui_min_css, "jquery-ui-css", OrderOptions.Middle);

                var jqueryTheme = Resources.jquery_ui_1_10_0_custom_css;
                                
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_flat_0_aaaaaa_40x100.png", BootstrapController.MakeAction(this.UrlHelper, "ui_bg_flat_0_aaaaaa_40x100_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_glass_55_fbf9ee_1x400.png", BootstrapController.MakeAction(this.UrlHelper, "ui_bg_glass_55_fbf9ee_1x400_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_glass_65_ffffff_1x400.png", BootstrapController.MakeAction(this.UrlHelper, "ui_bg_glass_65_ffffff_1x400_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_glass_75_dadada_1x400.png", BootstrapController.MakeAction(this.UrlHelper, "ui_bg_glass_75_dadada_1x400_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_glass_75_e6e6e6_1x400.png", BootstrapController.MakeAction(this.UrlHelper, "ui_bg_glass_75_e6e6e6_1x400_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_glass_75_ffffff_1x400.png", BootstrapController.MakeAction(this.UrlHelper, "ui_bg_glass_75_ffffff_1x400_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_highlight_soft_75_cccccc_1x100.png", BootstrapController.MakeAction(this.UrlHelper, "ui_bg_highlight_soft_75_cccccc_1x100_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-bg_inset_soft_95_fef1ec_1x100.png", BootstrapController.MakeAction(this.UrlHelper, "ui_bg_inset_soft_95_fef1ec_1x100_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_222222_256x240.png", BootstrapController.MakeAction(this.UrlHelper, "ui_icons_222222_256x240_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_2e83ff_256x240.png", BootstrapController.MakeAction(this.UrlHelper, "ui_icons_2e83ff_256x240_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_454545_256x240.png", BootstrapController.MakeAction(this.UrlHelper, "ui_icons_454545_256x240_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_888888_256x240.png", BootstrapController.MakeAction(this.UrlHelper, "ui_icons_888888_256x240_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_cd0a0a_256x240.png", BootstrapController.MakeAction(this.UrlHelper, "ui_icons_cd0a0a_256x240_png"));
                jqueryTheme = jqueryTheme.Replace("images/ui-icons_f6cf3b_256x240.png", BootstrapController.MakeAction(this.UrlHelper, "ui_icons_f6cf3b_256x240_png"));

                this.Cruncher.AddCssInline(jqueryTheme, "jquery-ui-theme", OrderOptions.Middle);
			}
	    }

	    public void IncludeMoment()
	    {
		    if (!this.Cruncher.HasJsItem("moment.js"))
		    {
				this.Cruncher.AddJsInline(Resources.moment_js, "moment.js", OrderOptions.Middle);
		    }
	    }

        #endregion

        #endregion

        #region protected methods

        protected Dictionary<string, string> AddAttributes(
            Dictionary<string, string> attributes,
            Dictionary<string, string> newAttributes)
        {
            if (attributes == null)
            {
                attributes = new Dictionary<string, string>();
            }

            if (newAttributes != null)
            {
                foreach (var i in newAttributes.Where(i => i.Key != null && !attributes.ContainsKey(i.Key)))
                {
                    attributes.Add(i.Key, i.Value);
                }
            }

            return attributes;
        }

        protected Dictionary<string, string> AddAttribute(Dictionary<string, string> attributes, string name, string value)
        {
            if (attributes == null)
            {
                attributes = new Dictionary<string, string>();
            }

            if (attributes.ContainsKey(name))
            {
                attributes[name] = value;
            }

            attributes.Add(name, value);

            return attributes;
        }

        protected Dictionary<string, string> AddNotEmptyAttribute(Dictionary<string, string> attributes, string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return attributes ?? new Dictionary<string, string>();
            }

            return this.AddAttribute(attributes, name, value);
        }
        
        protected string MakeAttributesList(Dictionary<string, string> attributes)
        {
            if (attributes == null)
            {
                return string.Empty;
            }

            return string.Join(" ", attributes.Select(i => string.Format("{0}=\"{1}\"", i.Key, i.Value)));
        }

        /// <summary>
        /// The add class to the classes and return the result.
        /// If the supplied classes is null then a new classes is created.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <param name="className">
        /// The class name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        protected List<string> AddClass(List<string> classes, string className)
        {
            if (classes == null)
            {
                classes = new List<string>();
            }

            if (!string.IsNullOrEmpty(className) && !classes.Contains(className))
            {
                classes.Add(className);
            }

            return classes;
        }

        /// <summary>
        /// Add the new classes to the classes and return the result.
        /// If the supplied classes is null then a new classes is created.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <param name="newClasses">
        /// The class names to add.
        /// </param>
        /// <returns>
        /// The result list.
        /// </returns>
        protected List<string> AddClasses(List<string> classes, List<string> newClasses)
        {
            if (classes == null)
            {
                classes = new List<string>();
            }

            if (newClasses == null || !newClasses.Any())
            {
                return classes;
            }

            classes.AddRange(newClasses);

            return classes.Distinct().ToList();
        }

        /// <summary>
        /// Generates a class attribute content with the given classes. Duplicates are removed.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <returns>
        /// The class <see cref="string"/>.
        /// </returns>
        protected string MakeClassAttributeContent(List<string> classes)
        {
            if (classes == null || !classes.Any())
            {
                return string.Empty;
            }

            return string.Join(" ", classes.Distinct());
        }

        /// <summary>
        /// Generates a class attribute with the given classes. Duplicates are removed.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <returns>
        /// The class <see cref="string"/>.
        /// </returns>
        protected string MakeClassAttribute(List<string> classes)
        {
            if (classes == null || !classes.Any())
            {
                return "class=\"\"";
            }

            return string.Format("class=\"{0}\"", this.MakeClassAttributeContent(classes));
        }

        protected string GetJsBool(bool? value)
        {
            return Conversion.ConvertToBoolean(value, false).ToString().ToLower();
        }

        /// <summary>
        /// Makes an icon using the supplied icon image from the BootStrap library. 
        /// <see cref="http://getbootstrap.com/components/"/>
        /// Only use the last part of the icon name. E.g.'volume-up' from 'glyphicon-volume-up'
        /// </summary>
        /// <param name="iconImage">
        /// The icon image name.
        /// </param>
        /// <returns>
        /// The html string.
        /// </returns>
        protected string MakeIcon(string iconImage)
        {
            return string.Format("<span class=\"input-group-addon\">\n<span class=\"glyphicon glyphicon-{0}\"></span>\n</span>\n", iconImage);
		}

		#endregion
	}
}
