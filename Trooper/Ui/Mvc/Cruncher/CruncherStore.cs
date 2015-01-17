//--------------------------------------------------------------------------------------
// <copyright file="CruncherStore.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Cruncher
{
    using System;

    /// <summary>
    /// Contains the references to the Scripts and CSS files. When the Controller
    /// wants to create the JavaScript and CSS responses it will look at the file paths
    /// recorded here.
    /// </summary>
    public class CruncherStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CruncherStore"/> class.
        /// </summary>
        /// <param name="id">
        /// The unique id of the store.
        /// </param>
        public CruncherStore(Guid id)
        {
            this.HeaderJsItems = new StoreItemList();
            this.FooterJsItems = new StoreItemList();
            this.HeaderCssItems = new StoreItemList();
            this.Id = id;
        }
         
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets Header JavaScript Items.
        /// </summary>
        private StoreItemList HeaderJsItems { get; set; }

        /// <summary>
        /// Gets or sets Footer JavaScript Items.
        /// </summary>
        private StoreItemList FooterJsItems { get; set; }

        /// <summary>
        /// Gets or sets Header CSS Items.
        /// </summary>
        private StoreItemList HeaderCssItems { get; set; }

        /// <summary>
        /// The list of Header JavaScript file references
        /// </summary>
        /// <returns>
        /// Returns a StoreItemList 
        /// </returns>
        public StoreItemList HeaderJs()
        {
            return this.HeaderJsItems;
        }

        /// <summary>
        /// The list of Footer JavaScript file references
        /// </summary>
        /// <returns>
        /// Returns a StoreItemList 
        /// </returns>
        public StoreItemList FooterJs()
        {
            return this.FooterJsItems;
        }

        /// <summary>
        /// The list of Header Less file references
        /// </summary>
        /// <returns>
        /// Returns a StoreItemList 
        /// </returns>
        public StoreItemList HeaderCss()
        {
            return this.HeaderCssItems;
        }
    }
}
