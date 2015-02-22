namespace Trooper.Ui.Interface.Mvc.Cruncher
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

    public interface IStoreItem
    {
        /// <summary>
        /// Gets or sets File reference
        /// </summary>
        string File { get; set; }

        /// <summary>
        /// Gets or sets a unique name for this Item. If an item with the same
        /// name already exists in the StoreItemList then this item wont be included.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets Content.
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the CSS should be treated as Less and therefor
        /// pre-processed before bing included into the CSS output.
        /// </summary>
        bool Less { get; set; }

        /// <summary>
        /// Gets or sets Order.
        /// </summary>
        OrderOptions Order { get; set; }

        /// <summary>
        /// Gets or sets Reference.
        /// </summary>
        ReferenceOptions Reference { get; set; }
    }
}
