//--------------------------------------------------------------------------------------
// <copyright file="FacadeCanUserArg.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Business
{
    using System.Collections.Generic;
    using System.Linq;

    using Trooper.BusinessOperation.Interface;

    /// <summary>
    /// Instances of this class are passed to the CanUser method in the facade.
    /// </summary>
    /// <typeparam name="TSearch">The search class</typeparam>
    /// <typeparam name="TEntityNav">The entity nav class</typeparam>
    /// <typeparam name="TEntity">The entity class</typeparam>
    /// <typeparam name="TEntityPrp">The entity property class</typeparam>
    /// <typeparam name="TEntityKey">The entity key class</typeparam>
    public class FacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
       : IFacadeCanUserArg<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
        where TSearch : class, ISearch, new()
        where TEntityNav : class, TEntity, new()
        where TEntity : class, TEntityPrp, new()
        where TEntityPrp : class, TEntityKey, new()
        where TEntityKey : class, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// Gets or sets the action being undertaken. If this is left blank then the check determines
        /// if the user has access to any action at all. The Access class should have a mechanism
        /// for dealing with the "AllActions" string.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the search instance. Only supplied if appropriated to the action.
        /// </summary>
        public TSearch Search { get; set; }

        /// <summary>
        /// Gets or sets the entity nav. This will get the first element in entities
        /// as an entity nav (if there are any) or set the first element in entities.
        /// </summary>
        public TEntityNav Entity
        {
            get
            {
                if (this.Entities == null || !this.Entities.Any())
                {
                    return null;
                }

                return this.Entities.First();
            }

            set
            {
                if (this.Entities == null)
                {
                    this.Entities = new List<TEntityNav> { value };
                }
                else if (this.Entities.Any())
                {
                    this.Entities[0] = value;
                }
                else
                {
                    this.Entities.Add(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        public IList<TEntityNav> Entities { get; set; }
    }
}
