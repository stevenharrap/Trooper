//--------------------------------------------------------------------------------------
// <copyright file="CruncherStore.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Cruncher
{
    using System;
    using System.Collections.Generic;
    using Trooper.Ui.Interface.Mvc.Cruncher;

    /// <summary>
    /// Contains the references to the Scripts and CSS files. When the Controller
    /// wants to create the JavaScript and CSS responses it will look at the file paths
    /// recorded here.
    /// </summary>
    public class CruncherStore : Trooper.Ui.Mvc.Cruncher.ICruncherStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CruncherStore"/> class.
        /// </summary>
        /// <param name="id">
        /// The unique id of the store.
        /// </param>
        public CruncherStore(Guid id)
        {
            this.JsItems = new List<IStoreItem>();
            this.CssItems = new List<IStoreItem>();
            this.Id = id;
        }
         
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets JavaScript Items.
        /// </summary>
        private IList<IStoreItem> JsItems { get; set; }

        /// <summary>
        /// Gets or sets CSS Items.
        /// </summary>
        private IList<IStoreItem> CssItems { get; set; }

        /// <summary>
        /// The list of JavaScript file references
        /// </summary>
        /// <returns>
        /// Returns a StoreItemList 
        /// </returns>
        public IList<IStoreItem> Js()
        {
            return this.JsItems;
        }

        /// <summary>
        /// The list of Css file references
        /// </summary>
        /// <returns>
        /// Returns a StoreItemList 
        /// </returns>
        public IList<IStoreItem> Css()
        {
            return this.CssItems;
        }
    }
}
