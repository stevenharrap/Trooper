namespace Trooper.Ui.Interface.Mvc.Cruncher
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// The types of content that can be collected
    /// </summary>
    public enum MimeTypes
    {
        /// <summary>
        /// JavaScript type
        /// </summary>
        Js,

        /// <summary>
        /// CSS types
        /// </summary>
        Css
    }

    public interface ICruncherController
    {
        ActionResult GetSources(MimeTypes mimeType, Guid id);
    }
}
