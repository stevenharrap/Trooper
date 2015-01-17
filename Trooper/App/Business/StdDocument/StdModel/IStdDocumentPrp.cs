//--------------------------------------------------------------------------------------
// <copyright file="IStdDocumentPrp.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Business.StdDocument.StdModel
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The document property.
    /// </summary>
    public interface IStdDocumentPrp : IStdDocumentKey
    {
        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        string Filename { get; set; }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        string Extension { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [NotMapped]
        byte[] Data { get; set; }
    }
}
