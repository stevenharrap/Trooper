//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCanUserArg.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Interface
{
    using System.Collections.Generic;

    /// <summary>
    /// Instances deriving this interface are passed to the CanUser Method in the business object class.
    /// of Access.
    /// </summary>
    /// <typeparam name="TSearch">The search class</typeparam>
    /// <typeparam name="TEntity">The entity class</typeparam>
    /// <typeparam name="TEntityPrp">The entity property class</typeparam>
    /// <typeparam name="TEntityKey">The entity key property class</typeparam>
    /// <summary>
    /// Instances deriving this interface are passed to the CanUser Method
    /// of Access.
    /// </summary>
    public interface IBusinessCanUserArg<TSearch, TEntity, TEntityPrp, TEntityKey>
        where TSearch : class, ISearch, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// Gets or sets the action being undertaken. If this is left blank then the check determines
        /// if the user has access to any action at all. The Access class should have a mechanism
        /// for dealing with the "AllActions" string.
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// Gets or sets the search instance. Only supplied if appropriated to the action.
        /// </summary>
        TSearch Search { get; set; }

        /// <summary>
        /// Gets or sets the entity key. Gets or sets the first item in Entities
        /// </summary>
        TEntityKey EntityKey { get; set; }

        /// <summary>
        /// Gets or sets the entity. Gets or sets the first item in Entities
        /// </summary>
        TEntity Entity { get; set; }

        /// <summary>
        /// Gets or sets the entity Keys. Gets or sets the items in Entities
        /// </summary>
        IList<TEntityKey> EntityKeys { get; set; }

        /// <summary>
        /// Gets or sets the entities. All entity information stored in here
        /// </summary>
        IList<TEntity> Entities { get; set; }
    }
}
