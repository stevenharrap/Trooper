//--------------------------------------------------------------------------------------
// <copyright file="StdDocumentContentFacade.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Business.StdDocument
{
    using System.Data.Entity;

    using Trooper.App.Business.StdDocument.StdModel;
    using Trooper.BusinessOperation;
    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.Interface;

    /// <summary>
    /// The document content facade CRUD.
    /// </summary>
    /// <typeparam name="TDbContext">
    /// The context
    /// </typeparam>
    /// <typeparam name="TSearch">
    /// The document content search class
    /// </typeparam>
    /// <typeparam name="TEntityNav">
    /// The document content nav class
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The document content entity class
    /// </typeparam>
    /// <typeparam name="TEntityPrp">
    /// The document content property class
    /// </typeparam>
    /// <typeparam name="TEntityKey">
    /// The document content key class
    /// </typeparam>
    /// <typeparam name="TDocumentNav">
    /// The document nav class
    /// </typeparam>
    public class StdDocumentContentFacade<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey, TDocumentNav> :
        Facade<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
        where TDbContext : DbContext, new()
        where TSearch : Search, new()
        where TEntityNav : class, IStdDocumentContentNav<TDocumentNav>, TEntity, new()
        where TEntity : class, IStdDocumentContent, TEntityPrp, new()
        where TEntityPrp : class, IStdDocumentContentPrp, TEntityKey, new()
        where TEntityKey : class, IStdDocumentContentKey, IEntityKey<TEntityKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StdDocumentContentFacade{TDbContext,TSearch,TEntityNav,TEntity,TEntityPrp,TEntityKey,TDocumentNav}"/> class. 
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        public StdDocumentContentFacade(string username)
            : base(username)
        {
        }
    }
}