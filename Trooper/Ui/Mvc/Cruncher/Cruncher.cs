//--------------------------------------------------------------------------------------
// <copyright file="Cruncher.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Cruncher
{
    using System;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;

    using Trooper.Utility;

    /// <summary>
    /// Cruncher crunches your JavaScripts and StyleSheets into 2 single files. The class can be used in
    /// partial views and layouts and will result in the server serving up a composite of all the included files.
    /// The CSS files will be processed by Less and URLs corrected to suit the location from where the single
    /// file is being served. You can use inline CSS and JavaScript to suit situations where your code is
    /// generating client side script and CSS.
    /// <example>
    /// <![CDATA[
    ///     //In your base layout
    ///     <head>
    ///         @{
    ///             var cruncher = new Cruncher(Html);
    ///             cruncher.HeaderCss()
    ///                 .AddFile("~/Content/kendo/2012.3.1315/kendo.common.min.css")
    ///                 .AddFile("~/Content/kendo/2012.3.1315/kendo.dataviz.min.css");  
    ///             cruncher.HeaderJs()
    ///                 .AddFile("~/Scripts/kendo/2012.3.1315/jquery.min.js")
    ///                 .AddFile("~/Scripts/kendo/2012.3.1315/kendo.all.min.js");   
    ///             cruncher.GetCruncherHeader();
    ///         } 
    ///     </head> 
    ///     //In your views and partial views
    ///     @ {
    ///             var cruncher = new Cruncher(Html);
    ///             cruncher.HeaderJs()
    ///             .AddFile("~/yourfile.js");
    ///     }
    /// ]]>
    /// </example>
    /// </summary>
    public class Cruncher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cruncher"/> class.
        /// You can instantiate this many times and add your JS and CSS from all your Views, Partial Views,
        /// Controllers and other helper classes and methods.
        /// </summary>
        /// <param name="htmlHelper">
        /// The html helper.
        /// </param>
        public Cruncher(HtmlHelper htmlHelper)
        {
            this.HtmlHelper = htmlHelper;

            this.UrlHelper = new UrlHelper(this.HtmlHelper.ViewContext.RequestContext);
        }

        /// <summary>
        /// Gets the html helper from your View.
        /// </summary>
        public HtmlHelper HtmlHelper { get; private set; }

        /// <summary>
        /// Gets or sets the url helper from your View.
        /// </summary>
        private UrlHelper UrlHelper { get; set; }

        /// <summary>
        /// Gets the store name for the given id
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The store id
        /// </returns>
        public static CruncherStore GetStore(Guid id)
        {
            return HttpRuntime.Cache[id.ToString()] as CruncherStore;
        }

        /// <summary>
        /// Returns the Footer JavaScript items lists
        /// </summary>
        /// <returns>
        /// The Footer JavaScript items lists
        /// </returns>
        public StoreItemList FooterJs()
        {
            return this.GetStore().FooterJs();
        }

        /// <summary>
        /// Generates all the HTML required for the header tag of the destination HTML
        /// </summary>
        /// <returns>
        /// The HTML for the JavaScript and StyleSheet.
        /// </returns>
        public MvcHtmlString Header()
        {
            return
                MvcHtmlString.Create(
                    string.Format(
                        "{0}\n{1}\n",
                        this.GetHtml(CruncherController.AreaOptions.HeaderCss),
                        this.GetHtml(CruncherController.AreaOptions.HeaderJs)));
        }

        /// <summary>
        /// Returns the Header JavaScript items lists
        /// </summary>
        /// <returns>
        /// The Header JavaScript items lists
        /// </returns>
        public StoreItemList HeaderJs()
        {
            return this.GetStore().HeaderJs();
        }

        /// <summary>
        /// Returns the Header StyleSheet CSS items lists
        /// </summary>
        /// <returns>
        /// The Header StyleSheet items lists
        /// </returns>
        public StoreItemList HeaderCss()
        {
            return this.GetStore().HeaderCss();
        }

        /// <summary>
        /// Generates the HTML for the JavaScript and StyleSheet elements.
        /// </summary>
        /// <param name="area">
        /// The area to generate for
        /// </param>
        /// <returns>
        /// The HTML output
        /// </returns>
        private MvcHtmlString GetHtml(CruncherController.AreaOptions area)
        {
            switch (area)
            {
                case CruncherController.AreaOptions.HeaderJs:
                case CruncherController.AreaOptions.FooterJs:
                    return
                        MvcHtmlString.Create(
                            string.Format(
                                "<script type=\"text/javascript\" src=\"{0}\"></script>",
                                this.GetUrl(area)));

                case CruncherController.AreaOptions.HeaderCss:
                    return
                        MvcHtmlString.Create(
                            string.Format(
                                "<link rel=\"stylesheet\" type=\"text/css\" media=\"screen,print\" href=\"{0}\" />",
                                this.GetUrl(area)));
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Generates the URL of the cruncher store that will be called from the client.
        /// </summary>
        /// <param name="area">
        /// The area to generate for
        /// </param>
        /// <returns>
        /// The URL string
        /// </returns>
        private string GetUrl(CruncherController.AreaOptions area)
        {
            return this.GetUrl("GetSources", "Cruncher", area);
        }

        /// <summary>
        /// Generates the URL of the cruncher store that will be called from the client.
        /// </summary>
        /// <param name="actionName">
        /// The action name. If you need a specific action instead of the default.
        /// </param>
        /// <param name="controllerName">
        /// The controller name.
        /// </param>
        /// <param name="area">
        /// The area to generate for
        /// </param>
        /// <returns>
        /// The URL string
        /// </returns>
        private string GetUrl(string actionName, string controllerName, CruncherController.AreaOptions area)
        {
            var store = this.GetStore();
            return this.UrlHelper.Action(actionName, controllerName, new { area, id = store.Id });
        }

        /// <summary>
        /// Gets the store from the HttpRuntime or creates one if none is found
        /// </summary>
        /// <returns>
        /// The store
        /// </returns>
        private CruncherStore GetStore()
        {
            Guid id;
            var storename = this.GetStoreName();

            if (HttpContext.Current.Items.Contains(storename))
            {
                id = Guid.Parse(Conversion.ConvertToString(HttpContext.Current.Items[storename], string.Empty));
            }
            else
            {
                id = Guid.NewGuid();
                HttpContext.Current.Items.Add(storename, id);
            }

            var cruncherStore = HttpRuntime.Cache[id.ToString()] as CruncherStore;

            if (cruncherStore == null)
            {
                cruncherStore = new CruncherStore(id);
                HttpRuntime.Cache.Add(id.ToString(), cruncherStore, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(30.0), CacheItemPriority.Normal, null);
            }

            return cruncherStore;
        }

        /// <summary>
        /// Gets the store name
        /// </summary>
        /// <returns>
        /// The store name
        /// </returns>
        private string GetStoreName()
        {
            return string.Format("Trooper.Ui.Cruncher");
        }
    }
}