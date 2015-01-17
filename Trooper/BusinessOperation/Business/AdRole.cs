// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdRole.cs" company="Trooper Inc">
//   Copyright (c) Trooper 2014 - Onwards
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Business
{
    using System.Collections.Generic;

    /// <summary>
    /// The Active Directory role.
    /// This class provides the data for the Business classes to determine if the given user
    /// can perform the required action.
    /// </summary>
    public class AdRole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdRole"/> class.
        /// </summary>
        public AdRole()
        {
            this.Allow = true;
        }

        /// <summary>
        /// Gets or sets the action required.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the user groups who can perform this action.
        /// </summary>
        public List<string> UserGroups { get; set; }

        /// <summary>
        /// Gets or sets the users who can perform this action.
        /// </summary>
        public List<string> Users { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether allow or prevent the action.
        /// By default it will be true. I.e the user will be allowed to perform the action.
        /// </summary>
        public bool Allow { get; set; }
    }
}
