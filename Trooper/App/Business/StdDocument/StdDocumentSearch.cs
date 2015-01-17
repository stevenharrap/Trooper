//--------------------------------------------------------------------------------------
// <copyright file="StdDocumentSearch.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Business.StdDocument
{
    using Trooper.BusinessOperation.Business;

    /// <summary>
    /// The document search.
    /// </summary>
    public class StdDocumentSearch : Search
    {
        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        public string Extension { get; set; }
    }
}
