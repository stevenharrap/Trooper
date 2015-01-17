namespace Trooper.App.Ui.StdTemplate
{
    /// <summary>
    /// The commands. All forms want to do at least one of these action on posting.
    /// There may be other actions - not sure how to handle that. But it hasn't happened yet!
    /// </summary>
    public class Commands
    {
        /// <summary>
        /// Update a record.
        /// </summary>
        public const string Update = "Update";

        /// <summary>
        /// Add a record
        /// </summary>
        public const string Add = "Add";

        /// <summary>
        /// Adding or updating a record
        /// </summary>
        public const string Save = "Save";

        /// <summary>
        /// Delete a record
        /// </summary>
        public const string Delete = "Delete";

        /// <summary>
        /// Unknown default action
        /// </summary>
        public const string Unknown = "Unknown";

        /// <summary>
        /// Refresh the current view without saving anything
        /// </summary>
        public const string Refresh = "Refresh";

        /// <summary>
        /// View a record
        /// </summary>
        public const string View = "View";

        /// <summary>
        /// Duplicate a record
        /// </summary>
        public const string Duplicate = "Duplicate";

        /// <summary>
        /// Clear user input
        /// </summary>
        public const string Clear = "Clear";

        /// <summary>
        /// Clear user input
        /// </summary>
        public const string Validate = "Validate";

        /// <summary>
        /// Upload a document
        /// </summary>
        public const string Upload = "Upload";

        /// <summary>
        /// Provide a list of records
        /// </summary>
        public const string List = "List";
    }
}
