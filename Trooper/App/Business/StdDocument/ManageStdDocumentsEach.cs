//--------------------------------------------------------------------------------------
// <copyright file="ManageStdDocumentsEach.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Business.StdDocument
{
    using Trooper.App.Business.StdDocument.StdModel;

    /// <summary>
    /// An instance of this class passed for each to function in the ManageDocumentsM2M method
    /// of the StdDocumentFacade for each document processed.
    /// </summary>
    /// <typeparam name="TStdDocumentNav">
    /// The Document nav class
    /// </typeparam>
    /// <typeparam name="TStdDocumentContentNav">
    /// The document content nav class
    /// </typeparam>
    public class ManageStdDocumentsEach<TStdDocumentNav, TStdDocumentContentNav>
        where TStdDocumentNav : class, IStdDocumentNav<TStdDocumentContentNav>, new()
    {
        /// <summary>
        /// The changes that can take place.
        /// </summary>
        public enum ChangeTypes
        {
            /// <summary>
            /// The document was added
            /// </summary>
            Add,

            /// <summary>
            /// The document was updated
            /// </summary>
            Update,

            /// <summary>
            /// The document was deleted
            /// </summary>
            Delete
        }

        /// <summary>
        /// Gets or sets the change type that took place on the document.
        /// </summary>
        public ChangeTypes ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the std document nav that was effected.
        /// </summary>
        public TStdDocumentNav StdDocumentNav { get; set; }
    }
}
