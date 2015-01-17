//--------------------------------------------------------------------------------------
// <copyright file="DbContextScope.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.DataManager
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Defines a scope wherein only one DbContext instance is created, and shared by all of those who use it. 
    /// </summary>
    /// <remarks>Instances of this class are supposed to be used in a using() statement.</remarks>
    public class DbContextScope : IDisposable
    {
        /// <summary>
        /// DbContext scope definition.
        /// </summary>
        [ThreadStatic]
        private static DbContextScope currentScope;

        /// <summary>
        /// List of current DbContexts (supports multiple contexts).
        /// </summary>
        private readonly List<DbContext> contextList;

        /// <summary>
        /// Holds a value indicating whether the context is disposed or not.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextScope"/> class.
        /// </summary>
        /// <param name="saveAllChangesAtEndOfScope">if set to <c>true</c> [save all changes at end of scope].</param>
        protected DbContextScope(bool saveAllChangesAtEndOfScope)
        {
            if (currentScope != null && !currentScope.isDisposed)
            {
                throw new InvalidOperationException("DbContextScope instances cannot be nested.");
            }

            this.SaveAllChangesAtEndOfScope = saveAllChangesAtEndOfScope;

            this.contextList = new List<DbContext>();

            this.isDisposed = false;

            Thread.BeginThreadAffinity();

            currentScope = this;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically save all object changes at end of the scope.
        /// </summary>
        /// <value><c>true</c> if [save all changes at end of scope]; otherwise, <c>false</c>.</value>
        private bool SaveAllChangesAtEndOfScope { get; set; }        

        /// <summary>
        /// Save all object changes to the underlying data store.
        /// </summary>
        public void SaveAllChanges()
        {
            var transactions = new List<DbTransaction>();

            foreach (var context in this.contextList
                .Select(dbcontext => ((IObjectContextAdapter)dbcontext)
                    .ObjectContext))
            {
                context.Connection.Open();

                var databaseTransaction = context.Connection.BeginTransaction();

                transactions.Add(databaseTransaction);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    /* Rollback & dispose all transactions: */
                    foreach (var transaction in transactions)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        // ReSharper disable EmptyGeneralCatchClause
                        catch
                        // ReSharper restore EmptyGeneralCatchClause
                        {
                            // "Empty general catch clause suppresses any errors."
                            // Haven't quite figured out what to do here yet.
                        }
                        finally
                        {
                            databaseTransaction.Dispose();
                        }
                    }

                    transactions.Clear();

                    if (e.InnerException == null)
                    {
                        throw new Exception("The transaction did not work for unknown reasons.", e);
                    }

                    //// This will be because of a SQL error;
                    throw new Exception(e.InnerException.Message);
                }
            }

            try
            {
                /* Commit all complete transactions: */
                foreach (var completeTransaction in transactions)
                {
                    completeTransaction.Commit();
                }
            }
            finally
            {
                /* Dispose all transactions: */
                foreach (var transaction in transactions)
                {
                    transaction.Dispose();
                }

                transactions.Clear();

                /* Close all open connections: */
                foreach (var context in this.contextList
                    .Select(dbcontext => ((IObjectContextAdapter)dbcontext).ObjectContext)
                    .Where(context => context.Connection.State != System.Data.ConnectionState.Closed))
                {
                    context.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Disposes the DbContext.
        /// </summary>
        public void Dispose()
        {
            // Monitor for possible future bugfix.
            // CA1063 : Microsoft.Design : Provide an overridable implementation of Dispose(bool) 
            // on 'DbContextScope' or mark the type as sealed. A call to Dispose(false) should 
            // only clean up native resources. A call to Dispose(true) should clean up both managed 
            // and native resources.
            if (this.isDisposed)
            {
                return;
            }

            // Monitor for possible future bugfix.
            // CA1063 : Microsoft.Design : Modify 'DbContextScope.Dispose()' so that it calls 
            // Dispose(true), then calls GC.SuppressFinalize on the current object instance 
            // ('this' or 'Me' in Visual Basic), and then returns.
            currentScope = null;

            Thread.EndThreadAffinity();

            try
            {
                if (this.SaveAllChangesAtEndOfScope && this.contextList.Count > 0)
                {
                    this.SaveAllChanges();
                }
            }
            finally
            {
                foreach (var context in this.contextList)
                {
                    try
                    {
                        context.Dispose();
                    }
                    catch (ObjectDisposedException)
                    {
                        // Monitor for possible future bugfix.
                        // CA2202 : Microsoft.Usage : Object 'databaseTransaction' can be disposed 
                        // more than once in method 'DbContextScope.SaveAllChanges()'. 
                        // To avoid generating a System.ObjectDisposedException you should not call 
                        // Dispose more than one time on an object.
                    }
                }

                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Returns a reference to a DbContext of a specific type that is - or will be -
        /// created for the current scope. If no scope currently exists, null is returned.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the db context.</typeparam>
        /// <returns>The current DbContext</returns>
        protected internal static TDbContext GetCurrentDbContext<TDbContext>()
            where TDbContext : DbContext, new()
        {
            if (currentScope == null)
            {
                return null;
            }

            var contextOfType = currentScope.contextList
                .OfType<TDbContext>()
                .FirstOrDefault();

            if (contextOfType == null)
            {
                contextOfType = new TDbContext();

                currentScope.contextList.Add(contextOfType);
            }

            return contextOfType;
        }
    }
}