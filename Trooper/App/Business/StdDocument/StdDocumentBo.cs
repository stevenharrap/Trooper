//--------------------------------------------------------------------------------------
// <copyright file="StdDocumentBo.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Business.StdDocument
{
    using System.Data.Entity;

    using Trooper.App.Business.StdDocument.StdModel;
    using Trooper.BusinessOperation;
    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.DataManager;
    using Trooper.BusinessOperation.Interface;
    using Trooper.BusinessOperation.Response;
    using Trooper.BusinessOperation.Utility;

    /// <summary>
    /// The document business operation.
    /// </summary>
    /// <typeparam name="TDbContext">
    /// The context class
    /// </typeparam>
    /// <typeparam name="TSearch">
    /// The document search class
    /// </typeparam>
    /// <typeparam name="TEntityNav">
    /// The document nav class
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The document class
    /// </typeparam>
    /// <typeparam name="TEntityPrp">
    /// The document property class
    /// </typeparam>
    /// <typeparam name="TEntityKey">
    /// The document key class
    /// </typeparam>
    /// <typeparam name="TStdDocumentContentNav">
    /// The document content nav class
    /// </typeparam>
    /// <typeparam name="TStdDocumentContent">
    /// The document content class
    /// </typeparam>
    /// <typeparam name="TStdDocumentContentPrp">
    /// The document content property class
    /// </typeparam>
    /// <typeparam name="TStdDocumentContentKey">
    /// The document content key class
    /// </typeparam>
    public class StdDocumentBo<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey,
        TStdDocumentContentNav, TStdDocumentContent, TStdDocumentContentPrp, TStdDocumentContentKey> :
        BusinessR<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
        where TDbContext : DbContext, new()
        where TSearch : StdDocumentSearch, new()
        where TEntityNav : class, IStdDocumentNav<TStdDocumentContentNav>, TEntity, new()
        where TEntity : class, IStdDocument, TEntityPrp, new()
        where TEntityPrp : class, IStdDocumentPrp, TEntityKey, new()
        where TEntityKey : class, IStdDocumentKey, IEntityKey<TEntityKey>, new()
        where TStdDocumentContentNav : class, IStdDocumentContentNav<TEntityNav>, TStdDocumentContent, new()
        where TStdDocumentContent : class, IStdDocumentContent, TStdDocumentContentPrp, new()
        where TStdDocumentContentPrp : class, IStdDocumentContentPrp, TStdDocumentContentKey, new()
        where TStdDocumentContentKey : class, IStdDocumentContentKey, IEntityKey<TStdDocumentContentKey>, new()
    {
        /// <summary>
        /// The get document content by key.
        /// </summary>
        /// <param name="documentKey">
        /// The document key.
        /// </param>
        /// <returns>
        /// The response from the operation.
        /// </returns>
        public SingleResponse<TEntity> GetDocumentContentByKey(TEntityKey documentKey)
        {
            var response = new SingleResponse<TEntity>();

            using (new UnitOfWorkScope())
            {
                var f = this.FacadeFactory();
                var cua = f.CanUserArgFactory();
                
                var doc = f.GetByKey(documentKey);

                if (doc == null)
                {
                    MessageUtility.Errors.Add("The document does not exist.", response);
                    return response;
                }

                cua.Action = UserAction.DeleteByKeyAction;
                f.LoadCanUserArgData(cua, documentKey);

                if (!f.CanUser(cua, response))
                {
                    f.GuardFault(response);

                    return response;
                }

                response.Item = doc;
                response.Item.Data = doc.DocumentContentNav.Data;
            }

            return response;
        }

        /// <summary>
        /// The document facade factory.
        /// </summary>
        /// <returns>
        /// The new Doc Facade.
        /// </returns>
        protected override IFacade<TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey> FacadeFactory()
        {
            return
                new StdDocumentFacade<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey, TStdDocumentContentNav, TStdDocumentContent, TStdDocumentContentPrp, TStdDocumentContentKey>(this.Username);
        }
    }
}