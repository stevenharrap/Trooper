//--------------------------------------------------------------------------------------
// <copyright file="AccessUtility.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Utility
{
    using System.Collections.Generic;
    using System.Linq;

    using Trooper.ActiveDirectory;
    using Trooper.BusinessOperation.Interface;

    /// <summary>
    /// The access utility used by the Services
    /// </summary>
    public class AccessUtility
    {
        /// <summary>
        /// The is operation accessible for the user.
        /// </summary>
        /// <param name="requiredGroups">
        /// The required groups.
        /// </param>
        /// <param name="user">
        /// The user that is performing the operation.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsAccessible(List<string> requiredGroups, ActiveDirectoryUser user)
        {
            var userGroups = user.Groups.ToList();

            return requiredGroups == null || !requiredGroups.Any()
                   || userGroups.Any(requiredGroups.Contains);
        }

        /// <summary>
        /// Adds a no-access-error to the errors list for the response.
        /// </summary>
        /// <param name="method">
        /// The method that the user was trying to access.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="user">
        /// The user that is performing the operation.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of entity that was requested
        /// </typeparam>
        public static void AddNoAccessError<TEntity>(string method, TEntity entity, IOperationResponse response, ActiveDirectoryUser user)
            where TEntity : class
        {
            var entityName = entity == null ? string.Empty : entity.GetType().FullName;

            MessageUtility.Errors.Add(
                string.Format(
                    "The user {0} does not have access to perform {1} against {2}.",
                    user.UserName,
                    method,
                    entityName),
                entity,
                null,
                response);
        }

        /// <summary>
        /// Adds a no-access-error to the errors list for the response.
        /// </summary>
        /// <param name="method">
        /// The method that the user was trying to access.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="user">
        /// The user to act as instead of the current user (if required).
        /// </param>
        public static void AddNoAccessError(string method, IOperationResponse response, ActiveDirectoryUser user)
        {
            MessageUtility.Errors.Add(
                string.Format("The user {0} does not have access to perform {1}.", user.UserName, method),
                null,
                response);
        }
    }
}