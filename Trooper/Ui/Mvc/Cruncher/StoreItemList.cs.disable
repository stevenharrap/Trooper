//--------------------------------------------------------------------------------------
// <copyright file="StoreItemList.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Cruncher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.WebPages;

    /// <summary>
    /// Provides a clear means of adding file and inline script references to CrunchStores
    /// </summary>
    public class StoreItemList : List<StoreItem>
    {
        /// <summary>
        /// Adds a file to the store
        /// </summary>
        /// <param name="relativePath">
        /// The relative path to the store (e.g. '~/about/stuff.js')
        /// </param>
        /// <param name="order">
        /// The order in which the reference should be added (inserted first or appended last)
        /// </param>
        /// <param name="less">
        /// If you are using CSS and it should be treated as Less then make this true.
        /// </param>
        /// <returns>
        /// The StoreItemList to which the file has been added
        /// </returns>
        public StoreItemList AddFile(string relativePath, StoreItem.OrderOptions order = StoreItem.OrderOptions.First, bool less = false)
        {
            if (this.Any(si => si.File == relativePath))
            {
                return this;
            }

            this.Add(new StoreItem { File = relativePath, Order = order, Reference = StoreItem.ReferenceOptions.File, Less = less });

            return this;
        }

        /// <summary>
        /// Adds a reference to content that should be directly added. I.e. inline code that is generated
        /// from a data source etc.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="relativePath">
        /// The relative Path if the content should is CSS and its URL references should be corrected to
        /// work from somewhere else other than the current page;
        /// </param>
        /// <param name="name">
        /// The name of the store item
        /// </param>
        /// <param name="order">
        /// The order in which the reference should be added (inserted first or appended last)
        /// </param>
        /// <param name="less">
        /// If you are using CSS and it should be treated as Less then make this true.
        /// </param>
        /// <returns>
        /// The StoreItemList to which the code has been added
        /// </returns>
        public StoreItemList AddInline(string content, string relativePath = null, string name = null, StoreItem.OrderOptions order = StoreItem.OrderOptions.Last, bool less = false)
        {
            if (!string.IsNullOrEmpty(name) && this.Exists(i => i.Name == name))
            {
                return this;
            }

            this.Add(
                new StoreItem
                {
                    File = relativePath,
                    Content = content,
                    Name = name,
                    Order = order,
                    Less = less,
                    Reference = StoreItem.ReferenceOptions.Inline
                });

            return this;
        }

        /// <summary>
        /// Adds a reference to content that should be directly added. I.e. inline code that is generated
        /// from a data source etc.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="relativePath">
        /// The relative Path if the content should is CSS and its URL references should be corrected to
        /// work from somewhere else other than the current page;
        /// </param>
        /// <param name="name">
        /// The name of the store item
        /// </param>
        /// <param name="order">
        /// The order in which the reference should be added (inserted first or appended last)
        /// </param>
        /// <param name="less">
        /// If you are using CSS and it should be treated as Less then make this true.
        /// </param>
        /// <returns>
        /// The StoreItemList to which the code has been added
        /// </returns>
        public StoreItemList AddInline(Func<object, HelperResult> content, string relativePath = null, string name = null, StoreItem.OrderOptions order = StoreItem.OrderOptions.Last, bool less = false)
        {
            if (!string.IsNullOrEmpty(name) && this.Exists(i => i.Name == name))
            {
                return this;
            }

            return this.AddInline(content.Invoke(null).ToString(), relativePath, name, order, less);
        }

        /// <summary>
        /// Determines if a name item exists in the store
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// Returns true if the item exists
        /// </returns>
        public bool HasItem(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return this.Exists(i => i.Name == name);
        }
    }
}
