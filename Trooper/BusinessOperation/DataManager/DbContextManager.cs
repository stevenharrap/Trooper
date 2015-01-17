//--------------------------------------------------------------------------------------
// <copyright file="DbContextManager.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.DataManager
{
    using System.Data.Entity;

    /// <summary>
    /// Abstract base class for all other DbContextManager classes.
    /// </summary>
    public abstract class DbContextManager
    {
        /// <summary>
        /// Returns a reference to an DbContext instance.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the db context.</typeparam>
        /// <returns>The current DbContext</returns>
        public abstract TDbContext GetDbContext<TDbContext>() 
            where TDbContext : DbContext, new();
    }
}