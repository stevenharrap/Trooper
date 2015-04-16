//--------------------------------------------------------------------------------------
// <copyright file="RowFormat.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Rabbit.Props.Table
{
    /// <summary>
    /// The row highlighting.
    /// </summary>
    public enum RowHighlight
    {
        /// <summary>
        /// The none.
        /// </summary>
        None,

        /// <summary>
        /// The success.
        /// </summary>
        Success,

        /// <summary>
        /// The danger.
        /// </summary>
        Danger,

        /// <summary>
        /// The warning.
        /// </summary>
        Warning,

        /// <summary>
        /// The active.
        /// </summary>
        Active,

        /// <summary>
        /// The info.
        /// </summary>
        Info
    }

    /// <summary>
    /// The rule style.
    /// </summary>
    public enum RuleStyle
    {
        /// <summary>
        /// The default.
        /// </summary>
        Default,

        /// <summary>
        /// The solid.
        /// </summary>
        Solid,

        /// <summary>
        /// The dashed.
        /// </summary>
        Dashed
    }

    /// <summary>
    /// The class for describing how a row should display
    /// </summary>
    /// <typeparam name="TKey">The data type on the key value for the row</typeparam>
    public class RowFormat<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RowFormat{TKey}"/> class.
        /// </summary>
        public RowFormat()
        {
            this.Highlighted = RowHighlight.None;
            this.RuleUnderStyle = RuleStyle.Default;
            this.RuleUnderStyle = RuleStyle.Default;
            this.RowTextHighlightStyle = RowTextHighlight.None;
        }

        /// <summary>
        /// Gets or sets The key for the row
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// Gets or sets the highlighted.
        /// </summary>
        public RowTextHighlight RowTextHighlightStyle { get; set; }

        /// <summary>
        /// Gets or sets highlighted - should the row be highlighted?
        /// </summary>
        public RowHighlight Highlighted { get; set; }

        /// <summary>
        /// Gets or sets the rule under style. Should the row have a line under it. A following row
        /// will have any top border removed.
        /// </summary>
        public RuleStyle RuleUnderStyle { get; set; }

        /// <summary>
        /// Gets or sets the rule over style.
        /// Should the row have a line over it. If the row above
        /// has RuleUnderStyle set then this property will be ignored.
        /// </summary>
        public RuleStyle RuleOverStyle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether bold.
        /// Make the text of the row bold
        /// </summary>
        public bool Bold { get; set; }
    }
}
