//--------------------------------------------------------------------------------------
// <copyright file="IStdDocumentContentPrp.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Business.StdDocument.StdModel
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The document content property.
    /// </summary>
    public interface IStdDocumentContentPrp : IStdDocumentContentKey
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [Required]
        byte[] Data { get; set; }
    }
}
