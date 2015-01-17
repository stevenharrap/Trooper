//--------------------------------------------------------------------------------------
// <copyright file="IEntityKey.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Interface
{
    using System;

    /// <summary>
    /// The EntityKey interface. The key component of your entities should inherit this interface.
    /// </summary>
    /// <typeparam name="TEntityKey">
    /// The entity key that is inheriting this interface.
    /// </typeparam>
    public interface IEntityKey<TEntityKey> : IEquatable<TEntityKey>
    {
        /// <summary>
        /// Is that key an automatically generated key. You should return false
        /// if the key of your table is not automatically generated. If you key is not automatically
        /// generated then IsEntityNew should always return false.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsKeyAuto();

        /// <summary>
        /// Does the properties of the class indicate that the entity does not yet exist in the
        /// DB. I.e. integers are 0 and strings are empty. Any other situation does not indicate that 
        /// the entity exists. 
        /// </summary>
        /// <remarks>
        /// If IsKeyAuto is false then a value in the property could indicate that entity is to be inserted
        /// and that is the value for the new key. In such a case this should be made to always return false.
        /// </remarks>
        /// <returns>
        /// Returns true if the entity is considered new.
        /// </returns>
        bool IsEntityNew();
    }
}