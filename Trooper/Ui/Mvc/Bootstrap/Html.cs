//--------------------------------------------------------------------------------------
// <copyright file="Html.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

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

            if (!this.Cruncher.HasCssItem("BootstrapHtmlHelper_less"))
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

                this.Cruncher.AddJsInline(Resources.jquery_min_js).Set(name: "jquery_min_js", order: OrderOptions.First);

                this.Cruncher.AddJsInline(Resources.jquery_ui_min_js).Set(name: "jquery_ui_min_js", order: OrderOptions.First);

                this.Cruncher.AddJsInline(Resources.bootstrap_min_js).Set(name: "bootstrap_min_js", order: OrderOptions.First);

                /*var ghw = urlHelper.Action("GetGhw", "Bootstrap");
                var gh = urlHelper.Action("GetGh", "Bootstrap");*/

                var ghre = urlHelper.Action("GetGhrEot", "Bootstrap");
                var ghrs = urlHelper.Action("GetGhrSvg", "Bootstrap");
                var ghrt = urlHelper.Action("GetGhrTtf", "Bootstrap");
                var ghrw = urlHelper.Action("GetGhrWoff", "Bootstrap");

                var css = Resources.bootstrap_css;

                /*css = css.Replace("../img/glyphicons-halflings-white.png", ghw);
                css = css.Replace("../img/glyphicons-halflings.png", gh);*/

                css = css.Replace("../fonts/glyphicons-halflings-regular.eot", ghre);
                css = css.Replace("../fonts/glyphicons-halflings-regular.woff", ghrw);
                css = css.Replace("../fonts/glyphicons-halflings-regular.ttf", ghrt);
                css = css.Replace("../fonts/glyphicons-halflings-regular.svg", ghrs);

                this.Cruncher.AddCssInline(css).Set(name: "bootstrap_css", order: OrderOptions.First);

                this.Cruncher.AddLessInline(Resources.BootstrapHtmlHelper_less).Set(name: "BootstrapHtmlHelper_less", order: OrderOptions.First);
            }

            if (!this.Cruncher.HasJsItem("BootstrapHtml_js"))
            {
                this.Cruncher.AddJsInline(Resources.BootstrapHtml_js).Set(name: "BootstrapHtml_js", order: OrderOptions.Middle);

                this.Cruncher.AddJsInline("var bootstrapHtml = new BootstrapHtml();");
            }
        }

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

            if (!this.Cruncher.HasJsItem("BootstrapPanelGroup_js"))
            {
                this.Cruncher.AddJsInline(Resources.BootstrapPanelGroup_js).Set(name: "BootstrapPanelGroup_js", order: OrderOptions.Middle);
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

            this.Cruncher.AddJsInline(string.Format(
                "var {0}_BootstrapPanelGroup = new BootstrapPanelGroup({{id:'{0}', active:'{1}', hasErrors: {2}}});",
                pgProps.Id, 
                active,
                this.GetJsBool(pgProps.WorstMessageLevel == MessageAlertLevel.Error)));

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

            if (!this.Cruncher.HasJsItem("BootstrapVirtualModalWindow_js"))
            {
                this.Cruncher.AddJsInline(Resources.BootstrapVirtualModalWindow_js).Set(name: "BootstrapVirtualModalWindow_js", order: OrderOptions.Middle);
            }

            var buttonsString = mwProps.Buttons != null
                                    ? mwProps.Buttons.Aggregate(
                                        "new Array('",
                                        (current, b) => current + string.Join("','", b.ToString().Replace("'", "\'")))
                                      + "')"
                                    : string.Empty;

            this.Cruncher.AddJsInline(
                string.Format(
                "new BootstrapVirtualModalWindow({{ id:'{0}', title:'{1}', frameUrl:'{2}', buttons: {3}, incCloseButton:{4}, frameHeight:{5}  }});",
                mwProps.Id,
                mwProps.Title,
                Conversion.ConvertToString(mwProps.FrameUrl, string.Empty),
                buttonsString,
                this.GetJsBool(mwProps.IncCloseButton),
                mwProps.FrameHeight));
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
    }
}
