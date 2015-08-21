//--------------------------------------------------------------------------------------
// <copyright file="ActiveDirectoryUser.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.ActiveDirectory
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Security.Principal;
    using System.Web;
    using Trooper.Properties;

    /// <summary>
    /// Minor Active Directory wrapper for accessing active directory user information.
    /// </summary>
    public class ActiveDirectoryUser
    {
        /// <summary>
        /// List of active directory groups a user belongs to.
        /// </summary>
        private readonly List<string> activeDirectoryGroups;

        /// <summary>
        /// Encapsulates a node in the Active Directory domain services hierarchy.
        /// </summary>
        private SearchResult searchResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectoryUser"/> class.
        /// </summary>
        public ActiveDirectoryUser()
        {
            string userName = null;

            if (HttpContext.Current == null)
            {
                var currentIdentity = WindowsIdentity.GetCurrent();

                if (currentIdentity != null && currentIdentity.Name != null)
                {
                    userName = currentIdentity.Name;
                }
            }
            else
            {
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Resources.WindowsIdentityUsernameCannotBeNull);
            }

            this.activeDirectoryGroups = new List<string>();

            this.SetUserName(userName);

            this.SetUserProperties();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectoryUser"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public ActiveDirectoryUser(string userName)
        {
            this.activeDirectoryGroups = new List<string>();

            this.SetUserName(userName);

            this.SetUserProperties();
        }

        /// <summary>
        /// Gets the user's forename.
        /// </summary>
        /// <value>The forename.</value>
        public string Forename { get; private set; }

        /// <summary>
        /// Gets the user's initials.
        /// </summary>
        /// <value>The initials.</value>
        public string Initials { get; private set; }

        /// <summary>
        /// Gets the middle name of the user.
        /// </summary>
        /// <value>The middle nam.</value>
        public string MiddleName { get; private set; }

        /// <summary>
        /// Gets the user's surname.
        /// </summary>
        /// <value>The surname.</value>
        public string Surname { get; private set; }

        /// <summary>
        /// Gets the description of the user.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the user's email address.
        /// </summary>
        /// <value>The email address.</value>
        public string EmailAddress { get; private set; }

        /// <summary>
        /// Gets the mobile phone number for the user.
        /// </summary>
        /// <value>The mobile number.</value>
        public string Mobile { get; private set; }

        /// <summary>
        /// Gets the user's department.
        /// </summary>
        /// <value>The department.</value>
        public string Department { get; private set; }

        /// <summary>
        /// Gets the user's full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the user's log-on name.
        /// </summary>
        /// <value>The name of the user logon.</value>
        public string UserLogOnName { get; private set; }

        /// <summary>
        /// Gets the user's distinguished name.
        /// </summary>
        /// <value>The distinguished name.</value>
        /// <remarks>This is the unique identifier for any object in Active Directory.</remarks>
        public string DistinguishedName { get; private set; }

        /// <summary>
        /// Gets the name of the distinguished.
        /// </summary>
        /// <value>The name of the distinguished.</value>
        public string Office { get; private set; }

        /// <summary>
        /// Gets the user's display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the user's telephone.
        /// </summary>
        /// <value>The telephone.</value>
        public string Telephone { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the user exists.
        /// </summary>
        public bool Exists { get; private set; }

        /// <summary>
        /// Gets a list of Active Directory groups that the user belongs to.
        /// </summary>
        /// <value>The groups.</value>
        public IEnumerable<string> Groups
        {
            get
            {
                return this.activeDirectoryGroups;
            }
        }

        /// <summary>
        /// Determines whether a user [is in group] [the specified group].
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>
        ///     <c>true</c> if [is in group] [the specified group]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInGroup(string group)
        {
            return this.activeDirectoryGroups != null && 
                   this.activeDirectoryGroups.Contains(@group);
        }

        /// <summary>
        /// Sets the user properties.
        /// </summary>
        public void SetUserProperties()
        {
            this.searchResult = this.GetProperties();

            if (this.searchResult != null && this.activeDirectoryGroups != null)
            {
                this.SetProperties();
            }
        }

        /// <summary>
        /// Gets the active directory path.
        /// </summary>
        /// <returns>
        /// The path of Active Directory when executed on a domain.
        /// </returns>
        private static string GetActiveDirectoryPath()
        {
            var activeDirectoryPath = string.Empty;

            using (var directoryEntry = new DirectoryEntry(Resources.ActiveDirectoryRoot))
            {
                var directoryEntryProperty = directoryEntry.Properties[Resources.ActiveDirectoryDefaultNamingContext];

                if (directoryEntryProperty != null && directoryEntryProperty.Count > 0)
                {
                    var defaultNamingContext = directoryEntryProperty[0].ToString();

                    directoryEntryProperty = directoryEntry.Properties[Resources.ActiveDirectoryDnsHostName];

                    if (directoryEntryProperty != null && directoryEntryProperty.Count > 0)
                    {
                        var dnsHostName = directoryEntryProperty[0].ToString();

                        activeDirectoryPath = string.Format(
                            CultureInfo.InvariantCulture, 
                            Resources.ActiveDirectoryLdapPathFormat, 
                            dnsHostName, 
                            defaultNamingContext);
                    }
                }
            }

            return activeDirectoryPath;
        }

        /// <summary>
        /// Sets the name of the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        private void SetUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return;
            }

            var usernameParts = userName.Split('\\');

            if (usernameParts.Length > 1)
            {
                userName = usernameParts[1];
            }

            this.UserName = userName;

            this.Exists = true;
        }

        /// <summary>
        /// Sets the properties for the provided SearchResult.
        /// </summary>
        private void SetProperties()
        {
            if (this.searchResult == null)
            {
                return;
            }

            if (this.searchResult.Properties == null)
            {
                return;
            }

            this.Forename = this.GetPropertyValue(Resources.ActiveDirectoryGivenName);
            
            this.Initials = this.GetPropertyValue(Resources.ActiveDirectoryInitials);
            
            this.MiddleName = this.GetPropertyValue(Resources.ActiveDirectoryMiddleName);

            this.Surname = this.GetPropertyValue(Resources.ActiveDirectorySn);

            this.DisplayName = this.GetPropertyValue(Resources.ActiveDirectoryDisplayName);

            this.FullName = this.GetPropertyValue(Resources.ActiveDirectoryCn);

            this.Description = this.GetPropertyValue(Resources.ActiveDirectoryDescription);

            this.Telephone = this.GetPropertyValue(Resources.ActiveDirectoryTelephoneNumber);

            this.EmailAddress = this.GetPropertyValue(Resources.ActiveDirectoryMail);

            this.Mobile = this.GetPropertyValue(Resources.ActiveDirectoryMobile);

            this.Department = this.GetPropertyValue(Resources.ActiveDirectoryDepartment);

            this.UserLogOnName = this.GetPropertyValue(Resources.ActiveDirectoryUserPrincipalName);
            
            this.DistinguishedName = this.GetPropertyValue(Resources.ActiveDirectoryDn);

            this.Office = this.GetPropertyValue(Resources.ActiveDirectoryPhysicalDeliveryOfficeName);

            if (!this.PropertyExists(Resources.ActiveDirectoryMemberOf))
            {
                return;
            }

            var propertyCount = this.searchResult.Properties[Resources.ActiveDirectoryMemberOf].Count;

            for (var propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
            {
                var dn = (string)this.searchResult.Properties[Resources.ActiveDirectoryMemberOf][propertyCounter];

                if (!string.IsNullOrEmpty(dn) && dn.Length > 1)
                {
                    var equalsIndex = dn.IndexOf("=", 1, StringComparison.Ordinal);

                    var commaIndex = dn.IndexOf(",", 1, StringComparison.Ordinal);

                    if (-1 == equalsIndex)
                    {
                        return;
                    }

                    var startIndex = equalsIndex + 1;

                    var length = (commaIndex - equalsIndex) - 1;

                    if (0 <= length)
                    {
                        dn = dn.Substring(startIndex, length);
                    }
                }

                this.activeDirectoryGroups.Add(dn);
            }
        }

        /// <summary>
        /// Gets the property value for the specified active directory property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The property value.</returns>
        private string GetPropertyValue(string propertyName)
        {
            return this.PropertyExists(propertyName) 
                ? this.searchResult.Properties[propertyName][0].ToString() 
                : null;
        }

        /// <summary>
        /// Gets the <see cref="DirectoryEntry"/> user.
        /// </summary>
        /// <returns><see cref="DirectoryEntry"/> matching the supplied user name or null if not found.</returns>
        private SearchResult GetProperties()
        {
            if (this.UserName == null)
            {
                return null;
            }

            var directoryPath = GetActiveDirectoryPath();

            using (var directoryEntry = new DirectoryEntry(directoryPath))
            {
                using (var search = new DirectorySearcher(directoryEntry))
                {
                    search.CacheResults = true;
                    search.Filter = string.Format(CultureInfo.InvariantCulture, Resources.ActiveDirectorySamAccountFormat, this.UserName);
                    search.PropertiesToLoad.AddRange(new[]
                    {
                        Resources.ActiveDirectoryCn,
                        Resources.ActiveDirectoryGivenName,
                        Resources.ActiveDirectoryInitials,
                        Resources.ActiveDirectoryMiddleName,
                        Resources.ActiveDirectorySn,
                        Resources.ActiveDirectoryDisplayName,
                        Resources.ActiveDirectoryDescription,
                        Resources.ActiveDirectoryTelephoneNumber,
                        Resources.ActiveDirectoryMail,
                        Resources.ActiveDirectoryUserPrincipalName,
                        Resources.ActiveDirectoryMobile,
                        Resources.ActiveDirectoryDepartment,
                        Resources.ActiveDirectoryDn,
                        Resources.ActiveDirectoryPhysicalDeliveryOfficeName,
                        Resources.ActiveDirectoryMemberOf
                    });

                    SearchResult result;

                    try
                    {
                        result = search.FindOne();
                    }
                    catch (DirectoryServicesCOMException directoryServicesComException)
                    {
                        directoryServicesComException.Data.Add(
                            Resources.MessageTitleAdditionalInfo, 
                            Resources.MessageServerRestart);

                        throw;
                    }
                    catch (COMException comException)
                    {
                        comException.Data.Add(
                            Resources.MessageTitleAdditionalInfo, 
                            Resources.MessageServerRestart);

                        throw;
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Checks whether the specified property exists.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns><c>true</c> if the property exists; <c>false</c> if not.</returns>
        private bool PropertyExists(string property)
        {
            return this.searchResult.Properties.Contains(property) && this.searchResult.Properties[property].Count > 0;
        }
    }
}