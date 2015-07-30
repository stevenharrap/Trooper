namespace Trooper.Thorny.Interface.UnitTestBase
{
    using Trooper.Thorny.Interface.DataManager;

    /// <summary>
    ///     IItemGenerator creates instance of the generic type Tc
    /// </summary>
    /// <typeparam name="TEnt"></typeparam>
    /// <typeparam name="TPoco"></typeparam>
    public interface IItemGenerator<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
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
        TEnt CopyItem(TEnt item);

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
        TEnt NewItem(IFacade<TEnt, TPoco> facade);
    }
}
