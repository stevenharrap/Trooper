//--------------------------------------------------------------------------------------
// <copyright file="Cruncher.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Cruncher
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;
    using Trooper.Ui.Interface.Mvc.Cruncher;
    using Trooper.Utility;
    using System.Web.WebPages;

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
    public class Cruncher : ICruncher
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

        #region private properties

        /// <summary>
        /// Gets the html helper from your View.
        /// </summary>
        private HtmlHelper HtmlHelper { get; set; }

        /// <summary>
        /// Gets or sets the url helper from your View.
        /// </summary>
        private UrlHelper UrlHelper { get; set; }

        #endregion

        #region public static methods

        /// <summary>
        /// Gets the store name for the given id
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The store id
        /// </returns>
        public static ICruncherStore GetStore(Guid id)
        {
            return HttpRuntime.Cache[id.ToString()] as ICruncherStore;
        }

        #endregion

        #region public methods

        #region Javascript methods

        public void AddJsItem(IStoreItem item)
        {
            AddItem(this.Js(), item);
        }

        /// <summary>
        /// Adds the contents of the file to as middle order.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public IStoreItem AddJsFile(string relativePath, OrderOptions order)
        {
            return AddFile(this.Js(), relativePath, order);
        }

        public IStoreItem AddJsInline(string content, string name, OrderOptions order)
        {
            return AddInline(this.Js(), content, name, order);
        }

        public IStoreItem AddJsInline(string content, OrderOptions order)
        {
            return AddInline(this.Js(), content, order);
        }

        public IStoreItem AddJsInline(Func<object, IHtmlString> content, string name, OrderOptions order)
        {
            return AddInline(this.Js(), content, name, order);
        }

        public IStoreItem AddJsInline(Func<object, IHtmlString> content, OrderOptions order)
        {
            return AddInline(this.Js(), content, order);
        }

        /// <summary>
        /// Determines if a name item exists in the store
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// Returns true if the item exists
        /// </returns>
        public bool HasJsItem(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return this.Js().Any(i => i.Name == name);
        }

        /// <summary>
        /// Determines if a file item exists in the store
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// Returns true if the item exists
        /// </returns>
        public bool HasJsFile(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return false;
            }

            return this.Js().Any(i => i.File == file);
        }

        #endregion

        #region Css methods

        /// <summary>
        /// Determines if a name item exists in the store
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// Returns true if the item exists
        /// </returns>
        public bool HasCssItem(string name)
        {
            return HasItem(this.Css(), name);
        }

        /// <summary>
        /// Determines if a file item exists in the store
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// Returns true if the item exists
        /// </returns>
        public bool HasCssFile(string file)
        {
            return HasFile(this.Css(), file);
        }

        public void AddCssItem(IStoreItem item)
        {
            AddItem(this.Css(), item);
        }

        public IStoreItem AddCssFile(string relativePath, OrderOptions order)
        {
            return AddFile(this.Css(), relativePath, order);
        }

        public IStoreItem AddCssInline(string content, string name, OrderOptions order)
        {
            return AddInline(this.Css(), content, name, order);
        }

        public IStoreItem AddCssInline(string content, OrderOptions order)
        {
            return AddInline(this.Css(), content, order);
        }

        public IStoreItem AddCssInline(Func<object, IHtmlString> content, string name, OrderOptions order)
        {
            return AddInline(this.Css(), content, name, order);
        }

        public IStoreItem AddCssInline(Func<object, IHtmlString> content, OrderOptions order)
        {
            return AddInline(this.Css(), content, order);
        }

        #endregion

        #region Less methods

        public IStoreItem AddLessFile(string relativePath, OrderOptions order)
        {
            var item = AddFile(this.Css(), relativePath, order);

            item.Less = true;

            return item;
        }

        public IStoreItem AddLessInline(string content, string name, OrderOptions order)
        {
            var item = AddInline(this.Css(), content, name, order);

            item.Less = true;

            return item;
        }

        public IStoreItem AddLessInline(string content, OrderOptions order)
        {
            var item = AddInline(this.Css(), content, order);

            item.Less = true;

            return item;
        }

        public IStoreItem AddLessInline(Func<object, IHtmlString> content, string name, OrderOptions order)
        {
            var item = AddInline(this.Css(), content, name, order);

            item.Less = true;

            return item;
        }

        public IStoreItem AddLessInline(Func<object, IHtmlString> content, OrderOptions order)
        {
            var item = AddInline(this.Css(), content, order);

            item.Less = true;

            return item;
        }

        #endregion               

        /// <summary>
        /// Generates all the HTML required for the header tag of the destination HTML
        /// </summary>
        /// <returns>
        /// The HTML for the JavaScript and StyleSheet.
        /// </returns>
        public IHtmlString Header()
        {
            return
                MvcHtmlString.Create(
                    string.Format(
                        "{0}\n{1}\n",
                        this.GetHtml(MimeTypes.Css),
                        this.GetHtml(MimeTypes.Js)));
        }

        /// <summary>
        /// Returns the JavaScript items lists
        /// </summary>
        /// <returns>
        /// The JavaScript items lists
        /// </returns>
        public IList<IStoreItem> Js()
        {
            return this.GetStore().Js();
        }

        /// <summary>
        /// Returns the StyleSheet CSS items lists
        /// </summary>
        /// <returns>
        /// The StyleSheet items lists
        /// </returns>
        public IList<IStoreItem> Css()
        {
            return this.GetStore().Css();
        }

        #endregion

        #region private static methods

        private static void AddItem(IList<IStoreItem> items, IStoreItem item)
        {
            items.Add(item);
        }

        private static IStoreItem AddFile(IList<IStoreItem> items, string relativePath, OrderOptions order)
        {
            var item = new StoreItem
            {
                File = relativePath,
                Order = order,
                Reference = ReferenceOptions.File
            };

            AddItem(items, item);

            return item;
        }

        private static IStoreItem AddInline(IList<IStoreItem> items, string content, string name, OrderOptions order)
        {
            var item = new StoreItem
            {
                Order = order,
                Reference = ReferenceOptions.Inline,
                Content = content,
                Name = name                
            };

            AddItem(items, item);

            return item;
        }

        private static IStoreItem AddInline(IList<IStoreItem> items, string content, OrderOptions order)
        {
            var item = new StoreItem
            {
                Order = order,
                Reference = ReferenceOptions.Inline,
                Content = content
            };

            AddItem(items, item);

            return item;
        }

        private static IStoreItem AddInline(IList<IStoreItem> items, Func<object, IHtmlString> content, string name, OrderOptions order)
        {
            var item = new StoreItem
            {
                Order = order,
                Reference = ReferenceOptions.Inline,
                Content = content.Invoke(null).ToString(),
                Name = name
            };

            AddItem(items, item);

            return item;
        }

        private static IStoreItem AddInline(IList<IStoreItem> items, Func<object, IHtmlString> content,  OrderOptions order)
        {
            var item = new StoreItem
            {
                Order = order,
                Reference = ReferenceOptions.Inline,
                Content = content.Invoke(null).ToString()
            };

            AddItem(items, item);

            return item;
        }

        private static bool HasItem(IList<IStoreItem> items, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return items.Any(i => i.Name == name);
        }

        private static bool HasFile(IList<IStoreItem> items, string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return false;
            }

            return items.Any(i => i.File == file);
        }

        #endregion

        #region private methods

        /// <summary>
        /// Generates the HTML for the JavaScript and StyleSheet elements.
        /// </summary>
        /// <param name="area">
        /// The area to generate for
        /// </param>
        /// <returns>
        /// The HTML output
        /// </returns>
        private IHtmlString GetHtml(MimeTypes mimeType)
        {
            switch (mimeType)
            {
                case MimeTypes.Js:
                    return
                        MvcHtmlString.Create(
                            string.Format(
                                "<script type=\"text/javascript\" src=\"{0}\"></script>",
                                this.GetUrl(mimeType)));

                case MimeTypes.Css:
                    return
                        MvcHtmlString.Create(
                            string.Format(
                                "<link rel=\"stylesheet\" type=\"text/css\" media=\"screen,print\" href=\"{0}\" />",
                                this.GetUrl(mimeType)));
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
        private string GetUrl(MimeTypes mimeType)
        {
            return this.GetUrl("GetSources", "Cruncher", mimeType);
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
        private string GetUrl(string actionName, string controllerName, MimeTypes mimeType)
        {
            var store = this.GetStore();
            return this.UrlHelper.Action(actionName, controllerName, new { mimeType, id = store.Id });
        }

        /// <summary>
        /// Gets the store from the HttpRuntime or creates one if none is found
        /// </summary>
        /// <returns>
        /// The store
        /// </returns>
        private ICruncherStore GetStore()
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

        #endregion
    }
}