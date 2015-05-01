// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserRole.cs" company="Trooper Inc">
//   Copyright (c) Trooper 2014 - Onwards
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Trooper.Interface.BusinessOperation2.Business.Security
{
	/// <summary>
    /// This class provides the data for the Business classes to determine if the given user
    /// can perform the required action.
    /// </summary>
    public interface IUserRole
    {
        /// <summary>
        /// Gets or sets the action required.
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// Gets or sets the user groups who can perform this action.
        /// </summary>
        IList<string> UserGroups { get; set; }

        /// <summary>
        /// Gets or sets the users who can perform this action.
        /// </summary>
        IList<string> Users { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether allow or prevent the action.
        /// By default it will be true. I.e the user will be allowed to perform the action.
        /// </summary>
        bool Allow { get; set; }
    }
}
