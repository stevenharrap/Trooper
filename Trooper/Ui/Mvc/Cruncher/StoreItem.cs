//--------------------------------------------------------------------------------------
// <copyright file="StoreItem.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Cruncher
{
    /// <summary>
    /// Holds a reference to a file or actual content 
    /// </summary>
    public class StoreItem
    {
        /// <summary>
        /// In relation to other items in the store should this one be first or last in the list 
        /// of concatenated content that is sent to the client. These settings are important in
        /// relation to JavaScripts as they will ensure that core classes are declared first, then
        /// application specific classes and then class declarations last.
        /// </summary>
        public enum OrderOptions
        {
            /// <summary>
            /// Will be first (or near the front)
            /// Core scripts that have no requirements for other scripts to be present should be first.
            /// An example would by JQuery
            /// </summary>
            First = 1,

            /// <summary>
            /// The middle.
            /// Application specific scripts should be here. These are javaScript function classes that
            /// do not try to instantiate them selves.
            /// </summary>
            Middle = 2,

            /// <summary>
            /// Will be last (or near the end). These would typically be instantiations of the function
            /// classes in the first and middle
            /// </summary>
            Last = 3
        }

        /// <summary>
        /// Does this item represent a reference to a file or actual content
        /// </summary>
        public enum ReferenceOptions
        {
            /// <summary>
            /// File reference
            /// </summary>
            File = 1,

            /// <summary>
            /// Actual content
            /// </summary>
            Inline = 2
        }

        /// <summary>
        /// Gets or sets File reference
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets a unique name for this Item. If an item with the same
        /// name already exists in the StoreItemList then this item wont be included.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the CSS should be treated as Less and therefor
        /// pre-processed before bing included into the CSS output.
        /// </summary>
        public bool Less { get; set; }

        /// <summary>
        /// Gets or sets Order.
        /// </summary>
        public OrderOptions Order { get; set; }

        /// <summary>
        /// Gets or sets Reference.
        /// </summary>
        public ReferenceOptions Reference { get; set; }
    }
}
