namespace Trooper.Ui.Mvc.Cruncher
{
    using System;
    using System.Collections.Generic;
    using Trooper.Ui.Interface.Mvc.Cruncher;

    interface ICruncherStore
    {/// <summary>
        /// Gets or sets Id.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The list of JavaScript file references
        /// </summary>
        /// <returns>
        /// Returns a StoreItemList 
        /// </returns>
        IList<IStoreItem> Js();

        /// <summary>
        /// The list of Css file references
        /// </summary>
        /// <returns>
        /// Returns a StoreItemList 
        /// </returns>
        IList<IStoreItem> Css();
    }
}
