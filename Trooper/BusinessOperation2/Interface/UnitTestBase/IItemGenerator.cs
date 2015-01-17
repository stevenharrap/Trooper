using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.BusinessOperation2.Interface.UnitTestBase
{
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
        ///     Makes a new instance of Tc
        /// </summary>
        /// <param name="identical">
        ///     If true then the new item will have properties with the same value as the supplied item.
        ///     If false then ItemObjCount will be used as the seed for values in the returned instance.
        /// </param>
        /// <param name="item">
        ///     If identical is true then the new item will have properties that are identical to this instance.
        /// </param>
        /// <returns>
        ///     An instance of Tc
        /// </returns>
        Tc MakeItem(bool identical, Tc entity);

        /// <summary>
        ///     Makes a new instance of Tc. The properties of the instance will have values
        ///     that have not been used before. Asserts that the result will not be null.
        /// </summary>
        /// <returns>
        ///     An instance of Tc
        /// </returns>
        Tc ItemFactory();

        /// <summary>
        ///     Makes a new instance of Tc and Asserts that the result will not be null.
        /// </summary>
        /// <param name="identical">
        ///     If true then the new item will have properties with the same value as the supplied item.
        ///     If false then ItemObjCount will be used as the seed for values in the returned instance.
        /// </param>
        /// <param name="item">
        ///     If identical is true then the new item will have properties that are identical to this instance.
        /// </param>
        /// <returns>
        ///     An instance of Tc
        /// </returns>
        Tc ItemFactory(bool identical, Tc item);
    }
}
