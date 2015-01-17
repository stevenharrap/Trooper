// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPagedList.cs" company="Trooper Inc">
//   Copyright (c) Trooper 2014 - Onwards
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Trooper.Utility.PagedList
{
    /// <summary>
    /// <seealso cref="http://stackoverflow.com/questions/2375379/paging-ienumerable-dataset"/>
    /// PagedList wrapper around a List by Rob Conery. There is also an extended version by Troy Goode.
    /// Rob Conery: <seealso cref="http://blog.wekeroad.com/2007/12/10/aspnet-mvc-pagedlistt/"/>
    /// Troy Goode: <seealso cref="http://www.squaredroot.com/2008/04/08/updated-pagedlist-class/"/> 
    /// </summary>
    public interface IPagedList
    {
        /// <summary>
        /// Gets TotalPages.
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// Gets Page Index.
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// Gets Page Size.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Gets a value indicating whether IsPreviousPage.
        /// </summary>
        bool IsPreviousPage { get; }

        /// <summary>
        /// Gets a value indicating whether IsNextPage.
        /// </summary>
        bool IsNextPage { get; }
    }
}
