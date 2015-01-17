//--------------------------------------------------------------------------------------
// <copyright file="IFacadeCanUserArg.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Interface
{
    using System.Collections.Generic;

    /// <summary>
    /// Instances deriving this interface are passed to the CanUser Method
    /// of Access.
    /// </summary>
    /// <typeparam name="TSearch">
    /// The search class
    /// </typeparam>
    /// <typeparam name="TEntityNav">
    /// The entity nav class
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The entity class
    /// </typeparam>
    /// <typeparam name="TEntityPrp">
    /// The entity property class
    /// </typeparam>
    /// <typeparam name="TEntityKey">
    /// The entity key property class
    /// </typeparam>
    /// <summary>
    /// Instances deriving this interface are passed to the CanUser Method
    /// of Access.
    /// </summary>
    public interface IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
        where TSearch : class, ISearch, new()
        where TEntityNav : class, TEntity, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
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
        TSearch Search { get; set; }

        /// <summary>
        /// Gets or sets the first item in Entities. 
        /// These should always be fully connected entities.
        /// </summary>
        TEntityNav Entity { get; set; }

        /// <summary>
        /// Gets or sets the entities. All entity information stored in here. 
        /// These should always be fully connected entities.
        /// </summary>
        IList<TEntityNav> Entities { get; set; }
    }
}
