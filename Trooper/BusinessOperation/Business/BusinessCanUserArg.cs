// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusinessCanUserArg.cs" company="Trooper Inc">
//   Copyright (c) Trooper 2014 - Onwards
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Business
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Trooper.BusinessOperation.Interface;

    /// <summary>
    /// Instances of this class are passed to the CanUser method of the business object class.
    /// </summary>
    /// <typeparam name="TSearch">
    /// The search class.
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The entity class.
    /// </typeparam>
    /// <typeparam name="TEntityPrp">
    /// The entity property class.
    /// </typeparam>
    /// <typeparam name="TEntityKey">
    /// The entity key class.
    /// </typeparam>
    public class BusinessCanUserArg<TSearch, TEntity, TEntityPrp, TEntityKey>
       : IBusinessCanUserArg<TSearch, TEntity, TEntityPrp, TEntityKey>
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
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the search instance. Only supplied if appropriated to the action.
        /// </summary>
        public TSearch Search { get; set; }

        /// <summary>
        /// Gets or sets the entity key. This will get the first element in entities
        /// as an entity key (if there are any) or set the first element in entities.
        /// </summary>
        public TEntityKey EntityKey
        {
            get
            {
                return this.Entities == null ? null : this.Entities.First();
            }

            set
            {
                if (this.Entities == null)
                {
                    this.Entities = Mapper.Map<List<TEntity>>(value);
                }
                else if (this.Entities.Any())
                {
                    this.Entities[0] = Mapper.Map<TEntity>(value);
                }
                else
                {
                    this.Entities.Add(Mapper.Map<TEntity>(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets the entity. This will get the first element in entities
        /// as an entity (if there are any) or set the first element in entities.
        /// </summary>
        public TEntity Entity
        {
            get
            {
                return this.Entities == null ? null : this.Entities.First();
            }

            set
            {
                if (this.Entities == null)
                {
                    this.Entities = Mapper.Map<List<TEntity>>(value);
                }
                else if (this.Entities.Any())
                {
                    this.Entities[0] = Mapper.Map<TEntity>(value);
                }
                else
                {
                    this.Entities.Add(Mapper.Map<TEntity>(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets the entity keys. This will get the element in entities
        /// as entity keys (if there are any) or set elements in entities.
        /// </summary>
        public IList<TEntityKey> EntityKeys
        {
            get
            {
                return this.Entities == null ? null : this.Entities as List<TEntityKey>;
            }

            set
            {
                this.Entities = value as List<TEntity>;
            }
        }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        public IList<TEntity> Entities { get; set; }
    }
}
