//--------------------------------------------------------------------------------------
// <copyright file="IStdDocumentKey.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Business.StdDocument.StdModel
{
    using Trooper.BusinessOperation.Interface;

    /// <summary>
    /// The document key.
    /// </summary>
    public interface IStdDocumentKey : IEntityKey<IStdDocumentKey>
    {
        /// <summary>
        /// Gets or sets the document id.
        /// </summary>
        int DocumentId { get; set; }
    }
}
