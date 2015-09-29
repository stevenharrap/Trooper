//--------------------------------------------------------------------------------------
// <copyright file="IFacadeCanUserArg.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;
using Trooper.Thorny.Interface.DataManager;

namespace Trooper.Interface.Thorny.Business.Security
{
	/// <summary>
    /// Instances deriving this interface are passed to the CanUser Method
    /// of Access.
    /// </summary>
    public interface IRequestArg<TPoco>         
    {
        /// <summary>
        /// Gets or sets The action being undertaken. If this is left blank then the check determines
        /// if the user has access to any action at all. The Access class should have a mechanism
        /// for dealing with the "AllActions" string.
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// Gets or sets the search instance. Only supplied if appropriated to the action.
        /// </summary>
        ISearch Search { get; set; }

        /// <summary>
        /// Gets or sets the first item in Entities. 
        /// These should always be fully connected entities.
        /// </summary>
        IEnumerable<TPoco> Items { get; set; }
    }
}
