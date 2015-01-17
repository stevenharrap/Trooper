//--------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkScope.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.DataManager
{
    /// <summary>
    /// Defines a scope for a business transaction. At the end of the scope all object changes can be persisted to the underlying data store. 
    /// </summary>
    /// <remarks>Instances of this class are supposed to be used in a using() statement.</remarks>
    public sealed class UnitOfWorkScope : DbContextScope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkScope"/> class.
        /// </summary>
        /// <param name="saveAllChangesAtEndOfScope">if set to <c>true</c> [save all changes at end of scope].</param>
        public UnitOfWorkScope(bool saveAllChangesAtEndOfScope)
            : base(saveAllChangesAtEndOfScope)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkScope"/> class. Will not call all changes
        /// at the end of scope. SaveAllChanges must be called by your code.
        /// </summary>
        public UnitOfWorkScope()
            : base(false)
        {
        }
    }
}