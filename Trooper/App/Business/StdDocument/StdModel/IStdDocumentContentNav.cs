//--------------------------------------------------------------------------------------
// <copyright file="IStdDocumentContentNav.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Business.StdDocument.StdModel
{
    /// <summary>
    /// The document content nav.
    /// </summary>
    /// <typeparam name="TDocumentNav">
    /// The document nav class
    /// </typeparam>
    public interface IStdDocumentContentNav<TDocumentNav> : IStdDocumentContent
    {
        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        TDocumentNav DocumentNav { get; set; }
    }
}
