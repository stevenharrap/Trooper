//--------------------------------------------------------------------------------------
// <copyright file="ISearch.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Thorny.Interface.DataManager
{
    /// <summary>
    /// The Search interface.
    /// </summary>
    public interface ISearch
    {
        /// <summary>
        /// Gets or sets the skip items. Will skip N records if greater than 0.
        /// </summary>
        int SkipItems { get; set; }

        /// <summary>
        /// Gets or sets the number of items that the search should return. 
        /// If 0 then it will be ignored.
        /// If SkipItems is used then it N items will be skipped and then N items taken.
        /// </summary>
        int TakeItems { get; set; }
    }
}
