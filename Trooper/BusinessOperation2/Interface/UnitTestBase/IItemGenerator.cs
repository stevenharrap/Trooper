﻿namespace Trooper.BusinessOperation2.Interface.UnitTestBase
{
    using Trooper.BusinessOperation2.Interface.DataManager;

    /// <summary>
    ///     IItemGenerator creates instance of the generic type Tc
    /// </summary>
    /// <typeparam name="Tc"></typeparam>
    /// <typeparam name="Ti"></typeparam>
    public interface IItemGenerator<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        /// <summary>
        ///     Any genenerated instance which is not requested to be identical to a supplied
        ///     instance will use this value as the seed for its strings and numeric values.
        ///     The value will be incremented after each use.
        /// </summary>
        int ItemObjCount { get; set; }

        /// <summary>
        ///     Makes a new instance of Tc. The properties of the new item are identical to the supplied item.
        /// </summary>
        /// <param name="item">
        ///     The returned item will have properties that are identical to this instance.
        /// </param>
        /// <returns>
        ///     An instance of Tc
        /// </returns>
        Tc CopyItem(Tc item);

        /// <summary>
        ///     Makes a new instance of Tc. The properties of the instance will have values
        ///     that have not been used before. Key properties will not be populated.
        /// </summary>
        /// <param name="facade">
        ///     The facade to get the key properties from.
        /// </param>
        /// <returns>
        ///     An instance of Tc
        /// </returns>
        Tc NewItem(IFacade<Tc, Ti> facade);
    }
}
