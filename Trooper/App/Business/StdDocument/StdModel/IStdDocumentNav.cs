//--------------------------------------------------------------------------------------
// <copyright file="IStdDocumentNav.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Business.StdDocument.StdModel
{
    /// <summary>
    /// The document nav.
    /// </summary>
    /// <typeparam name="TDocumentContentNav">
    /// The document content nav class
    /// </typeparam>
    public interface IStdDocumentNav<TDocumentContentNav> : IStdDocument
    {
        /// <summary>
        /// Gets or sets the document content.
        /// </summary>
        TDocumentContentNav DocumentContentNav { get; set; }
    }
}
