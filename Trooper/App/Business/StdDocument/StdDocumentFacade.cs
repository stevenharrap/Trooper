//--------------------------------------------------------------------------------------
// <copyright file="StdDocumentFacade.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Business.StdDocument
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Microsoft.Practices.EnterpriseLibrary.Validation;

    using Trooper.App.Business.StdDocument.StdModel;
    using Trooper.BusinessOperation;
    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.Interface;
    using Trooper.BusinessOperation.Utility;

    /// <summary>
    /// The document facade.
    /// </summary>
    /// <typeparam name="TDbContext">
    /// The context
    /// </typeparam>
    /// <typeparam name="TSearch">
    /// The document search class
    /// </typeparam>
    /// <typeparam name="TEntityNav">
    /// The document nav class
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The document entity class
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
    public class StdDocumentFacade<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey,
        TStdDocumentContentNav, TStdDocumentContent, TStdDocumentContentPrp, TStdDocumentContentKey> :
        Facade<TDbContext, TSearch, TEntityNav, TEntity, TEntityPrp, TEntityKey>
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
        /// Initializes a new instance of the <see cref="StdDocumentFacade{TDbContext,TSearch,TEntityNav,TEntity,TEntityPrp,TEntityKey,TStdDocumentContentNav,TStdDocumentContent,TStdDocumentContentPrp,TStdDocumentContentKey}"/> class.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        public StdDocumentFacade(string username)
            : base(username)
        {
        }

        /// <summary>
        /// The standard search for documents. Call this from your GetSome if you
        /// are overriding GetSome.
        /// </summary>
        /// <param name="search">
        /// The search.
        /// </param>
        /// <returns>
        /// List of document nav instances.
        /// </returns>
        public IEnumerable<TEntityNav> StandardSearch(TSearch search)
        {
            return from d in this.GetAll()
                   where
                       (string.IsNullOrEmpty(search.Filename)
                        || d.Filename.ToLower().Contains(search.Filename.ToLower()))
                       && (string.IsNullOrEmpty(search.Extension)
                           || d.Extension.ToLower().Equals(search.Extension.ToLower()))
                   select d;
        }

        /// <summary>
        /// The get some method using StandardSearch by default.
        /// </summary>
        /// <param name="search">
        /// The search.
        /// </param>
        /// <returns>
        /// List of document nav instances.
        /// </returns>
        public override IEnumerable<TEntityNav> GetSome(TSearch search)
        {
            return this.Limit(this.StandardSearch(search), search);
        }

        /// <summary>
        /// If manageItem indicates that it should be deleted then it is deleted otherwise it is added
        /// or updated and then validated. 
        /// </summary>
        /// <typeparam name="TForeign">
        /// The data type of the entity that references the document
        /// </typeparam>
        /// <typeparam name="TForeignKey">
        /// The data type on the key to document in entity that references the document 
        /// (probably integer?)
        /// </typeparam>
        /// <param name="manageItem">
        /// The manage item that is either a change or delete.
        /// </param>
        /// <param name="existingDocumentNav">
        /// The existing Document Nav.
        /// </param>
        /// <param name="foreignEntity">
        /// The foreign Entity that references the document
        /// </param>
        /// <param name="foreignKeyExp">
        /// The expression for the foreign Key on the entity that references the document.
        /// </param>
        /// <param name="vr">
        /// The validation results to send back any issues.
        /// </param>
        /// <returns>
        /// The updated document nav or null if it is a delete.
        /// </returns>
        public TEntityNav ManageDocument<TForeign, TForeignKey>(
            ManageItem<TEntityPrp> manageItem, 
            TEntityNav existingDocumentNav,
            TForeign foreignEntity,
            Expression<Func<TForeign, TForeignKey>> foreignKeyExp, 
            IOperationResponse response)
        {
            if (manageItem == null)
            {
                return existingDocumentNav;
            }

            var ftMemExpr = (MemberExpression)foreignKeyExp.Body;
            var fkProp = (PropertyInfo)ftMemExpr.Member;

            if (ManageItemUtility.IsDelete(manageItem) && existingDocumentNav != null)
            {
                this.DeleteByKey(new TEntityKey { DocumentId = existingDocumentNav.DocumentId });
                fkProp.SetValue(foreignEntity, default(TForeignKey));
                
                return null;
            }

            if (ManageItemUtility.IsChange(manageItem))
            {
                var item = this.Map(manageItem.Item);

                var r = this.Replace(item, existingDocumentNav, true, true) as IStdDocumentKey;

                if (r == null)
                {
                    MessageUtility.Errors.Add("The Document could not be added or updated.", response);
                }

                if (Nullable.GetUnderlyingType(typeof(TForeignKey)) == null)
                {
                    var newId = r == null ? null : r.IsEntityNew() ? null : (int?)r.DocumentId;
                    fkProp.SetValue(foreignEntity, newId);
                }
                else
                {
                    var newId = r == null ? 0 : r.IsEntityNew() ? 0 : r.DocumentId;
                    fkProp.SetValue(foreignEntity, newId);
                }

                this.ValidateEntity(r as TEntityNav, response);

                return r as TEntityNav;
            }

            return null;
        }

        /// <summary>
        /// Manage some documents when they form part of a many-to-many relationship with an
        /// other entity.
        /// </summary>
        /// <param name="managedDocuments">
        /// The managed document that indicates that change that should take place.
        /// </param>
        /// <param name="documentNavs">
        /// The document navs that document is in.
        /// </param>
        /// <param name="eachParam">
        /// The parameter that will be passed in the onEach function.
        /// </param>
        /// <param name="onEach">
        /// The function that will be called after the document is managed. You could
        /// record a log of the action here.
        /// </param>
        /// <typeparam name="TEachParam">
        /// The type of the value which passed to the onEach function
        /// </typeparam>
        /// <returns>
        /// The <see cref="TEntityNav"/>.
        /// </returns>
        public bool ManageDocumentsM2M<TEachParam>(
            List<ManageItem<TEntityPrp>> managedDocuments, 
            ICollection<TEntityNav> documentNavs,
            TEachParam eachParam,
            Func<ManageStdDocumentsEach<TEntityNav, TStdDocumentContentNav>, TEachParam, Action> onEach = null)
        {
            foreach (var md in managedDocuments)
            {
                var docNav = this.ManageDocumentM2M(md, documentNavs, eachParam, onEach);

                if (docNav == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Manage the document when it forms part of a many-to-many relationship with an
        /// other entity.
        /// </summary>
        /// <param name="managedDocument">
        /// The managed document that indicates that change that should take place.
        /// </param>
        /// <param name="documentNavs">
        /// The document navs that document is in.
        /// </param>
        /// <param name="eachParam">
        /// The parameter that will be passed in the onEach function.
        /// </param>
        /// <param name="onEach">
        /// The function that will be called after the document is managed. You could
        /// record a log of the action here.
        /// </param>
        /// <typeparam name="TEachParam">
        /// The type of the value which passed to the onEach function
        /// </typeparam>
        /// <returns>
        /// The <see cref="TEntityNav"/>.
        /// </returns>
        public TEntityNav ManageDocumentM2M<TEachParam>(
            ManageItem<TEntityPrp> managedDocument,
            ICollection<TEntityNav> documentNavs,
            TEachParam eachParam,
            Func<ManageStdDocumentsEach<TEntityNav, TStdDocumentContentNav>, TEachParam, Action> onEach = null)
        {
            var documentNav = this.GetByKey(new TEntityKey { DocumentId = managedDocument.Item.DocumentId });

            if (documentNav != null && ManageItemUtility.IsDelete(managedDocument))
            {
                documentNavs.Remove(documentNav);
                this.DeleteByKey(new TEntityKey { DocumentId = managedDocument.Item.DocumentId });
                this.InvokeManageStdDocumentsEach(eachParam, onEach, documentNav, ManageStdDocumentsEach<TEntityNav, TStdDocumentContentNav>.ChangeTypes.Delete);
            }
            else if (documentNav != null
                && ManageItemUtility.IsChange(managedDocument) 
                && !(managedDocument.Item as IStdDocumentKey).IsEntityNew())
            {
                documentNav.Filename = managedDocument.Item.Filename;
                documentNav.Data = managedDocument.Item.Data;
                this.InvokeManageStdDocumentsEach(eachParam, onEach, documentNav, ManageStdDocumentsEach<TEntityNav, TStdDocumentContentNav>.ChangeTypes.Update);
            }
            else if (documentNav == null
                && ManageItemUtility.IsChange(managedDocument) 
                && (managedDocument.Item as IStdDocumentKey).IsEntityNew())
            {
                documentNav = this.Add(this.Map(managedDocument.Item));
                documentNavs.Add(documentNav);
                this.InvokeManageStdDocumentsEach(eachParam, onEach, documentNav, ManageStdDocumentsEach<TEntityNav, TStdDocumentContentNav>.ChangeTypes.Add);
            }
            else
            {
                return null;
            }

            return documentNav;
        }

        /// <summary>
        /// Add a document.
        /// </summary>
        /// <param name="newEntityNav">
        /// The new document.
        /// </param>
        /// <returns>
        /// The added document
        /// </returns>
        public override TEntityNav Add(TEntityNav newEntityNav)
        {
            var dcf = this.MakeDocumentContentFacade();
            var dc = new TStdDocumentContentNav { Data = newEntityNav.Data };

            var docNav = base.Add(newEntityNav);
            dc.DocumentNav = docNav;
            dcf.Add(dc);

            return docNav;
        }

        /// <summary>
        /// Update a document.
        /// </summary>
        /// <param name="entityNav">
        /// The entity nav.
        /// </param>
        /// <returns>
        /// The updated document
        /// </returns>
        public override TEntityNav Update(TEntityNav entityNav)
        {
            var dcf = this.MakeDocumentContentFacade();
            var dc = dcf.GetByKey(new TStdDocumentContentKey { DocumentId = entityNav.DocumentId });

            if (dc == null)
            {
                dc = new TStdDocumentContentNav { DocumentId = entityNav.DocumentId, DocumentNav = entityNav };
                entityNav.DocumentContentNav = dc;
            }

            dc.Data = entityNav.Data;

            return base.Update(entityNav);
        }

        /// <summary>
        /// Delete a document.
        /// </summary>
        /// <param name="entityKey">
        /// The entity key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool DeleteByKey(TEntityKey entityKey)
        {
            if (!base.DeleteByKey(entityKey))
            {
                return false;
            }

            var dcf = this.MakeDocumentContentFacade();

            return dcf.DeleteByKey(new TStdDocumentContentKey { DocumentId = entityKey.DocumentId });
        }

        /// <summary>
        /// Make a document facade
        /// </summary>
        /// <returns>
        /// The new document facade
        /// </returns>
        private StdDocumentContentFacade<TDbContext, Search, TStdDocumentContentNav, TStdDocumentContent, TStdDocumentContentPrp, TStdDocumentContentKey, TEntityNav> MakeDocumentContentFacade()
        {
            return
                new StdDocumentContentFacade<TDbContext, Search, TStdDocumentContentNav, TStdDocumentContent, TStdDocumentContentPrp, TStdDocumentContentKey, TEntityNav>(
                    this.Username);
        }

        /// <summary>
        /// Is called after each item is processed by ManageDocumentM2M ManageDocumentsM2M.
        /// </summary>
        /// <param name="eachParam">
        /// The each parameter to pass to the onEach function.
        /// </param>
        /// <param name="onEach">
        /// The function to call after each item is processed.
        /// </param>
        /// <param name="documentNav">
        /// The document nav.
        /// </param>
        /// <param name="changeType">
        /// The change type.
        /// </param>
        /// <typeparam name="TEachParam">
        /// The data type of the parameter passed to the enEach function
        /// </typeparam>
        private void InvokeManageStdDocumentsEach<TEachParam>(
            TEachParam eachParam,
            Func<ManageStdDocumentsEach<TEntityNav, TStdDocumentContentNav>, TEachParam, Action> onEach,
            TEntityNav documentNav,
            ManageStdDocumentsEach<TEntityNav, TStdDocumentContentNav>.ChangeTypes changeType)
        {
            if (onEach != null)
            {
                onEach.Invoke(
                    new ManageStdDocumentsEach<TEntityNav, TStdDocumentContentNav>
                    {
                        StdDocumentNav = documentNav,
                        ChangeType = changeType
                    }, 
                    eachParam);
            }
        }
    }
}