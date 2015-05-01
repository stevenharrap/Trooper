namespace Trooper.Ui.Interface.Mvc.Cruncher
{
    using System;
    using System.Collections.Generic;

    public interface ICruncherStore
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
