// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserRole.cs" company="Trooper Inc">
//   Copyright (c) Trooper 2014 - Onwards
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Trooper.Interface.Thorny.Business.Security
{
	/// <summary>
    /// This class provides the data for the Business classes to determine if the given user
    /// can perform the required action.
    /// </summary>
    public interface IAssignment
    {
        /// <summary>
        /// Gets or sets the action required.
        /// </summary>
        IRole Role { get; set; }

        /// <summary>
        /// Gets or sets the user groups who can perform this action.
        /// </summary>
        IList<string> UserGroups { get; set; }

        /// <summary>
        /// Gets or sets the users who can perform this action.
        /// </summary>
        IList<string> Users { get; set; }

		int Precedence { get; set; }
    }
}
