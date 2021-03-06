﻿//--------------------------------------------------------------------------------------
// <copyright file="Action.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Thorny.Business.Security
{
    /// <summary>
    /// This class provides the Business Operation methods names as constant strings. 
    /// These are used against the CanUser method in Access interface.
    /// </summary>
    public class OperationAction
    {
        /// <summary>
        /// Represents all actions. Testing against this action implies that
        /// you want to know if the user can access any action.
        /// </summary>
        public const string AllActions = "*";

	    public const string AllAddActions = "*Add";

        /// <summary>
        /// Represents all actions that result in removal of records. Testing against this action implies that
        /// you want to know if the user can access any action that results in deletion. The supplied method
        /// tests against DeleteByKey DeleteSomeByKey
        /// </summary>
        public const string AllRemoveActions = "*Remove";

        /// <summary>
        /// Represents all actions that result in change. Testing against this action implies that
        /// you want to know if the user can access any action that results in change. The supplied method
        /// tests against Add, AddSome, Update, Save, SaveSome, DeleteByKey and DeleteSomeByKey
        /// </summary>
        public const string AllChangeActions = "*Change";

		public const string AllUpdateActions = "*Update";

        public const string AllSaveActions = "*Save";

        /// <summary>
        /// Represents all actions that result in reads. Testing against this action implies that
        /// you want to know if the user can access any action that only results in a read. The supplied method
        /// tests against GetByKey, GetAll and GetSome.
        /// </summary>
        public const string AllReadActions = "*Read";

        /// <summary>
        /// The add action.
        /// </summary>
        public const string AddAction = "Add";

        /// <summary>
        /// The add some action.
        /// </summary>
        public const string AddSomeAction = "AddSome";

        /// <summary>
        /// The update action.
        /// </summary>
        public const string UpdateAction = "Update";

		public const string UpdateSomeAction = "UpdateSome";

        public const string SaveAction = "Save";

        public const string SaveSomeAction = "SaveSome";

        /// <summary>
        /// The delete by key action.
        /// </summary>
        public const string DeleteByKeyAction = "DeleteByKey";

        /// <summary>
        /// The delete some by key action.
        /// </summary>
        public const string DeleteSomeByKeyAction = "DeleteSomeByKey";

        /// <summary>
        /// The get by key action.
        /// </summary>
        public const string GetByKeyAction = "GetByKey";

		public const string GetSomeByKeyAction = "GetByKey";

        /// <summary>
        /// The exists action
        /// </summary>
        public const string ExistsByKeyAction = "ExistsByKey";

        /// <summary>
        /// The get some action.
        /// </summary>
        public const string GetSomeAction = "GetSome";

        /// <summary>
        /// The get all action.
        /// </summary>
        public const string GetAllAction = "GetAll";

        public const string IsAllowedAction = "IsAllowed";

        public const string GetSession = "GetSession";
    }
}
