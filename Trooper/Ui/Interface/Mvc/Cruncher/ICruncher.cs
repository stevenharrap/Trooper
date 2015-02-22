namespace Trooper.Ui.Interface.Mvc.Cruncher
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;
    using Trooper.Ui.Interface.Mvc.Cruncher;

    public interface ICruncher
    {
        /// <summary>
        /// Generates all the HTML required for the header tag of the destination HTML
        /// </summary>
        /// <returns>
        /// The HTML for the JavaScript and StyleSheet.
        /// </returns>
        IHtmlString Header();

        IList<IStoreItem> Css();

        IList<IStoreItem> Js();

        /// <summary>
        /// Adds the contents of the file to as middle order.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        IStoreItem AddCssFile(string relativePath, OrderOptions order);

        /// <summary>
        /// Adds the contents as last order.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        IStoreItem AddCssInline(Func<object, IHtmlString> content, string name, OrderOptions order);

        IStoreItem AddCssInline(Func<object, IHtmlString> content, OrderOptions order);

        /// <summary>
        /// Adds the contents as last order.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        IStoreItem AddCssInline(string content, string name, OrderOptions order);

        IStoreItem AddCssInline(string content, OrderOptions order);

        void AddCssItem(IStoreItem item);

        /// <summary>
        /// Determines if a name item exists in the store
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// Returns true if the item exists
        /// </returns>
        bool HasCssItem(string name);

        bool HasCssFile(string file);

        /// <summary>
        /// Adds the contents of the file to as middle order.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        IStoreItem AddJsFile(string relativePath, OrderOptions order);

        /// <summary>
        /// Adds the contents as last order.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        IStoreItem AddJsInline(Func<object, IHtmlString> content, string name, OrderOptions order);

        IStoreItem AddJsInline(Func<object, IHtmlString> content, OrderOptions order);

        /// <summary>
        /// Adds the contents as last order.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        IStoreItem AddJsInline(string content, string name, OrderOptions order);

        IStoreItem AddJsInline(string content, OrderOptions order);

        void AddJsItem(IStoreItem item);

        /// <summary>
        /// Determines if a name item exists in the store
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// Returns true if the item exists
        /// </returns>
        bool HasJsItem(string name);

        bool HasJsFile(string file);

        /// <summary>
        /// Adds the contents of the file to as middle order.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        IStoreItem AddLessFile(string relativePath, OrderOptions order);

        /// <summary>
        /// Adds the contents as last order.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        IStoreItem AddLessInline(Func<object, IHtmlString> content, string name, OrderOptions order);

        IStoreItem AddLessInline(Func<object, IHtmlString> content, OrderOptions order);

        /// <summary>
        /// Adds the contents as last order.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        IStoreItem AddLessInline(string content, string name, OrderOptions order);

        IStoreItem AddLessInline(string content, OrderOptions order);
    }
}
