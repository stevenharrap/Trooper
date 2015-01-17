//--------------------------------------------------------------------------------------
// <copyright file="ScopedDbContextManager.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.DataManager
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Manages multiple db contexts.
    /// </summary>
    public sealed class ScopedDbContextManager : DbContextManager
    {
        /// <summary>
        /// List of Object Contexts.
        /// </summary>
        private List<DbContext> contextList;

        /// <summary>
        /// Returns the DbContext instance that belongs to the current DbContextScope.
        /// If currently no DbContextScope exists, a local instance of an DbContext
        /// class is returned.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the db context.</typeparam>
        /// <returns>Current scoped DbContext.</returns>
        public override TDbContext GetDbContext<TDbContext>()
        {
            var currentDbContext = DbContextScope.GetCurrentDbContext<TDbContext>();

            if (currentDbContext != null)
            {
                return currentDbContext;
            }

            if (this.contextList == null)
            {
                this.contextList = new List<DbContext>();
            }

            currentDbContext = this.contextList.OfType<TDbContext>().FirstOrDefault();

            if (currentDbContext == null)
            {
                currentDbContext = new TDbContext();

                this.contextList.Add(currentDbContext);
            }

            return currentDbContext;
        }
    }
}