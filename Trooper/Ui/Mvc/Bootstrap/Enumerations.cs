//--------------------------------------------------------------------------------------
// <copyright file="Enumerations.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Bootstrap
{
    /// <summary>
    /// Possible sizes for the display text boxes
    /// </summary>
    public enum ButtonTypes
    {
        /// <summary>
        /// No special formatting should be applied
        /// </summary>
        None,

        /// <summary>
        /// Used as the default styles
        /// </summary>
        Default,

        /// <summary>
        /// Provides extra visual weight and identifies the primary action in a set of buttons
        /// </summary>
        Primary,

        /// <summary>
        /// Used as an alternative to the default styles
        /// </summary>
        Info,

        /// <summary>
        /// Indicates a successful or positive action
        /// </summary>
        Success,

        /// <summary>
        /// Indicates caution should be taken with this action
        /// </summary>
        Warning,

        /// <summary>
        /// Indicates a dangerous or potentially negative action
        /// </summary>
        Danger,

        /// <summary>
        /// Alternate dark gray button, not tied to a semantic action or use
        /// </summary>
        Inverse,

        /// <summary>
        /// Deemphasize a button by making it look like a link while maintaining button behavior
        /// </summary>
        Link,
    }

    /// <summary>
    /// The text size in text boxes
    /// </summary>
    public enum InputTextSizes
    {
        /// <summary>
        /// Large size
        /// </summary>
        Large,

        /// <summary>
        /// Default size
        /// </summary>
        Default,

        /// <summary>
        /// Small size
        /// </summary>
        Small
    }

    /// <summary>
    /// The date time format to use in the DateTime controls
    /// </summary>
    public enum DateTimeFormat
    {
        /// <summary>
        /// The date and time.
        /// </summary>
        DateAndTime,

        /// <summary>
        /// Just date.
        /// </summary>
        Date,

        /// <summary>
        /// Just time.
        /// </summary>
        Time,

        /// <summary>
        /// Just time with no seconds.
        /// </summary>
        TimeNoSeconds,

        /// <summary>
        /// Date and time with no seconds.
        /// </summary>
        DateTimeNoSeconds
    }

    /// <summary>
    /// The grid sizes used in Bootstrap
    /// </summary>
    public enum GridCellSize
    {
        /// <summary>
        /// Large grid size (lg)
        /// </summary>
        Large,

        /// <summary>
        /// Medium grid size (md)
        /// </summary>
        Medium,

        /// <summary>
        /// Small grid size (sm)
        /// </summary>
        Small,

        /// <summary>
        /// Extra small grid size (xs)
        /// </summary>
        ExtraSmall
    }

    /// <summary>
    /// The button drop-down direction.
    /// </summary>
    public enum ButtonDropDirection
    {
        /// <summary>
        /// The up.
        /// </summary>
        Up,

        /// <summary>
        /// The down.
        /// </summary>
        Down
    }

    /// <summary>
    /// Some controls use popovers - this sets the orientation of those popovers.
    /// </summary>
    public enum PopoverPlacements
    {
        /// <summary>
        /// On top.
        /// </summary>
        Top,

        /// <summary>
        /// On bottom.
        /// </summary>
        Bottom,

        /// <summary>
        /// On left.
        /// </summary>
        Left,

        /// <summary>
        /// On right.
        /// </summary>
        Right,

		/// <summary>
		/// Auto placement
		/// </summary>
		Auto

    }

    /// <summary>
    /// The title modes that define how the title is associated with the control
    /// </summary>
    public enum TitleModes
    {
        /// <summary>
        /// The control should have a label above it.
        /// </summary>
        Label,

        /// <summary>
        /// The control should have a label pre-pended to the start of it.
        /// </summary>
        PrePend
    }

    /// <summary>
    /// The numeric type used by the numeric inputs.
    /// </summary>
    public enum NumericType
    {
        /// <summary>
        /// Numeric Integer type.
        /// </summary>
        Integer,

        /// <summary>
        /// Numeric Decimal type.
        /// </summary>
        Decimal,

        /// <summary>
        /// Numeric currency type.
        /// </summary>
        Currency,

        /// <summary>
        /// Numeric percentage type.
        /// </summary>
        Percentage
    }

    /// <summary>
    /// The table row selection modes.
    /// </summary>
    public enum TableRowSelectionModes
    {
        /// <summary>
        /// No selection.
        /// </summary>
        None,

        /// <summary>
        /// Only 1 row can be selected.
        /// </summary>
        Single,

        /// <summary>
        /// Multiple rows can be selected.
        /// </summary>
        Multiple
    }

    /// <summary>
    /// The panel group error modes.
    /// </summary>
    public enum PanelGroupErrorModes
    {
        /// <summary>
        /// The scroll to top.
        /// </summary>
        ScrollToTop,

        /// <summary>
        /// The highlight titles.
        /// </summary>
        HightlighTitles
    }

    /// <summary>
    /// The row text highlight.
    /// </summary>
    public enum RowTextHighlight
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

}
