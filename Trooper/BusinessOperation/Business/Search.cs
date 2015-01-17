//--------------------------------------------------------------------------------------
// <copyright file="Search.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Business
{
    using Trooper.BusinessOperation.Interface;

    /// <summary>
    /// Provides a default implementation of ISearch
    /// </summary>
    public class Search : ISearch
    {
        /// <summary>
        /// Gets or sets the skip items. Will skip N records if greater than 0.
        /// </summary>
        public int SkipItems { get; set; }

        /// <summary>
        /// Gets or sets the number of items that the search should return. 
        /// If 0 then it will be ignored.
        /// If SkipItems is used then it N items will be skipped and then N items taken.
        /// </summary>
        public int TakeItems { get; set; }
    }
}
