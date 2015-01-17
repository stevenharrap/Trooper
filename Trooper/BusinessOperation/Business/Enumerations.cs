namespace Trooper.BusinessOperation.Business
{
    /// <summary>
    /// Indicates if the save was an add or update.
    /// </summary>
    public enum SaveChangeType
    {
        /// <summary>
        /// It was an Add operation.
        /// </summary>
        Add,

        /// <summary>
        /// It was an update operation.
        /// </summary>
        Update,

        /// <summary>
        /// No change was accomplished
        /// </summary>
        None
    }

    /// <summary>
    /// The possible levels of error for a message
    /// </summary>
    public enum MessageAlertLevel 
    {
        /// <summary>
        /// The operation resulted in an error and the 
        /// user should be notified to correct the issue.
        /// </summary>
        Error,

        /// <summary>
        /// The operation was successful but the user should be notified
        /// of an usual but valid state. I.e. Age > 100
        /// </summary>
        Warning,

        /// <summary>
        /// Some helpful but none critical information for the user.
        /// </summary>
        Note
    }

    /// <summary>
    /// The manage action types.
    /// </summary>
    public enum ManageAction
    {
        /// <summary>
        /// The delete record referenced by this item
        /// </summary>
        Delete,

        /// <summary>
        /// Remove the reference to this in the primary record
        /// </summary>
        Remove,

        /// <summary>
        /// The the properties of this item
        /// </summary>
        Change
    }
}
