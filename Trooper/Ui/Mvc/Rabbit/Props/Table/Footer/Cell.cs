//--------------------------------------------------------------------------------------
// <copyright file="Cell.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Rabbit.Props.Table.Footer
{
    /// <summary>
    /// This is a cell within a footer row. It allows you to set specific class, colspan and content
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class and specifies that the content type is plain text.
        /// </summary>
        public Cell()
        {
            this.ContentType = ContentTypes.Text;
        }

        /// <summary>
        /// The content types of a cell.
        /// </summary>
        public enum ContentTypes
        {
            /// <summary>
            /// Complex html.
            /// </summary>
            Html,

            /// <summary>
            /// Plain text.
            /// </summary>
            Text
        }

        /// <summary>
        /// Gets or sets any extra CSS classes on the TD. At the moment "right" can be added to text-align right the content.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the column span that this cell should use. E.g. you want a cell that says "Total" that uses 7 of your 8 columns.
        /// </summary>
        public int ColSpan { get; set; }

        /// <summary>
        /// Gets or sets the content od the cell.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the content type of the cell.
        /// </summary>
        public ContentTypes ContentType { get; set; }
    }
}
